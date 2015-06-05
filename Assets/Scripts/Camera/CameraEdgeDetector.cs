/* --------------------------
 *
 * CameraEdgeDetector.cs
 *
 * Description: Handles all the screen edge detection (boats, bullets, moving objects, etc)
 *
 * Author: Jeremy Smellie
 *
 * Editors:
 *
 * 6/4/2015 - Starvoxel
 *
 * All rights reserved.
 *
 * -------------------------- */

#region Includes
#region Unity Includes
using UnityEngine;
#endregion

#region System Includes
using System.Collections;
#endregion

#region Other Includes

#endregion
#endregion

namespace Starvoxel.ThatBoatGame
{
    public class CameraEdgeDetector : MonoBehaviour
    {
        #region Fields & Properties
        //const

        //public

        //protected

        //private
        private Vector2 m_CamMin;
        private Vector2 m_CamMax;

        [SerializeField] private bool m_ShowLogs = true;

        //properties
        #endregion

        #region Unity Methods
        private void Awake()
        {
            Camera cam = this.GetComponent<Camera>();

            if (cam != null)
            {
                Vector2 halfCamSize;
                halfCamSize.y = cam.orthographicSize;
                halfCamSize.x = halfCamSize.y * cam.aspect;

                Vector2 camPos = (Vector2)cam.transform.position;

                m_CamMin = camPos - halfCamSize;
                m_CamMax = camPos + halfCamSize;

                Log("Cam Min Pos: " + m_CamMin + " | Cam Max Pos: " + m_CamMax);
            }
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            ShipController shipController = collider.GetComponent<ShipController>();

            //This is a ship!  If it's going offscreen we want to move it to the other side
            if (shipController != null)
            {
                //this.Log("Ship going off screen: " + shipController.name);

                Bounds colliderBounds = collider.bounds;

                this.Log("Ship bounds: " + colliderBounds + " | bounds size: " + colliderBounds.size);

                float halfShipSizeX = colliderBounds.size.x / 2;
                float halfShipSizeY = colliderBounds.size.y / 2;

                Vector3 position = collider.GetComponent<Transform>().position;

                //this.Log("Ship position: " + position);

                //Make sure the ship is within the camera bounds
                if (position.x < m_CamMin.x)
                {
                    position.x = m_CamMax.x + halfShipSizeX;
                }
                else if (position.x > m_CamMax.x)
                {
                    position.x = m_CamMin.x - halfShipSizeX;
                }

                if (position.y < m_CamMin.y)
                {
                    position.y = m_CamMax.y + halfShipSizeY;
                }
                else if (position.y > m_CamMax.y)
                {
                    position.y = m_CamMin.y - halfShipSizeY;
                }

                collider.GetComponent<Transform>().position = position;

                return;
            }

            CanonBall canonBall = collider.GetComponent<CanonBall>();

            //A canon ball is going off screen, delete it
            if (canonBall != null)
            {
                Destroy(collider.gameObject);

                return;
            }
        }
        #endregion

        #region Public Methods
        #endregion

        #region Protected Methods
        protected void Log(string message)
        {
            if (m_ShowLogs)
            {
                Debug.Log(message);
            }
        }
        #endregion

        #region Private Methods
        #endregion
    }
}