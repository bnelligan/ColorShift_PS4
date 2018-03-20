using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKillable
{
    void Die();
}

public interface IDamageable<T>
{
    void TakeDamage(T damageTaken);
}