using System;
using UnityEngine;

namespace Main.Lib.Level
{
    [CreateAssetMenu(menuName = "Unique Identifier", fileName = "Door1")]
    public class UniqueIdentifier : ScriptableObject
    {
        public string debugIdentifier;
        
        [SerializeField, HideInInspector] private string guid;

        public string GUID => guid;
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(guid))
            {
                guid = Guid.NewGuid().ToString(); // Generate only once
            }
        }
    }
}
