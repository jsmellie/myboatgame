using UnityEngine;
using System.Collections;

public class CanonBall : MonoBehaviour 
{
  const string RESTART_TRIGGER = "Restart";
  const string FIRE_TRIGGER = "Fired";

  [SerializeField]
  protected float _damage = 5.0f;

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

    Fired();
  }

  public void Fired()
  {
    if (_animator != null)
    {
      _animator.SetTrigger(FIRE_TRIGGER);
    }
  }

  public void Reset()
  {
    if (_animator != null)
    {
      _animator.SetTrigger(RESTART_TRIGGER);
    }

    _friendlyTag = null;
  }

  void OnTriggerEnter2D(Collider2D collidingObj)
  {
    if (collidingObj.tag != _friendlyTag)
    {
      
    }
    //TODO: Make what ever it hit take dmg
  }

  public void Splashed()
  {
    //TODO: Spawn the splash animation/particle effect
    Destroy(this.gameObject);
  }


}
