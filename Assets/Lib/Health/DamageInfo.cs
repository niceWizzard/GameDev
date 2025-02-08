using UnityEngine;

namespace Lib.Health
{
    public struct DamageInfo
    {
        public DamageInfo(float damage, GameObject source)
        {
            this.damage = damage;
            damageSource = source;
        } 
        public readonly float damage;
        public readonly GameObject damageSource;
    }
}
