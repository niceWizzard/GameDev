using System;
using CleverCrow.Fluid.UniqueIds;
using DG.Tweening;
using Main.Lib;
using Main.Lib.Save;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.World.Save_Station
{
    [RequireComponent(typeof(UniqueId), typeof(Interactable))]
    public class SaveStationController : MonoBehaviour
    {
        private UniqueId _uniqueId;
        private Interactable _interactable;

        private void Start()
        {
            _uniqueId = GetComponent<UniqueId>();
           _interactable = GetComponent<Interactable>();
           _interactable.OnInteract += Interact;
        }

        private void Interact()
        {
            _ = SaveManager.Instance.SaveDataAsync(data => data with
            {
                LastSaveStation = new SaveStation()
                {
                    levelName = SceneManager.GetActiveScene().name,
                    stationId = _uniqueId.Id,
                } 
            });
        }


    }
}
