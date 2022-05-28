using System.Collections;
using System.Linq;
using System.Runtime.Serialization;

namespace IntegratedCircuits
{
    [DataContract]
    class PQ220_EEPROM16K : IntegratedCircuit
    {
        /// <summary>
        /// Write enable
        /// </summary>
        private const int W_low = 20;

        /// <summary>
        /// Output enable
        /// </summary>
        private const int G_low = 19;

        /// <summary>
        /// Chip enable
        /// </summary>
        private const int E_low = 17;

        [DataMember]
        private BitArray[] memory;

        /// <summary>
        /// Allows clearing the DQ outputs before swapping them to inputs.
        /// </summary>
        [DataMember]
        private bool clearingOutputs = false;

        public PQ220_EEPROM16K() : base(24)
        {
            IcType = ICType.ic6;
            ModelName = "IC24_Wide";

            PinNames = new string[] { "A7", "A6", "A5", "A4", "A3", "A2", "A1", "A0", "DQ0", "DQ1", "DQ2", "GND", "DQ3", "DQ4", "DQ5", "DQ6", "DQ7", "E'", "A10", "G'", "W'", "A9", "A8", "VCC" };
            isEditable = true;

            clearMemory();

            // Will change based on write enable
            SetPinModes(8, 10, PinMode.Ouput);
            SetPinModes(12, 16, PinMode.Ouput);
        }

        private void clearMemory()
        {
            memory = Enumerable.Repeat(new BitArray(Enumerable.Repeat(false, 8).ToArray()), 2048).ToArray();
        }

        private void setOutputOff()
        {
            PinState[8] = State.OFF;
            PinState[9] = State.OFF;
            PinState[10] = State.OFF;
            PinState[12] = State.OFF;
            PinState[13] = State.OFF;
            PinState[14] = State.OFF;
            PinState[15] = State.OFF;
            PinState[16] = State.OFF;
        }

        protected override void InternalUpdate(bool reset)
        {
            // If chip enable is not low then turn off all outputs
            if (PinState[E_low] != State.LOW)
            {
                for(int i = 0; i < Pins; i++)
                {
                    if(GetPinMode(i) == PinMode.Ouput)
                    {
                        PinState[i] = State.OFF;
                    }
                }
                return;
            }

            BitArray address = new BitArray(Enumerable.Repeat(false, 11).ToArray());
            address[0] = PinState[7] == State.HIGH;
            address[1] = PinState[6] == State.HIGH;
            address[2] = PinState[5] == State.HIGH;
            address[3] = PinState[4] == State.HIGH;
            address[4] = PinState[3] == State.HIGH;
            address[5] = PinState[2] == State.HIGH;
            address[6] = PinState[1] == State.HIGH;
            address[7] = PinState[0] == State.HIGH;
            address[8] = PinState[22] == State.HIGH;
            address[9] = PinState[21] == State.HIGH;
            address[10] = PinState[18] == State.HIGH;

            int[] array = new int[1];
            address.CopyTo(array, 0);

            BitArray page = memory[array[0]];

            // write to word
            if (PinState[W_low] == State.LOW)
            {
                if(!clearingOutputs)
                {
                    clearingOutputs = true;
                    // Outputs were swapped so we need to wait for the states to be propagated
                    setOutputOff();
                    BreadBoardRef.UpdateComponent(Id);
                    return;
                }
                clearingOutputs = false;

                SetPinModes(8, 10, PinMode.Input);
                SetPinModes(12, 16, PinMode.Input);

                UpdateState(false, false);

                page[0] = PinState[8] == State.HIGH;
                page[1] = PinState[9] == State.HIGH;
                page[2] = PinState[10] == State.HIGH;
                page[3] = PinState[12] == State.HIGH;
                page[4] = PinState[13] == State.HIGH;
                page[5] = PinState[14] == State.HIGH;
                page[6] = PinState[15] == State.HIGH;
                page[7] = PinState[16] == State.HIGH;
            }
            else if(PinState[G_low] == State.LOW)
            {
                SetPinModes(8, 10, PinMode.Ouput);
                SetPinModes(12, 16, PinMode.Ouput);

                PinState[8] = page[0] ? State.HIGH : State.LOW;
                PinState[9] = page[1] ? State.HIGH : State.LOW;
                PinState[10] = page[2] ? State.HIGH : State.LOW;
                PinState[12] = page[3] ? State.HIGH : State.LOW;
                PinState[13] = page[4] ? State.HIGH : State.LOW;
                PinState[14] = page[5] ? State.HIGH : State.LOW;
                PinState[15] = page[6] ? State.HIGH : State.LOW;
                PinState[16] = page[7] ? State.HIGH : State.LOW;
            }
        }

        protected override void InternalReset(bool disable)
        {
            for (int i = 0; i < Pins; i++)
            {
                if (GetPinMode(i) == PinMode.Ouput)
                {
                    PinState[i] = State.OFF;
                }
            }
            base.InternalReset(disable);
        }
    }
}
