using Main.Lib.Level;
using NavMeshPlus.Components;
using NavMeshPlus.Extensions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace Editor
{
    public class LevelSetupEditor : EditorWindow
    {
        [MenuItem("Tools/Setup Level")]
        private static void SetupLevel()
        {
            
            if (FindAnyObjectByType<LevelManager>())
            {
                Debug.LogWarning("Level Manager already exists in the scene!");
                return;
            }
            // Start an Undo group
            Undo.IncrementCurrentGroup();
            var undoGroup = Undo.GetCurrentGroup();

            // Create the Level Manager
            var lvlManagerGO = new GameObject("Level Manager");
            var levelManager = lvlManagerGO.AddComponent<LevelManager>();
            var serializedObject = new SerializedObject(levelManager);
            var property = serializedObject.FindProperty("safeSpawn");
            if (property != null)
            {
                property.objectReferenceValue = lvlManagerGO.transform;
                serializedObject.ApplyModifiedProperties(); // Apply the changes
            }
            else
            {
                Debug.LogWarning("Level Manager safe spawn could not be set!");
            }
            
            Undo.RegisterCreatedObjectUndo(lvlManagerGO, "Create Level Manager");

            AddTilemaps();
            AddNavMesh();
            AddPlayer();
            
            var lightGo = new GameObject("Global Light");
            var light = lightGo.AddComponent<Light2D>();
            light.lightType = Light2D.LightType.Global;
            light.intensity = 0.7f;
            
            Undo.RegisterCreatedObjectUndo(lightGo, "Create Light");

            // Complete the Undo group
            Undo.CollapseUndoOperations(undoGroup);

            // Select the Level Manager in the hierarchy
            Selection.activeGameObject = lvlManagerGO;

            Debug.Log("Level Manager with Tilemap Created!");
            
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene()); 
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }

        private static void AddTilemaps()
        {
            // Create a Grid object (required for Tilemaps)
            var grid = new GameObject("Grid");
            var gridComponent = grid.AddComponent<Grid>();
            Undo.RegisterCreatedObjectUndo(grid, "Create Grid");

            // Create a Tilemap GameObject (child of Grid)
            
            var wallsGo = CreateTileMap("Walls", grid.transform, sortingOrder:1);
            wallsGo.AddComponent<TilemapCollider2D>();
            var modifier = wallsGo.AddComponent<NavMeshModifier>();
            modifier.overrideArea = true;
            modifier.area = 0;
            CreateTileMap("BG", grid.transform);
        }

        private static void AddNavMesh()
        {
            // Create NavMesh
            var navMeshGo = new GameObject("NavMesh")
            {
                transform =
                {
                    eulerAngles = new Vector3(-90, 0, 0)
                }
            };
            var surface = navMeshGo.AddComponent<NavMeshSurface>();
            navMeshGo.AddComponent<CollectSources2d>();
            surface.defaultArea = 1;
            
            Undo.RegisterCreatedObjectUndo(navMeshGo, "Create Nav");
        }

        private static void AddPlayer()
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Main/Player/Player.prefab");
            if (prefab != null)
            {
                var player = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                Undo.RegisterCreatedObjectUndo(player, "Create Prefab");
            }
            else
            {
                Debug.LogError("Player Prefab not found at path!");
            }
        }

        private static GameObject CreateTileMap(string name, Transform grid, int sortingOrder = 0)
        {
            var tilemap = new GameObject(name)
            {
                transform =
                {
                    parent = grid
                }
            };
            tilemap.AddComponent<Tilemap>();
            var renderer =tilemap.AddComponent<TilemapRenderer>();
            renderer.sortingLayerName = "background";
            renderer.sortingOrder = sortingOrder;
            Undo.RegisterCreatedObjectUndo(tilemap, "Create Tilemap");
            return tilemap;
        }
    }
}