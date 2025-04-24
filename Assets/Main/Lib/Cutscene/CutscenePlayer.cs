using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Main.Lib.Singleton;
using Main.UI;
using UnityEngine;

namespace Main.Lib.Cutscene
{
    public class CutscenePlayer : MonoBehaviour
    {
        [SerializeField] protected List<Camera> cameras;
        private int _cameraIndex;
        
        protected Camera CurrentCamera => cameras[_cameraIndex];

        protected virtual void Awake()
        {
            CutscenePanel.ShowImmediately();
            HUDController.Instance.EnterCutsceneMode();
            foreach (var cam in cameras)
            {
                cam.gameObject.SetActive(false);
            }
            SetCamera(0);
        }
        
        protected void SetCamera(int index)
        {
            _cameraIndex = index;
            MainCamera.Instance.SetPosition(CurrentCamera.transform.position);
        }
        
        protected async UniTask Wait(float seconds)
        {
            await UniTask.WaitForSeconds(seconds, cancellationToken:destroyCancellationToken);
        }

   
    }
}
