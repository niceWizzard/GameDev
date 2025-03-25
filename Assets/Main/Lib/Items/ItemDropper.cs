using System;
using System.Collections.Generic;
using Unity.Mathematics.Geometry;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main.Lib.Items
{
    [Serializable]
    public class WithRandom
    {
        public float chance;
        public Item item;
    }
    public class ItemDropper : MonoBehaviour
    {
        private  List<Item> _childrenItems = new();

        [SerializeField]
        private List<WithRandom> _randomizedItems = new();
        private void Awake()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (!child.TryGetComponent(out Item item)) continue;
                _childrenItems.Add(item);
                item.Disable();
            } 
        }

        public void DropItems()
        {
            foreach (var childrenItem in _childrenItems)
            {
                childrenItem.transform.parent = null;
                childrenItem.Enable(transform.position);
            }

            var random = GetRandomItem(_randomizedItems);
            if (!random) return;
            var a = Instantiate(random);
            a.Enable(transform.position);
        }

        private static Item GetRandomItem(List<WithRandom> items)
        {
            var roll = Random.Range(0,100f); 
            float cumulativeProbability = 0;
            foreach (var kvp in items)
            {
                cumulativeProbability += kvp.chance; 
                if (roll <= cumulativeProbability)
                    return kvp.item;
            }
            return null;
        }
    }
}
