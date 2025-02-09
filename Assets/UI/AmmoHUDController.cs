#nullable enable
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class AmmoHUDController : MonoBehaviour
    {
        [SerializeField] private TMP_Text currentAmmoText = null!;
        [SerializeField] private TMP_Text maxAmmoText = null!;

        [HideInInspector]
        public PlayerController? player;
        private void Start()
        {
            if (!player)
                return;
            player.Gun.OnAmmoUsed += OnAmmoUsed;
            SetMaxAmmo(player.Gun.AmmoCapacity);
            SetAmmoText(player.Gun.CurrentAmmo);
        }
        
        private void SetAmmoText(int ammo)
        {
            currentAmmoText.text = ammo.ToString();
        }

        private void SetMaxAmmo(int maxAmmo)
        {
            maxAmmoText.text = $"/{maxAmmo}";
        } 
        
        private void OnAmmoUsed(int curAmmo)
        {
            SetAmmoText(curAmmo);
        }
    }
}
