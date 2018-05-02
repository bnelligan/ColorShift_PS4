using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlType { KEYBOARD_MOUSE, GAMEPAD }
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    PlayerMotor motor;
    Player player;
    [SerializeField]
    Transform playerModel;
    public ControlType controlType = ControlType.KEYBOARD_MOUSE;

    string _joystickName;
    float _prevAxisX = 0;


    // Use this for initialization
    private void Awake()
    {
        // Get components
        motor = GetComponent<PlayerMotor>();
        player = GetComponent<Player>();
    }
    void Start () {
        // Output names of connected joysticks
        string[] joystickNames = Input.GetJoystickNames();
        Debug.Log("Joystick Count: " + joystickNames.Length);
        Debug.Log("--- Joystick names ---");
        foreach(string s in joystickNames)
        {
            Debug.Log("_" + s + "_");
        }
        Debug.Log("----------------------");
	}
	
	// Update is called once per frame
	void Update () {
        VerifyControlType();

        HandleInput();
	}

    private void HandleInput()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        motor.xInput = xAxis;

        if(xAxis > 0)
        {
            playerModel.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (xAxis < 0)
        {
            playerModel.rotation = Quaternion.Euler(0, 180, 0);
        }

        // Jump Input Check
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump button pressed");
            motor.TryJump();
        }
        // Color Shift Input
        if(Input.GetButtonDown("ShiftRight"))
        {
            ColorManager.ShiftRight();
        }
        else if(Input.GetButtonDown("ShiftLeft"))
        {
            ColorManager.ShiftLeft();
        }

        // Fire Weapon Input
        if(player.ActiveWeapon.fireMode == FireMode.SEMI)       
        {
            // Semi auto 
            if (Input.GetButtonDown("Fire1"))
            {
                player.ActiveWeapon.Trigger();
            }
        }
        else if(player.ActiveWeapon.fireMode == FireMode.AUTO)  
        {
            // Full auto
            if(Input.GetButton("Fire1"))
            {
                player.ActiveWeapon.Trigger();
            }
        }
        
        // Weapon Aiming
        
        // Exit game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    private void VerifyControlType()
    {
        // Check if a joystick is connected
        string[] joystickNames = Input.GetJoystickNames();
        bool found = false;
        foreach(string jName in joystickNames)
        {
            if(jName != "")
            {
                found = true;
                UseJoystick(jName);
                break;
            }
        }
        // If not found, use keyboard and mouse
        if(!found && controlType == ControlType.GAMEPAD)
        {
            UseKeyboardMouse();
        }
    }

    private void UseJoystick(string joystickName)
    {
        _joystickName = joystickName;
        controlType = ControlType.GAMEPAD;
    }
    private void UseKeyboardMouse()
    {
        controlType = ControlType.KEYBOARD_MOUSE;
    }
}
