using System.Collections;
using Main.Lib.Singleton;
using Main.Player;
using Main.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Lib.Level
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Transform safeSpawn;
        private void Awake()
        {
            if(safeSpawn == null)
                Debug.LogError($"Safe spawn is null at {SceneManager.GetActiveScene().name}");
            var player = FindAnyObjectByType<PlayerController>();
            MainCamera.Instance.Follow(player);
            HUDController.Instance.SetPlayer(player);
            StartCoroutine(SetPlayerPosition(player));
        }

        private static IEnumerator SetPlayerPosition(PlayerController player)
        {
            yield return new WaitForEndOfFrame();
            var doorLoaded = LevelLoader.Instance.GetDoorToLoad();
            if (doorLoaded == null) 
                yield break;
            var levelSwitcher = LevelSwitcher.FindLevelSwitch(doorLoaded);
            player.transform.position = levelSwitcher.SafePosition;
            MainCamera.Instance.MoveTo(levelSwitcher.SafePosition);
        }
    }
}
