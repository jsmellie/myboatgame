using UnityEngine;
using System.Collections;

public class CanonBall : MonoBehaviour 
{
  const string RESTART_TRIGGER = "Restart";
  const string FIRE_TRIGGER = "Fired";
  const float SIZE_MODIFIER = 0.5f;
  const float MAX_LIFESPAN = 5.0f;

  protected float _size = float.NaN;
  protected float _firedTime = float.NaN;
  protected float _chargeModifier = float.NaN;

  [SerializeField]
  protected float _baseDamage = 5.0f;
  public float damage
  {
    get
    {
      return _baseDamage * _size * (1 + _chargeModifier);
    }
  }

  [SerializeField]
  protected float _cooldownModifier = 1.0f;
  public float cooldownModifier
  {
    get
    {
      return _cooldownModifier;
    }
  }

  protected string _friendlyTag = null;
  public string friendlyTag
  {
    get
    {
      return _friendlyTag;
    }
    set
    {
      if (string.IsNullOrEmpty(_friendlyTag))
      {
        _friendlyTag = value;
      }
    }
  }

  protected Animator _animator;

  protected virtual void Awake()
  {
    _animator = GetComponent<Animator>();
  }

  protected virtual void Update()
  {
    if (Time.time > _firedTime + MAX_LIFESPAN)
    {
      Destroy(this.gameObject);
    }
  }

  public virtual void Fired(float chargeModifier)
  {
    if (_animator != null)
    {
      _animator.SetTrigger(FIRE_TRIGGER);
    }

    _chargeModifier = chargeModifier;
    _firedTime = Time.time;
  }

  public void SetSize(Canon.Size size)
  {
    _size = (float)size * SIZE_MODIFIER;
  }

  public void Reset()
  {
    if (_animator != null)
    {
      _animator.SetTrigger(RESTART_TRIGGER);
    }

    _friendlyTag = null;
    _firedTime = float.NaN;
    _chargeModifier = float.NaN;
  }

  void OnTriggerEnter2D(Collider2D collidingObj)
  {
    if (collidingObj.tag != _friendlyTag && collidingObj.tag != "MainCamera")
    {
      Debug.Log("Canon Ball collided with " + collidingObj.name);
    }
  }

  public void Splashed()
  {
    //TODO: Spawn the splash animation/particle effect
    Destroy(this.gameObject);
  }


}
