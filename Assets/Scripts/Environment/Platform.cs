using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Platform : MonoBehaviour {
    
    /// <summary>
    /// True if this is a moving platform
    /// </summary>
    [SerializeField]
    private bool _isMoving = false;

    /// <summary>
    /// Destination relative to the current position in tiles
    /// </summary>
    [SerializeField]
    private Vector2 _relativeDest = new Vector3(0f,0f);

    /// <summary>
    /// Time it takes to move from the start position to the destination and back
    /// </summary>
    [SerializeField]
    private float Period = 2f;
    // Amplitude of oscelation
    private Vector2 Amp;
    // Angular frequency = 2*pi/period
    private float AFreq;
    // Phase constant [cos(pi/2) == 0]
    private float Phi = Mathf.PI;
    // Start and destination positions
    private Vector2 _startPos;
    private Vector2 _destPos;
    private Vector2 _midpoint;
    private Vector3 _lastPos;

    [HideInInspector]
    public GameObject AttachedPlayer;
    public Vector2 Velocity { get { return -AFreq * Amp * Mathf.Sin(AFreq * Time.time); } }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        // Set start position and destination relative to start position
        _relativeDest *= LevelGenerator.TileSize;
        _startPos = transform.localPosition;
        _destPos = _startPos + _relativeDest;
        _midpoint = (_startPos + _destPos) / 2;

        // Set angular frequency and amplitude
        AFreq = 2 * Mathf.PI / Period;
        Amp = _relativeDest / 2;
        _lastPos = transform.localPosition;

        // Set initial position
        if(_isMoving)
            UpdatePosition();
    }
    // Fixed update for physics
    void FixedUpdate () {
		if(_isMoving)
        {
            // Update this platform's position
            UpdatePosition();

            // Move the player by the amount it's position changed by
            if(AttachedPlayer)
            {
                //Debug.Log("Player is attached!");
                if(AttachedPlayer.GetComponent<Rigidbody2D>().velocity.y <= 0)
                    AttachedPlayer.transform.position += (transform.localPosition - _lastPos);
            }  
        }
	}

    private void UpdatePosition()
    {
        _lastPos = transform.localPosition;
        transform.localPosition = _midpoint + Amp * Mathf.Cos(AFreq * Time.time - Phi);
    }
    
    public void SetMovementPattern(bool isMoving, float period, Vector2 relativeDest)
    {
        transform.localPosition = _startPos;
        _relativeDest = relativeDest;
        _isMoving = isMoving;
        Period = period;
        Init();
    }
    
}
