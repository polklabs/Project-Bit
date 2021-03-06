﻿using System.Runtime.Serialization;

namespace IntegratedCircuits
{
    [DataContract]
    public class PQ141_Decode2To4 : IntegratedCircuit
    {
        [DataMember]
        private readonly Chips.Dual_Decode_2_To_4 Chip;

        public PQ141_Decode2To4() : base(16)
        {
            IcType = ICType.ic4;
            ModelName = "IC16";

            Chip = new Chips.Dual_Decode_2_To_4();

            PinModes[3] = PinMode.Ouput;
            PinModes[4] = PinMode.Ouput;
            PinModes[5] = PinMode.Ouput;
            PinModes[6] = PinMode.Ouput;
            PinModes[8] = PinMode.Ouput;
            PinModes[9] = PinMode.Ouput;
            PinModes[10] = PinMode.Ouput;
            PinModes[11] = PinMode.Ouput;            
        }

        protected override void InternalUpdate(bool reset)
        {
            Chip.Input[0] = PinState[1] == State.HIGH;
            Chip.Input[1] = PinState[2] == State.HIGH;
            Chip.Input[2] = PinState[0] == State.HIGH;
            Chip.Input[3] = PinState[13] == State.HIGH;
            Chip.Input[4] = PinState[12] == State.HIGH;
            Chip.Input[5] = PinState[14] == State.HIGH;

            Chip.Update(reset);

            PinState[3] = Chip.Output[0] ? State.HIGH : State.LOW;
            PinState[4] = Chip.Output[1] ? State.HIGH : State.LOW;
            PinState[5] = Chip.Output[2] ? State.HIGH : State.LOW;
            PinState[6] = Chip.Output[3] ? State.HIGH : State.LOW;
            PinState[8] = Chip.Output[4] ? State.HIGH : State.LOW;
            PinState[9] = Chip.Output[5] ? State.HIGH : State.LOW;
            PinState[10] = Chip.Output[6] ? State.HIGH : State.LOW;
            PinState[11] = Chip.Output[7] ? State.HIGH : State.LOW;            
        }

        protected override void InternalReset(bool disable)
        {
            InternalUpdate(true);
            base.InternalReset(disable);
        }

    }
}

