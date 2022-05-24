using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_Text _text;
    private string ButtonText;

    void Start()
    {
        _text = gameObject.GetComponentInChildren<TMP_Text>();
        _text.richText = true;
        ButtonText = _text.text;
    }

    // Update is called once per frame
    public void OnPointerEnter(PointerEventData eventData)
    {
        _text.text = $"<u>{ButtonText}</u>";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _text.text = ButtonText;
    }
}
