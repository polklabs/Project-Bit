using System;
using System.Collections;
using UnityEngine;
using IntegratedCircuits;

public class Mono_Switch : MonoBehaviour
{
    public GeneralManager gm;
    public BreadBoard breadBoard;
    public float momentaryLength = 0.3f;
    public float switchLength = 0.3f;

    public void TriggerSwitch(string id, int index, GameObject switchObject)
    {
        Switch ic = (Switch)breadBoard.GetIntegratedCircuit(Guid.Parse(id));       

        switch (ic.SwitchType)
        {
            case SwitchTypes.Pulse:
                MoveButton(switchObject);
                PulseSwitch(ic, index);
                break;
            case SwitchTypes.Momentary:
                if (OnSwitch(ic, index))
                {
                    MoveButton(switchObject);
                    StartCoroutine(MomentarySwitch(ic, index, momentaryLength));                                        
                }
                break;
            case SwitchTypes.Toggle:
                ToggleSwitch(ic, index);
                break;
        }
        
    }

    private void ToggleSwitch(Switch ic, int index)
    {
        ic.SwitchState[index] = !ic.SwitchState[index];
        ic.Update();        
        ic.CustomMethod();
        breadBoard.SetComponentDisplay();
    }

    private void PulseSwitch(Switch ic, int index)
    {
        ic.SwitchState[index] = !ic.SwitchState[index];
        ic.Update();        
        ic.CustomMethod();
        breadBoard.SetComponentDisplay();
        ic.SwitchState[index] = !ic.SwitchState[index];
        breadBoard.UpdateComponent(ic.GetId());
    }

    private bool OnSwitch(Switch ic, int index)
    {
        if (ic.SwitchState[index] == false)
        {
            ic.SwitchState[index] = true;
            ic.Update();
            ic.CustomMethod();
            breadBoard.SetComponentDisplay();
            return true;
        }
        return false;
    }

    private IEnumerator MomentarySwitch(Switch ic, int index, float delayTime)
    {
        float timer = 0f;
        while (timer < delayTime)
        {
            while (gm.paused)
            {
                yield return null;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        ic.SwitchState[index] = false;
        ic.Update();
        ic.CustomMethod();
    }

    private void MoveButton(GameObject child)
    {
        Vector3 v = child.transform.localPosition;
        v.y = 2.45f;
        child.transform.localPosition = v;
        child.GetComponentInParent<Mono_Switch_Count>().toggleCount++;
        StartCoroutine(MoveButtonBack(switchLength, child));
    }

    private IEnumerator MoveButtonBack(float delayTime, GameObject child)
    {
        float timer = 0f;
        while (timer < delayTime)
        {
            while (gm.paused)
            {
                yield return null;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        Mono_Switch_Count monoSwitchCount = child.GetComponentInParent<Mono_Switch_Count>();

        monoSwitchCount.toggleCount--;
        if (monoSwitchCount.toggleCount == 0)
        {
            Vector3 v = child.transform.localPosition;
            v.y = 2.95f;
            child.transform.localPosition = v;
        }
    }
}
