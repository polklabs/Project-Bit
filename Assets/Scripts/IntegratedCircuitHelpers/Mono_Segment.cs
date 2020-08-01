using System.Collections;
using UnityEngine;

public class Mono_Segment : MonoBehaviour
{
    public Material onMaterial;
    public Material offMaterial;

    private string[] childNames = { "A", "B", "C", "D", "E", "F", "G", "DP" };

    private Coroutine[] coroutines;
    private MeshRenderer[] meshRenderers;

    public void Start()
    {
        coroutines = new Coroutine[8];
        meshRenderers = new MeshRenderer[8];

        for(int i = 0; i < 8; i++)
        {
            string name = childNames[i];
            Transform t = transform.Find(name);
            meshRenderers[i] = t.gameObject.GetComponent<MeshRenderer>();
        }
    }

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
        meshRenderers[index].material = m;
    }
}
