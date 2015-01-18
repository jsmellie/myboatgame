using UnityEngine;
using System.Collections;

public class FourPlayerSpawner : BaseSpawner
{
  #region Constants

  #if UNITY_EDITOR
  static readonly Vector2 ARROW_HEAD_POINT = Vector2.up;
  static readonly Vector2 LEFT_POINT = new Vector2(-0.25f, 0.75f);
  static readonly Vector2 RIGHT_POINT = new Vector2(0.25f, 0.75f);
  #endif

  #endregion

  #region Fields & Properties

  public int playerCount = 2;

  public PlayerShipSpawnInfo player1SpawnInfo;
  public PlayerShipSpawnInfo player2SpawnInfo;
  public PlayerShipSpawnInfo player3SpawnInfo;
  public PlayerShipSpawnInfo player4SpawnInfo;

#if UNITY_EDITOR
  public bool drawGizmo = true;
  public Color player1GizmoColor = Color.green;
  public Color player2GizmoColor = Color.red;
  public Color player3GizmoColor = Color.blue;
  public Color player4GizmoColor = Color.yellow;
#endif

  #endregion

  #region Functions

  protected virtual void Awake()
  {
    StartSpawner();
  }

  public override bool StartSpawner()
  {
    //TODO: Spawn all the players here based on the positions specified
    Ship player1 = player1SpawnInfo.Spawn();
    player1.tag = Tags.Player1;
    player1.name = Tags.Player1.ToString();
    player1.SetHullSprite(player1SpawnInfo.hullImage);

    Ship player2 = player2SpawnInfo.Spawn();
    player2.tag = Tags.Player2;
    player2.name = Tags.Player2.ToString();
    player2.SetHullSprite(player2SpawnInfo.hullImage);
    return true;
  }

  public override bool StopSpawner()
  {
    throw new System.NotImplementedException();
  }

#if UNITY_EDITOR
  protected void OnDrawGizmos()
  {
    if (drawGizmo)
    {
      //Draw player1 gizmo
      if (playerCount >= 1)
      {
        DrawPlayerGizmo(player1GizmoColor, player1SpawnInfo.spawnLocation, player1SpawnInfo.rotation);
      }
      if (playerCount >= 2)
      {
        DrawPlayerGizmo(player2GizmoColor, player2SpawnInfo.spawnLocation, player2SpawnInfo.rotation);
      }
      if (playerCount >= 3)
      {
        DrawPlayerGizmo(player3GizmoColor, player3SpawnInfo.spawnLocation, player3SpawnInfo.rotation);
      }
      if (playerCount >= 4)
      {
        DrawPlayerGizmo(player4GizmoColor, player4SpawnInfo.spawnLocation, player4SpawnInfo.rotation);
      }
    }
  }

  protected void DrawPlayerGizmo(Color gizmoColor, Vector2 position, float rotation)
  {
    //Set gizmo color
    Gizmos.color = gizmoColor;

    //Draw cube for position
    Gizmos.DrawWireCube((Vector3)position, Vector3.one / 4);
    
    //Draw arrow for rotation
    Quaternion quartRotation = Quaternion.Euler(new Vector3(0, 0, rotation));

    Vector2 headPoint = position + (Vector2)((quartRotation * ARROW_HEAD_POINT) / 2);
    Vector2 leftPoint = position + (Vector2)((quartRotation * LEFT_POINT) / 2);
    Vector2 rightPoint = position + (Vector2)((quartRotation * RIGHT_POINT) / 2);

    Gizmos.DrawLine((Vector3)position, (Vector3)headPoint);
    Gizmos.DrawLine((Vector3)headPoint, (Vector3)leftPoint);
    Gizmos.DrawLine((Vector3)headPoint, (Vector3)rightPoint);
  }
#endif


  #endregion
}
