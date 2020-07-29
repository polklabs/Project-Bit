using System.Runtime.Serialization;


namespace IntegratedCircuits
{
    [DataContract]
    public class PQ130G_Adder4Bit : IntegratedCircuit
    {
        [DataMember]
        private readonly Chips.Binary_Adder_4Bit Chip;

        public PQ130G_Adder4Bit() : base(16)
        {
            IcType      = ICType.ic4;
            ModelName   = "IC16";

            Chip = new Chips.Binary_Adder_4Bit();

            PinModes[0]     = PinMode.Ouput;
            PinModes[3]     = PinMode.Ouput;
            PinModes[8]     = PinMode.Ouput;
            PinModes[9]     = PinMode.Ouput;
            PinModes[12]    = PinMode.Ouput;            
        }

        protected override void InternalUpdate()
        {
            //Inputs
            Chip.Input[0] = PinState[10] == State.HIGH ? true : false; //B4
            Chip.Input[1] = PinState[11] == State.HIGH ? true : false; //A4

            Chip.Input[2] = PinState[14] == State.HIGH ? true : false; //B3
            Chip.Input[3] = PinState[13] == State.HIGH ? true : false; //A3

            Chip.Input[4] = PinState[1] == State.HIGH ? true : false; //B2
            Chip.Input[5] = PinState[2] == State.HIGH ? true : false; //A2

            Chip.Input[6] = PinState[5] == State.HIGH ? true : false; //B1
            Chip.Input[7] = PinState[4] == State.HIGH ? true : false; //A1
            Chip.Input[8] = PinState[6] == State.HIGH ? true : false; //CI

            Chip.Update();

            //Outputs
            PinState[8]     = Chip.Output[0] ? State.HIGH : State.LOW; //CO
            PinState[9]     = Chip.Output[1] ? State.HIGH : State.LOW; //S1
            PinState[12]    = Chip.Output[2] ? State.HIGH : State.LOW; //S2
            PinState[0]     = Chip.Output[3] ? State.HIGH : State.LOW; //S3
            PinState[3]     = Chip.Output[4] ? State.HIGH : State.LOW; //S4
        }

        protected override void InternalReset(bool disable)
        {
            InternalUpdate();
            base.InternalReset(disable);
        }

    }
}
