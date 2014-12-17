using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
  #region Fields & Properties

  public ShipController player1Controller = null;
  public ShipController player2Controller = null;

  #endregion

  #region Functions

  #region Unity Functions

  void Update()
  {
    if (player1Controller != null)
    {
      if (Input.GetKey(KeyCode.D))
      {
        player1Controller.Rotate(-1);
      }
      else if (Input.GetKey(KeyCode.A))
      {
        player1Controller.Rotate(1);
      }

      if (Input.GetKeyDown(KeyCode.W))
      {
        player1Controller.SpeedUp();
      }
      else if (Input.GetKeyDown(KeyCode.S))
      {
        player1Controller.SlowDown();
      }
    }

    if (player2Controller != null)
    {

    }
  }

  #endregion

  #endregion
}
