using System.Collections;
using System.Runtime.Serialization;

/**
 * Dual D Flip Flop
 */
namespace IntegratedCircuits
{
    [DataContract]
    public class PQ121_DFf : IntegratedCircuit
    {

        [DataMember]
        private readonly Chips.Dual_D_FlipFlop Chip;

        public PQ121_DFf() : base(14)
        {
            IcType = ICType.ic4;
            ModelName = "IC14";

            Chip = new Chips.Dual_D_FlipFlop();

            PinModes[4] = PinMode.Ouput;
            PinModes[5] = PinMode.Ouput;
            PinModes[7] = PinMode.Ouput;
            PinModes[8] = PinMode.Ouput;            
        }

        protected override void InternalUpdate(bool reset)
        {
            Chip.Input[0] = PinState[1] == State.HIGH;
            Chip.Input[1] = PinState[11] == State.HIGH;
            Chip.Input[2] = PinState[2] == State.HIGH;
            Chip.Input[3] = PinState[10] == State.HIGH;
            Chip.Input[4] = PinState[0] == State.HIGH;
            Chip.Input[5] = PinState[12] == State.HIGH;
            Chip.Input[6] = PinState[3] == State.HIGH;
            Chip.Input[7] = PinState[9] == State.HIGH;

            Chip.Update(reset);

            PinState[4] = Chip.Output[0] ? State.HIGH : State.LOW;
            PinState[5] = Chip.Output[2] ? State.HIGH : State.LOW;
            PinState[7] = Chip.Output[3] ? State.HIGH : State.LOW;
            PinState[8] = Chip.Output[1] ? State.HIGH : State.LOW;
        }

        protected override void InternalReset(bool disable)
        {
            InternalUpdate(true);
            base.InternalReset(disable);
        }

    }
}
