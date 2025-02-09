using DG.Tweening;
using Lib.Health;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Weapons.Gun;

namespace Player
{
    public class PlayerController : MobController, IDamageable
    {
        [Header("Stats")]
        public float friction = 0.5f;
        public float movementSpeed = 3f;
        public float dashDistance = 1f;
        [Header("Components")]
        public Rigidbody2D rigidbody2d;
        [SerializeField]
        private GunController gun;
        [SerializeField]
        private CircleCollider2D circleCollider2D;

        [SerializeField] private TMP_Text reloadingText;
        public CircleCollider2D CircleCollider2D => circleCollider2D;

        [SerializeField] private Transform gunAnchor;
        public GunController Gun => gun;
        public Transform GunAnchor => gunAnchor;
        public float FacingDirection { get; private set; } = 1;

        private void Start()
        {
            gun.OnReloadStart += GunOnReloadStart;
            gun.OnReloadEnd += GunOnReloadEnd;
            reloadingText.color = new Vector4(0,0,0,0);
        }

        private void GunOnReloadEnd()
        {
            reloadingText.DOColor(new Vector4(0,0,0,0), 0.2f);
        }

        private void GunOnReloadStart()
        {
            reloadingText.DOColor(Color.white, 0.3f);
        }

        public void UpdateFacingDirection(Vector2 input)
        {
            if (math.abs(input.x) < 0.01f)
                return;
            FacingDirection = math.sign(input.x);
            spriteRenderer.flipX = FacingDirection < 0;
        }

        public Vector2 GetMovementInput()
        {
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
    }
}
