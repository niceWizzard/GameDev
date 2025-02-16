using Main.Lib.Singleton;
using Main.Player;
using UnityEditor;
using UnityEngine;

namespace Main.Lib.Level
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LevelSwitcher : MonoBehaviour
    {
        [SerializeField] private SceneAsset levelToLoad;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent<PlayerController>(out var player))
                return;
            LevelLoader.Instance.LoadLevel(levelToLoad);
        }
    }
}
