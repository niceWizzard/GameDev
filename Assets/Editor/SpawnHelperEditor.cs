using System;
using Main.Lib.Level;
using Main.World.Objects.CompletionStatue;
using Main.World.Objects.Door;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class SpawnHelperEditor : EditorWindow
    {
        private enum Keys
        {
            Ghost,
            BigGhost,
            Door,
            Key,
            TimedSpikeTrap,
            TimedFlameTrap,
            FlameTrap,
            CompletionStatue,
            SealLamp,
            Pedestal,
            HealthPotion,
        }

        private static string GetPrefabPath(Keys keys) => keys switch
        {
            Keys.Ghost => "Assets/Main/World/Mobs/Ghost/Ghost.prefab",
            Keys.BigGhost => "Assets/Main/World/Mobs/Ghost/Big Ghost.prefab",
            Keys.Door => "Assets/Main/World/Objects/Door/Door.prefab",
            Keys.Key => "Assets/Main/World/Objects/Door/Key.prefab",
            Keys.TimedSpikeTrap => "Assets/Main/World/Objects/Traps/Timed/TimedSpikes/TimedSpikes.prefab",
            Keys.TimedFlameTrap => "Assets/Main/World/Objects/Traps/Timed/TimedFlame/TimedFlame.prefab",
            Keys.FlameTrap => "Assets/Main/World/Objects/Traps/Flame thrower/Flame Thrower.prefab",
            Keys.CompletionStatue => "Assets/Main/World/Objects/CompletionStatue/StatueCompletion.prefab",
            Keys.SealLamp => "Assets/Main/World/Objects/Lamp/SealLamp.prefab",
            Keys.Pedestal => "Assets/Main/World/Objects/Pedestal/Pedestal.prefab",
            Keys.HealthPotion => "Assets/Main/World/Objects/Items/Health Potion/HealthPotion.prefab",
            _ => throw new ArgumentOutOfRangeException(nameof(keys), keys, null)
        };

        [MenuItem("Tools/Spawn/Mobs/Ghost")]
        public static void SpawnGhost() => SpawnPrefab(Keys.Ghost);
        [MenuItem("Tools/Spawn/Mobs/Big Ghost")]
        public static void SpawnBigGhost() => SpawnPrefab(Keys.BigGhost);

        [MenuItem("Tools/Spawn/Objects/Door")]
        public static void SpawnDoor() => SpawnPrefab(Keys.Door);
        
        [MenuItem("Tools/Spawn/Objects/Key")]
        public static void SpawnKey() => SpawnPrefab(Keys.Key);

        [MenuItem("Tools/Spawn/Objects/Door and Key")]
        public static void SpawnDoorAndKey()
        {
            var door = SpawnPrefab(Keys.Door).GetComponent<Door>();
            var key = SpawnPrefab(Keys.Key);
            
            door.transform.position += Vector3.up * 1.5f;
            key.transform.position += Vector3.down * 1.5f;
            
            var doorObject = new SerializedObject(door);
            var property = doorObject.FindProperty("requiredKeys");
            if (property == null)
            {
                Debug.LogError("Couldn't find required Keys field.");   
                Undo.PerformUndo();
                return;
            }
            property.arraySize++;
            property.GetArrayElementAtIndex(property.arraySize - 1).objectReferenceValue = key;

            // Apply changes
            doorObject.ApplyModifiedProperties();
        }

        [MenuItem("Tools/Spawn/Traps/Timed Spike Trap")]
        public static void SpawnTimedSpikeTrap() => SpawnPrefab(Keys.TimedSpikeTrap);
        
        [MenuItem("Tools/Spawn/Traps/Timed Flame Thrower")]
        public static void SpawnTimedFlame() => SpawnPrefab(Keys.TimedFlameTrap);
        
        [MenuItem("Tools/Spawn/Traps/Flame Thrower")]
        public static void SpawnFlameThrower() => SpawnPrefab(Keys.FlameTrap);
        
        [MenuItem("Tools/Spawn/Level/Completion Statue")]
        public static void SpawnCompletionStatue()
        {
            if (FindAnyObjectByType<CompletionStatue>())
            {
                Debug.LogWarning("Completion Statue is already exists in the level.");
                return;
            }
            var statue = SpawnPrefab(Keys.CompletionStatue);
            var levelManager = FindAnyObjectByType<LevelManager>();
            if (!levelManager)
            {
                Debug.LogError("Couldn't find any level manager in the scene.");
                return;
            }
            
            var serializedObject = new SerializedObject(levelManager);
            var property = serializedObject.FindProperty("completionStatue");
            if (property == null)
            {
                Debug.LogError($"Couldn't find the completionStatue field in the {nameof(levelManager)}");
                return;
            }
            property.objectReferenceValue = statue;
        }

        [MenuItem("Tools/Spawn/Level/Seal Lamp")]
        public static void SpawnSealLamp() => SpawnPrefab(Keys.SealLamp);
        
        [MenuItem("Tools/Spawn/Level/Pedestal")]
        public static void SpawnPedestal() => SpawnPrefab(Keys.Pedestal);
        
        [MenuItem("Tools/Spawn/Items/Health Potion")]
        public static void SpawnHealthPotion() => SpawnPrefab(Keys.HealthPotion);
        
        
        private static GameObject SpawnPrefab(Keys key)
        {
            var prefabPath = GetPrefabPath(key);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab == null)
            {
                Debug.LogError($"Prefab not found at path: {prefabPath}");
                return null;
            }
            var instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (!instance) return null;
            Selection.activeGameObject = instance;
            Undo.RegisterCreatedObjectUndo(instance, $"Spawn {key.ToString()}");
            PositionInScene(instance);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene()); 
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            return instance;
        }

        private static void PositionInScene(GameObject instance)
        {
            if (SceneView.lastActiveSceneView)
            {
                Vector2 pos = SceneView.lastActiveSceneView.camera.transform.position + SceneView.lastActiveSceneView.camera.transform.forward * 2;
                instance.transform.position = pos;
            }
            else
            {
                instance.transform.position = Vector3.zero;
            }
        }
    }
}