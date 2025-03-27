#nullable enable
using System;
using Main.Player;
using Main.Weapons.Gun;
using TMPro;
using UnityEngine;

namespace Main.UI
{
    public class AmmoHUDController : MonoBehaviour
    {
        [SerializeField] private TMP_Text currentAmmoText = null!;
        [SerializeField] private TMP_Text maxAmmoText = null!;
        [SerializeField] private TMP_Text ammoText = null!;
        private void Awake()
        {
            Disable();
        }

        public void Disable()
        {
            currentAmmoText.gameObject.SetActive(false);
            maxAmmoText.gameObject.SetActive(false);
            ammoText.gameObject.SetActive(false);
        }
        private void Enable()
        {
            currentAmmoText.gameObject.SetActive(true);
            maxAmmoText.gameObject.SetActive(true);
            ammoText.gameObject.SetActive(true);
        }

        private PlayerController? _player;
        public void Setup(PlayerController p)
        {
            if (!p)
                return;
            Enable();
            _player = p;
            _player.Gun.OnAmmoUsed += OnAmmoUsed;
            SetMaxAmmo(_player.Gun.AmmoCapacity);
            SetAmmoText(_player.Gun.CurrentAmmo);
            PlayerController.StatChange += PlayerControllerOnStatChange;
        }

        private void OnDestroy()
        {
            PlayerController.StatChange -= PlayerControllerOnStatChange;
        }

        private void PlayerControllerOnStatChange(RangedStats stats)
        {
            SetMaxAmmo(stats.AmmoCapacity);
            if(_player)
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
