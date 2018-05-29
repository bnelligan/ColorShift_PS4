using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable<float>, IKillable {

    public float currentHp;
    public float maxHp;
    public float damage;
    /// <summary>
    /// Tiles per second
    /// </summary>
    public float moveSpeed;
    /// <summary>
    /// Attacks per second
    /// </summary>
    public float attackSpeed;
    public float attackDelay {
        get {
            if (attackSpeed > 0)
                return 1 / attackSpeed;
            else
                return 0f;
        }
    }
    public string Name;
    [HideInInspector]
    public Player player;

    private Rigidbody2D rb;

    private float _lastAttack;
    private Vector3 _moveVector;

	// Use this for initialization
	void Awake () {
        player = GameObject.Find("Player").GetComponent<Player>();
        currentHp = maxHp;

        rb = GetComponent<Rigidbody2D>();
        _lastAttack = Time.time - attackDelay;
	}

    public void Die()
    {
        Destroy(gameObject);
    }

    public void Die(GameObject killer)
    {
        Die();
    }
    
    // Heal to full
    public void Heal()
    {
        currentHp = maxHp;
    }
    // Heal by an amount
    public void Heal(float amount)
    {
        currentHp += amount;
        if(currentHp > maxHp)
        {
            currentHp = maxHp;
        }
    }

    public void TakeDamage(float dmg)
    {
        if (dmg >= 0)
        {
            currentHp -= dmg;
            Debug.Log(Name + " took " + dmg + " damage.");
        }
        else
            Debug.Log("Enemy cannot take " + dmg + " damage.");

        // Death check
        if(currentHp <= 0)
        {
            Die(player.gameObject);
        }
    }
    
    public void DealDamage(Player player)
    {
        player.TakeDamage(damage);
    }
    public virtual void AttackPlayer(Player player)
    {
        Debug.Log("Attacking player: " + player.name);
        
        // Attack cooldown
        if(Time.time >= _lastAttack + attackDelay)
        {
            _lastAttack = Time.time;
            DealDamage(player);
        }
    }
    public virtual void MoveTowards(Vector3 destVec)
    {
        Vector3 vecToTarget = destVec - transform.position;
        _moveVector = vecToTarget.normalized;
        
    }
    public virtual void StopMoving()
    {
        _moveVector = Vector2.zero;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + _moveVector * moveSpeed * Time.deltaTime);
    }
}
