using System.Runtime.Serialization;

/**
 * 4-Bit Binary Counter
 */
namespace IntegratedCircuits
{
    [DataContract]
    public class PQ150G : IntegratedCircuit
    {
        [DataMember]
        private readonly Chips.Binary_Counter_4Bit Chip;

        public PQ150G() : base(8)
        {
            IcType = ICType.ic4;
            ModelName = "IC8";

            Chip = new Chips.Binary_Counter_4Bit();

            PinModes[2] = PinMode.Ouput;
            PinModes[4] = PinMode.Ouput;
            PinModes[5] = PinMode.Ouput;
            PinModes[6] = PinMode.Ouput;

            Chip.Input[2] = true;
        }

        protected override void InternalUpdate(bool reset)
        {
            Chip.Input[0] = PinState[0] == State.HIGH ? true : false;
            Chip.Input[1] = PinState[1] == State.HIGH ? true : false;
            Chip.Update(reset);
            PinState[2] = Chip.Output[0] ? State.HIGH : State.LOW;
            PinState[4] = Chip.Output[1] ? State.HIGH : State.LOW;
            PinState[5] = Chip.Output[2] ? State.HIGH : State.LOW;
            PinState[6] = Chip.Output[3] ? State.HIGH : State.LOW;
        }

        protected override void InternalReset(bool disable)
        {
            Chip.Input[2] = false;
            InternalUpdate(true);
            Chip.Input[2] = true;
            base.InternalReset(disable);            
        }

    }
}

