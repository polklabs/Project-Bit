using System.Runtime.Serialization;

namespace IntegratedCircuits
{
    [DataContract]
    public class PQ142_Sel2To1 : IntegratedCircuit
    {
        [DataMember]
        private readonly Chips.Quad_2_To_1_Selector Chip;

        public PQ142_Sel2To1() : base(16)
        {
            IcType = ICType.ic4;
            ModelName = "IC16";

            Chip = new Chips.Quad_2_To_1_Selector();

            PinModes[3] = PinMode.Ouput;
            PinModes[6] = PinMode.Ouput;
            PinModes[8] = PinMode.Ouput;
            PinModes[11] = PinMode.Ouput;            
        }

        protected override void InternalUpdate(bool reset)
        {
            Chip.Input[0] = PinState[1] == State.HIGH;
            Chip.Input[1] = PinState[4] == State.HIGH;
            Chip.Input[2] = PinState[10] == State.HIGH;
            Chip.Input[3] = PinState[13] == State.HIGH;
            Chip.Input[4] = PinState[2] == State.HIGH;
            Chip.Input[5] = PinState[5] == State.HIGH;
            Chip.Input[6] = PinState[9] == State.HIGH;
            Chip.Input[7] = PinState[12] == State.HIGH;
            Chip.Input[8] = PinState[0] == State.HIGH;
            Chip.Input[9] = PinState[14] == State.HIGH;

            Chip.Update(reset);

            PinState[3] = Chip.Output[0] ? State.HIGH : State.LOW;
            PinState[6] = Chip.Output[1] ? State.HIGH : State.LOW;
            PinState[8] = Chip.Output[2] ? State.HIGH : State.LOW;
            PinState[11] = Chip.Output[3] ? State.HIGH : State.LOW;            
        }

        protected override void InternalReset(bool disable)
        {
            InternalUpdate(true);
            base.InternalReset(disable);
        }

    }
}

