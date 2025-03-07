using System;
using System.Collections.Generic;
using System.Linq;
using Main.Lib.Level;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

namespace Main.UI
{
    public class ObjectivesUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text objectiveText;
        [SerializeField] private TMP_Text headerText;
        [SerializeField] private GameObject objectiveTextContainer;
        private List<(TMP_Text text, Requirement req)> _textRequirements;

        void Awake()
        {
            GameManager.OnLevelLoaded += GameManagerOnLevelLoaded;
        }

        private void OnDestroy()
        {
            GameManager.OnLevelLoaded -= GameManagerOnLevelLoaded;
        }

        private void Update()
        {
            _textRequirements?.ForEach(req =>
            {
                req.text.text = req.req.GetText();
            });
        }

        private void GameManagerOnLevelLoaded(LevelManager lvl)
        {
            foreach (Transform child in objectiveTextContainer.transform)
            {
                Destroy(child.gameObject);
            }

            headerText.gameObject.SetActive(lvl.Requirements.Count > 0);
            _textRequirements = lvl.Requirements
                .Select(req =>
                {
                    var t = Instantiate(objectiveText, objectiveTextContainer.transform, false);
                    t.gameObject.SetActive(true);
                    return (t, req);
                }).ToList();
        }
    }
}
