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
        [DataMember]
        public bool invertValue { get; set; }
        
        /// <summary>
        /// Creates a "wire" connecting two inputs/outputs and or gates/chips
        /// </summary>
        /// <param name="fromIndex">Index of output to grab value from (Input for .this)</param>
        /// <param name="toIndex">Index of input to send value to (Output for .this)</param>
        /// <param name="circuitIndex">Index of circuit to connect to (-1 for .this)</param>
        /// <param name="isChip">If the circuit is of type Chip</param>
        public Wire(int fromIndex, int toIndex, int circuitIndex, bool isChip = false, bool invert = false)
        {
            IsChip = isChip;
            CircuitIndex = circuitIndex;
            FromIndex = fromIndex;
            ToIndex = toIndex;
            invertValue = invert;
        }
    }

    [DataContract]
    public abstract class Chip
    {
        [DataMember]
        public Guid ID { get; set; }
       
        [DataMember]
        public BitArray Input { get; set; }

        [DataMember]
        public BitArray Output { get; set; }

        [DataMember]
        public List<Gate> Gates { get; set; }
        [DataMember]
        public List<Chip> Chips { get; set; }
        [DataMember]
        public Dictionary<Guid, List<Wire>> WireDict { get; set; }

        [DataMember]
        protected BitArray Dirty { get; set; }

        [DataMember]
        protected BitArray OldInput { get; set; }
        [DataMember]
        protected bool FirstRun = true;        
        [IgnoreDataMember]
        // When updating gates should the gate be updated everytime output changes or only based on the original output
        // Helps when dealing with gates that loop back onto themselves (Flip Flops)
        protected bool ScrubOutput = false;

        /// <summary>
        /// Create a new chip object
        /// </summary>
        /// <param name="inputs">Size of input array</param>
        /// <param name="outputs">Size of output array</param>
        public Chip(int inputs, int outputs)
        {
            ID = Guid.NewGuid();

            Input = new BitArray(inputs, false);
            OldInput = new BitArray(inputs, true);
            Output = new BitArray(outputs, false);
            Dirty = new BitArray(outputs, false);            

            Gates = new List<Gate>();
            Chips = new List<Chip>();
            WireDict = new Dictionary<Guid, List<Wire>>();
        }

        protected int AddGate(Gate gate)
        {
            Gates.Add(gate);
            if (!WireDict.ContainsKey(gate.ID))
            {
                WireDict.Add(gate.ID, new List<Wire>());
            }
            return Gates.Count - 1;
        }

        protected int AddChip(Chip chip)
        {
            Chips.Add(chip);
            if (!WireDict.ContainsKey(chip.ID))
            {
                WireDict.Add(chip.ID, new List<Wire>());
            }
            return Chips.Count - 1;
        }

        protected void AddWire(Chip chip, Wire wire)
        {
            AddWire(chip.ID, wire);
        }

        protected void AddWire(Gate gate, Wire wire)
        {
            AddWire(gate.ID, wire);
        }

        /// <summary>
        /// Adds a new connecting wire to the chip
        /// </summary>
        /// <param name="id">Id of circuit to use as start of wire</param>        
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

            Dirty = (BitArray)(oldOutput.Xor(Output)).Clone();
            OldInput = (BitArray)Input.Clone();
            FirstRun = false;

            return Output;
        }

        protected virtual void GetOutput(bool forceUpdate)
        {
            int loops = 0;
            Queue<Guid> queue = new Queue<Guid>();
            Queue<Guid> outputQueue = new Queue<Guid>();

            foreach (Chip chip in Chips)
            {
                chip.Dirty.SetAll(false);
            }
            foreach (Gate gate in Gates)
            {
                gate.SetClean();
            }

            // Update input wires
            foreach (Wire wire in WireDict[ID])
            {
                if (forceUpdate || FirstRun || (OldInput[wire.FromIndex] != Input[wire.FromIndex]))
                {
                    if (wire.IsChip)
                    {
                        Chip chip = Chips[wire.CircuitIndex];
                        chip.SetInputBit(wire.ToIndex, Input[wire.FromIndex] ^ wire.invertValue);
                        chip.Update(forceUpdate);
                        //Debug.Log("Updated chip: " + (char)(wire.CircuitIndex + 65));

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
                        gate.SetInputBit(wire.ToIndex, Input[wire.FromIndex] ^ wire.invertValue);
                        gate.Update(ScrubOutput);
                        //Debug.Log("Updated gate: " + (char)(wire.CircuitIndex + 65));

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

            //Update internal components
            while (queue.Count > 0)
            {
                //Stop infinite loops from continuing
                loops++;
                if (loops >= 100)
                {
                    Debug.Log("Infinite loop, breaking");
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
                            if (!outputQueue.Contains(guid))
                            {
                                outputQueue.Enqueue(guid);
                            }
                            Output[wire.ToIndex] = FromValues[wire.FromIndex] ^ wire.invertValue;
                            //Debug.Log("Updated output: " + wire.ToIndex);
                        }
                        else if (FromDirty[wire.FromIndex])
                        {
                            Chip chip = Chips[wire.CircuitIndex];
							chip.SetInputBit(wire.ToIndex, FromValues[wire.FromIndex] ^ wire.invertValue);
							chip.Update(forceUpdate);
                            //Debug.Log("Updated chip: " + (char)(wire.CircuitIndex + 65));

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
                        gate.SetInputBit(wire.ToIndex, FromValues[wire.FromIndex] ^ wire.invertValue);
                        gate.Update(ScrubOutput);
                        //Debug.Log("Updated gate: " + (char)(wire.CircuitIndex + 65));

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

            //Update output wires
            while (outputQueue.Count > 0)
            {

                Guid guid = outputQueue.Dequeue();

                BitArray FromValues;

                int findIndex = Gates.FindIndex(x => x.ID == guid);
                if (findIndex != -1)
                {
                    FromValues = new BitArray(1, Gates[findIndex].Output);
                }
                else
                {
                    Chip c = Chips.Find(x => x.ID == guid);
                    FromValues = c.Output;
                }

                foreach (Wire wire in WireDict[guid])
                {
                    if (wire.IsChip && wire.CircuitIndex == -1)
                    {
                        Output[wire.ToIndex] = FromValues[wire.FromIndex] ^ wire.invertValue;
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

}
