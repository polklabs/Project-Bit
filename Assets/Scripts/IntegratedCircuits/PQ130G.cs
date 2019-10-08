using System.Runtime.Serialization;


namespace IntegratedCircuits
{
    [DataContract]
    public class PQ130G : IntegratedCircuit
    {
        [DataMember]
        private readonly Chips.Binary_Adder_4Bit Chip;

        public PQ130G() : base(16)
        {
            IcType = ICType.ic4;
            ModelName = "IC16";

            Chip = new Chips.Binary_Adder_4Bit();

            PinModes[1] = PinMode.Ouput;
            PinModes[2] = PinMode.Ouput;
            PinModes[4] = PinMode.Ouput;
            PinModes[5] = PinMode.Ouput;
            PinModes[6] = PinMode.Ouput;
            PinModes[10] = PinMode.Ouput;
            PinModes[11] = PinMode.Ouput;
            PinModes[13] = PinMode.Ouput;
            PinModes[14] = PinMode.Ouput;
        }

        protected override void InternalUpdate()
        {
            //Inputs
            Chip.Input[0] = PinState[10] == State.HIGH ? true : false;
            Chip.Input[1] = PinState[11] == State.HIGH ? true : false;

            Chip.Input[2] = PinState[14] == State.HIGH ? true : false;
            Chip.Input[3] = PinState[13] == State.HIGH ? true : false;

            Chip.Input[4] = PinState[1] == State.HIGH ? true : false;
            Chip.Input[5] = PinState[2] == State.HIGH ? true : false;

            Chip.Input[6] = PinState[5] == State.HIGH ? true : false;
            Chip.Input[7] = PinState[4] == State.HIGH ? true : false;
            Chip.Input[8] = PinState[6] == State.HIGH ? true : false;

            Chip.Update();

            //Outputs
            PinState[8] = Chip.Output[0] ? State.HIGH : State.LOW;
            PinState[9] = Chip.Output[1] ? State.HIGH : State.LOW;
            PinState[12] = Chip.Output[2] ? State.HIGH : State.LOW;
            PinState[0] = Chip.Output[3] ? State.HIGH : State.LOW;
            PinState[3] = Chip.Output[4] ? State.HIGH : State.LOW;
        }

        protected override void InternalReset(bool disable)
        {
            InternalUpdate();
            base.InternalReset(disable);
        }

    }
}
