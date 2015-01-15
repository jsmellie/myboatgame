using UnityEngine;
using System.Collections;

public class FourPlayerSpawner : BaseSpawner
{
  #region Fields & Properties

  [Header("Player Spawn Info")]
  [SerializeField]
  protected PlayerShipSpawnInfo player1SpawnInfo;
  [SerializeField]
  protected PlayerShipSpawnInfo player2SpawnInfo;
  [SerializeField]
  protected PlayerShipSpawnInfo player3SpawnInfo;
  [SerializeField]
  protected PlayerShipSpawnInfo player4SpawnInfo;

#if UNITY_EDITOR
  [Header("Editor Fields")]
  [SerializeField]
  protected Color player1GizmoColor = Color.green;
  [SerializeField]
  protected Color player2GizmoColor = Color.red;
  [SerializeField]
  protected Color player3GizmoColor = Color.blue;
  [SerializeField]
  protected Color player4GizmoColor = Color.yellow;
#endif

  #endregion

  #region Functions

  public override bool StartSpawner()
  {
    //TODO: Spawn all the players here based on the positions specified
    return true;
  }

  public override bool StopSpawner()
  {
    throw new System.NotImplementedException();
  }

#if UNITY_EDITOR
  protected void OnDrawGizmosSelected()
  {
    //Draw player1 gizmo
    DrawPlayerGizmo(player1GizmoColor, player1SpawnInfo.spawnLocation, player1SpawnInfo.rotation);
    DrawPlayerGizmo(player2GizmoColor, player2SpawnInfo.spawnLocation, player2SpawnInfo.rotation);
    DrawPlayerGizmo(player3GizmoColor, player3SpawnInfo.spawnLocation, player3SpawnInfo.rotation);
    DrawPlayerGizmo(player4GizmoColor, player4SpawnInfo.spawnLocation, player4SpawnInfo.rotation);
  }

  protected void DrawPlayerGizmo(Color gizmoColor, Vector2 position, float rotation)
  {
    //Set gizmo color
    Gizmos.color = gizmoColor;

    //Draw cube for position
    Gizmos.DrawWireCube((Vector3)position, Vector3.one / 4);
    
    //Draw arrow for rotation
    //TODO : Use Gizmos.DrawLine to make the arrow
  }
#endif


  #endregion
}
