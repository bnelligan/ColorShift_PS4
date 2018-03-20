using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { UP, RIGHT, DOWN, LEFT, NULL}
public class LevelGenerator : MonoBehaviour {

    public static readonly float TileSize = 1.28f;

    public Vector2 MapUpperBounds;
    public Vector2 MapLowerBounds;

    // Min and Max distance between map nodes [x and y distances, not diagonal]
    public int minSeperation = 6;
    /// <summary>
    /// Maximum seperation between level segments in tiles
    /// </summary>
    public int maxSeperation = 15;
    public float maxPlacementAngle;

    [SerializeField]
    public List<Path> PossiblePathPrefabs;
    [SerializeField]
    public List<MapNode> PossibleNodePrefabs;

    [SerializeField]
    MapNode rootNode;   // Node the player starts at
    
    // Node prefabs with gates in each cardinal direction
    [HideInInspector]
    public List<MapNode> LeftNodePrefabs;
    [HideInInspector]
    public List<MapNode> RightNodePrefabs;
    [HideInInspector]
    public List<MapNode> UpNodePrefabs;
    [HideInInspector]
    public List<MapNode> DownNodePrefabs;
   

    private void Awake()
    {
        LeftNodePrefabs = new List<MapNode>();
        RightNodePrefabs = new List<MapNode>();
        UpNodePrefabs = new List<MapNode>();
        DownNodePrefabs = new List<MapNode>();


        foreach (MapNode node in PossibleNodePrefabs)
        {
            // Check and log gate directions
            if (node.HasUpGate)
            {
                UpNodePrefabs.Add(node);
            }  
            if (node.HasRightGate)
            {
                RightNodePrefabs.Add(node);
            }
            if(node.HasDownGate)
            {
                DownNodePrefabs.Add(node);
            }
            if(node.HasLeftGate)
            {
                LeftNodePrefabs.Add(node);
            }
        }
    }
    private void Start()
    {
        rootNode.Init();
        rootNode.Populate();
    }
   
    public MapNode CreateNode(MapNode parentNode, Gate parentGate)
    {
        // Initialize return vars
        MapNode newNode = null;
        Gate newGate = null;
        List<MapNode> prefabList = null;

        // Randomize placement info
        float angle = Random.Range(-maxPlacementAngle, maxPlacementAngle) * Mathf.Deg2Rad;
        Debug.Log("Angle: " + angle);
        float dist = Random.Range(minSeperation, maxSeperation);
        float dx;
        float dy;
        Vector2 dispVec;

        // Calculate displacement vector and choose prefab list
        Direction dir = parentGate.direction;
        switch(dir)
        {
            case Direction.UP:
                dx = Mathf.Sin(angle);
                dy = Mathf.Cos(angle);
                prefabList = DownNodePrefabs;
                break;
            case Direction.RIGHT:
                dx = Mathf.Cos(angle);
                dy = Mathf.Sin(angle);
                prefabList = LeftNodePrefabs;
                break;
            case Direction.DOWN:
                dx = Mathf.Sin(angle);
                dy = -Mathf.Cos(angle); // Flip sign
                prefabList = UpNodePrefabs;
                break;
            case Direction.LEFT:
                dx = -Mathf.Cos(angle); // Flip sign
                dy = Mathf.Sin(angle);
                prefabList = RightNodePrefabs;
                break;
            default:
                dx = 0;
                dy = 0;
                break;
        }
        // Round up to the nearest integer so it locks in place ot the tiles
        dispVec = new Vector2(dx, dy) * dist;
        dispVec.x = Mathf.Ceil(dispVec.x);
        dispVec.y = Mathf.Ceil(dispVec.y);
        Debug.Log("DX: " + dispVec.x);
        Debug.Log("DY: " + dispVec.y);

        // Verify prefab list
        if(prefabList == null)
        {
            Debug.LogWarning("Level generator unable to choose a list of possible nodes.");
            Debug.Log("Parent Node: " + parentNode.name);
            Debug.Log("Parent Gate: " + parentGate.name);
            return null;
        }
        else if(prefabList.Count == 0)
        {
            Debug.LogWarning("No nodes available to connect");
            Debug.Log("Parent Node: " + parentNode.name);
            Debug.Log("Parent Gate: " + parentGate.name);
            return null;
        }

        // Create a random node from the list of valid prefabs
        int inode = Random.Range(0, prefabList.Count);
        //Debug.Log("Random node index: " + inode);
        MapNode nodePrefab = prefabList[inode];

        
        
        Vector3 newPos = parentGate.transform.position + (Vector3)dispVec * TileSize;
        GameObject goNode = Instantiate(nodePrefab.gameObject, newPos, parentNode.transform.rotation);
        
        newNode = goNode.GetComponent<MapNode>();
        // Move the new node by a half it's size
        if (parentGate.direction == Direction.RIGHT)
            newNode.WorldPosition += new Vector2(newNode.Width / 2, 0);
        else if (parentGate.direction == Direction.LEFT)
            newNode.WorldPosition -= new Vector2(newNode.Width / 2, 0);
        else if (parentGate.direction == Direction.UP)
            newNode.WorldPosition += new Vector2(0, newNode.Height / 2);
        else if(parentGate.direction == Direction.DOWN)
            newNode.WorldPosition -= new Vector2(0, newNode.Height / 2);
        newNode.Init();

        Debug.Log("Node position: " + newNode.WorldPosition);
        Debug.Log("Parent node position: " + parentNode.WorldPosition);


        // Check if it's overlapping another section
        if (!newNode.IsValidPosition)
        {
            Debug.LogWarning("Invalid node: " + newNode.name);
            Destroy(newNode.gameObject);
            return null;
        }
        else
        {
            // Tag the node as secure
            newNode.Secure();
        }
           
        // Check that the node is in bounds
        if(!CheckBounds(newNode.WorldPosition))
        {
            Debug.LogWarning("Node out of bounds: " + newNode.name);
            Destroy(newNode.gameObject);
            return null;
        }

        // Create a random path to connect the nodes
        Path pathPrefab = PossiblePathPrefabs[Random.Range(0, PossiblePathPrefabs.Count)];
        Debug.Log("Path chosen: " + pathPrefab.name);
        GameObject goPath = Instantiate(pathPrefab.gameObject, parentGate.transform);
        Path path = goPath.GetComponent<Path>();
        
        // Connect the gates with the path
        newGate = newNode.FindClosestGate(parentGate, 1000, out dist);
        if(path.CreatePath(parentGate, newGate))
        {
            Debug.Log("Path created successfully from: " + path.GateA.WorldPosition + " to " + path.GateB.WorldPosition);
        }
        else
        {
            Debug.LogWarning("Unable to created path.");
        }

        // Return the new node to it's parent node
        return newNode;
    }

    // To be implemented
    public void ConnectNodes(MapNode nodeA, MapNode nodeB, Gate gateA, Gate gateB)
    {

    }

    public bool CheckBounds(Vector2 position)
    {
        return position.x > MapLowerBounds.x && position.y > MapLowerBounds.y && position.x < MapUpperBounds.x && position.y < MapUpperBounds.y;
    }

   
}

