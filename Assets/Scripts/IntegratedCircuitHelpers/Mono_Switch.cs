using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IntegratedCircuits;

public class Mono_Switch : MonoBehaviour
{

    public BreadBoard breadBoard;
    public float momentaryLength = 0.3f;

    public void TriggerSwitch(string id, int index)
    {
        //Debug.Log("Trigger Switch");
        Switch ic = (Switch)breadBoard.GetIntegratedCircuit(Guid.Parse(id));


        switch (ic.SwitchType)
        {
            case SwitchTypes.Pulse:
                //Debug.Log("Pulse");
                PulseSwitch(ic, index);
                break;
            case SwitchTypes.Momentary:
                //Debug.Log("Momentary");
                if (OnSwitch(ic, index))
                {
                    StartCoroutine(MomentarySwitch(ic, index, momentaryLength));                                        
                }
                break;
            case SwitchTypes.Toggle:
                //Debug.Log("Toggle");
                ToggleSwitch(ic, index);
                break;
        }
        
    }

    private void ToggleSwitch(Switch ic, int index)
    {
        ic.SwitchState[index] = !ic.SwitchState[index];
        ic.Update();
        ic.CustomMethod();
    }

    private void PulseSwitch(Switch ic, int index)
    {
        ic.SwitchState[index] = !ic.SwitchState[index];
        ic.Update();
        ic.CustomMethod();
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
            return true;
        }
        return false;
    }

    private IEnumerator MomentarySwitch(Switch ic, int index, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        ic.SwitchState[index] = false;
        ic.Update();
        ic.CustomMethod();
    }
}
