using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace userInterface {

    [RequireComponent(typeof(Button))]
    public class PartController : MonoBehaviour
    {
        public PartGroup desiredPartGroup;

        PartManager partManager;
        Button menuButton;

        void Start()
        {
            partManager = PartManager.GetInstance();
            menuButton = GetComponent<Button>();
            menuButton.onClick.AddListener(OnButtonClicked);
        }

        void OnButtonClicked()
        {
            partManager.SelectPartGroup(desiredPartGroup);
        }
    }
}