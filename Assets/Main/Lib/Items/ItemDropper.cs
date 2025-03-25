using System.Collections.Generic;
using Unity.Mathematics.Geometry;
using UnityEngine;

namespace Main.Lib.Items
{
    public class ItemDropper : MonoBehaviour
    {
        private readonly List<Item> _childrenItems = new();

        [SerializeField]
        private List<Item> _itemPrefabs = new();
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
                
                childrenItem.Enable(transform.position);
            }

            foreach (var item in _itemPrefabs)
            {
                var itemInstance = Instantiate(item, transform.position, Quaternion.identity);
                itemInstance.Enable(transform.position);
            }
        }
    }
}
