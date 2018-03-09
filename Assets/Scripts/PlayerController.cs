using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    // Actually moves the player
    PlayerMotor motor;
    [SerializeField]
    float _killPlane = -15f;

    Vector2 _startPos;
    
	// Use this for initialization
	void Start () {
        _startPos = transform.position;
        motor = GetComponent<PlayerMotor>();
	}
	
	// Update is called once per frame
	void Update () {
        HandleInput();

        // Check for killzone
        if(transform.position.y <= _killPlane)
        {
            Die();
        }
	}

    private void HandleInput()
    {
        float xAxis = Input.GetAxis("Horizontal");
        motor.xInput = xAxis;

        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump button pressed");
            if(motor.Grounded)
            {
                motor.Jump();
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(Input.GetButtonDown("ShiftRight"))
        {
            ColorManager.ShiftRight();
        }
        else if(Input.GetButtonDown("ShiftLeft"))
        {
            ColorManager.ShiftLeft();
        }
    }

    private void Die()
    {
        // Reset transform
        transform.position = _startPos;
        // Reset velocity
        motor.ResetVelocity();
        // Reset the level
        ColorManager.ResetLevel();
        Debug.Log("You Died!");
    }
}
