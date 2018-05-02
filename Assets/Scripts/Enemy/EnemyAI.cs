using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAI : MonoBehaviour {
    
    EnemyState state;
    Enemy enemy;

    private void Awake()
    {
        state = GetComponent<EnemyState>();
        enemy = GetComponent<Enemy>();
    }
    // Update is called once per frame
    void Update () {
		
	}

    public void ChangeState(AI_State aistate)
    {
        EnemyState oldState = state;


        if(aistate == AI_State.Patrol)
        {
            Destroy(oldState);
            state = gameObject.AddComponent<EnemyPatrol>();
        }
        else if(aistate == AI_State.Chase)
        {
            Destroy(oldState);
            state = gameObject.AddComponent<EnemyChase>();
        }
    }
}

