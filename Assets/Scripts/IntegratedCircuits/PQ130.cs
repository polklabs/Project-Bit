using System.Runtime.Serialization;

/**
 * 4 Bit Full Adder
 */
namespace IntegratedCircuits
{
    public class PQ130G : IntegratedCircuit
    {

        public PQ130G() : base(16)
        {
            IcType = ICType.ic4;
            ModelName = "IC16";

            PinModes[1]     = PinMode.Ouput;
            PinModes[2]     = PinMode.Ouput;
            PinModes[4]     = PinMode.Ouput;
            PinModes[5]     = PinMode.Ouput;
            PinModes[6]     = PinMode.Ouput;
            PinModes[10]    = PinMode.Ouput;
            PinModes[11]    = PinMode.Ouput;
            PinModes[13]    = PinMode.Ouput;
            PinModes[14]    = PinMode.Ouput;
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
         
            a.copyTo(ab, 0);
            b.copyTo(ab, 1);
         
            int output = ab[0] + ab[1] + (PinState[6] == State.HIGH ? 1 : 0);
         
            BitArray bitOut = new BitArray(new byte[] { output });
         
            //Outputs
            PinState[8]     = bitOut[4] ? State.HIGH : State.LOW; //C4
            PinState[3]     = bitOut[0] ? State.HIGH : State.LOW; //S1
            PinState[0]     = bitOut[1] ? State.HIGH : State.LOW; //S2
            PinState[12]    = bitOut[2] ? State.HIGH : State.LOW; //S3
            PinState[9]     = bitOut[3] ? State.HIGH : State.LOW; //S4
        }

    }
}
