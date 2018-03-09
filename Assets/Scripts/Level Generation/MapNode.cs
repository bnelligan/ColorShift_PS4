using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour {

    #region Public Variables
    public MapNode ParentNode;
    public List<MapNode> ChildNodes;
    public MapNode HitNode;

    public float Width;
    public float Height;
    public bool Secured = false;
    public bool HasUpGate = false;
    public bool HasRightGate = false;
    public bool HasDownGate = false;
    public bool HasLeftGate = false;
    #endregion

    #region Variables
    private bool _validPosition = true;

    private Vector2 _worldPosition;
    private List<Gate> _gates;
    private List<Gate> _emptyGates;
    private List<Gate> _fullGates;
    private List<MapNode> _nodesToPopulate;
    private Collider2D _nodeCollider;
    LevelGenerator levelGenerator;

    public List<Gate> upGates;
    public List<Gate> rightGates;
    public List<Gate> downGates;
    public List<Gate> leftGates;
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
    public List<Gate> FullGates { get { return _fullGates; } }
    #endregion


    #region Unity Callbacks
    private void Awake()
    {
        // Set World Position
        _worldPosition = transform.position / LevelGenerator.TileSize;

        // Get components
        _nodeCollider = GetComponent<Collider2D>();

        // Initialize lists
        ChildNodes = new List<MapNode>();
        _nodesToPopulate = new List<MapNode>();
        _gates = new List<Gate>();
        _emptyGates = new List<Gate>();
        _fullGates = new List<Gate>();
        if(upGates == null) upGates = new List<Gate>();
        if(rightGates == null) rightGates = new List<Gate>();
        if(downGates == null) downGates = new List<Gate>();
        if(leftGates == null) leftGates = new List<Gate>();

        // Find level generator 
        GameObject lGen = GameObject.Find("LevelGenerator");
        levelGenerator = lGen.GetComponent<LevelGenerator>();
    }
    private void Update()
    {
        if(_nodesToPopulate.Count > 0)
        {
            foreach(MapNode n in _nodesToPopulate)
            {
                n.Populate();
            }
            _nodesToPopulate.Clear();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if node is secured. This means it was the first one there.
        if (!Secured)
        {
            HitNode = collision.gameObject.GetComponentInParent<MapNode>();
            _validPosition = false;
        }
    }
    #endregion


    #region Public Methods
    public void Init()
    {
        foreach(Gate g in GetComponentsInChildren<Gate>())
        {
            RegisterGate(g);
        }
    }
    /// <summary>
    /// Find the gate on this node closest to the source gate.
    /// </summary>
    /// <param name="source">Starting gate to measure from</param>
    /// <param name="maxDist">Maximum allowed distance in tiles</param>
    /// <returns></returns>
    public Gate FindClosestGate(Gate source, float maxDist, out float seperation)
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
            seperation = distance;
        else
            seperation = 0;

        return closest;
    }
    
    public void RegisterGate(Gate gate)
    {
        // Add to master and empty list
        _gates.Add(gate);
        if(gate.Full == false)
        {
            _emptyGates.Add(gate);
        }

        // Add to directional list
        switch(gate.Side)
        {
            case Direction.UP:
                HasUpGate = true;
                upGates.Add(gate);
                break;
            case Direction.RIGHT:
                HasRightGate = true;
                rightGates.Add(gate);
                break;
            case Direction.LEFT:
                HasLeftGate = true;
                leftGates.Add(gate);
                break;
            case Direction.DOWN:
                HasDownGate = true;
                downGates.Add(gate);
                break;
        }
    }
    /// <summary>
    /// Create child nodes at each gate. Uses functions from the level generator.
    /// </summary>
    public void Populate()
    {
        Debug.Log("Populating: " + name);
        List<Gate> egates = _emptyGates;
        for(int i = 0; i < egates.Count; i++)
        {
            Gate g = egates[i];
            if (g.Full == false)
            {
                MapNode childNode = levelGenerator.CreateNode(this, g);
                if (childNode)
                {
                    Debug.Log("Node Created: " + childNode.name);
                    ChildNodes.Add(childNode);
                    // Continue populating
                    _nodesToPopulate.Add(childNode);
                }
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
