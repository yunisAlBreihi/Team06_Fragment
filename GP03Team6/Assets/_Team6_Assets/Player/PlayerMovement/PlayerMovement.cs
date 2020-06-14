using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager), typeof(CapsuleCollider), typeof(CharacterController))]
[RequireComponent(typeof(PlayerAbilities))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Ground Check Settings")]
    [SerializeField] public float groundCastDistance = 0.001f;
    private Vector3 groundCastStart;
    private float groundCastRadius;
    private bool grounded = false;
    public RaycastHit groundHit;
    private CapsuleCollider pushBox;

    public Quaternion respawnRot;
    public Vector3 respawnPos;

    public LayerMask blockingLayers;

    public MusicScript music;
    public GroundMovementSettings groundSettings;
    public AirMovementSettings airSettings;
    private static PlayerMovement p_player;
    public static PlayerMovement MyPlayer;//{ set { MyPlayer = p_player; } get { return p_player; } }

    public Animator anim;

    [SerializeField] public AirState airState;
    [SerializeField] public CutsceneState cutsceneState;
    [SerializeField] public BoxMoveState boxMoveState;
    [SerializeField] public GroundMoveState groundMoveState;
    //[SerializeField] public ClimbState climbState;
    [SerializeField] public SlopeSlideState slopeSlideState;
    [SerializeField] public BeamMoveState beamMoveState;
    [SerializeField] public ClimbingState climbingState;


    [SerializeField] public IMovement currentState; //interface variable to keep track of current state.

    public InputManager input;
    public CharacterController cc;
    public PlayerAbilities abilities;
    public bool addToMove = false;
    public Vector3 addMoveVector;
    //[Header("Wall Climbing")]
    //public CheckIfClimb climbChecker;

    public InteractPrompt interactPrompt;

    private void Awake()
    {
        MyPlayer = this;
        anim = GetComponent<Animator>();
        FindReferences();
    }

    void Start()
    {
       
        CreatePlayerStates();
        groundCastStart = ( new Vector3(0f, pushBox.radius - pushBox.bounds.extents.y, 0f) + pushBox.center );
        groundCastRadius = pushBox.radius;
        airState.climbCheck = GetComponent<ClimbCheck>();
        currentState = groundMoveState;   //Set starting state to idle. 
        currentState.Start();
        //If we want to reset values in the beginning, we can call its start function. 
        //                     //OR if we want to reset it EVERYTIME we go into this state, then call idleStart() before setting currentState = idleState
    }

    private void FindReferences()
    {
        abilities = GetComponent<PlayerAbilities>();
        cc = GetComponent<CharacterController>();
        pushBox = GetComponent<CapsuleCollider>();
        input = GetComponent<InputManager>();
        //if(!climbChecker)
        //climbChecker = GetComponent<CheckIfClimb>();
        
    }
    private void CreatePlayerStates()
    {
        ///////////////////////////////// Here we create the states. They are not monobehaviour and can therefor not be placed on an object. 
        airState = new AirState(this);
        cutsceneState = new CutsceneState(this);
        boxMoveState = new BoxMoveState(this);
        groundMoveState = new GroundMoveState(this);
        //climbState = new ClimbState(this);
        slopeSlideState = new SlopeSlideState(this);
        beamMoveState = new BeamMoveState(this);
        climbingState = new ClimbingState(this);
        /////////////////////////////////
    }
    void Update()
    {
        grounded = GroundCheck();

        Vector3 move = currentState.UpdateState(input.MovementInput(), cc.velocity); //Here we update the current state. Now in Update, it is called every frame

        if(addToMove)
        {
            move += addMoveVector;
            addMoveVector = Vector3.zero;
            addToMove = false;
        }

        cc.Move(move * Time.deltaTime);

        UpdateAnim();
    }

    public void Respawn()
    {
        transform.position = respawnPos;
        transform.rotation = respawnRot;
    }

    private bool GroundCheck()
    {
        return (Physics.SphereCast(transform.position + groundCastStart, groundCastRadius, Vector3.down, out groundHit, groundCastDistance, blockingLayers));   //Add a layerMask to this at some point.
    }

    public float GroundAngle()
    {
        return Vector3.Angle(Vector3.up, groundHit.normal);
    }
    public bool ReturnGrounded()        //use to check if player is grounded (There's probably a better way to do this??)
    {
        return grounded;
    }
    public RaycastHit ReturnGroundHit()
    {
        return groundHit;
    }
    //public bool IsClimbing()
    //{
    //    return climbState.isClimbing;
    //}
    public IEnumerator SwitchState(IMovement state)                 //Use this to switch states
    {
        yield return new WaitForEndOfFrame();

        Debug.Log("Switching from: " + currentState + ", To: " + state);

        currentState.ExitState();
        currentState = state;
        currentState.Start();
    }

    private void UpdateAnim()
    {
        anim.SetBool("grounded", grounded);
        Vector3 vec = cc.velocity;
        vec.y = 0f;
        anim.SetFloat("velocity", vec.magnitude/groundSettings.speed);
    }
}
