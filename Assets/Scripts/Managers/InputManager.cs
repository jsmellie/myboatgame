using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
  #region Fields & Properties

  ShipController player1Controller = null;
  ShipController player2Controller = null;

  #endregion

  #region Functions

  #region Unity Functions

  void Awake()
  {
    GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
    if (player1 != null && player1.GetComponent<ShipController>() != null)
    {
      player1Controller = player1.GetComponent<ShipController>();
    }
    else
    {
      Debug.LogError("Could find Player 1.  The game cannot start without a player.");
      return;
    }

    GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
    if (player2 != null && player2.GetComponent<ShipController>() != null)
    {
      player2Controller = player2.GetComponent<ShipController>();
    }
  }

  void Update()
  {
    //If we have a player1, check input for it
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

    //If we have a player 2, check input for it
    if (player2Controller != null)
    {
      if (Input.GetKey(KeyCode.L))
      {
        player2Controller.Rotate(-1);
      }
      else if (Input.GetKey(KeyCode.J))
      {
        player2Controller.Rotate(1);
      }

      if (Input.GetKeyDown(KeyCode.I))
      {
        player2Controller.SpeedUp();
      }
      else if (Input.GetKeyDown(KeyCode.K))
      {
        player2Controller.SlowDown();
      }
    }
  }

  #endregion

  #endregion
}
