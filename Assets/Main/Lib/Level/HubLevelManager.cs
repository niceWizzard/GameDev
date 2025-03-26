using Main.Player;
using UnityEngine;

public class HubLevelManager : MonoBehaviour
{
    [SerializeField] private Transform safeSpawn;
    private void Awake()
    {
        var player = FindAnyObjectByType<PlayerController>();
        player.Position = safeSpawn.position;
    }
}
