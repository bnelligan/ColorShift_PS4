using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : EnemyState
{
    public bool relativeToStart = true;
    public float AggroRange;
    public Vector3[] PatrolPoints;

    private Vector3 _destPoint;
    private int _destIndex = 0;
    private Vector3 _prevPoint;
    
    private static float smallDist = 0.15f * 0.15f;

    protected override void Awake()
    {
        base.Awake();
        if (relativeToStart)
        {
            for (int i = 0; i < PatrolPoints.Length; i++)
            {
                PatrolPoints[i] *= LevelGenerator.TileSize;
                PatrolPoints[i] += transform.position;
            }
        }
        SetDest(_destIndex);
    }
    private void Update()
    {
        // Aggro check
        float sqDist = (transform.position - player.transform.position).sqrMagnitude;
        if (sqDist <= AggroRange*AggroRange)
        {
            ai.ChangeState(AI_State.Chase);
            return;
        }

        MoveToDest();
    }

    private bool ReachedDest()
    {
        float sqDist = (transform.position - _destPoint).sqrMagnitude;
        return sqDist <= smallDist;
    }

    private void MoveToDest()
    {
        enemy.MoveTowards(_destPoint);
        if(ReachedDest())
        {
            NextPoint();
        }
    }
    private void NextPoint()
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
            _prevPoint = _destPoint;
            _destPoint = PatrolPoints[index];
            _destIndex = index;
        }
    }
}
