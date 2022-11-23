using System.Runtime.Serialization;

namespace IntegratedCircuits
{
    [DataContract]
    class PQ211_Register4Bit : IntegratedCircuit
    {
        [DataMember]
        private readonly Chips.D_Register_4Bit Chip;

        public PQ211_Register4Bit(): base(16)
        {
            IcType = ICType.ic4;
            ModelName = "IC16";

            PinNames = new string[] { "M", "N", "Q1", "Q2", "Q3", "Q4", "CLK", "GND", "G1'", "G2'", "D4", "D3", "D2", "D1", "CLR", "VCC" };

            Chip = new Chips.D_Register_4Bit();

            SetPinModes(2, 5, PinMode.Output);
            DefaultState[10] = State.HIGH;
            DefaultState[11] = State.HIGH;
            DefaultState[12] = State.HIGH;
            DefaultState[13] = State.HIGH;
        }

        protected override void InternalUpdate(bool reset)
        {
            Chip.Input[0] = PinState[0] == State.HIGH ? true : false;
            Chip.Input[1] = PinState[1] == State.HIGH ? true : false;
            Chip.Input[2] = PinState[13] == State.HIGH ? true : false;
            Chip.Input[3] = PinState[8] == State.HIGH ? true : false;
            Chip.Input[4] = PinState[9] == State.HIGH ? true : false;
            Chip.Input[5] = PinState[12] == State.HIGH ? true : false;
            Chip.Input[6] = PinState[6] == State.HIGH ? true : false;
            Chip.Input[7] = PinState[11] == State.HIGH ? true : false;
            Chip.Input[8] = PinState[10] == State.HIGH ? true : false;
            Chip.Input[9] = PinState[14] == State.HIGH ? true : false;

            Chip.Update();

            PinState[2] = Chip.Output[0] ? State.HIGH : State.LOW;
            PinState[3] = Chip.Output[1] ? State.HIGH : State.LOW;
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
