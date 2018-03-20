using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Path : MonoBehaviour {

    [SerializeField]
    protected Gate _gateA;
    [SerializeField]
    protected Gate _gateB;

    protected COLOR _color = COLOR.NONE;

    /// <summary>
    /// Distance from gate A to B
    /// </summary>
    protected float _length;
    protected Vector2 distVector;

    #region Access Variables
    public Gate GateA { get { return _gateA; } }
    public Gate GateB { get { return _gateB; } }
    #endregion
    #region Size Constraints
    public Vector2 minDist;
    public Vector2 maxDist;
    #endregion
    
    public virtual bool CreatePath(Gate startGate, Gate endGate)
    {
        Debug.Log("Creating path");
        // Assign gates
        _gateA = startGate;
        _gateB = endGate;

        // Set distance vector and path length
        distVector = endGate.WorldPosition - startGate.WorldPosition;
        _length = Mathf.Sqrt(distVector.x * distVector.x + distVector.y * distVector.y);

        // Check that the path can reach
        if(Mathf.Abs(distVector.x) < minDist.x || Mathf.Abs(distVector.y) < minDist.y)
        {
            Debug.LogWarning("Path too short.");
            //return false;
        }
        else if(distVector.x > maxDist.x || distVector.y > maxDist.y)
        {
            Debug.LogWarning("Path too long.");
            //return false;
        }

        // Don't forget to connect the gates in each implementation

        return true;
    }
}
