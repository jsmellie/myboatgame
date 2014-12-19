using UnityEngine;
using System.Collections;

public abstract class DamageableObject : MonoBehaviour
{
  [SerializeField]
  protected float _maxHealth;

  [SerializeField]
  protected float _maxArmour;

  protected float _health;
  public float health
  {
    get
    {
      return _health;
    }
  }

  protected float _armour;
  public float armour
  {
    get
    {
      return _armour;
    }
    set
    {
      if (value > _maxArmour)
      {
        _armour = _maxArmour;
      }
      else if (value < 0.0f)
      {
        _armour = 0.0f;
      }
      else
      {
        _armour = value;
      }
    }
  }

  public void DamageTaken(float damage)
  {
    damage = damage * (1 - _armour);

    if (damage < 0)
    {
      return;
    }

    _health -= damage;

    if (_health < 0)
    {
      //TODO: Dead
    }
  }

  public abstract void Dead();
}
