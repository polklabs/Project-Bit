using System.Runtime.Serialization;

namespace IntegratedCircuits
{
    [DataContract]
    public class PQ011G_NandGate : IntegratedCircuit
    {
        [DataMember]
        private readonly Chips.Quad_Nand_Gates Chip;

        public PQ011G_NandGate() : base(14)
        {
            IcType = ICType.ic4;
            ModelName = "IC14";

            Chip = new Chips.Quad_Nand_Gates();

            PinModes[2] = PinMode.Ouput;
            PinModes[5] = PinMode.Ouput;
            PinModes[7] = PinMode.Ouput;
            PinModes[10] = PinMode.Ouput;
        }

        protected override void InternalUpdate(bool reset)
        {
            Chip.Input[0] = PinState[0] == State.HIGH ? true : false;
            Chip.Input[1] = PinState[1] == State.HIGH ? true : false;
            Chip.Input[2] = PinState[3] == State.HIGH ? true : false;
            Chip.Input[3] = PinState[4] == State.HIGH ? true : false;
            Chip.Input[4] = PinState[8] == State.HIGH ? true : false;
            Chip.Input[5] = PinState[9] == State.HIGH ? true : false;
            Chip.Input[6] = PinState[11] == State.HIGH ? true : false;
            Chip.Input[7] = PinState[12] == State.HIGH ? true : false;

            Chip.Update();

            PinState[2] = Chip.Output[0] ? State.HIGH : State.LOW;
            PinState[5] = Chip.Output[1] ? State.HIGH : State.LOW;
            PinState[7] = Chip.Output[2] ? State.HIGH : State.LOW;
            PinState[10] = Chip.Output[3] ? State.HIGH : State.LOW;
        }

        protected override void InternalReset(bool disable)
        {
            InternalUpdate(true);
            base.InternalReset(disable);
        }

    }
}
