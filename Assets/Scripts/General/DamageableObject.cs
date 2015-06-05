/* --------------------------
 *
 * DamageableObject.cs
 *
 * Description: Abstract base class for any object that can take damage including health and armour tracking.
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
    public abstract class DamageableObject : MonoBehaviour
    {
        #region Fields & Properties
        //const

        //public

        //protected

        //private
        [SerializeField] protected float m_MaxHealth;
        [SerializeField] protected float m_StartingHealth;
        [SerializeField] protected float m_Armour;

        protected float m_Health;

        //properties
        public virtual float MaxHealth
        {
            get { return m_MaxHealth; }
        }

        public virtual float StartingHealth
        {
            get { return m_StartingHealth; }
        }

        public virtual float Health
        {
            get { return m_Health; }
        }

        public virtual float Armour
        {
            get { return m_Armour; }
        }
        #endregion

        #region Unity Methods
        protected virtual void Awake()
        {
            m_Health = m_StartingHealth;
        }
        #endregion

        #region Public Methods
        public virtual void DamageTaken(float damage)
        {
            damage = damage - Armour;

            if (damage <= 0)
            {
                return;
            }

            m_Health -= damage;

            if (m_Health <= 0)
            {
                Dead();
            }
        }

        public abstract void Dead();
        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        #endregion
    }
}
