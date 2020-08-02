using System;
using System.Linq;
using System.Runtime.Serialization;

namespace IntegratedCircuits
{
    [DataContract]
    public class Seg_Disp : IntegratedCircuit
    {
        [DataMember]
        public bool[] Ons;
        [DataMember]
        public bool[] OldOns;

        private Mono_Segment mono;

        public Seg_Disp() : base(10)
        {
            IcType = ICType.ic6;
            ModelName = "7SegmentDisplay";
            Ons = Enumerable.Repeat(false, 8).ToArray();
            OldOns = Enumerable.Repeat(false, 8).ToArray();
            NeedsObjRef = true;

            DefaultState[Vdd] = State.LOW;
            DefaultState[Gnd] = State.LOW;
            Vdd = 7;
            Gnd = 2;
            DefaultState[Vdd] = State.OFF;
            DefaultState[Gnd] = State.OFF;
        }

        public override void CustomMethod()
        {
            if (mono == null)
            {
                mono = GameObjectRef.GetComponentInChildren<Mono_Segment>();
            }
            for(int i = 0; i < 8; i++)
            {
                //if (Ons[i] != OldOns[i])
                //{
                    mono.ToggleLed(i, Ons[i]);
                //}
            }
            
        }

        protected override void InternalUpdate()
        {
            Array.Copy(Ons, 0, OldOns, 0, Ons.Length);

            if (PinState[Vdd] != State.HIGH && PinState[Gnd] != State.HIGH && (PinState[Vdd] == State.LOW || PinState[Gnd] == State.LOW))
            {
                Ons[0] = PinState[6] == State.HIGH ? true : false; //A
                Ons[1] = PinState[5] == State.HIGH ? true : false; //B
                Ons[2] = PinState[3] == State.HIGH ? true : false; //C
                Ons[3] = PinState[1] == State.HIGH ? true : false; //D
                Ons[4] = PinState[0] == State.HIGH ? true : false; //E
                Ons[5] = PinState[8] == State.HIGH ? true : false; //F
                Ons[6] = PinState[9] == State.HIGH ? true : false; //G
                Ons[7] = PinState[4] == State.HIGH ? true : false; //DP
            }
            else
            {
                for(int i = 0; i < 8; i++)
                {
                    Ons[i] = false;
                }
            }

            CustomMethod();

        }

        protected override void InternalReset(bool disable)
        {
            InternalUpdate();
        }
    }
}