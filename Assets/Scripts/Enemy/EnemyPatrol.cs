using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : EnemyState
{
    public bool relativeToStart = true;
    public Vector3[] PatrolPoints;

    private Vector3 _destPoint;
    private int _destIndex = 0;
    private Vector3 _prevPoint;
    
    // Compared to squared magnitude
    private static float sqrSmDist = 0.15f * 0.15f;

    protected override void Awake()
    {
        base.Awake();
        // Check that the patrol points are not too close together
        for(int i = PatrolPoints.Length-1; i > 0; i--)
        {
            float d = (PatrolPoints[i] - PatrolPoints[i - 1]).sqrMagnitude;
            if(d < sqrSmDist)
            {
                Debug.LogWarning("Patrol points too close.");
                ai.ChangeState(AI_State.Idle);
                return;
            }
        }
        if (relativeToStart)
        {
            for (int i = 0; i < PatrolPoints.Length; i++)
            {
                // Scale the patrol points by the tile size
                PatrolPoints[i] = transform.position + PatrolPoints[i] * LevelGenerator.TileSize;
            }
        }
        SetDest(_destIndex);
    }
    private void Update()
    {
        // Aggro check
        float sqDist = (transform.position - player.transform.position).sqrMagnitude;
        if (sqDist <= ai.AggroRange*ai.AggroRange)
        {
            // Chase player
            ai.ChangeState(AI_State.Chase);
            return;
        }
        // Move to current destination point
        MoveToDest();
    }

    private bool ReachedDest()
    {
        float sqDist = (transform.position - _destPoint).sqrMagnitude;
        return sqDist <= sqrSmDist;
    }

    private void MoveToDest()
    {
        enemy.MoveTowards(_destPoint);
        if(ReachedDest())
        {
            NextPoint();
        }
    }
    public void NextPoint()
    {
        _destIndex++;
        if(_destIndex >= PatrolPoints.Length || _destIndex < 0)
        {
            _destIndex = 0;
        }
        SetDest(_destIndex);
    }

    public void SetDest(int index)
    {
        if(index < PatrolPoints.Length)
        {
            _destPoint = PatrolPoints[index];
            _destIndex = index;
        }
    }
}
