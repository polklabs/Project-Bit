using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Assets.Scripts.Utils;
using Gates;
using UnityEngine;
using Wires;

namespace Chips
{
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

        /// <summary>
        /// When updating gates should the gate be updated everytime output changes or only based on the original output
        /// Helps when dealing with gates that loop back onto themselves (Flip Flops) 
        /// </summary>
        [IgnoreDataMember]
        protected bool ScrubOutput = false;

        [IgnoreDataMember]
        public bool KeepDirty = false;

        /// <summary>
        /// Chip index on a chip when part of other chips
        /// </summary>
        [IgnoreDataMember]
        public int index;

        /// <summary>
        /// Stores mappings of Gate IDs to objects
        /// </summary>
        [IgnoreDataMember]
        private readonly Dictionary<Guid, Gate> GateDict = new Dictionary<Guid, Gate>();
        /// <summary>
        /// Stores mappings of Chip IDs to objects
        /// </summary>
        [IgnoreDataMember]
        private readonly Dictionary<Guid, Chip> ChipDict = new Dictionary<Guid, Chip>();

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
            return AddGateIndexed(gate).index;
        }

        protected Gate AddGateIndexed(Gate gate)
        {
            Gates.Add(gate);
            if (!WireDict.ContainsKey(gate.ID))
            {
                WireDict.Add(gate.ID, new List<Wire>());
            }
            gate.index = Gates.Count - 1;
            return gate;
        }

        protected int AddChip(Chip chip)
        {
            return AddChipIndexed(chip).index;
        }

        protected Chip AddChipIndexed(Chip chip)
        {
            Chips.Add(chip);
            if (!WireDict.ContainsKey(chip.ID))
            {
                WireDict.Add(chip.ID, new List<Wire>());
            }
            chip.index = Chips.Count - 1;
            return chip;
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
        protected void AddWire(Chip chip, Wire wire)
        {
            AddWire(chip.ID, wire);
        }
        protected void AddWire(Gate gate, Wire wire)
        {
            AddWire(gate.ID, wire);
        }

        /// <summary>
        /// Adds new connecting wires to the chip
        /// </summary>
        /// <param name="id">Id of circuit to use as start of wire</param>
        /// <param name="wires">List of wires to add</param>
        protected void AddWires(Guid id, Wire[] wires)
        {
            foreach(Wire w in wires)
            {
                AddWire(id, w);
            }
        }
        protected void AddWires(Gate gate, Wire[] wires)
        {
            AddWires(gate.ID, wires);
        }
        protected void AddWires(Chip chip, Wire[] wires)
        {
            AddWires(chip.ID, wires);
        }

        /// <summary>
        /// Adds a new connecting input wire to the chip
        /// </summary>
        /// <param name="wire">Wire to add to input</param>
        protected void AddInputWire(Wire wire)
        {
            AddWire(ID, wire);
        }

        /// <summary>
        /// Adds a new connecting input wires to the chip
        /// </summary>
        /// <param name="wires">List of wires to add to input</param>
        protected void AddInputWires(Wire[] wires)
        {
            foreach(Wire w in wires)
            {
                AddInputWire(w);
            }
            
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
            Input = input;
            return this;
        }

        public void Update(bool forceUpdate = false)
        {
            BitArray oldOutput = (BitArray)Output.Clone();

            GetOutput(forceUpdate);

            if (KeepDirty)
            {
                Dirty.Or(oldOutput.Xor(Output));
            }
            else
            {
                Dirty = (BitArray)oldOutput.Xor(Output).Clone();
            }
            OldInput = (BitArray)Input.Clone();
            FirstRun = false;
        }

        private Gate FindGate(Guid id)
        {
            if (!GateDict.ContainsKey(id))
            {
                GateDict[id] = Gates.Find(x => x.ID == id);
            }
            return GateDict[id];
        }
        private Chip FindChip(Guid id)
        {
            if (!ChipDict.ContainsKey(id))
            {
                ChipDict[id] = Chips.Find(x => x.ID == id);
            }
            return ChipDict[id];
        }

        protected virtual void GetOutput(bool forceUpdate)
        {
            int loops = 0;
            UniqueQueue<Guid> queue = new UniqueQueue<Guid>();
            UniqueQueue<Guid> outputQueue = new UniqueQueue<Guid>();

            // This is a new update loops so make sure gates and chips are not dirty
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
                        chip.SetInputBit(wire.ToIndex, Input[wire.FromIndex] ^ wire.InvertValue);
                        chip.Update(forceUpdate);

                        if (forceUpdate || FirstRun || chip.IsDirty())
                        {
                            queue.Enqueue(chip.ID);
                        }
                    }
                    else
                    {
                        Gate gate = Gates[wire.CircuitIndex];
                        gate.SetInputBit(wire.ToIndex, Input[wire.FromIndex] ^ wire.InvertValue);
                        gate.Update(ScrubOutput);

                        if (forceUpdate || FirstRun || gate.IsDirty())
                        {
                            queue.Enqueue(gate.ID);
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

                // The output of the previous gate
                Gate previousGate = FindGate(guid);                
                if (previousGate != null)
                {					
                    FromValues = new BitArray(1, previousGate.Output);
					FromDirty = new BitArray(1, previousGate.IsDirty());
                }
                else
                {
                    Chip c = FindChip(guid);
                    FromValues = c.Output;
					FromDirty = c.Dirty;
                }

                foreach (Wire wire in WireDict[guid])
                {
                    if (wire.IsChip)
                    {                        
                        if (wire.CircuitIndex == -1)
                        {
                            outputQueue.Enqueue(guid);
                            Output[wire.ToIndex] = FromValues[wire.FromIndex] ^ wire.InvertValue;
                            //Debug.Log("Updated output: " + wire.ToIndex);
                        }
                        else if (FromDirty[wire.FromIndex])
                        {
                            Chip chip = Chips[wire.CircuitIndex];
							chip.SetInputBit(wire.ToIndex, FromValues[wire.FromIndex] ^ wire.InvertValue);
							chip.Update(false);
                            //Debug.Log("Updated chip: " + (char)(Chips.FindIndex(x => x.ID == guid) + 65) + "->" + (char)(wire.CircuitIndex + 65));

							if (chip.IsDirty())
							{
						        queue.Enqueue(chip.ID);
							}
						
                        }
                    }
                    else if(FromDirty[wire.FromIndex])
                    {
                        Gate gate = Gates[wire.CircuitIndex];
                        gate.SetInputBit(wire.ToIndex, FromValues[wire.FromIndex] ^ wire.InvertValue);
                        gate.Update(ScrubOutput);
                        //Debug.Log("Updated gate: " + (char)(wire.CircuitIndex + 65));

                        if (forceUpdate || FirstRun || gate.IsDirty())
                        {
                            queue.Enqueue(gate.ID);
                        }

                    }
                }
            }

            //Update output wires
            while (outputQueue.Count > 0)
            {

                Guid guid = outputQueue.Dequeue();

                BitArray FromValues;

                Gate gate = FindGate(guid);
                if (gate != null)
                {
                    FromValues = new BitArray(1, gate.Output);
                }
                else
                {
                    FromValues = FindChip(guid).Output;
                }

                foreach (Wire wire in WireDict[guid])
                {
                    if (wire.IsChip && wire.CircuitIndex == -1)
                    {
                        Output[wire.ToIndex] = FromValues[wire.FromIndex] ^ wire.InvertValue;
                    }
                }
            }

        }

        private bool IsDirty()
        {
            for(int i = 0; i < Dirty.Length; i++)
            {
                if (Dirty[i] == true) return true;
            }
            return false;
        }

    }

}
