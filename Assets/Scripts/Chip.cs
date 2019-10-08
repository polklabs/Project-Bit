using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Gates;
using UnityEngine;

namespace Chips
{
    [DataContract]
    public class Wire
    {
        [DataMember]
        public bool IsChip { get; set; }
        [DataMember]
        public int CircuitIndex { get; set; }
        [DataMember]
        public int FromIndex { get; set; }
        [DataMember]
        public int ToIndex { get; set; }

        public Wire(int fromIndex, int toIndex, int circuitIndex, bool isChip)
        {
            IsChip = isChip;
            CircuitIndex = circuitIndex;
            FromIndex = fromIndex;
            ToIndex = toIndex;
        }
    }

    [DataContract]
    public class Chip
    {
        [DataMember]
        public Guid ID { get; set; }
       
        public BitArray Input { get; set; }
        [DataMember]
        private bool[] _Input {
            get {
                bool[] b = new bool[Input.Length];
                Input.CopyTo(b, 0);
                return b;
            }
            set
            {
                Input = new BitArray(value);
            }
        }        

        public BitArray Output { get; set; }
        [DataMember]
        private bool[] _Output {
            get
            {
                bool[] b = new bool[Output.Length];
                Output.CopyTo(b, 0);
                return b;
            }
            set
            {
                Output = new BitArray(value);
            }
        }

        [DataMember]
        public List<Gate> Gates { get; set; }
        [DataMember]
        public List<Chip> Chips { get; set; }
        [DataMember]
        public Dictionary<Guid, List<Wire>> WireDict { get; set; }

        
        protected BitArray Dirty { get; set; }
        [DataMember]
        private bool[] _Dirty
        {
            get
            {
                bool[] b = new bool[Dirty.Length];
                Dirty.CopyTo(b, 0);
                return b;
            }
            set
            {
                Dirty = new BitArray(value);
            }
        }

        protected Chip(int inputs, int outputs)
        {
            ID = Guid.NewGuid();

            Input = new BitArray(inputs, false);
            Output = new BitArray(outputs, false);
            Dirty = new BitArray(outputs, false);

            Gates = new List<Gate>();
            Chips = new List<Chip>();
            WireDict = new Dictionary<Guid, List<Wire>>();
        }

        protected int AddGate(Gate gate)
        {
            Gates.Add(gate);
            return Gates.Count - 1;
        }

        protected int AddChip(Chip chip)
        {
            Chips.Add(chip);
            return Chips.Count - 1;
        }

        protected void AddWire(Guid id, Wire wire)
        {
            if (!WireDict.ContainsKey(id))
            {
                WireDict.Add(id, new List<Wire>());
            }
            WireDict[id].Add(wire);
        }

        public void Reset()
        {
            Input.SetAll(false);
            Output.SetAll(false);

            foreach(Chip chip in Chips)
            {
                chip.Reset();
            }
            foreach(Gate gate in Gates)
            {
                gate.Reset();
            }

        }

        public Chip SetInputBit(int index, bool value)
        {
            if (index >= 0 && Input.Length > index)
            {
                Input[index] = value;
            }

            return this;
        }
        public Chip SetInputBits(BitArray input)
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

        public BitArray Update() { return Update(false); }
        public BitArray Update(bool forceUpdate)
        {
            BitArray oldOutput = (BitArray)Output.Clone();

            GetOutput(forceUpdate);

            Dirty = Dirty.Or(oldOutput.Xor(Output));

            return Output;
        }

        protected virtual void GetOutput(bool forceUpdate)
        {
            int loops = 0;
            Queue<Guid> queue = new Queue<Guid>();

            foreach (Chip chip in Chips)
            {
                chip.Dirty.SetAll(false);
            }

            foreach (Wire wire in WireDict[ID])
            {
                if (wire.IsChip)
                {
                    Chip chip = Chips[wire.CircuitIndex];
                    chip.SetInputBit(wire.ToIndex, Input[wire.FromIndex]);
                    chip.Update(forceUpdate);

                    if (forceUpdate || GetCardinality(chip.Dirty) > 0)
                    {
                        if (!queue.Contains(chip.ID))
                        {
                            queue.Enqueue(chip.ID);
                        }
                    }
                }
                else
                {
                    Gate gate = Gates[wire.CircuitIndex];
                    gate.SetInputBit(wire.ToIndex, Input[wire.FromIndex]);
                    gate.Update();

                    if (forceUpdate || gate.IsDirty())
                    {
                        if (!queue.Contains(gate.ID))
                        {
                            queue.Enqueue(gate.ID);
                        }
                    }

                }
            }

            while (queue.Count > 0)
            {
                //Stop infinite loops from continuing
                loops++;
                if (loops >= 1000)
                {
                    Console.WriteLine("Infinite loop, breaking");
                    return;
                }

                Guid guid = queue.Dequeue();

                BitArray FromValues;
				BitArray FromDirty;

                int findIndex = Gates.FindIndex(x => x.ID == guid);
                if (findIndex != -1)
                {					
                    FromValues = new BitArray(1, Gates[findIndex].Output);
					FromDirty = new BitArray(1, Gates[findIndex].IsDirty());
                }
                else
                {
					Chip c = Chips.Find(x => x.ID == guid);
                    FromValues = c.Output;
					FromDirty = c.Dirty;
                }

                foreach (Wire wire in WireDict[guid])
                {
                    if (wire.IsChip)
                    {
                        if (wire.CircuitIndex == -1)
                        {
                            Output[wire.ToIndex] = FromValues[wire.FromIndex];
                        }
                        else if (FromDirty[wire.FromIndex])
                        {
                            Chip chip = Chips[wire.CircuitIndex];
							chip.SetInputBit(wire.ToIndex, FromValues[wire.FromIndex]);
							chip.Update(forceUpdate);

							if (forceUpdate || GetCardinality(chip.Dirty) > 0)
							{
								if (!queue.Contains(chip.ID))
								{
									queue.Enqueue(chip.ID);
								}
							}
						
                        }
                    }
                    else if(FromDirty[wire.FromIndex])
                    {
                        Gate gate = Gates[wire.CircuitIndex];
                        gate.SetInputBit(wire.ToIndex, FromValues[wire.FromIndex]);
                        gate.Update();

                        if (forceUpdate || gate.IsDirty())
                        {
                            if (!queue.Contains(gate.ID))
                            {
                                queue.Enqueue(gate.ID);
                            }
                        }

                    }
                }
            }

        }

        public static int GetCardinality(BitArray bitArray)
        {

            int[] ints = new int[(bitArray.Count >> 5) + 1];

            bitArray.CopyTo(ints, 0);

            int count = 0;

            // fix for not truncated bits in last integer that may have been set to true with SetAll()
            ints[ints.Length - 1] &= ~(-1 << (bitArray.Count % 32));

            for (int i = 0; i < ints.Length; i++)
            {

                int c = ints[i];

                // magic (http://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetParallel)
                unchecked
                {
                    c = c - ((c >> 1) & 0x55555555);
                    c = (c & 0x33333333) + ((c >> 2) & 0x33333333);
                    c = ((c + (c >> 4) & 0xF0F0F0F) * 0x1010101) >> 24;
                }

                count += c;

            }

            return count;

        }

    }

    //Working
    public class AND4Chip : Chip
    {
        public AND4Chip() : base(3, 1)
        {
            Gate gateA = new ANDGate();
            int indexA = AddGate(gateA);

            Gate gateB = new ORGate();
            int indexB = AddGate(gateB);

            AddWire(ID, new Wire(2, 0, indexA, false));
            AddWire(ID, new Wire(1, 1, indexA, false));
            AddWire(ID, new Wire(0, 1, indexB, false));
            AddWire(gateA.ID, new Wire(0, 0, indexB, false));
            AddWire(gateB.ID, new Wire(0, 0, -1, true));

        }
    }

    public class TestChip2 : Chip
    {
        public TestChip2() : base(3, 1)
        {
            Gate gateA = new NANDGate();
            int indexA = AddGate(gateA);

            Gate gateB = new NORGate();
            int indexB = AddGate(gateB);

            Gate gateC = new ANDGate(3);
            int indexC = AddGate(gateC);

            AddWire(ID, new Wire(2, 0, indexA, false));
            AddWire(ID, new Wire(2, 0, indexB, false));
            AddWire(ID, new Wire(1, 1, indexC, false));
            AddWire(ID, new Wire(0, 1, indexA, false));
            AddWire(ID, new Wire(0, 1, indexB, false));

            AddWire(gateA.ID, new Wire(0, 0, indexC, false));
            AddWire(gateB.ID, new Wire(0, 2, indexC, false));

            AddWire(gateC.ID, new Wire(0, 0, -1, true));

        }
    }

    public class TestChip3 : Chip
    {
        public TestChip3() : base(4, 1)
        {
            Gate gateA = new ORGate();
            int indexA = AddGate(gateA);

            Gate gateB = new ORGate();
            int indexB = AddGate(gateB);

            Gate gateC = new ANDGate();
            int indexC = AddGate(gateC);

            AddWire(ID, new Wire(3, 0, indexA, false));
            AddWire(ID, new Wire(2, 1, indexA, false));
            AddWire(ID, new Wire(1, 0, indexB, false));
            AddWire(ID, new Wire(0, 1, indexB, false));

            AddWire(gateA.ID, new Wire(0, 0, indexC, false));
            AddWire(gateB.ID, new Wire(0, 1, indexC, false));

            AddWire(gateC.ID, new Wire(0, 0, -1, true));

        }
    }

    public class FlipFlop : Chip
    {
        public FlipFlop() : base(2, 2)
        {
            Gate gateA = new NANDGate();
            int indexA = AddGate(gateA);

            Gate gateB = new NANDGate();
            int indexB = AddGate(gateB);

            AddWire(ID, new Wire(1, 0, indexA, false));
            AddWire(ID, new Wire(0, 0, indexB, false));

            AddWire(gateA.ID, new Wire(0, 0, -1, true));
            AddWire(gateA.ID, new Wire(0, 1, indexB, false));

            AddWire(gateB.ID, new Wire(0, 1, -1, true));
            AddWire(gateB.ID, new Wire(0, 1, indexA, false));
        }
    }

    public class JKFlipFlop : Chip
    {
        public JKFlipFlop() : base(2, 2)
        {
            Gate gateA = new NANDGate();
            int indexA = AddGate(gateA);

            Gate gateB = new NANDGate();
            int indexB = AddGate(gateB);

            Gate gateC = new NANDGate();
            int indexC = AddGate(gateC);

            Gate gateD = new NANDGate();
            int indexD = AddGate(gateD);

            AddWire(ID, new Wire(1, 1, indexA, false));
            AddWire(ID, new Wire(0, 0, indexB, false));

            AddWire(gateA.ID, new Wire(0, 0, indexC, false));
            AddWire(gateB.ID, new Wire(0, 1, indexD, false));

            AddWire(gateC.ID, new Wire(0, 0, indexD, false));
            AddWire(gateC.ID, new Wire(0, 1, indexB, false));
            AddWire(gateC.ID, new Wire(0, 0, -1, true));

            AddWire(gateD.ID, new Wire(0, 1, indexC, false));
            AddWire(gateD.ID, new Wire(0, 0, indexA, false));
            AddWire(gateD.ID, new Wire(0, 1, -1, true));

        }
    }

    public class ChipInChip : Chip
    {
        public ChipInChip() : base(4, 1)
        {
            Chip chipA = new TestChip3();
            int indexA = AddChip(chipA);

            Gate gateB = new NotGate();
            int indexB = AddGate(gateB);

            AddWire(ID, new Wire(3, 3, indexA, true));
            AddWire(ID, new Wire(2, 2, indexA, true));
            AddWire(ID, new Wire(1, 1, indexA, true));
            AddWire(ID, new Wire(0, 0, indexA, true));

            AddWire(chipA.ID, new Wire(0, 0, indexB, false));

            AddWire(gateB.ID, new Wire(0, 0, -1, true));

            Output[0] = true;

        }
    }

}
