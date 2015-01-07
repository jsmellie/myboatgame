using UnityEngine;
using System.Collections;

public abstract class DamageableObject : MonoBehaviour
{
  [SerializeField]
  protected float _maxHealth;
  public virtual float maxHealth
  {
    get
    {
      return _maxHealth;
    }
  }

  [SerializeField]
  protected float _startingHealth;
  public virtual float startingHealth
  {
    get
    {
      return _startingHealth;
    }
  }

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

  protected virtual void Awake()
  {
    _health = _startingHealth;
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
