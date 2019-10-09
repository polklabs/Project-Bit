using System.Collections;
using System.Runtime.Serialization;

/**
 * 4 Bit Full Adder
 */
namespace IntegratedCircuits
{
    public class PQ130 : IntegratedCircuit
    {

        public PQ130() : base(16)
        {
            IcType = ICType.ic4;
            ModelName = "IC16";

            PinModes[0]     = PinMode.Ouput;
            PinModes[3]     = PinMode.Ouput;
            PinModes[8]     = PinMode.Ouput;
            PinModes[9]     = PinMode.Ouput;
            PinModes[12]     = PinMode.Ouput;            
        }

        protected override void InternalUpdate()
        {
            BitArray a = new BitArray(new bool[4] 
            { 
              PinState[4]   == State.HIGH ? true : false,
              PinState[2]   == State.HIGH ? true : false,
              PinState[13]  == State.HIGH ? true : false,
              PinState[11]  == State.HIGH ? true : false
            });
            BitArray b = new BitArray(new bool[4] 
            { 
              PinState[5]   == State.HIGH ? true : false,
              PinState[1]   == State.HIGH ? true : false,
              PinState[14]  == State.HIGH ? true : false,
              PinState[10]  == State.HIGH ? true : false
            });
            int[] ab = new int[2];
         
            a.CopyTo(ab, 0);
            b.CopyTo(ab, 1);
         
            int output = ab[0] + ab[1] + (PinState[6] == State.HIGH ? 1 : 0);
         
            BitArray bitOut = new BitArray(System.BitConverter.GetBytes(output));
         
            //Outputs
            PinState[8]     = bitOut[4] ? State.HIGH : State.LOW; //C4
            PinState[3]     = bitOut[0] ? State.HIGH : State.LOW; //S1
            PinState[0]     = bitOut[1] ? State.HIGH : State.LOW; //S2
            PinState[12]    = bitOut[2] ? State.HIGH : State.LOW; //S3
            PinState[9]     = bitOut[3] ? State.HIGH : State.LOW; //S4
        }

    }
}
