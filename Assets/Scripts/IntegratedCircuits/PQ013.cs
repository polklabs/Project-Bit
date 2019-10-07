using UnityEngine;
using System.Runtime.Serialization;

namespace IntegratedCircuits
{
    [DataContract]
    public class PQ013 : IntegratedCircuit
    {
        [DataMember]
        private readonly Chips.JKFlipFlop Chip;

        public PQ013() : base(8)
        {
            IcType = ICType.ic6;
            ModelName = "IC20_Wide";

            Chip = new Chips.JKFlipFlop();

            PinModes[5] = PinMode.Ouput;
            PinModes[6] = PinMode.Ouput;
        }      

        protected override void InternalUpdate()
        {
            Chip.Input[0] = PinState[0] == State.HIGH ? true : false;
            Chip.Input[1] = PinState[1] == State.HIGH ? true : false;
            Chip.Update();
            PinState[6] = Chip.Output[0] ? State.HIGH : State.LOW;
            PinState[5] = Chip.Output[1] ? State.HIGH : State.LOW;
        }
    }
}
