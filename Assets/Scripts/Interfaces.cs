using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKillable
{
    void Die();

    void Die(GameObject killer);
}

public interface IDamageable<T>
{
    void TakeDamage(T damageTaken);
}