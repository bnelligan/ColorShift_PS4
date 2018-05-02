using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ak47 : Weapon {

    [SerializeField]
    Projectile bullet;
    public override void Fire()
    {
        // Clone a bullet at the offset point
        Projectile proj = Instantiate(bullet, _offsetPoint.position, _offsetPoint.rotation);
        proj.Shooter = owner;
        proj.Damage = _damage;
    }
}
