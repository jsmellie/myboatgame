/* --------------------------
 *
 * ObjectController.cs
 *
 * Description: Base controller class for all controllers.
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
    [RequireComponent(typeof(Rigidbody2D))]
    public class ObjectController : MonoBehaviour
    {
        #region Fields & Properties

        protected float _cachedSpeed = float.NaN;
        public float cachedSpeed
        {
            get { return _cachedSpeed; }
        }

        protected Vector2 _cachedVelocity = Vector2.zero;
        public Vector2 cachedVelocity
        {
            get { return _cachedVelocity; }
        }

        protected Rigidbody2D _body;

        #endregion

        #region Functions

        protected virtual void Awake()
        {
            _body = this.GetComponent<Rigidbody2D>();
        }

        protected virtual void Update()
        {
            _cachedVelocity = _body.velocity;
            _cachedSpeed = _cachedVelocity.magnitude;
        }

        #endregion
    }
}
