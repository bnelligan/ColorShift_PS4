﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMotor : MonoBehaviour {

    #region Private Variables
    Player player;
    [SerializeField]
    LayerMask jumpMask;
    [SerializeField]
    float _fallMultiplier = 1f;
    [SerializeField]
    float _lowJumpMultiplier = 1f;

    // Internal Velocity
    float _xVelocity = 0f;
    private bool _grounded = false;

    Rigidbody2D rb;
    #endregion

    #region Public Variables
    /// <summary>
    /// X input axis as a float between -1 and 1
    /// </summary>
    [HideInInspector]
    public float xInput;

    public bool IsGrounded { get { return _grounded; } }
    #endregion


    #region Unity Callbacks
    // Use this for initialization
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
	}

    // Update for game logic
    private void Update()
    {
        _xVelocity = xInput * player.MoveSpeed;
        rb.velocity = new Vector2(_xVelocity, rb.velocity.y);

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    // Collision Logic
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            _grounded = true;
            Platform plat = collision.gameObject.GetComponent<Platform>();
            plat.AttachedPlayer = gameObject;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            _grounded = false;
            Platform plat = collision.gameObject.GetComponent<Platform>();
            plat.AttachedPlayer = null;
        }
    }

    #endregion


    #region Public Methods
    /// <summary>
    /// Try to jump
    /// </summary>
    public void TryJump()
    {
        // Jump check constraints
        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        Vector2 origin = (transform.position + (Vector3)collider.offset + transform.up * -0.5f * collider.size.y);
        float dist = 0.1f;
        Debug.Log("Origin: " + origin);

        //Debug.DrawRay(origin + new Vector2(-.5f * collider.size.x, 0f), Vector3.down * 0.2f, Color.red, 1f);
        //Debug.DrawRay(origin, Vector3.down * 0.2f, Color.red, 1f);
        //Debug.DrawRay(origin + new Vector2(+.5f * collider.size.x, 0f), Vector3.down * 0.2f, Color.red, 1f);
        RaycastHit2D r1Hit = Physics2D.Raycast(origin + new Vector2(-.5f * collider.size.x, 0f), Vector3.down, dist, jumpMask);
        RaycastHit2D r2Hit = Physics2D.Raycast(origin, Vector3.down, dist, jumpMask);
        RaycastHit2D r3Hit = Physics2D.Raycast(origin + new Vector2(+.5f * collider.size.x, 0f), Vector3.down, dist, jumpMask);

        if (r1Hit || r2Hit || r3Hit)
        {
            Jump();
        }

    }
    /// <summary>
    /// Makes the player jump. Does not check if the player can jump first
    /// </summary>
    public void Jump()
    {
        //Debug.DrawRay(transform.position, Vector3.down * 1f, Color.red, 1f);
        
        rb.velocity = Vector2.up * player.JumpPower;
        Debug.Log("Jumped!");
    }

    
    public void ResetVelocity()
    {
        rb.velocity = Vector2.zero;
    }
    #endregion

    

}
