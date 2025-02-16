#nullable enable
using Main.Player;
using TMPro;
using UnityEngine;

namespace Main.UI
{
    public class AmmoHUDController : MonoBehaviour
    {
        [SerializeField] private TMP_Text currentAmmoText = null!;
        [SerializeField] private TMP_Text maxAmmoText = null!;

        private PlayerController? _player;
        public void Setup(PlayerController p)
        {
            if (!p)
                return;
            _player = p;
            _player.Gun.OnAmmoUsed += OnAmmoUsed;
            SetMaxAmmo(_player.Gun.AmmoCapacity);
            SetAmmoText(_player.Gun.CurrentAmmo);
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
