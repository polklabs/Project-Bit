using UnityEngine;
using UnityEngine.UI;

public class Resume_Button : MonoBehaviour
{
    private GeneralManager generalManager;
    private Button button;

    void Start()
    {
        generalManager = GeneralManager.GetInstance();
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(Resume);
    }

    void Resume()
    {
        generalManager.PlayPause(false, true);
    }
}
