using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CanonController : MonoBehaviour, IShipPart
{
  #region Fields & Properties

  protected List<Canon> _canonList = new List<Canon>();

  protected Ship _ship;
  public Ship ship
  {
    get { return _ship; }
  }

  #endregion

  #region Functions

  #region IShipPart Implementation

  //For the time being, we don't use this here.  In the future, we might use it to determine if certain canons have broken or something if we g down that route
  public void DamageTaken(float damage, ContactPoint2D[] contacts) { }

  #endregion

  #region List Manipulation

  public void AddCanon(Canon canon)
  {
    if (!_canonList.Contains(canon))
    {
      _canonList.Add(canon);
    }
  }

  public void RemoveCanon(Canon canon)
  {
    _canonList.Remove(canon);
  }

  #endregion

  #region Input

  public void FireLeftSide()
  {
    for (int i = 0; i < _canonList.Count; ++i)
    {
      if (_canonList[i] != null)
      {
        _canonList[i].FireLeft();
      }
    }
  }

  public void FireRightSide()
  {
    for (int i = 0; i < _canonList.Count; ++i)
    {
      if (_canonList[i] != null)
      {
        _canonList[i].FireRight();
      }
    }
  }

  #endregion

  #region Initialization

  void Awake()
  {
    //Add this to the ships list of parts
    Transform curXform = this.GetComponent<Transform>();

    while (curXform != null && _ship == null)
    {
      _ship = curXform.GetComponent<Ship>();

      curXform = curXform.parent;
    }

    if (_ship == null)
    {
      throw new System.NotSupportedException("A " + this.GetType().Name + " must have a Ship on it or somewhere up it's hierarchy.");
    }
    else
    {
      _ship.AddShipPart(this);
    }
  }

  #endregion

  #endregion
}
