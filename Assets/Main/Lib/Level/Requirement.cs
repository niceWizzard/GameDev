using UnityEngine;

namespace Main.Lib.Level
{
    public abstract class Requirement : MonoBehaviour
    {
        public abstract bool CheckCompleted();
        public abstract string GetText();
    }

    
}



