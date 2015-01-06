using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IShipPart
{
  Ship ship { get; }

  //Any global ship callbacks can be placed here (like collision locations?)
}

/// <summary>
/// This is a intermediate class used so that all parts of the ship can easily talk to one another without making references in an editor.
/// This class will eventually be used to create all the sub part of the ship based on blueprint.
/// </summary>
public class Ship : MonoBehaviour
{
  #region Fields & Properties

  protected List<IShipPart> _shipParts = new List<IShipPart>();

  public ShipController shipController
  {
    get
    {
      ShipController retVal = null;

      for (int i = 0; i < _shipParts.Count; ++i)
      {
        retVal = _shipParts[i] as ShipController;

        if (retVal != null)
        {
          break;
        }
      }

      return retVal;
    }
  }

  public CanonController canonController
  {
    get
    {
      CanonController retVal = null;

      for (int i = 0; i < _shipParts.Count; ++i)
      {
        retVal = _shipParts[i] as CanonController;

        if (retVal != null)
        {
          break;
        }
      }

      return retVal;
    }
  }

  #endregion

  #region Functions

  #region Initialization Functions

  //TODO: Will probably have the function that creates all the ship in here

  #endregion

  #region ShipParts Manipulation

  public void AddShipPart(IShipPart part)
  {
    if (!_shipParts.Contains(part))
    {
      _shipParts.Add(part);
    }
  }

  public IShipPart[] GetShipParts()
  {
    return _shipParts.ToArray();
  }

  #endregion

  #endregion
}
