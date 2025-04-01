using System;
using UnityEngine;

namespace Main.World.Mobs.Death_Animation
{
    [RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
    public class DeathAnimation : MonoBehaviour
    {
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            Destroy(gameObject, 5f);
        }
        
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        public void Setup(string animName)
        {
            _animator.Play(animName);
        }
        
        
    }
}
