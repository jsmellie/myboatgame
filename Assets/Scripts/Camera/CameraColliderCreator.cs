/* --------------------------
 *
 * CameraColliderCreator.cs
 *
 * Description: Simple script that creates 2 colliders for screen edge detection
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
    public class CameraColliderCreator : MonoBehaviour
    {
		#region Fields & Properties
		//const
	
		//public
	
		//protected
	
		//private
        [SerializeField] private float m_ColliderSizeDif = 1.0f;

#if UNITY_EDITOR
        [Header("Editor Variables")]
        [SerializeField] private bool m_ShoulRrenderColliderLines = true;

        [SerializeField] private Color m_FullscreenColliderColor = Color.blue;
        [SerializeField] private Color m_FailsafeColliderColor = Color.green;
#endif
	
		//properties
		#endregion

        #region Unity Methods
        void Awake()
        {
            Camera camera = this.GetComponent<Camera>();

            if (camera != null)
            {
                Vector2 camSize = new Vector2(camera.orthographicSize * camera.aspect, camera.orthographicSize);

                BoxCollider2D fullscreenCollider = gameObject.AddComponent<BoxCollider2D>();
                fullscreenCollider.isTrigger = true;
                fullscreenCollider.size = camSize * 2;
                fullscreenCollider.offset = Vector2.zero;

                BoxCollider2D failsafeCollider = gameObject.AddComponent<BoxCollider2D>();
                failsafeCollider.isTrigger = true;
                failsafeCollider.offset = fullscreenCollider.offset;
                failsafeCollider.size = fullscreenCollider.size + new Vector2(m_ColliderSizeDif, m_ColliderSizeDif);

#if UNITY_EDITOR
                if (m_ShoulRrenderColliderLines)
                {
                    float camZ = camera.transform.position.z;

                    Vector2 fullscreenColliderMin = fullscreenCollider.bounds.min;
                    Vector2 fullscreenColliderMax = fullscreenCollider.bounds.max;
                    Vector2 failsafeColliderMin = failsafeCollider.bounds.min;
                    Vector2 failsafeColliderMax = failsafeCollider.bounds.max;

                    //Rendering a line of the colliders so that you can see it without having to have it selected.
                    Vector3 botLeft = new Vector3(fullscreenColliderMin.x, fullscreenColliderMin.y, camZ);
                    Vector3 botRight = new Vector3(fullscreenColliderMax.x, fullscreenColliderMin.y, camZ);
                    Vector3 topLeft = new Vector3(fullscreenColliderMin.x, fullscreenColliderMax.y, camZ);
                    Vector3 topRight = new Vector3(fullscreenColliderMax.x, fullscreenColliderMax.y, camZ);

                    Debug.DrawLine(botLeft, botRight, m_FullscreenColliderColor, float.PositiveInfinity);
                    Debug.DrawLine(botRight, topRight, m_FullscreenColliderColor, float.PositiveInfinity);
                    Debug.DrawLine(topRight, topLeft, m_FullscreenColliderColor, float.PositiveInfinity);
                    Debug.DrawLine(topLeft, botLeft, m_FullscreenColliderColor, float.PositiveInfinity);

                    botLeft = new Vector3(failsafeColliderMin.x, failsafeColliderMin.y, camZ);
                    botRight = new Vector3(failsafeColliderMax.x, failsafeColliderMin.y, camZ);
                    topLeft = new Vector3(failsafeColliderMin.x, failsafeColliderMax.y, camZ);
                    topRight = new Vector3(failsafeColliderMax.x, failsafeColliderMax.y, camZ);

                    Debug.DrawLine(botLeft, botRight, m_FailsafeColliderColor, float.PositiveInfinity);
                    Debug.DrawLine(botRight, topRight, m_FailsafeColliderColor, float.PositiveInfinity);
                    Debug.DrawLine(topRight, topLeft, m_FailsafeColliderColor, float.PositiveInfinity);
                    Debug.DrawLine(topLeft, botLeft, m_FailsafeColliderColor, float.PositiveInfinity);
                }
#endif
            }
        }
        #endregion

        #region Public Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        #endregion
    }
}
