using System.Collections;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace IntegratedCircuits
{
    [DataContract]
    public class PQ160T : IntegratedCircuit
    {
        [DataMember]
        public bool On;

        private Mono_Clock mono_Clock = null;

        public PQ160T() : base(6)
        {
            IcType = ICType.ic4;
            ModelName = "IC_Clock";
            On = false;
            NeedsObjRef = true;
            PinModes[4] = PinMode.Ouput;
        }

        public override void CustomMethod()
        {
            InternalUpdate(new State[0]);
        }

        protected override void InternalUpdate(State[] oldState)
        {
            if (!Enumerable.SequenceEqual(oldState, PinState))
            {
                BitArray bitField = new BitArray(new bool[] {
                    PinState[0] == State.HIGH,
                    PinState[1] == State.HIGH,
                    PinState[3] == State.HIGH
                });

                int[] array = new int[1];
                bitField.CopyTo(array, 0);

                // float ts = 1.0f / ((((array[0]+0.0f) / 7.0f) * 29.0f) + 1.0f);
                float ts = 1.0f / (Mathf.Pow(2, array[0]));
                Debug.Log(ts);

                if (mono_Clock == null)
                {
                    mono_Clock = GameObjectRef.GetComponent<Mono_Clock>();
                }
                mono_Clock.StartClock(ts, this);
            }

            PinState[4] = On ? State.HIGH : State.LOW;
        }

        protected override void InternalReset(bool disable)
        {
            if (mono_Clock == null)
            {
                mono_Clock = GameObjectRef.GetComponent<Mono_Clock>();
            }
            mono_Clock.StopClock();
            PinState[4] = State.OFF;
            On = false;
        }
    }
}
