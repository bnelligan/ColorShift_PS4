using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Enemy : MonoBehaviour {

    public Slider hpbar;

    Enemy enemy;
    
    private void Start()
    {
        enemy = GetComponent<Enemy>();
        hpbar.maxValue = enemy.maxHp;
        hpbar.value = enemy.maxHp;
    }

    private void Update()
    {
        // Update healthbar
        hpbar.value = enemy.currentHp;
    }

}
