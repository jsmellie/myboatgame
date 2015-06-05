using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Starvoxel.ThatBoatGame
{
    public interface IShipPart
    {
        Ship ship { get; }

        //Any global ship callbacks can be placed here (like collision locations?)

        /// <summary>
        /// The ship has taken damage of some sort
        /// </summary>
        /// <param name="damage">Amount of damage taken</param>
        /// <param name="contacts">Contact points between the ship and the colliding object</param>
        void DamageTaken(float damage, ContactPoint2D[] contacts);
    }

    /// <summary>
    /// This is a intermediate class used so that all parts of the ship can easily talk to one another without making references in an editor.
    /// This class will eventually be used to create all the sub part of the ship based on blueprint.
    /// </summary>
    public class Ship : MonoBehaviour
    {
        #region Constants

        const float FRONTAL_HIT_COS_VAL = 0.5f;
        const float GLANCING_HIT_COS_VAL = 0.5f;

        const float FRONTAL_HIT_PERCENTAGE = 0.25f;
        const float GLANCING_HIT_PERCENTAGE = 0.25f;

        #endregion

        #region Fields & Properties

#if UNITY_EDITOR
        float _lastSpeed = float.NaN;
#endif

        [SerializeField]
        protected float _collisionCooldown = 1.0f;
        protected float _lastCollisionTime = float.NaN;

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

        public ShipHealth shipHealth
        {
            get
            {
                ShipHealth retVal = null;

                for (int i = 0; i < _shipParts.Count; ++i)
                {
                    retVal = _shipParts[i] as ShipHealth;

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

        protected virtual void Update()
        {
            if (float.IsNaN(_lastCollisionTime) == false)
            {
                if (_lastCollisionTime + _collisionCooldown < Time.time)
                {
                    _lastCollisionTime = float.NaN;
                }
            }

#if UNITY_EDITOR
            _lastSpeed = this.GetComponent<Rigidbody2D>().velocity.magnitude;
#endif
        }

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

        #region Collision

        void OnCollisionEnter2D(Collision2D coll)
        {
            CanonBall canonBall = coll.gameObject.GetComponent<CanonBall>();

            if (canonBall != null && canonBall.friendlyTag != this.tag)
            {
                for (int i = 0; i < _shipParts.Count; ++i)
                {
                    _shipParts[i].DamageTaken(canonBall.damage, coll.contacts);
                }
            }
            //If it's been enough time since out last collision, calc damage
            else if (float.IsNaN(_lastCollisionTime))
            {
                Vector2 avgContactPoint = Vector2.zero;

                for (int i = 0; i < coll.contacts.Length; ++i)
                {
                    avgContactPoint += coll.contacts[i].point;
                }

                avgContactPoint /= coll.contacts.Length;

                Vector2 otherVelocity = Vector2.zero;
                float otherMass = 1;

                ObjectController otherController = coll.collider.GetComponent<ObjectController>();

                Rigidbody2D otherBody = coll.rigidbody;

                if (otherController != null && otherBody != null)
                {
                    otherVelocity = otherController.cachedVelocity;
                    otherMass = otherBody.mass;
                }
                else if (otherBody != null)
                {
                    otherVelocity = otherBody.velocity;
                    otherMass = otherBody.mass;
                }

                Rigidbody2D thisBody = this.GetComponent<Rigidbody2D>();

                float thisMass = thisBody.mass;
                Vector2 thisVelocity = shipController.cachedVelocity;

                if (thisBody == null)
                {
                    throw new System.NotSupportedException("All ships must have rigidbodies!");
                }

                Vector2 direction = thisVelocity.normalized;

                Vector2 curPos = (Vector2)this.GetComponent<Transform>().position;

                Vector2 dirToContact = (avgContactPoint - curPos).normalized;

                float frontalAngle = Vector2.Dot(direction, dirToContact);

                float frontalMod = frontalAngle > FRONTAL_HIT_COS_VAL ? FRONTAL_HIT_PERCENTAGE : 1.0f;

                float glancingMod = 1.0f;

                if (otherVelocity != Vector2.zero)
                {
                    Vector2 otherDir = otherVelocity.normalized;

                    float glancingAngle = Mathf.Abs(Vector2.Dot(direction, otherDir));

                    glancingMod = glancingAngle > GLANCING_HIT_COS_VAL ? GLANCING_HIT_PERCENTAGE : 1.0f;
                }

                /* Damage formula:
                 * The damage formula is simplistic for the time being.  
                 * It's based off:  - the momentum of both objects
                 *                  - the angle between the avg contact point and the position of this object
                 *                  - the angle between the directions of both objects (if a object doesn't have a speed, it's treated as a perpendicular hit)
                 *                  
                 * The formula is: (thisMomentum - otherMomentum)'s magnitude * frontal angle modifier * glancing angle modifier
                 * */
                Vector2 thisMomentum = thisVelocity * thisMass;
                Vector2 otherMomentum = otherVelocity * otherMass;

                float damage = (thisMomentum - otherMomentum).magnitude * frontalMod * glancingMod;

                Debug.Log(this.name + "'s Damage Formula: (" + thisMomentum + " - " + otherMomentum + ").magnitude * " + frontalMod + " * " + glancingMod + " = " + damage);

                for (int i = 0; i < _shipParts.Count; ++i)
                {
                    _shipParts[i].DamageTaken(damage, coll.contacts);
                }

                _lastCollisionTime = Time.time;
            }

        }

        #endregion

        public void SetHullSprite(Sprite hull)
        {
            GetComponent<SpriteRenderer>().sprite = hull;
        }
        #endregion
    }
}
