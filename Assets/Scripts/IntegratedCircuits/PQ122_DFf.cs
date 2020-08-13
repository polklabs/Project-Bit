using System.Collections;
using System.Runtime.Serialization;

/**
 * Octal D Flip Flop
 */
namespace IntegratedCircuits
{
    [DataContract]
    public class PQ122_DFf : IntegratedCircuit
    {

        [DataMember]
        private readonly Chips.Octal_D_FlipFlop Chip;

        public PQ122_DFf() : base(20)
        {
            IcType = ICType.ic4;
            ModelName = "IC20";

            Chip = new Chips.Octal_D_FlipFlop();

            PinModes[1]  = PinMode.Ouput;
            PinModes[4]  = PinMode.Ouput;
            PinModes[5]  = PinMode.Ouput;
            PinModes[8]  = PinMode.Ouput;
            PinModes[11] = PinMode.Ouput;
            PinModes[14] = PinMode.Ouput;
            PinModes[15] = PinMode.Ouput;
            PinModes[18] = PinMode.Ouput;
        }

        protected override void InternalUpdate(bool reset)
        {
            Chip.Input[0] = PinState[2] == State.HIGH;
            Chip.Input[1] = PinState[3] == State.HIGH;
            Chip.Input[2] = PinState[6] == State.HIGH;
            Chip.Input[3] = PinState[7] == State.HIGH;
            Chip.Input[4] = PinState[12] == State.HIGH;
            Chip.Input[5] = PinState[13] == State.HIGH;
            Chip.Input[6] = PinState[16] == State.HIGH;
            Chip.Input[7] = PinState[17] == State.HIGH;

            Chip.Input[8] = PinState[10] == State.HIGH;
            Chip.Input[9] = PinState[0] == State.HIGH;

            Chip.Update(reset);

            PinState[1] = Chip.Output[0] ? State.HIGH : State.LOW;
            PinState[4] = Chip.Output[1] ? State.HIGH : State.LOW;
            PinState[5] = Chip.Output[2] ? State.HIGH : State.LOW;
            PinState[8] = Chip.Output[3] ? State.HIGH : State.LOW;
            PinState[11] = Chip.Output[4] ? State.HIGH : State.LOW;
            PinState[14] = Chip.Output[5] ? State.HIGH : State.LOW;
            PinState[15] = Chip.Output[6] ? State.HIGH : State.LOW;
            PinState[18] = Chip.Output[7] ? State.HIGH : State.LOW;
        }

        protected override void InternalReset(bool disable)
        {
            InternalUpdate(true);
            base.InternalReset(disable);
        }

    }
}
