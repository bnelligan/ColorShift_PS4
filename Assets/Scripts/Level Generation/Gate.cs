using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gate : MonoBehaviour {

    MapNode _parentNode;
    MapNode _connectedNode;
    BasePath _path;
    Direction _side;

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
    }
    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void ConnectGate(Gate other, BasePath path)
    {
        _parentNode.EmptyGates.Remove(this);
        _connectedNode = other.ParentNode;
        _path = path;
    }

    static public void ConnectGates(Gate gateA, Gate gateB, BasePath path)
    {
        gateA.ConnectGate(gateB, path);
        gateB.ConnectGate(gateA, path);
    }
}
