using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gate : MonoBehaviour {

    public Direction direction = Direction.NULL;
    public bool Full = false;

    MapNode _parentNode;
    MapNode _connectedNode;
    Path _path;

    #region Access Variables
    public Vector2 WorldPosition
    {
        get {
            return transform.position / LevelGenerator.TileSize; }
        set {
            transform.position = value * LevelGenerator.TileSize; }
    }
    public Vector2 LocalPosition
    {
        get { 
            return transform.localPosition / LevelGenerator.TileSize; }
        set {
            transform.localPosition = value * LevelGenerator.TileSize; }
    }
   
    MapNode ParentNode { get { return _parentNode; } }
    MapNode ConnectedNode { get { return _connectedNode; } }
    #endregion

    private void Awake()
    {
        _parentNode = GetComponentInParent<MapNode>();
        

        // Initialize, could be called externally instead
        Init();
    }
    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Init()
    {
        Full = false;
        // Make a guess at the direction if not set manually
        if(direction == Direction.NULL)
        {
            if(transform.localPosition.x < 0)
            {
                direction = Direction.LEFT;
            }
            else
            {
                direction = Direction.RIGHT;
            }

            // Check if the gate is near the vertical center of its node
            if(Mathf.Abs(transform.localPosition.x) < 0.15*ParentNode.Width)
            {
                if(transform.localPosition.y < 0)
                {
                    direction = Direction.DOWN;
                }
                else
                {
                    direction = Direction.UP;
                }
            }
        }
        
    }

    public void ConnectGate(Gate other, Path path)
    {
        _parentNode.FullGates.Add(this);
        _parentNode.EmptyGates.Remove(this);
        _connectedNode = other.ParentNode;
        _path = path;
        Full = true;
    }

    static public void ConnectGates(Gate gateA, Gate gateB, Path path)
    {
        gateA.ConnectGate(gateB, path);
        gateB.ConnectGate(gateA, path);
    }
}
