using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock : Weapon {

    [SerializeField]
    Projectile bullet;
    public override void Fire()
    {
        // Clone a bullet at the offset point
        Projectile proj = Instantiate(bullet, _offsetPoint.position, _offsetPoint.rotation);
        proj.Shooter = owner;
        proj.Damage = _damage;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
