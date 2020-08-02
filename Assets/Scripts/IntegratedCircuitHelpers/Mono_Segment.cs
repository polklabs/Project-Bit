using System.Collections;
using UnityEngine;

public class Mono_Segment : MonoBehaviour
{
    public Material onMaterial;
    public Material offMaterial;

    private string[] childNames = { "A", "B", "C", "D", "E", "F", "G", "DP" };

    private Coroutine[] coroutines = new Coroutine[8];
    private MeshRenderer[] meshRenderers = new MeshRenderer[8];

    public void ToggleLed(int index, bool value)
    {
        if (value)
        {
            if (coroutines[index] != null)
            {
                StopCoroutine(coroutines[index]);
                coroutines[index] = null;
            }
            else
            {
                setMaterial(index, onMaterial);
            }
        }
        else
        {
            if (coroutines[index] == null)
            {
                coroutines[index] = StartCoroutine(TriggerLed(index));
            }
        }

    }

    private IEnumerator TriggerLed(int index)
    {
        yield return new WaitForSeconds(0.015f);
        setMaterial(index, offMaterial);
        coroutines[index] = null;
    }

    private void setMaterial(int index, Material m)
    {
        if(meshRenderers[index] == null)
        {
            meshRenderers[index] = transform.Find(childNames[index]).gameObject.GetComponent<MeshRenderer>();
        }

        meshRenderers[index].material = m;
    }
}
