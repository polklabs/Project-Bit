using System.Collections;
using UnityEngine;
using IntegratedCircuits;

public class Mono_Clock : MonoBehaviour
{
    private PQ160_Clock IcClock;
    private Coroutine Clock;

    public void StartClock(float length, PQ160_Clock clockRef)
    {
        IcClock = clockRef;
        StopClock();
        Clock = StartCoroutine(TriggerClock(length));
    }    

    public void StopClock()
    {
        if(Clock != null)
        {
            StopCoroutine(Clock);
            Clock = null;
        }
    }

    private IEnumerator TriggerClock(float length)
    {
        yield return new WaitForSeconds(length);

        IcClock.On = !IcClock.On;
        IcClock.Update();

        Clock = StartCoroutine(TriggerClock(length));
    }

}
