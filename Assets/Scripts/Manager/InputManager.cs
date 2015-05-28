/* --------------------------
 *
 * InputManager.cs
 *
 * Description: 
 *  Input listener for player objects
 *
 * Author: Jeremy Smellie
 *
 * Editors:
 *
 * 1/17/2015 - Starvoxel
 *
 * All rights reserved.
 * 
 * TODO : Most of this class will be changed...  
 * I'm thinking of doing some kind of way of saving/loading keybindings and making ship/cannon controllers take care of that
 * Also I think that it'll just have a list of PlayerInput which would have a ship and cannon controller inside of it
 *
 * -------------------------- */

#region Includes
#region Unity Includes
using UnityEngine;
#endregion

#region System Includes
using System.Collections;
#endregion
#endregion

public class InputManager : MonoBehaviour
{
    #region Fields & Properties
    //const

    //public

    //protected

    //private
    private ShipController m_Player1Controller = null;
    private CanonController m_Player1CanonController = null;

    private ShipController m_Player2Controller = null;
    private CanonController m_Player2CanonController = null;

    private Collider2D m_LastSlotCollider = null;

    //properties
    #endregion

    #region Unity Methods
    void Awake()
    {
        GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
        if (player1 != null && player1.GetComponent<ShipController>() != null)
        {
            m_Player1Controller = player1.GetComponent<ShipController>();
            m_Player1CanonController = player1.GetComponentInChildren<CanonController>();
        }
        else
        {
            Debug.LogError("Could find Player 1.  The game cannot start without a player.");
            return;
        }

        GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
        if (player2 != null && player2.GetComponent<ShipController>() != null)
        {
            m_Player2Controller = player2.GetComponent<ShipController>();
            m_Player2CanonController = player2.GetComponentInChildren<CanonController>();
        }
    }

    void Update()
    {
        //If we have a player1, check input for it
        if (m_Player1Controller != null)
        {
            if (Input.GetKey(KeyCode.D))
            {
                m_Player1Controller.Rotate(-1);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                m_Player1Controller.Rotate(1);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                m_Player1Controller.SpeedUp();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                m_Player1Controller.SlowDown();
            }
        }

        //If we have a player 2, check input for it
        if (m_Player2Controller != null)
        {
            if (Input.GetKey(KeyCode.L))
            {
                m_Player2Controller.Rotate(-1);
            }
            else if (Input.GetKey(KeyCode.J))
            {
                m_Player2Controller.Rotate(1);
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                m_Player2Controller.SpeedUp();
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                m_Player2Controller.SlowDown();
            }
        }

        //If we have a player 1 canon controller, check input for it
        if (m_Player1CanonController != null)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                m_Player1CanonController.FireLeftSide();
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                m_Player1CanonController.FireRightSide();
            }
        }

        //If we have a player 2 canon controller, check input for it
        if (m_Player2CanonController != null)
        {
            if (Input.GetKeyDown(KeyCode.Period))
            {
                m_Player2CanonController.FireLeftSide();
            }
            if (Input.GetKeyDown(KeyCode.Slash))
            {
                m_Player2CanonController.FireRightSide();
            }
        }
        /*
        if (Input.GetMouseButton(0))
        {
          RaycastHit2D testHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100, m_CanonSlotMask.value);

          if (testHit.collider != null)
          {
            if (m_LastSlotCollider != testHit.collider)
            {
              if (m_LastSlotCollider != null)
              {
                ResetLastSlotCollider();
              }
              m_LastSlotCollider = testHit.collider;
              SpriteRenderer renderer = m_LastSlotCollider.GetComponent<SpriteRenderer>();
              if (renderer != null)
              {
                renderer.color = Color.blue;
              }
            }
          }
          else if (m_LastSlotCollider != null)
          {
            ResetLastSlotCollider();
          }
        }
        else if (Input.GetMouseButtonUp(0) && m_LastSlotCollider != null)
        {
          //TODO: If we are currently trying to place a canon, spawn one on the collider
          ResetLastSlotCollider();
        }*/
    }
    #endregion

    #region Public Methods
    #endregion

    #region Protected Methods
    #endregion

    #region Private Methods
    void ResetLastSlotCollider()
    {
        SpriteRenderer oldRenderer = m_LastSlotCollider.GetComponent<SpriteRenderer>();
        if (oldRenderer != null)
        {
            oldRenderer.color = Color.white;
        }
        m_LastSlotCollider = null;
    }
    #endregion
}
