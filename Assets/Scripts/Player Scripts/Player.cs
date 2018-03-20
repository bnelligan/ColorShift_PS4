using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable<float>, IKillable {

    #region Private Variables
    // Component Refs
    PlayerMotor motor;
    [SerializeField]
    Weapon activeWeapon;

    Vector3 _startPos;
    [SerializeField]
    float _killPlane = -50f;

    // Stats
    float _currentHp;
    [SerializeField]
    float _maxHpBase = 100f;
    
    [SerializeField]
    float _jumpPower;
    [SerializeField]
    float _moveSpeed;
    #endregion

    #region Public Variables
    // Stat Modifiers
    /// <summary>
    /// Increase to max hp from the base
    /// </summary>
    float _modMaxHp = 0f;
    /// <summary>
    /// Percent increase to move speed.
    /// </summary>
    float _modMoveSpeed = 0f;
    /// <summary>
    /// Percent increase to shot speed.
    /// </summary>
    float _modShotSpeed = 0f;
    #endregion

    #region Public Properties
    public float MaxHP { get { return _maxHpBase + _modMaxHp; } }
    public float CurrentHP { get { return _currentHp; } }
    public float JumpPower { get { return _jumpPower; } }
    public float MoveSpeed { get { return _moveSpeed; } }
    public Weapon ActiveWeapon { get { return activeWeapon; } }
    #endregion


    // Use this for initialization
    private void Awake()
    {
        _currentHp = MaxHP;
        _startPos = transform.position;
        motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update () {
		if(transform.position.y < _killPlane)
        {
            Die();
        }
	}

    public void Die()
    {
        // Reset transform
        transform.position = _startPos;
        // Reset velocity
        motor.ResetVelocity();
        // Reset the level
        ColorManager.ResetLevel();
        Debug.Log("You Died!");
    }
    public void TakeDamage(float damageTaken)
    {
        _currentHp -= damageTaken;
        if(_currentHp < 0)
        {
            Die();
        }
    }
        
}
