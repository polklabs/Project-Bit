using System.Collections;
using UnityEngine;
using IntegratedCircuits;

public class Mono_Clock : MonoBehaviour
{
    public GeneralManager gm;
    private PQ160_Clock IcClock;
    private Coroutine Clock;

    void Start()
    {
        gm = GameObject.Find("Manager").GetComponent<GeneralManager>();
    }

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
        while (true)
        {
            //yield return new WaitForSeconds(length);
            float timer = 0f;
            while (timer < length)
            {
                while (gm && gm.paused && gm.step == 0)
                {
                    yield return null;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            IcClock.On = !IcClock.On;
            IcClock.Update();
        }
    }

}
