﻿using System.Runtime.Serialization;

namespace IntegratedCircuits
{
    [DataContract]
    public class PQ110_BcdToDec : IntegratedCircuit
    {
        [DataMember]
        private readonly Chips.Bcd_To_Dec Chip;

        public PQ110_BcdToDec() : base(16)
        {
            IcType = ICType.ic4;
            ModelName = "IC16";

            Chip = new Chips.Bcd_To_Dec();

            PinModes[0] = PinMode.Ouput;
            PinModes[1] = PinMode.Ouput;
            PinModes[2] = PinMode.Ouput;
            PinModes[3] = PinMode.Ouput;
            PinModes[4] = PinMode.Ouput;
            PinModes[5] = PinMode.Ouput;
            PinModes[6] = PinMode.Ouput;
            PinModes[8] = PinMode.Ouput;
            PinModes[9] = PinMode.Ouput;
            PinModes[10] = PinMode.Ouput;
        }

        protected override void InternalUpdate(bool reset)
        {
            Chip.Input[0] = PinState[14] == State.HIGH;
            Chip.Input[1] = PinState[13] == State.HIGH;
            Chip.Input[2] = PinState[12] == State.HIGH;
            Chip.Input[3] = PinState[11] == State.HIGH;

            Chip.Update(reset);

            PinState[0] = Chip.Output[0] ? State.HIGH : State.LOW;
            PinState[1] = Chip.Output[1] ? State.HIGH : State.LOW;
            PinState[2] = Chip.Output[2] ? State.HIGH : State.LOW;
            PinState[3] = Chip.Output[3] ? State.HIGH : State.LOW;
            PinState[4] = Chip.Output[4] ? State.HIGH : State.LOW;
            PinState[5] = Chip.Output[5] ? State.HIGH : State.LOW;
            PinState[6] = Chip.Output[6] ? State.HIGH : State.LOW;
            PinState[8] = Chip.Output[7] ? State.HIGH : State.LOW;
            PinState[9] = Chip.Output[8] ? State.HIGH : State.LOW;
            PinState[10] = Chip.Output[9] ? State.HIGH : State.LOW;
        }

        protected override void InternalReset(bool disable)
        {
            InternalUpdate(true);
            base.InternalReset(disable);
        }

    }
}

