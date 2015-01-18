using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(FourPlayerSpawner))]
public class FourPlayerSpawnerInspector : Editor 
{
  bool[] playerFoldouts = new bool[4];
  FourPlayerSpawner _playerSpawner = null;

  FourPlayerSpawner playerSpawner
  {
    get
    {
      if (_playerSpawner == null)
      {
        Init();
      }

      return _playerSpawner;
    }
  }

  protected virtual void Init()
  {
    _playerSpawner = target as FourPlayerSpawner;

    playerFoldouts[0] = false;
    playerFoldouts[1] = false;
    playerFoldouts[2] = false;
    playerFoldouts[3] = false;
  }

  void OnSceneGUI()
  {
    if (GUI.changed)
    {
      EditorUtility.SetDirty(target);
    }

    //Draw player1 handles
    playerSpawner.player1SpawnInfo.rotation = Handles.RotationHandle(Quaternion.Euler(0, 0, playerSpawner.player1SpawnInfo.rotation), (Vector3)playerSpawner.player1SpawnInfo.spawnLocation).eulerAngles.z;
  }



  public override void  OnInspectorGUI()
  {
    GUILayout.BeginHorizontal();
    {
      GUILayout.FlexibleSpace();
      GUILayout.Label("PLAYER INFO", EditorStyles.boldLabel);
      GUILayout.FlexibleSpace();
    }
    GUILayout.EndHorizontal();

    int[] playerAmount = new int[]{1,2,3,4};
    string[] playerAmountStr = new string[] { "1 player", "2 player", "3 player", "4 player" };

    playerSpawner.playerCount = EditorGUILayout.IntPopup("Player Amount", playerSpawner.playerCount, playerAmountStr, playerAmount, EditorStyles.popup);

    GUI.enabled = playerSpawner.playerCount >= 1;
    DrawPlayerInfo(ref playerSpawner.player1SpawnInfo, 1);

    GUI.enabled = playerSpawner.playerCount >= 2;
    DrawPlayerInfo(ref playerSpawner.player2SpawnInfo, 2);

    GUI.enabled = playerSpawner.playerCount >= 3;
    DrawPlayerInfo(ref playerSpawner.player3SpawnInfo, 3);

    GUI.enabled = playerSpawner.playerCount >= 4;
    DrawPlayerInfo(ref playerSpawner.player4SpawnInfo, 4);

    GUI.enabled = true;

    EditorGUILayout.Space();

    GUILayout.BeginHorizontal();
    {
      GUILayout.FlexibleSpace();
      GUILayout.Label("EDITOR INFO", EditorStyles.boldLabel);
      GUILayout.FlexibleSpace();
    }
    GUILayout.EndHorizontal();

    playerSpawner.drawGizmo = EditorGUILayout.Toggle("Draw Gizmo", playerSpawner.drawGizmo);
    playerSpawner.player1GizmoColor = EditorGUILayout.ColorField("Player 1 Gizmo Color", playerSpawner.player1GizmoColor);
    playerSpawner.player2GizmoColor = EditorGUILayout.ColorField("Player 2 Gizmo Color", playerSpawner.player2GizmoColor);
    playerSpawner.player3GizmoColor = EditorGUILayout.ColorField("Player 3 Gizmo Color", playerSpawner.player3GizmoColor);
    playerSpawner.player4GizmoColor = EditorGUILayout.ColorField("Player 4 Gizmo Color", playerSpawner.player4GizmoColor);
    /*
     * Inspector look
     * 
     * PLAYER INFO
     * 
     * Number of Players (Drop down 1-4)
     * 
     * For each player fold out with following info:
     * Spawn location (X,Y)
     * Rotation
     * TEMP: Ship Prefab
     * TEMP: Hull Image
     * */
    GUILayout.Space(30);

    DrawDefaultInspector();
  }

  protected void DrawPlayerInfo(ref PlayerShipSpawnInfo playerInfo, int playerIndex)
  {
    playerFoldouts[playerIndex - 1] = EditorGUILayout.Foldout(playerFoldouts[playerIndex - 1], string.Format("Player {0} Info", playerIndex.ToString()));

    if (playerFoldouts[playerIndex - 1])
    {
      playerInfo.spawnLocation = EditorGUILayout.Vector2Field("Spawn Location", playerInfo.spawnLocation);
      playerInfo.rotation = EditorGUILayout.FloatField("Rotation", playerInfo.rotation);
      playerInfo.shipPrefab = EditorGUILayout.ObjectField(playerInfo.shipPrefab, typeof(Ship)) as Ship;
      playerInfo.hullImage = EditorGUILayout.ObjectField(playerInfo.hullImage, typeof(Sprite)) as Sprite;
    }
  }
}
