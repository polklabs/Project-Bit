using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace userInterface {
    public class PartGroupManager : Singleton<PartGroupManager>
    {
        public GameObject buttonTemplate;
        public Transform contentTransform;

        public void DrawPartGroup(PartGroup partGroup)
        {
            gameObject.SetActive(true);

            for(int i = 0; i < contentTransform.childCount; i++)
            {
                Destroy(contentTransform.GetChild(i).gameObject);
            }

            if (Part.PartList.ContainsKey(partGroup))
            {
                List<Part> parts = Part.PartList[partGroup];

                foreach (Part part in parts)
                {
                    GameObject obj = Instantiate(buttonTemplate, contentTransform);
                    var text = obj.GetComponentInChildren<Text>();
                    text.text = part.PartName;
                    obj.GetComponentInChildren<Component_Button>().SetPart(part);
                    obj.GetComponentInChildren<Info_Button>().SetDataSheet(part.ClassName);
                }
            } else
            {
                gameObject.SetActive(false);
            }
        }
    }
}