using UnityEngine;

namespace Main.Lib.Stat
{
    public class Stats : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 1f;
        [SerializeField] private float health = 100f;
        [SerializeField] private float attackPower = 10;
        public float MovementSpeed  => movementSpeed;
        public float Health => health;
        public float AttackPower => attackPower;
    }
}
