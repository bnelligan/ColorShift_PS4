using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePath : MonoBehaviour {

    protected Gate _gateA;
    protected Gate _gateB;

    protected COLOR _color = COLOR.NONE;

    /// <summary>
    /// Distance from gate A to B
    /// </summary>
    protected float _length;
    protected Vector2 distVector;

    #region Size Constraints
    public Vector2 minDist;
    public Vector2 maxDist;
    #endregion
    
    public virtual bool CreatePath(Gate startGate, Gate endGate)
    {
        // Assign gates
        _gateA = startGate;
        _gateB = endGate;

        // Set distance vector and path length
        distVector = endGate.WorldPosition - startGate.WorldPosition;
        _length = Mathf.Sqrt(distVector.x * distVector.x + distVector.y * distVector.y);

        // Check that the path can reach
        if(distVector.x < minDist.x || distVector.y < minDist.y)
        {
            Debug.LogWarning("Path too short.");
            return false;
        }
        else if(distVector.x > maxDist.x || distVector.y > maxDist.y)
        {
            Debug.LogWarning("Path too long.");
            return false;
        }

        // Don't forget to connect the gates in each implementation

        return true;
    }
}
