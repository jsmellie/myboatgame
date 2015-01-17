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
  public bool drawGizmo = true;
  [Space(10)]
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
      DrawPlayerGizmo(player1GizmoColor, player1SpawnInfo.spawnLocation, player1SpawnInfo.rotation);
      DrawPlayerGizmo(player2GizmoColor, player2SpawnInfo.spawnLocation, player2SpawnInfo.rotation);
      DrawPlayerGizmo(player3GizmoColor, player3SpawnInfo.spawnLocation, player3SpawnInfo.rotation);
      DrawPlayerGizmo(player4GizmoColor, player4SpawnInfo.spawnLocation, player4SpawnInfo.rotation);
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
