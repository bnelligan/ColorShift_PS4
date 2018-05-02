using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AI_State { Idle, Chase, Flee, Patrol }
public abstract class EnemyState : MonoBehaviour
{
    [HideInInspector]
    public Enemy enemy;
    [HideInInspector]
    public EnemyAI ai;
    [HideInInspector]
    public UI_Enemy ui;
    [HideInInspector]
    public Player player;

    protected virtual void Awake()
    {
        enemy = GetComponent<Enemy>();
        ai = GetComponent<EnemyAI>();
        ui = GetComponent<UI_Enemy>();
        player = FindObjectOfType<Player>();
    }
}
