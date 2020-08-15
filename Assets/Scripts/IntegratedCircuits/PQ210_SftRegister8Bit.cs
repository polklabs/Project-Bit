using System.Collections;
using System.Linq;
using System.Runtime.Serialization;

namespace IntegratedCircuits
{
    [DataContract]
    public class PQ210_SftRegister8Bit : IntegratedCircuit
    {
        [DataMember]
        private BitArray ShiftRegister;

        [DataMember]
        private BitArray Register;

        public PQ210_SftRegister8Bit() : base(16)
        {
            IcType = ICType.ic4;
            ModelName = "IC16";

            ShiftRegister = new BitArray(Enumerable.Repeat(false, 8).ToArray());
            Register = new BitArray(Enumerable.Repeat(false, 8).ToArray());

            PinModes[0] = PinMode.Ouput;
            PinModes[1] = PinMode.Ouput;
            PinModes[2] = PinMode.Ouput;
            PinModes[3] = PinMode.Ouput;
            PinModes[4] = PinMode.Ouput;
            PinModes[5] = PinMode.Ouput;
            PinModes[6] = PinMode.Ouput;
            PinModes[8] = PinMode.Ouput;
            PinModes[14] = PinMode.Ouput;            
        }

        protected override void InternalUpdate(bool reset)
        {
            if(oldState[11] != State.HIGH && PinState[11] == State.HIGH)
            {
                Register = (BitArray)ShiftRegister.Clone();
            }

            if (oldState[10] != State.HIGH && PinState[10] == State.HIGH)
            {
                for (int i = ShiftRegister.Length - 1; i > 0; i--)
                {
                    ShiftRegister[i] = ShiftRegister[i - 1];
                }
                ShiftRegister[0] = PinState[13] == State.HIGH;
            }

            if (PinState[9] != State.HIGH)
            {
                ShiftRegister = new BitArray(Enumerable.Repeat(false, 8).ToArray());
                Register = new BitArray(Enumerable.Repeat(false, 8).ToArray());
            }           

            SetOutput();
        }

        protected override void InternalReset(bool disable)
        {
            ShiftRegister = new BitArray(Enumerable.Repeat(false, 8).ToArray());
            Register = new BitArray(Enumerable.Repeat(false, 8).ToArray());
            SetOutput();
            base.InternalReset(disable);
        }

        private void SetOutput()
        {
            if (PinState[12] != State.HIGH) // OE
            {
                PinState[14] = Register[0] ? State.HIGH : State.LOW; // QA
                PinState[0] = Register[1] ? State.HIGH : State.LOW; // QB
                PinState[1] = Register[2] ? State.HIGH : State.LOW; // QC
                PinState[2] = Register[3] ? State.HIGH : State.LOW; // QD
                PinState[3] = Register[4] ? State.HIGH : State.LOW; // QE
                PinState[4] = Register[5] ? State.HIGH : State.LOW; // QF
                PinState[5] = Register[6] ? State.HIGH : State.LOW; // QG
                PinState[6] = Register[7] ? State.HIGH : State.LOW; // QH
                PinState[8] = ShiftRegister[7] ? State.LOW : State.HIGH; // QH'
            } 
            else
            {
                PinState[14] = State.LOW; // QA
                PinState[0] = State.LOW; // QB
                PinState[1] = State.LOW; // QC
                PinState[2] = State.LOW; // QD
                PinState[3] = State.LOW; // QE
                PinState[4] = State.LOW; // QF
                PinState[5] = State.LOW; // QG
                PinState[6] = State.LOW; // QH
                PinState[8] = ShiftRegister[7] ? State.LOW : State.HIGH; // QH'
            }
        }

    }
}

