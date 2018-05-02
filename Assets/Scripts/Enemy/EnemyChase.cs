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
