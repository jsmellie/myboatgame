/* --------------------------
 *
 * PlayerSpawnerInspector.cs
 *
 * Description: Custom inspector for PlayerSpawner.  Draws handles for easy configuration.
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
using UnityEditor;
#endregion

#region System Includes
using System.Collections;
#endregion

#region Other Includes

#endregion
#endregion

namespace Starvoxel.ThatBoatGame
{
    [CustomEditor(typeof(PlayerSpawner))]
    public class PlayerSpawnerInspector : Editor
    {
        #region Fields & Properties
        //const

        //public

        //protected

        //private
        Tool m_ActiveTool = Tool.None;
        Tool m_LocalTool = Tool.Move;
        bool[] m_PlayerFoldouts = new bool[4];
        PlayerSpawner m_PlayerSpawner = null;

        //properties
        PlayerSpawner PlayerSpawner
        {
            get
            {
                if (m_PlayerSpawner == null)
                {
                    Init();
                }

                return m_PlayerSpawner;
            }
        }
        #endregion

        

        #region Unity Methods
        private void OnEnable()
        {
            SetTool();
        }

        private void OnDisable()
        {
            Tools.current = m_ActiveTool;
        }
        #endregion

        #region Unity Editor Methods
        void OnSceneGUI()
        {
            SetTool();

            if (PlayerSpawner.drawGizmo)
            {
                if (m_LocalTool == Tool.Rotate)
                {
                    if (PlayerSpawner.playerCount >= 1)
                    {
                        PlayerSpawner.player1SpawnInfo.rotation = Handles.RotationHandle(Quaternion.Euler(0, 0, PlayerSpawner.player1SpawnInfo.rotation), (Vector3)PlayerSpawner.player1SpawnInfo.spawnLocation).eulerAngles.z;
                    }
                    if (PlayerSpawner.playerCount >= 2)
                    {
                        PlayerSpawner.player2SpawnInfo.rotation = Handles.RotationHandle(Quaternion.Euler(0, 0, PlayerSpawner.player2SpawnInfo.rotation), (Vector3)PlayerSpawner.player2SpawnInfo.spawnLocation).eulerAngles.z;
                    }
                    if (PlayerSpawner.playerCount >= 3)
                    {
                        PlayerSpawner.player3SpawnInfo.rotation = Handles.RotationHandle(Quaternion.Euler(0, 0, PlayerSpawner.player3SpawnInfo.rotation), (Vector3)PlayerSpawner.player3SpawnInfo.spawnLocation).eulerAngles.z;
                    }
                    if (PlayerSpawner.playerCount >= 4)
                    {
                        PlayerSpawner.player4SpawnInfo.rotation = Handles.RotationHandle(Quaternion.Euler(0, 0, PlayerSpawner.player4SpawnInfo.rotation), (Vector3)PlayerSpawner.player4SpawnInfo.spawnLocation).eulerAngles.z;
                    }
                }
                else
                {
                    if (PlayerSpawner.playerCount >= 1)
                    {
                        PlayerSpawner.player1SpawnInfo.spawnLocation = (Vector2)Handles.PositionHandle((Vector3)PlayerSpawner.player1SpawnInfo.spawnLocation, Quaternion.identity);
                    }
                    if (PlayerSpawner.playerCount >= 2)
                    {
                        PlayerSpawner.player2SpawnInfo.spawnLocation = (Vector2)Handles.PositionHandle((Vector3)PlayerSpawner.player2SpawnInfo.spawnLocation, Quaternion.identity);
                    }
                    if (PlayerSpawner.playerCount >= 3)
                    {
                        PlayerSpawner.player3SpawnInfo.spawnLocation = (Vector2)Handles.PositionHandle((Vector3)PlayerSpawner.player3SpawnInfo.spawnLocation, Quaternion.identity);
                    }
                    if (PlayerSpawner.playerCount >= 4)
                    {
                        PlayerSpawner.player4SpawnInfo.spawnLocation = (Vector2)Handles.PositionHandle((Vector3)PlayerSpawner.player4SpawnInfo.spawnLocation, Quaternion.identity);
                    }
                }
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(this);
            }
        }

        public override void OnInspectorGUI()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("PLAYER INFO", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

            int[] playerAmount = new int[] { 1, 2, 3, 4 };
            string[] playerAmountStr = new string[] { "1 player", "2 player", "3 player", "4 player" };

            PlayerSpawner.playerCount = EditorGUILayout.IntPopup("Player Amount", PlayerSpawner.playerCount, playerAmountStr, playerAmount, EditorStyles.popup);

            GUI.enabled = PlayerSpawner.playerCount >= 1;
            DrawPlayerInfo(ref PlayerSpawner.player1SpawnInfo, 1);

            GUI.enabled = PlayerSpawner.playerCount >= 2;
            DrawPlayerInfo(ref PlayerSpawner.player2SpawnInfo, 2);

            GUI.enabled = PlayerSpawner.playerCount >= 3;
            DrawPlayerInfo(ref PlayerSpawner.player3SpawnInfo, 3);

            GUI.enabled = PlayerSpawner.playerCount >= 4;
            DrawPlayerInfo(ref PlayerSpawner.player4SpawnInfo, 4);

            GUI.enabled = true;

            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("EDITOR INFO", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

            PlayerSpawner.drawGizmo = EditorGUILayout.Toggle("Draw Gizmo", PlayerSpawner.drawGizmo);
            PlayerSpawner.player1GizmoColor = EditorGUILayout.ColorField("Player 1 Gizmo Color", PlayerSpawner.player1GizmoColor);
            PlayerSpawner.player2GizmoColor = EditorGUILayout.ColorField("Player 2 Gizmo Color", PlayerSpawner.player2GizmoColor);
            PlayerSpawner.player3GizmoColor = EditorGUILayout.ColorField("Player 3 Gizmo Color", PlayerSpawner.player3GizmoColor);
            PlayerSpawner.player4GizmoColor = EditorGUILayout.ColorField("Player 4 Gizmo Color", PlayerSpawner.player4GizmoColor);
        }
        #endregion

        #region Public Methods
        #endregion

        #region Protected Methods
        protected virtual void Init()
        {
            m_PlayerSpawner = target as PlayerSpawner;

            m_PlayerFoldouts[0] = false;
            m_PlayerFoldouts[1] = false;
            m_PlayerFoldouts[2] = false;
            m_PlayerFoldouts[3] = false;
        }

        protected void SetTool()
        {
            m_ActiveTool = Tools.current;

            switch (m_ActiveTool)
            {
                case Tool.Move:
                case Tool.Rotate:
                    m_LocalTool = m_ActiveTool;
                    break;
            }

            Tools.current = Tool.None;
        }

        protected void DrawPlayerInfo(ref PlayerShipSpawnInfo playerInfo, int playerIndex)
        {
            m_PlayerFoldouts[playerIndex - 1] = EditorGUILayout.Foldout(m_PlayerFoldouts[playerIndex - 1], string.Format("Player {0} Info", playerIndex.ToString()));

            if (m_PlayerFoldouts[playerIndex - 1])
            {
                playerInfo.spawnLocation = EditorGUILayout.Vector2Field("Spawn Location", playerInfo.spawnLocation);
                playerInfo.rotation = EditorGUILayout.FloatField("Rotation", playerInfo.rotation);
                playerInfo.shipPrefab = EditorGUILayout.ObjectField(playerInfo.shipPrefab, typeof(Ship), true) as Ship;
                playerInfo.hullImage = EditorGUILayout.ObjectField(playerInfo.hullImage, typeof(Sprite), true) as Sprite;
            }
        }
        #endregion

        #region Private Methods
        #endregion
    }
}