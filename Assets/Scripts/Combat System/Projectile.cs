using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour {

    public float Damage;
    public float Speed;
    [HideInInspector]
    public GameObject Shooter;
    private Rigidbody2D rb;
    
    
	// Use this for initialization
	void Awake () {
        rb = GetComponent<Rigidbody2D>();
        
	}
    private void Start()
    {
        rb.velocity = transform.right * Speed;
        // Destroy this 3 seconds after firing
        Destroy(gameObject, 3);
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.gameObject != Shooter)
        {
            // Environment Layer
            if(collision.gameObject.layer == 9) 
            {
                Destroy(gameObject);
            }

            IDamageable<float> dmgable = collision.gameObject.GetComponent<IDamageable<float>>();
            if(dmgable != null)
            {
                dmgable.TakeDamage(Damage);
                Destroy(gameObject);
            }
        }
    }
}
