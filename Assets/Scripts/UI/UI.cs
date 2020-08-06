using UnityEngine;
using UnityEngine.UI;

namespace userInterface {
    public class UI : MonoBehaviour
    {
        public Color32 SelectedColor = new Color32(0, 174, 255, 255);
        public Color32 UnselectedColor = new Color32(255, 255, 255, 255);

        public Button[] ActionButtons = new Button[3];
        private int selectedButton = -1;

        public WorkspaceEditor workspaceEditor;
        public GeneralManager generalManager;

        public GameObject Inventory;
        public GameObject ScrollViewContent;
        public GameObject ButtonTemplate;

        public Image playPauseImage;
        public Sprite paused;
        public Sprite played;

        void Start()
        {
            clearButtonColors();
        }

        private void clearButtonColors()
        {
            foreach (Button b in ActionButtons)
            {
                if (b != null)
                {
                    setButtonColor(b, UnselectedColor);
                }
            }
        }

        private void setButtonColor(Button b, Color32 color)
        {
            ColorBlock colors_temp = b.colors;
            colors_temp.selectedColor = color;
            colors_temp.normalColor = color;
            b.colors = colors_temp;
        }

        public void SelectButton(int buttonIndex)
        {
            clearButtonColors();
            setButtonColor(ActionButtons[buttonIndex], (selectedButton == buttonIndex) ? UnselectedColor : SelectedColor);

            if (buttonIndex == selectedButton)
            {
                selectedButton = -1;                
            } else
            {
                selectedButton = buttonIndex;
            }
            
            DrawGroup();            
        }

        public void DrawGroup()
        {
            if (selectedButton == -1 || selectedButton >= Part.GroupCount())
            {
                if (selectedButton == -1)
                {
                    workspaceEditor.SetActionNone();
                }
                Inventory.SetActive(false);
                return;
            }

            workspaceEditor.SetActionPlace(true);
            Inventory.SetActive(true);

            string groupString = Part.IntToGroupString(selectedButton);

            //Delete children in scroll
            for (int i = 1; i < ScrollViewContent.transform.childCount; i++)
            {
                Destroy(ScrollViewContent.transform.GetChild(i).gameObject);
            }

            //Repopulate the scroll
            if (Part.PartList.ContainsKey(groupString))
            {
                foreach (Part part in Part.PartList[groupString])
                {
                    GameObject go = Instantiate(ButtonTemplate) as GameObject;
                    go.SetActive(true);
                    go.transform.SetParent(ScrollViewContent.transform, false);
                    go.GetComponentInChildren<Text>().text = part.PartName;
                    go.name = part.ClassName;
                    go.GetComponent<Component_Button>().isMultiPoint = part.IsMultiPoint;
                }
            }            
        }

        public void PlayPause()
        {
            generalManager.PlayPause();

            if (generalManager.paused)
            {
                playPauseImage.sprite = played;
            } 
            else
            {
                playPauseImage.sprite = paused;
            }
        }

        public void Step()
        {
            generalManager.Step();
        }

        public void SingleStep()
        {
            generalManager.SingleStep();
        }

    }

}
