using System;
using UnityEngine;

namespace Main.Weapons
{
    public interface IProjectileSender
    {
        public GameObject GameObject { get; }
        public event Action SenderDispose;
    }
}
