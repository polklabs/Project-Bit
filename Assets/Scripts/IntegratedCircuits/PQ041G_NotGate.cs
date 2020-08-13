using System.Runtime.Serialization;

namespace IntegratedCircuits
{
    [DataContract]
    public class PQ041G_NotGate : IntegratedCircuit
    {
        [DataMember]
        private readonly Chips.Hex_Not_Gates Chip;

        public PQ041G_NotGate() : base(14)
        {
            IcType = ICType.ic4;
            ModelName = "IC14";

            Chip = new Chips.Hex_Not_Gates();

            PinModes[1]  = PinMode.Ouput;
            PinModes[3]  = PinMode.Ouput;
            PinModes[5]  = PinMode.Ouput;
            PinModes[7]  = PinMode.Ouput;
            PinModes[9]  = PinMode.Ouput;
            PinModes[11] = PinMode.Ouput;
        }

        protected override void InternalUpdate(bool reset)
        {
            Chip.Input[0] = PinState[0]  == State.HIGH ? true : false;
            Chip.Input[1] = PinState[2]  == State.HIGH ? true : false;
            Chip.Input[2] = PinState[4]  == State.HIGH ? true : false;
            Chip.Input[3] = PinState[8]  == State.HIGH ? true : false;
            Chip.Input[4] = PinState[10] == State.HIGH ? true : false;
            Chip.Input[5] = PinState[12] == State.HIGH ? true : false;

            Chip.Update();

            PinState[1] = Chip.Output[0]  ? State.HIGH : State.LOW;
            PinState[3] = Chip.Output[1]  ? State.HIGH : State.LOW;
            PinState[5] = Chip.Output[2]  ? State.HIGH : State.LOW;
            PinState[7] = Chip.Output[3]  ? State.HIGH : State.LOW;
            PinState[9] = Chip.Output[4]  ? State.HIGH : State.LOW;
            PinState[11] = Chip.Output[5] ? State.HIGH : State.LOW;
        }

        protected override void InternalReset(bool disable)
        {
            InternalUpdate(true);
            base.InternalReset(disable);
        }

    }
}
