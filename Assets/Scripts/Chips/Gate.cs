using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Gates
{
    [DataContract]
    public abstract class Gate
    {
        [DataMember]
        public Guid ID { get; set; }
       
        [DataMember]
        public BitArray Input { get; set; }

        [DataMember]
        public bool Output { get; set; }

        [DataMember]
        protected bool Dirty;
        private bool oldOutput;

        /// <summary>
        /// Gate index on a chip 
        /// </summary>
        [IgnoreDataMember]
        public int index;

        public Gate(int inputs)
        {
            ID = Guid.NewGuid();

            Input = new BitArray(inputs, false);
            Output = false;
            Dirty = false;
        }

        public void Reset()
        {
            Input.SetAll(false);
            Output = false;
        }

        public bool IsDirty()
        {
            return Dirty;
        }
        public void SetClean()
        {
            oldOutput = Output;
        }

        public Gate SetInputBit(int index, bool value)
        {
            if (index >= 0 && Input.Length > index)
            {
                Input[index] = value;
            }

            return this;
        }
        public Gate SetInputBits(BitArray input)
        {
            if (input.Length == Input.Length)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    SetInputBit(i, input[i]);
                }
            }

            return this;
        }

        public void Update(bool ScrubOutput)
        {
            if (ScrubOutput)
            {
                SetClean();
            }

            Output = GetOutput();

            Dirty = oldOutput != Output;
        }

        protected virtual bool GetOutput() { return false; }
    }

    public class ANDGate : Gate
    {
        public ANDGate(int inputs = 2) : base(inputs) { }

        protected override bool GetOutput()
        {
            bool output = Input[0];
            foreach (bool bit in Input)
            {
                output &= bit;
            }
            return output;
        }
    }

    public class NANDGate : Gate
    {
        public NANDGate(int inputs = 2) : base(inputs) { }

        protected override bool GetOutput()
        {
            bool output = Input[0];
            foreach (bool bit in Input)
            {
                output &= bit;
            }
            return !output;
        }
    }

    public class ORGate : Gate
    {
        public ORGate(int inputs = 2) : base(inputs) { }

        protected override bool GetOutput()
        {
            bool output = Input[0];
            foreach (bool bit in Input)
            {
                output |= bit;
            }
            return output;
        }
    }

    public class NORGate : Gate
    {
        public NORGate(int inputs = 2) : base(inputs) { }

        protected override bool GetOutput()
        {
            bool output = Input[0];
            foreach (bool bit in Input)
            {
                output |= bit;
            }
            return !output;
        }
    }

    public class XORGate : Gate
    {
        public XORGate(int inputs = 2) : base(inputs) { }

        protected override bool GetOutput()
        {
            bool output = Input[0];
            for (int i = 1; i < Input.Length; i++)
            {
                output ^= Input[i];
            }
            return output;
        }
    }

    public class XNORGate : Gate
    {
        public XNORGate(int inputs = 2) : base(inputs) { }

        protected override bool GetOutput()
        {
            bool output = Input[0];
            for (int i = 1; i < Input.Length; i++)
            {
                output ^= Input[i];
            }
            return !output;
        }
    }

    public class BufferGate : Gate
    {
        public BufferGate() : base(1) { }

        protected override bool GetOutput()
        {
            return Input[0];
        }
    }

    public class TriBufferGate : Gate
    {
        public TriBufferGate() : base(2) { }

        protected override bool GetOutput()
        {
            return Input[0] & Input[1];
        }
    }

    public class NotGate : Gate
    {
        public NotGate() : base(1) { }

        protected override bool GetOutput()
        {
            return !Input[0];
        }
    }
}
