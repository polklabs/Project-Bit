using System.Runtime.Serialization;

namespace IntegratedCircuits
{
    [DataContract]
    public class PQ140_Decode3To8 : IntegratedCircuit
    {
        [DataMember]
        private readonly Chips.Decoder_3_To_8 Chip;

        public PQ140_Decode3To8() : base(14)
        {
            IcType = ICType.ic4;
            ModelName = "IC14";

            Chip = new Chips.Decoder_3_To_8();

            PinModes[4] = PinMode.Ouput;
            PinModes[5] = PinMode.Ouput;
            PinModes[7] = PinMode.Ouput;
            PinModes[8] = PinMode.Ouput;
            PinModes[9] = PinMode.Ouput;
            PinModes[10] = PinMode.Ouput;
            PinModes[11] = PinMode.Ouput;
            PinModes[12] = PinMode.Ouput;           
        }

        protected override void InternalUpdate(bool reset)
        {
            Chip.Input[0] = PinState[0] == State.HIGH;
            Chip.Input[1] = PinState[1] == State.HIGH;
            Chip.Input[2] = PinState[2] == State.HIGH;
            Chip.Input[3] = PinState[3] == State.HIGH;

            Chip.Update(reset);

            PinState[4] = Chip.Output[0] ? State.HIGH : State.LOW;
            PinState[5] = Chip.Output[1] ? State.HIGH : State.LOW;
            PinState[7] = Chip.Output[2] ? State.HIGH : State.LOW;
            PinState[8] = Chip.Output[3] ? State.HIGH : State.LOW;
            PinState[9] = Chip.Output[4] ? State.HIGH : State.LOW;
            PinState[10] = Chip.Output[5] ? State.HIGH : State.LOW;
            PinState[11] = Chip.Output[6] ? State.HIGH : State.LOW;
            PinState[12] = Chip.Output[7] ? State.HIGH : State.LOW;            
        }

        protected override void InternalReset(bool disable)
        {
            InternalUpdate(true);
            base.InternalReset(disable);
        }

    }
}

