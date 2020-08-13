using System.Collections;
using System.Linq;
using System.Runtime.Serialization;

/**
 * 64-bit ram
 */
namespace IntegratedCircuits
{
    [DataContract]
    public class PQ230_Ram64Bit : IntegratedCircuit
    {

        [DataMember]
        private readonly BitArray[] memory;

        public PQ230_Ram64Bit() : base(16)
        {
            IcType = ICType.ic4;
            ModelName = "IC16";

            memory = Enumerable.Repeat(new BitArray(Enumerable.Repeat(false, 4).ToArray()), 16).ToArray();

            PinModes[4] = PinMode.Ouput;
            PinModes[6] = PinMode.Ouput;
            PinModes[8] = PinMode.Ouput;
            PinModes[10] = PinMode.Ouput;
        }

        protected override void InternalUpdate(bool reset)
        {
            if (reset)
            {
                for (int i = 0; i < 16; i++)
                {
                    memory[i] = new BitArray(Enumerable.Repeat(false, 4).ToArray());
                }
            }

            // get word
            BitArray bitArray = new BitArray(new bool[] {PinState[0] == State.HIGH, PinState[14] == State.HIGH, PinState[13] == State.HIGH, PinState[12] == State.HIGH });

            int[] array = new int[1];
            bitArray.CopyTo(array, 0);

            BitArray word = memory[array[0]];

            // write to word
            if (PinState[1] == State.LOW && PinState[2] == State.LOW)
            {
                word[0] = PinState[3] == State.HIGH;
                word[1] = PinState[5] == State.HIGH;
                word[2] = PinState[9] == State.HIGH;
                word[3] = PinState[11] == State.HIGH;

                PinState[4] = State.HIGH;
                PinState[6] = State.HIGH;
                PinState[8] = State.HIGH;
                PinState[10] = State.HIGH;
            } 
            else if (PinState[1] == State.HIGH)
            {
                PinState[4] = State.HIGH;
                PinState[6] = State.HIGH;
                PinState[8] = State.HIGH;
                PinState[10] = State.HIGH;
            }
            else
            {
                PinState[4] = word[0] ? State.HIGH : State.LOW;
                PinState[6] = word[1] ? State.HIGH : State.LOW;
                PinState[8] = word[2] ? State.HIGH : State.LOW;
                PinState[10] = word[3] ? State.HIGH : State.LOW;
            }
        }

        protected override void InternalReset(bool disable)
        {
            InternalUpdate(true);
            base.InternalReset(disable);
        }

    }
}
