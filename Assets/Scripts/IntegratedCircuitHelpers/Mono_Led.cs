using System.Collections;
using UnityEngine;

public class Mono_Led : MonoBehaviour
{
    private Light LightRef;
    private GameObject CoreRef;
    private Coroutine c;

    public void ToggleLed(bool value)
    {
        if (LightRef == null)
        {
            LightRef = gameObject.GetComponentInChildren<Light>();
        }
        if(CoreRef == null)
        {
            CoreRef = transform.Find("Core").gameObject;
        }

        if (value)
        {
            if (c != null)
            {
                StopCoroutine(c);
                c = null;
            }
            LightRef.enabled = true;
            CoreRef.SetActive(true);
        }
        else
        {
            if(c == null)
            {
                c = StartCoroutine(TriggerLed());
            }
        }

    }

    private IEnumerator TriggerLed()
    {
        yield return new WaitForSeconds(0.015f);
        LightRef.enabled = false;
        CoreRef.SetActive(false);
        c = null;
    }
}
