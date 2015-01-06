using UnityEngine;
using System.Collections;

public abstract class DamageableObject : MonoBehaviour
{
  [SerializeField]
  protected float _maxHealth;

  protected float _health;
  public virtual float health
  {
    get
    {
      return _health;
    }
  }

  [SerializeField]
  protected float _armour;
  public virtual float armour
  {
    get
    {
      return _armour;
    }
  }

  public virtual void DamageTaken(float damage)
  {
    damage = damage - armour;

    if (damage <= 0)
    {
      return;
    }

    _health -= damage;

    if (_health <= 0)
    {
      Dead();
    }
  }

  public abstract void Dead();
}
