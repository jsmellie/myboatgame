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

  public void RemoveCanon(Canon canon)
  {
    _canonList.Remove(canon);
  }
  #endregion

  #region Input
  public void FireLeftSide()
  {
    FireSide(Canon.DirectionType.Left);
  }

  public void FireRightSide()
  {
    FireSide(Canon.DirectionType.Right);
  }

  public void FireSide(Canon.DirectionType direction)
  {
    Canon curCanon = null;

    for (int i = 0; i < _canonList.Count; ++i)
    {
      curCanon = _canonList[i];

      if (curCanon.direction == Canon.DirectionType.Right)
      {
        curCanon.Fire();
      }
    }
  }
  #endregion
  #endregion
}
