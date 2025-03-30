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


        protected virtual void Awake()
        {
            CutscenePanel.ShowImmediately();
            HUDController.Instance.EnterCutsceneMode();
            SetCamera(0);
        }
        
        protected void SetCamera(int index)
        {
            foreach (var cam in cameras)
            {
                cam.gameObject.SetActive(false);
            }
            cameras[index].gameObject.SetActive(true);
        }
        
        protected async UniTask Wait(float seconds)
        {
            await UniTask.WaitForSeconds(seconds, cancellationToken:destroyCancellationToken);
        }
    }
}
