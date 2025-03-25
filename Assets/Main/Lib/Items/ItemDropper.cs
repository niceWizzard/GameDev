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
                var angle = Random.Range(-Mathf.PI, Mathf.PI);
                var dir = Quaternion.Euler(0,0,angle) * Vector2.right;
                childrenItem.Enable(dir, transform.position);
            }

            foreach (var item in _itemPrefabs)
            {
                var angle = Random.Range(-Mathf.PI, Mathf.PI);
                var dir = Quaternion.Euler(0,0,angle) * Vector2.right;
                var itemInstance = Instantiate(item, transform.position, Quaternion.Euler(dir));
                itemInstance.Enable(dir, transform.position);
            }
        }
    }
}
