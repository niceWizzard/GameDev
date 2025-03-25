using UnityEngine;

namespace Main.Lib.Stat
{
    public class Stats : MonoBehaviour
    {
        [SerializeField] protected float movementSpeed = 1f;
        [SerializeField] protected float health = 100f;
        [SerializeField] protected float attackPower = 10;
        public float MovementSpeed  => movementSpeed;
        public float Health => health;
        public float AttackPower => attackPower;
    }
}
