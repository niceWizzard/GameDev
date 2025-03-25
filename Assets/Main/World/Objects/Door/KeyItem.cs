using System;
using CleverCrow.Fluid.UniqueIds;
using Cysharp.Threading.Tasks.Triggers;
using Main.Lib.Items;
using Main.Player;
using UnityEngine;

namespace Main.World.Objects.Door
{
    [RequireComponent(typeof(UniqueId))]
    public class KeyItem : Item
    {
        private UniqueId _uniqueId;

        public UniqueId UniqueId => _uniqueId;
        protected override void Awake()
        {
            base.Awake();
            _uniqueId = GetComponent<UniqueId>();
        }

        protected override void OnPickedUp(PlayerController player)
        {
            base.OnPickedUp(player);
            GameManager.CurrentLevel.Register(this);
        }
    }
}
