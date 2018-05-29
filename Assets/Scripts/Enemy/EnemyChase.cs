using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : EnemyState
{
    private GameObject target;

    private void Start()
    {
        target = player.gameObject;
        
    }

    private void Update()
    {
        // Check if player is out of range
        float sqDist = (transform.position - player.transform.position).sqrMagnitude;
        if (sqDist > ai.AggroRange*ai.AggroRange)
        {
            ai.ChangeState(AI_State.Patrol);
            return;
        }
        // Move towards player
        enemy.MoveTowards(target.transform.position);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if(player)
        {
            enemy.AttackPlayer(player);
        }
    }


}
