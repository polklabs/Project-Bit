using System.Runtime.Serialization;

namespace IntegratedCircuits
{
    [DataContract]
    public class PQ111_BcdToSeg : IntegratedCircuit
    {
        [DataMember]
        private readonly Chips.Bcd_To_Seg Chip;

        public PQ111_BcdToSeg() : base(16)
        {
            IcType = ICType.ic4;
            ModelName = "IC16";

            Chip = new Chips.Bcd_To_Seg();

            PinModes[14] = PinMode.Ouput;
            PinModes[13] = PinMode.Ouput;
            PinModes[12] = PinMode.Ouput;
            PinModes[11] = PinMode.Ouput;
            PinModes[10] = PinMode.Ouput;
            PinModes[9] = PinMode.Ouput;
            PinModes[8] = PinMode.Ouput;
        }

        protected override void InternalUpdate()
        {
            Chip.Input[3] = PinState[6] == State.HIGH ? true : false;
            Chip.Input[2] = PinState[0] == State.HIGH ? true : false;
            Chip.Input[1] = PinState[1] == State.HIGH ? true : false;
            Chip.Input[0] = PinState[5] == State.HIGH ? true : false;

            Chip.Update();

            PinState[12] = Chip.Output[0] ? State.HIGH : State.LOW;
            PinState[11] = Chip.Output[1] ? State.HIGH : State.LOW;
            PinState[10] = Chip.Output[2] ? State.HIGH : State.LOW;
            PinState[9] = Chip.Output[3] ? State.HIGH : State.LOW;
            PinState[8] = Chip.Output[4] ? State.HIGH : State.LOW;
            PinState[14] = Chip.Output[5] ? State.HIGH : State.LOW;
            PinState[13] = Chip.Output[6] ? State.HIGH : State.LOW;
        }

        protected override void InternalReset(bool disable)
        {
            InternalUpdate();
            base.InternalReset(disable);
        }

    }
}

