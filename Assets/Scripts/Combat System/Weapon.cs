﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireMode { SEMI, AUTO }
public enum WeaponType { HAND_GUN, RIFLE, MACHINE_GUN, SWORD, BALLISTIC,   }
public abstract class Weapon : MonoBehaviour {

    #region Private Fields
    [SerializeField]
    protected float _damage;
    [SerializeField]
    protected float _shotDelay;
    protected float _lastShotTime;
    public WeaponType Type;
    public FireMode fireMode;
    [SerializeField]
    protected Transform _offsetPoint;

    public GameObject owner;
    #endregion

    #region Public Properties
    public float Damage { get { return _damage; } }
    public float RoundsPerMinute { get { return 60 / _shotDelay; } }
    public float ShotDelay { get { return _shotDelay; } }
    #endregion


    
    private void Awake()
    {
        _lastShotTime = Time.time - _shotDelay;
    }
    public virtual void Trigger()
    {
        if (Time.time > _lastShotTime + _shotDelay)
        {
            Fire();
            _lastShotTime = Time.time;
        }
    }

    public abstract void Fire();
    /*
	public virtual void Fire()
    {
        switch(_type)
        {
            case WeaponType.RANGED:
                // Clone a bullet at the offset point
                Projectile proj = Instantiate(_projectile, _offsetPoint.position, _offsetPoint.rotation);
                proj.Shooter = owner;
                break;
            case WeaponType.MELEE:
                // No melee weapon logic yet, will be implemented after the ranged system is done
                break;
        }
        
    }
    */
    
}
