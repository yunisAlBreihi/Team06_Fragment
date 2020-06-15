using UnityEngine;
using UnityEngine.Timeline;

public class CameraController : MonoBehaviour
{
    const float Y_ANGLE_MIN = -90f;
    const float Y_ANGLE_MAX = 90f;

    const float OFFSET_MIN = 0f;
    const float OFFSET_MAX = 40f;

    public static CameraController PlayerCamera;

    const float e = 0.0001f; //Using e as in epsilon to mark a small positive number

    [SerializeField]
    Transform pivotPoint = default;

    [SerializeField, Tooltip("A point on where the camera uses as a focus point, this object will be in the middle of the game")]
    public Transform playerFocusPoint = default;

    [Header("Camera attributes")]
    [SerializeField, Tooltip("Choose what layers should not be breaking the line of site")]
    LayerMask obstructionMask = -1;

    [SerializeField, Range(OFFSET_MIN, OFFSET_MAX), Tooltip("The minimum distance you can have between the camera and the player")]
    float minOffsetDistance = 0f; //Distance between player and camera

    [SerializeField, Range(OFFSET_MIN, OFFSET_MAX), Tooltip("The distance between the player and the camera")]
    public float offsetDistance = 20f; //Distance between player and camera

    [SerializeField, Min(0f), Tooltip("An area where the player can move within without the camera moving")]
    float focusRadius = 1f; //Gives a radius where the player can move without the camera following

    [SerializeField, Range(0f, 1f), Tooltip("A percentage of the are you can move within before the camera starts moving. 0 = long delay, 1 = immediately")]
    float focusCentering = 0.75f;

    [SerializeField, Tooltip("Works as an extra offset on the camera, " +
        "change X to have the camera more on the left or right side of the player," +
        "change Y to raise or lower the camera")]
    Vector2 heightAndSideOffset = new Vector2(0, 2);


    [Header("Camera Controls")]
    [SerializeField, Tooltip("If you want to invert the horizontal controls")]
    bool useInvertedHorizontalControls = false;

    [SerializeField, Tooltip("If you want to invert the horizontal controls")]
    bool useInvertedVerticalControls = false;

    [Header("Looking")]
    [SerializeField, Range(1f, 360f), Tooltip("Mouse sensitivity for X-axis")]
    float XMouseSensitivity = 90f;

    [SerializeField, Range(1f, 360f), Tooltip("Mouse sensitivity for Y-axis")]
    float YMouseSensitivity = 90f;

    [SerializeField, Range(Y_ANGLE_MIN, Y_ANGLE_MAX), Tooltip("Capping the max and min vertical angle of the player controller")]
    float minVerticalAngle = -30f, maxVerticalAngle = 60f;

    Camera regularCamera;

    public Vector3 focusPoint; //What we want to focus on, in this case the players position

    public Vector3 lookPosition;

    public Vector2 orbitAngles; //We use Vector2D for there is no need for the Z axis

    float maxOffsetDist;

    public Transform newFocusPoint;

    private Quaternion lookRotation;

    private Vector2 input;

    private float XS;
    private float YS;

    public bool smoothOut = false;

    public InputManager i;

    void Awake()
    {
        smoothOut = false;
        PlayerCamera = this;
        if (playerFocusPoint == null || pivotPoint == null)
        {
            FindPlayer();
        }

        regularCamera = GetComponent<Camera>();
        focusPoint = playerFocusPoint.position;
        transform.localRotation = Quaternion.Euler(playerFocusPoint.eulerAngles);
        maxOffsetDist = offsetDistance;
    }

    private void Start()
    {
        i = PlayerMovement.MyPlayer.gameObject.GetComponent<InputManager>();
        SetPositionBehindPlayer();
    }

    private void FindPlayer()
    {
        Transform p = GameObject.FindGameObjectWithTag("Player").transform;
        pivotPoint = p.Find("CameraPivot");
        playerFocusPoint = p.Find("LookAtTarget").transform;
    }

    private void OnValidate()
    {
        if (maxVerticalAngle < minVerticalAngle) //Basically clamping the the max and min values of the vertical angle
        {
            maxVerticalAngle = minVerticalAngle;
        }

        if (offsetDistance < minOffsetDistance)
        {
            offsetDistance = minOffsetDistance;
        }
    }

    void LateUpdate()
    {
        //if (Input.GetKey(KeyCode.F)) 
        //{
        //    OverrideFocusPoint();
        //}
        //else
        //{
        //    UpdateFocusPoint();

        //}

        UpdateFocusPoint();

        ManualRotation(); //We only need to constraint the angles if they been changed, so we check for that

        ConstraintAngles();
        
        lookRotation = Quaternion.Euler(orbitAngles); //WE only need to recalculate the angles if they been changed, otherwise (else stat.) we retrieve the existing one

        //Set the position and rotation for the camera
        Vector3 lookDirection = lookRotation * Vector3.forward;
        lookPosition = focusPoint + (Vector3)heightAndSideOffset - lookDirection * offsetDistance;

        Vector3 rectOffset = lookDirection * regularCamera.nearClipPlane;
        Vector3 rectPosition = lookPosition + rectOffset;
        Vector3 castFrom = pivotPoint.position;
        Vector3 castLine = rectPosition - castFrom;
        float castDistance = castLine.magnitude;
        Vector3 castDirection = castLine / castDistance;

        if (Physics.BoxCast(castFrom, CameraHalfExtends, castDirection, out RaycastHit hit, lookRotation, castDistance - regularCamera.nearClipPlane, obstructionMask))
        {
            rectPosition = castFrom + castDirection * hit.distance;
            lookPosition = rectPosition - rectOffset;
        }

        if (smoothOut)
        {
            Quaternion rot = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 2f);
            rot = Quaternion.RotateTowards(rot, lookRotation, Time.deltaTime * 1f);

            Vector3 pos = Vector3.Lerp(transform.position, lookPosition, Time.deltaTime * 6f);
            pos = Vector3.MoveTowards(pos, lookPosition, Time.deltaTime * 2f);

            transform.SetPositionAndRotation(pos, rot);

            return;
        }
        transform.SetPositionAndRotation(lookPosition, lookRotation);

    }

    //void OverrideFocusPoint()
    //{
    //    Vector3 targetPoint = newFocusPoint.position;

    //    focusPoint = Vector3.Lerp(focusPoint, targetPoint, Time.deltaTime * 5f);
    //}

    void UpdateFocusPoint()
    {
        Vector3 targetPoint = pivotPoint.position;

        //Vector3 targetPoint = isPlayer ? player.position : newFocusPoint.position;

        if (focusRadius > 0f) //If the radius is bigger than 0 we want the effect of having the player move a bit without camera following, else we just want to update the camera follow directly
        {
            float distance = Vector3.Distance(targetPoint, focusPoint); //Gets the distance between "the old player pos" and the newly updated player pos

            if (distance > focusRadius) //We check if that distance is greater than the radius we allowed the player to move within
            {
                focusPoint = Vector3.Lerp(targetPoint, focusPoint, focusRadius / distance); //Lerps between the old and new player pos 
            }

            if (distance > 0.01f && focusCentering > 0f)
            {
                focusPoint = Vector3.Lerp(targetPoint, focusPoint, Mathf.Pow(1f - focusCentering, Time.unscaledDeltaTime)); //Lerping between the two points, smoothing the lerping out with the Pow method
            }
        }
        else
        {
            focusPoint = Vector3.Lerp(focusPoint, targetPoint, Time.deltaTime * 5f);
        }
    }

    //void CameraShake()
    //{
    //    //TODO: Implement camera shake 
    //}

    void ManualRotation()
    {
        Vector2 lookInput = i.CameraInput();
        if (!PauseMenu.isPaused)
        {
            input.x = useInvertedVerticalControls ? -lookInput.y * XMouseSensitivity : lookInput.y * XMouseSensitivity;
            input.y = useInvertedHorizontalControls ? -lookInput.x * YMouseSensitivity : lookInput.x * YMouseSensitivity;
            if (input.x < -e || input.x > e || input.y < -e || input.y > e) //here we check for movement from the mouse
            {
                orbitAngles += Time.unscaledDeltaTime * input; //Time.unscaledDeltaTime is independent from the in-game time, so if the timescale is tempered the orbitAngles are unaffected 

                if (orbitAngles.x < minVerticalAngle)
                {

                    //float absX = Input.GetAxis("Mouse Y");
                    //absX += Mathf.Abs(input.x * 0.01f);
                    //Debug.Log(absX);
                    //offsetDistance -= absX;
                    //offsetDistance = Mathf.Clamp(offsetDistance, minOffsetDistance, maxOffsetDist);
                }
            }
        }
    }

    void ConstraintAngles()
    {
        orbitAngles.x = Mathf.Clamp(orbitAngles.x, minVerticalAngle, maxVerticalAngle);

        if (orbitAngles.y < 0f) //Making sure the vertical angles stays within the 0-360 range and not exceeding that
        {
            orbitAngles.y += 360f;
        }
        else if (orbitAngles.y >= 360f)
        {
            orbitAngles.y -= 360f;
        }
    }

    Vector3 CameraHalfExtends
    {
        get
        {
            Vector3 halfExtends;
            halfExtends.y = regularCamera.nearClipPlane * Mathf.Tan(0.5f * Mathf.Deg2Rad * regularCamera.fieldOfView);
            halfExtends.x = halfExtends.y * regularCamera.aspect;
            halfExtends.z = 0f;
            return halfExtends;
        }
    }

    public void EnableInvertXAxis(bool enable)
    {
        useInvertedHorizontalControls = enable;
    }

    public void EnableInvertYAxis(bool enable)
    {
        useInvertedVerticalControls = enable;
    }

    public void ChangeSensativityX(float sens)
    {
        XMouseSensitivity = sens;
    }
    public void ChangeSensativityY(float sens)
    {
        YMouseSensitivity = sens;
    }
    public void SetPositionBehindPlayer()
    {
        orbitAngles = new Vector2(15f, playerFocusPoint.eulerAngles.y);
    }
    public void SwitchSensitivity()
    {
        XS = XMouseSensitivity;
        YS = YMouseSensitivity;
        YMouseSensitivity = 0;
        XMouseSensitivity = 0;
    }
    public void SwitchSensitivityBack()
    {
        XMouseSensitivity = XS;
        YMouseSensitivity = YS;
    }
}
