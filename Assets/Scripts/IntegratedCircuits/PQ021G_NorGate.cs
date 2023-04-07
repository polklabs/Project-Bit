using System.Runtime.Serialization;

namespace IntegratedCircuits
{
    // 74LS02
    [DataContract]
    public class PQ021G_NorGate : IntegratedCircuit
    {
        [DataMember]
        private readonly Chips.Quad_Nor_Gates Chip;

        public PQ021G_NorGate() : base(14)
        {
            IcType = ICType.ic4;
            ModelName = "IC14";

            PinNames = new string[] { "Y1", "A1", "B1", "Y2", "A2", "B2", "GND", "A3", "B3", "Y3", "A4", "B4", "Y4", "VCC" };

            Chip = new Chips.Quad_Nor_Gates();

            SetPinModes(2, PinMode.Output);
            SetPinModes(5, PinMode.Output);
            SetPinModes(7, PinMode.Output);
            SetPinModes(10, PinMode.Output);
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
