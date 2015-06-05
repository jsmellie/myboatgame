using UnityEngine;
using System.Collections;

namespace Starvoxel.ThatBoatGame
{
    [System.Serializable]
    public struct PlayerShipSpawnInfo
    {
        #region Fields & Properties

        public Vector2 spawnLocation;
        public float rotation;
        public Ship shipPrefab; //TODO: Probably resource load this
        public Sprite hullImage; //TODO: Probably resource load this

        #endregion

        #region Constructors

        public PlayerShipSpawnInfo(Vector2 spawnLocation, float rotation, Ship shipPrefab, Sprite hullImage)
        {
            this.spawnLocation = spawnLocation;
            this.rotation = rotation;
            this.shipPrefab = shipPrefab;
            this.hullImage = hullImage;
        }

        public Ship Spawn()
        {
            Vector3 pos = (Vector3)spawnLocation;

            Ship spawnedShip = GameObject.Instantiate(shipPrefab, pos, Quaternion.Euler(new Vector3(0, 0, rotation))) as Ship;

            return spawnedShip;
        }

        #endregion
    }

    abstract public class BaseSpawner : MonoBehaviour
    {

        #region Fields & Properties

        protected bool _isSpawning = false;
        public virtual bool isSpawning
        {
            get { return _isSpawning; }
        }

        #endregion

        #region Functions

        public abstract bool StartSpawner();

        public abstract bool StopSpawner();

        #endregion
    }
}
