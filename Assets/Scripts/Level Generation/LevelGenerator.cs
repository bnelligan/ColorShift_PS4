using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { NORTH, EAST, SOUTH, WEST}
public class LevelGenerator : MonoBehaviour {

    public static readonly float TileSize = 1.28f;

    public Vector2 MapBounds;

    // Min and Max distance between map nodes [x and y distances, not diagonal]
    public float xMinDist = 3;
    public float xMaxDist = 10;
    public float dy = 7;

    [SerializeField]
    public List<BasePath> PossiblePathPrefabs;
    [SerializeField]
    public List<MapNode> PossibleNodePrefabs;

    private Vector2 currPos;

    [SerializeField]
    MapNode rootNode;   // Node the player starts at
    MapNode currentNode;

    List<MapNode> _completedNodes;
    List<MapNode> _newNodes;

    private void Awake()
    {
        _completedNodes = new List<MapNode>();
        _newNodes = new List<MapNode>();
        currentNode = rootNode;
    }
    private void Start()
    {
        currentNode = rootNode;
        CreateLevel();
    }

    public void CreateLevel()
    {
        
    }

    private bool CreateNode(MapNode startNode, Gate startGate)
    {
        // Random x and y distance between nodes
        float dx = Random.Range(xMinDist, xMaxDist);
        // Vector2 for distance between nodes
        Vector2 dist = new Vector2(currentNode.Width + dx, dy);

        int iNode = Random.Range(0, PossibleNodePrefabs.Count);

        MapNode destNode = Instantiate( PossibleNodePrefabs[iNode]);
        destNode.WorldPosition = currentNode.WorldPosition + dist;
        return true; // Success
    }

    public bool CheckBounds(Vector2 position)
    {
        return position.x > 0 && position.y > 0 && position.x < MapBounds.x && position.y < MapBounds.y;
    }

   
}

