using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CanonController : MonoBehaviour
{
  #region Fields & Properties

  protected List<Canon> _canonList = new List<Canon>();

  #endregion

  #region Functions
  #region List Manipulation
  public void AddCanon(Canon canon)
  {
    if (!_canonList.Contains(canon))
    {
      _canonList.Add(canon);
    }
  }
  #endregion

  #region Input
  public void FireLeftSide()
  {
    //TODO: Go through the list of canons and fire all the left side ones
  }

  public void FireRightSide()
  {
    //TODO: Go through the list of canons and fire all the left side ones
  }
  #endregion
  #endregion
}
