using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : EnemyState {

    private void Update()
    {
        // Aggro check
        float sqDist = (transform.position - player.transform.position).sqrMagnitude;
        if (sqDist <= ai.AggroRange * ai.AggroRange)
        {
            ai.ChangeState(AI_State.Chase);
            return;
        }
    }
}
