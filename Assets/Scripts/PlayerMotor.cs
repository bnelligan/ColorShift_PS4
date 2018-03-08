using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMotor : MonoBehaviour {

    #region Private Variables
    /// <summary>
    /// Player speed as a positive scalar
    /// </summary>
    [SerializeField]
    float _speed = 10f;
    [SerializeField]
    float _jumpPower = 10f;
    [SerializeField]
    LayerMask rayMask;
    [SerializeField]
    float _fallMultiplier = 1f;
    [SerializeField]
    float _lowJumpMultiplier = 1f;

    // Internal Velocity
    float _xVelocity = 0f;
    

    Rigidbody2D rb;
    #endregion

    #region Public Variables
    public bool Grounded = false;
    /// <summary>
    /// X input axis as float between -1 and 1
    /// </summary>
    [HideInInspector]
    public float xInput;
    #endregion


    #region Unity Callbacks
    // Use this for initialization
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
	}

    // Update for game logic
    private void Update()
    {
        _xVelocity = xInput * _speed;
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

    // Collision Callbacks
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            Grounded = true;
            Platform plat = collision.gameObject.GetComponent<Platform>();
            plat.AttachedPlayer = gameObject;
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            Grounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            Grounded = false;
            Platform plat = collision.gameObject.GetComponent<Platform>();
            plat.AttachedPlayer = null;
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            Grounded = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!Grounded)
        if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("Ground"))
        {
            Grounded = true;
        }
    }
    #endregion


    public void Jump()
    {
        Debug.DrawRay(transform.position, Vector3.down * 1f, Color.red, 1f);
        
        rb.velocity = Vector2.up * _jumpPower;
        Debug.Log("Jumped!");
        
    }

    public void ResetVelocity()
    {
        rb.velocity = Vector2.zero;
    }

    

}
