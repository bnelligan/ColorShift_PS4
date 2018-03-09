using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gate : MonoBehaviour {

    public Direction Side = Direction.NULL;
    public bool Full = false;

    MapNode _parentNode;
    MapNode _connectedNode;
    BasePath _path;

    // Position in tiles relative to world and node position
    Vector2 _posWorld;
    Vector2 _posNode;

    #region Access Variables
    public Vector2 WorldPosition
    {
        get { return _posWorld; }
        set {
            _posWorld = value;
            transform.position = _posWorld * LevelGenerator.TileSize;
        }
    }
    public Vector2 NodePosition {
        get { return _posNode; }
        set {
            _posNode = value;
            transform.localPosition = _posNode * LevelGenerator.TileSize; 
        }
    }
    MapNode ParentNode { get { return _parentNode; } }
    MapNode ConnectedNode { get { return _connectedNode; } }
    #endregion

    private void Awake()
    {
        _posNode = transform.localPosition / LevelGenerator.TileSize;
        _posWorld = transform.position / LevelGenerator.TileSize;
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
        if(Side == Direction.NULL)
        {
            if(transform.localPosition.x < 0)
            {
                Side = Direction.LEFT;
            }
            else
            {
                Side = Direction.RIGHT;
            }

            // Check if the gate is near the vertical center of its node
            if(Mathf.Abs(transform.localPosition.x) < 0.15*ParentNode.Width)
            {
                if(transform.localPosition.y < 0)
                {
                    Side = Direction.DOWN;
                }
                else
                {
                    Side = Direction.UP;
                }
            }
        }
        
    }

    public void ConnectGate(Gate other, BasePath path)
    {
        _parentNode.FullGates.Add(this);
        _parentNode.EmptyGates.Remove(this);
        _connectedNode = other.ParentNode;
        _path = path;
        Full = true;
    }

    static public void ConnectGates(Gate gateA, Gate gateB, BasePath path)
    {
        gateA.ConnectGate(gateB, path);
        gateB.ConnectGate(gateA, path);
    }
}
