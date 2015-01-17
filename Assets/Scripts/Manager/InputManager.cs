using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
  #region Fields & Properties

  ShipController _player1Controller = null;
  CanonController _player1CanonController = null;

  ShipController _player2Controller = null;
  CanonController _player2CanonController = null;

  Collider2D _lastSlotCollider = null;

  [SerializeField]
  LayerMask _canonSlotMask = -1;

  #endregion

  #region Functions

  #region Unity Functions

  void Awake()
  {
    GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
    if (player1 != null && player1.GetComponent<ShipController>() != null)
    {
      _player1Controller = player1.GetComponent<ShipController>();
      _player1CanonController = player1.GetComponentInChildren<CanonController>();
    }
    else
    {
      Debug.LogError("Could find Player 1.  The game cannot start without a player.");
      return;
    }

    GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
    if (player2 != null && player2.GetComponent<ShipController>() != null)
    {
      _player2Controller = player2.GetComponent<ShipController>();
      _player2CanonController = player2.GetComponentInChildren<CanonController>();
    }
  }

  void Update()
  {
    //If we have a player1, check input for it
    if (_player1Controller != null)
    {
      if (Input.GetKey(KeyCode.D))
      {
        _player1Controller.Rotate(-1);
      }
      else if (Input.GetKey(KeyCode.A))
      {
        _player1Controller.Rotate(1);
      }

      if (Input.GetKeyDown(KeyCode.W))
      {
        _player1Controller.SpeedUp();
      }
      else if (Input.GetKeyDown(KeyCode.S))
      {
        _player1Controller.SlowDown();
      }
    }

    //If we have a player 2, check input for it
    if (_player2Controller != null)
    {
      if (Input.GetKey(KeyCode.L))
      {
        _player2Controller.Rotate(-1);
      }
      else if (Input.GetKey(KeyCode.J))
      {
        _player2Controller.Rotate(1);
      }

      if (Input.GetKeyDown(KeyCode.I))
      {
        _player2Controller.SpeedUp();
      }
      else if (Input.GetKeyDown(KeyCode.K))
      {
        _player2Controller.SlowDown();
      }
    }

    //If we have a player 1 canon controller, check input for it
    if (_player1CanonController != null)
    {
      if (Input.GetKeyDown(KeyCode.C))
      {
        _player1CanonController.FireLeftSide();
      }
      if (Input.GetKeyDown(KeyCode.V))
      {
        _player1CanonController.FireRightSide();
      }
    }

    //If we have a player 2 canon controller, check input for it
    if (_player2CanonController != null)
    {
      if (Input.GetKeyDown(KeyCode.Period))
      {
        _player2CanonController.FireLeftSide();
      }
      if (Input.GetKeyDown(KeyCode.Slash))
      {
        _player2CanonController.FireRightSide();
      }
    }
    /*
    if (Input.GetMouseButton(0))
    {
      RaycastHit2D testHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100, _canonSlotMask.value);

      if (testHit.collider != null)
      {
        if (_lastSlotCollider != testHit.collider)
        {
          if (_lastSlotCollider != null)
          {
            ResetLastSlotCollider();
          }
          _lastSlotCollider = testHit.collider;
          SpriteRenderer renderer = _lastSlotCollider.GetComponent<SpriteRenderer>();
          if (renderer != null)
          {
            renderer.color = Color.blue;
          }
        }
      }
      else if (_lastSlotCollider != null)
      {
        ResetLastSlotCollider();
      }
    }
    else if (Input.GetMouseButtonUp(0) && _lastSlotCollider != null)
    {
      //TODO: If we are currently trying to place a canon, spawn one on the collider
      ResetLastSlotCollider();
    }*/
  }

  void ResetLastSlotCollider()
  {
    SpriteRenderer oldRenderer = _lastSlotCollider.GetComponent<SpriteRenderer>();
    if (oldRenderer != null)
    {
      oldRenderer.color = Color.white;
    }
    _lastSlotCollider = null;
  }

  #endregion

  #endregion
}
