using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [Header("Input Mapping")]   //Input mappings
    public string hor = "KBHorizontal";
    public string ver = "KBVertical";
    public string camR = "Mouse X";
    public string camU = "Mouse Y";

    [Header("Input Settings")]
    public float cameraSensitivity = 2f;
    public bool useGamePad = false;

    private float camYMultiplier = 1f;

    public Toggle toggle;

    public Transform cam;

    public StandaloneInputModule module;
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        if (useGamePad)
        {
            MapToGamePad();  
        }
        else
        {
            MapToKeyboard();
        }
        cam = Camera.main.transform;
    }
    public Vector3 MovementInput()      //Returns player directional input on the X and Z axis of a vector3. (Y axis is always zero)
    {
        Vector3 vec = Vector3.zero;
        vec.x = Input.GetAxis(hor);
        vec.z = Input.GetAxis(ver);

        Quaternion q = Quaternion.Euler(0f, cam.rotation.eulerAngles.y, 0f);        //Rotate input direction, based on camera rotation
        vec = q * vec;

        return Vector3.ClampMagnitude(vec, 1f);
    }

    public Vector2 CameraInput()        //Returns player camera input as a Vector2 (x is yaw* input, y is pitch* input) *Yaw is left/right swivel, Pitch is up/down swivel 
    {
        Vector2 camVec = Vector3.zero;
        camVec.x = Input.GetAxis(camR);
        camVec.y = Input.GetAxis(camU) * camYMultiplier;

        return (Vector2.ClampMagnitude(camVec, 1f) * cameraSensitivity);
    }

    public string Buttons()
    {
        if (Input.GetButtonDown("Jump"))
        {
            return "Jump";
        }
        if(Input.GetButtonDown("Pause"))
        {
            return "Pause";
        }
        if(Input.GetButtonDown("Interact"))
        {
            return "Interact";
        }
        if (Input.GetButtonDown("Command")) //Make the pet do something
        {
            return "Command";
        }

        return null;
    }

    public void MapToGamePad()
    {
        module.horizontalAxis = "GPHorizontal";
        module.horizontalAxis = "GPVertical";
        camYMultiplier = -1f;
           useGamePad = true;
        hor = "GPHorizontal";
        ver = "GPVertical";
        camR = "LookRight";
        camU = "LookUp";
    }
    public void MapToKeyboard()
    {
        camYMultiplier = 1f;
        useGamePad = false;
        hor = "KBHorizontal";
        ver = "KBVertical";
        camR = "Mouse X";
        camU = "Mouse Y";
    }

    private void Update()
    {
        Debug.Log("LookUp: " + Input.GetAxis("LookUp"));
        if (PauseMenu.isPaused == true)
        {
            if(toggle.isOn && !useGamePad)
            {
                MapToGamePad();
            }
            if (!toggle.isOn && useGamePad)
            {
                MapToKeyboard();
            }
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
