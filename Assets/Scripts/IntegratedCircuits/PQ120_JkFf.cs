using System.Runtime.Serialization;

namespace IntegratedCircuits
{
    [DataContract]
    public class PQ120_JkFf : IntegratedCircuit
    {
        [DataMember]
        private readonly Chips.Dual_JK_FlipFlop Chip;

        public PQ120_JkFf() : base(14)
        {
            IcType = ICType.ic4;
            ModelName = "IC14";

            Chip = new Chips.Dual_JK_FlipFlop();

            PinModes[1] = PinMode.Ouput;
            PinModes[2] = PinMode.Ouput;
            PinModes[4] = PinMode.Ouput;
            PinModes[5] = PinMode.Ouput;
        }

        protected override void InternalUpdate(bool reset)
        {
            Chip.Input[0] = PinState[0] == State.HIGH;
            Chip.Input[1] = PinState[3] == State.HIGH;
            Chip.Input[2] = PinState[11] == State.HIGH;
            Chip.Input[3] = PinState[12] == State.HIGH;
            Chip.Input[4] = PinState[7] == State.HIGH;
            Chip.Input[5] = PinState[10] == State.HIGH;
            Chip.Input[6] = PinState[8] == State.HIGH;
            Chip.Input[7] = PinState[9] == State.HIGH;

            Chip.Update(reset);

            PinState[2] = Chip.Output[0] ? State.HIGH : State.LOW;
            PinState[1] = Chip.Output[1] ? State.HIGH : State.LOW;
            PinState[4] = Chip.Output[2] ? State.HIGH : State.LOW;
            PinState[5] = Chip.Output[3] ? State.HIGH : State.LOW;
        }

        protected override void InternalReset(bool disable)
        {
            InternalUpdate(true);
            base.InternalReset(disable);
        }

    }
}
