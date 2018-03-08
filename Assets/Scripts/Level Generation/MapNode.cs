using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour {

    #region Public Variables
    public float Width;
    public float Height;
    public bool Secured = false;
    public bool HasNorthGate = false;
    public bool HasEastGate = false;
    public bool HasSouthGate = false;
    public bool HasWestGate = false;
    #endregion

    #region Private Variables
    private bool _validPosition = true;
    private Vector2 _worldPosition;
    private List<Gate> _gates;
    private List<Gate> _emptyGates;
    private Collider2D _nodeCollider;
    #endregion


    #region Access Variables
    public bool IsValidPosition { get { return _validPosition; } }
    // Position relative to the root node in tiles
    public Vector2 WorldPosition {
        get {
            return _worldPosition;
        }
        set {
            _worldPosition = value;
            transform.position = _worldPosition * LevelGenerator.TileSize;
        }
    }
    public List<Gate> Gates { get { return _gates; } }
    public List<Gate> EmptyGates { get { return _emptyGates; } }
    #endregion


    #region Unity Callbacks
    private void Awake()
    {
        // Get all gate scripts from children
        Gate[] gts = GetComponentsInChildren<Gate>();
        _gates = new List<Gate>(gts);
        _emptyGates = _gates;

        // Get node collider
        _nodeCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("MapNode"))
        {
            _validPosition = false;
        }
    }
    #endregion


    #region Public Methods
    /// <summary>
    /// Find the gate on this node closest to the source gate.
    /// </summary>
    /// <param name="source">Starting gate to measure from</param>
    /// <param name="maxDist">Maximum allowed distance in tiles</param>
    /// <returns></returns>
    public Gate FindClosestGate(Gate source, float maxDist, out float dist)
    {
        Gate closest = null;
        float distance = maxDist;
        foreach(Gate g in EmptyGates)
        {
            // Calc distance between gates
            Vector2 vDist = g.WorldPosition - source.WorldPosition;
            float d = Mathf.Sqrt(vDist.x * vDist.x + vDist.y * vDist.y);
            // Check if this gate is the closest
            if(d < distance && d > 0)
            {
                closest = g;
                distance = d;
            }
        }
        if (distance != maxDist)
            dist = distance;
        else
            dist = 0;

        return closest;
    }
    public void ConnectNode(MapNode otherNode)
    {
        Gate myGate;
        Gate otherGate;
        float distance = 0;

        // Brute force search right now. Probably will make a better algorithm for finding closest gates
        foreach (Gate g in EmptyGates)
        {
            float dist;
            // Find closest gate on other node
            Gate closeGate = otherNode.FindClosestGate(g, 1000, out dist);

            // Check if these gates are closest
            if ((dist < distance || distance == 0) && closeGate != null)
            {
                distance = dist;
                myGate = g;
                otherGate = closeGate;
            }
        }
    }

    #endregion

    #region Event Callbacks
    public void OnGameStart()
    {
        // Disable node collider when the game starts
        _nodeCollider.enabled = false;
    }
    #endregion

}
