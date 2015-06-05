using UnityEngine;
using System.Collections;

namespace Starvoxel.ThatBoatGame
{
    public class ShipController : ObjectController, IShipPart
    {
        #region Constants
        protected float[] DEFAULT_INCREMENTS = { 0.5f, 1.0f, 1.5f };
        #endregion

        #region Fields & Properties

        protected Ship _ship;
        public Ship ship
        {
            get { return _ship; }
        }

        [SerializeField, Range(0.0001f, 1.0f)]
        protected float _movementAcceleration = 0.1f;
        protected float movementAcceleration
        {
            get
            {
                return _movementAcceleration / _body.mass;
            }
        }

        [SerializeField, Range(0.0001f, 5.0f)]
        protected float _rotationAcceleration = 0.1f;
        protected float rotationAcceleration
        {
            get
            {
                return _rotationAcceleration / _body.mass;
            }
        }
        [SerializeField]
        protected float _baseSpeed;
        [SerializeField]
        protected float[] _speedIncrements;

        protected int _speedIndex = -1;

        protected float _maxSpeed = float.NaN;

        protected Transform _xform;

        protected Vector2 _camMin;
        protected Vector2 _camMax;

        public Vector2 curVelocity
        {
            get { return _body.velocity; }
        }
        #endregion

        #region Functions

        #region IShipPart Implementation

        //For the time being, we don't use this here.  In the future, we might use it to determine if the ship have crappier control or turning or something
        public void DamageTaken(float damage, ContactPoint2D[] contacts) { }

        #endregion

        #region Unity Functions
        /// <summary>
        /// Called once 
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            //Save a ref to the xform to make it easier to use later
            _xform = this.GetComponent<Transform>();

            if (_xform == null)
            {
                throw new System.NotSupportedException("A ShipController must have a Transform on it!  I don't even know how this is possible...");
            }

            //Add this to the ships list of parts
            Transform curXform = _xform;

            while (curXform != null && _ship == null)
            {
                _ship = curXform.GetComponent<Ship>();

                curXform = curXform.parent;
            }

            if (_ship == null)
            {
                throw new System.NotSupportedException("A " + this.GetType().Name + " must have a Ship on it or somewhere up it's hierarchy.");
            }
            else
            {
                _ship.AddShipPart(this);
            }

            Camera mainCam = Camera.main;

            if (mainCam != null)
            {
                Vector2 halfCamSize;
                halfCamSize.y = mainCam.orthographicSize;
                halfCamSize.x = halfCamSize.y * mainCam.aspect;

                Vector2 camPos = (Vector2)mainCam.transform.position;

                _camMin = camPos - halfCamSize;
                _camMax = camPos + halfCamSize;
            }

            //If the user hasn't set any speed increments, yell at them and set the defaults
            if (_speedIncrements.Length == 0)
            {
                Debug.LogError("You must have some speed increments for a Ship Controller. " + _xform.name);
                _speedIncrements = DEFAULT_INCREMENTS;
            }

            float startingSpeed = 1.0f;

            if (Debug.isDebugBuild)
            {
                startingSpeed = 0.0f;
            }

            //If one of the speed increments is 1.0f, then use it as the default
            for (int i = 0; i < _speedIncrements.Length; ++i)
            {
                if (_speedIncrements[i] == startingSpeed)
                {
                    _speedIndex = i;
                    break;
                }
            }

            //Else just use the middle
            if (_speedIndex < 0)
            {
                _speedIndex = Mathf.RoundToInt(_speedIncrements.Length / 2);
            }

            CalcMaxSpeed();
        }

        /// <summary>
        /// Called every frame to update the ship.
        /// </summary>
        protected override void Update()
        {
            //Make sure everything is set up nicely
            if (_maxSpeed != float.NaN)
            {
                //Calc new current speed
                float currentSpeed = CalcCurrentSpeed();

                //Calc the direction we are moving
                Vector2 direction = _xform.rotation * Vector2.up;

                _body.velocity = direction * currentSpeed;

                _cachedVelocity = _body.velocity;
            }
        }
        #endregion

        #region Speed Modifiers

        /// <summary>
        /// Increment the speed index.  If it's already the highest, won't do anything
        /// </summary>
        public virtual void SpeedUp()
        {
            _speedIndex += 1;

            CalcMaxSpeed();
        }

        /// <summary>
        /// Decreases the speed index.  If it's already the lowest, won't do anything
        /// </summary>
        public virtual void SlowDown()
        {
            _speedIndex -= 1;

            CalcMaxSpeed();
        }

        /// <summary>
        /// Calculates the current speed
        /// </summary>
        protected virtual float CalcCurrentSpeed()
        {
            float currentSpeed = _body.velocity.magnitude;

            if (currentSpeed != _maxSpeed)
            {
                if (currentSpeed > _maxSpeed)
                {
                    currentSpeed -= movementAcceleration;
                }
                else
                {
                    currentSpeed += movementAcceleration;
                }

                if (currentSpeed > _maxSpeed - movementAcceleration && currentSpeed < _maxSpeed + movementAcceleration)
                {
                    currentSpeed = _maxSpeed;
                }
            }

            _cachedSpeed = currentSpeed;
            return currentSpeed;
        }

        [ContextMenu("Calc Max Speed")]
        protected virtual void CalcMaxSpeed()
        {
            if (_speedIndex >= _speedIncrements.Length)
            {
                _speedIndex = _speedIncrements.Length - 1;
            }
            else if (_speedIndex < 0)
            {
                _speedIndex = 0;
            }

            _maxSpeed = _speedIncrements[_speedIndex] * _baseSpeed;
        }
        #endregion

        #region Direction Modifiers

        /// <summary>
        /// Rotates the ship based on the value passed
        /// </summary>
        /// <param name="intensity">The intensity to rotate, -1 being negative rotation and 1 being positive.  Clamped between -1 and 1.</param>
        public void Rotate(float intensity)
        {
            if (_cachedSpeed > 0)
            {
                if (_body != null)
                {
                    _body.angularVelocity = 0;
                }

                //Make sure that intensity is never over 1
                intensity = Mathf.Clamp(intensity, -1, 1);

                float rotAmount = rotationAcceleration * intensity;

                _body.MoveRotation(_xform.rotation.eulerAngles.z + rotAmount);
                //_xform.Rotate(0, 0, rotAmount);
            }
        }

        #endregion

        #endregion
    }
}
