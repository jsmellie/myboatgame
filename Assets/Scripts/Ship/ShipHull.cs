using UnityEngine;
using System.Collections;

public class ShipHull : DamageableObject, IShipPart
{
  #region Fields & Properties

  protected Ship _ship;
  public Ship ship
  {
    get { return _ship; }
  }

  #endregion

  #region Functions

  public virtual void Awake()
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

  public override void Dead()
  {
    Debug.Log(_ship.name + " has died!");
  }



  #endregion
}
