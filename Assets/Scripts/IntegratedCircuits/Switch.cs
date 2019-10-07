using System.Runtime.Serialization;
using System.Linq;

namespace IntegratedCircuits
{   
    [DataContract]
    public class Switch : IntegratedCircuit
    {
        [DataMember]
        public bool[] SwitchState;

        protected readonly int SwitchPins;       
        public SwitchTypes SwitchType;

        public Switch(int pins) : base(pins == 1 ? 6 : pins)
        {
            SwitchPins = pins;
            SwitchState = Enumerable.Repeat(false, pins == 1 ? 1 : pins / 2).ToArray();
            SwitchType = SwitchTypes.Momentary;

            IcType = ICType.ic4;
            ModelName = "momentary_switch";

            if (pins == 1)
            {
                PinModes[2] = PinMode.Ouput;
                PinModes[3] = PinMode.Ouput;
            }
            else
            {
                for(int i = pins/2; i < pins; i++)
                {
                    PinModes[i] = PinMode.Ouput;
                }
            }
        }

        protected override void InternalReset(bool disable)
        {
            if (disable)
            {
                if (SwitchPins == 1)
                {
                    PinState[2] = State.OFF;
                    PinState[3] = State.OFF;
                }
                else
                {
                    for (int i = SwitchPins / 2; i < SwitchPins; i++)
                    {
                        PinState[i] = State.OFF;
                    }
                }
                return;
            }
            InternalUpdate();
        }

        protected override void InternalUpdate()
        {
            if (SwitchPins == 1)
            {
                PinState[2] = SwitchState[0] ? PinState[0] : State.OFF;
                PinState[3] = SwitchState[0] ? PinState[5] : State.OFF;
            }
            else
            {
                for (int i = SwitchPins / 2; i < SwitchPins; i++)
                {
                    PinState[i] = SwitchState[i - (SwitchPins / 2)] ? PinState[(SwitchPins / 2) - ((i - (SwitchPins / 2)) + 1)] : State.OFF;
                }
            }
        }
    }
}