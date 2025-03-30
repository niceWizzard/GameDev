#nullable enable
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Main.Lib.Cutscene
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
    public class CutsceneEntity : MonoBehaviour
    {
        private Animator _animator = null!;

        public SpriteRenderer SpriteRenderer { get; private set; } = null!;

        private Vector2? targetPosition;
        private float _speed;
        
        public Vector2 Position {
            get => transform.position;
            set => transform.position = value;
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            if(!_animator)
                Debug.LogError($"No animator attached to {nameof(CutsceneEntity)} {name}");
        }

        public void Play(string clipName)
        {
            _animator.Play(clipName);
        }

        public void WalkTo(Vector2 position, float speed)
        {
            targetPosition = position;
            _speed = speed;
        }

        public UniTask AsyncWaitToReachPosition() => UniTask.WaitUntil(() => targetPosition == null, cancellationToken: destroyCancellationToken);

        public void CancelWalk()
        {
            targetPosition = null;
        }

        private void FixedUpdate()
        {
            if (targetPosition == null)
                return;
            var dir = (targetPosition.Value - Position).normalized;
            Position +=  dir* (_speed * Time.fixedDeltaTime);
            if (Vector2.Distance(Position, targetPosition.Value) <= 0.1f)
            {
                targetPosition = null;
            }
        }
    }
}
