using Chips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace IntegratedCircuits
{
    public enum PinMode { Input, Ouput };
    public enum ICType { ic4, ic6, solo, dual, wire, breadboard, powerrail, unknown };
    public enum State { LOW = -1, OFF = 0, HIGH = 1 };
    public enum SwitchTypes { Toggle, Momentary, Pulse };

    [DataContract]
    public abstract class IntegratedCircuit
    {
        [DataMember]
        protected Guid Id;

        [DataMember]
        //ID of node connected to pin
        protected readonly string[] PinNodes;

        [DataMember]
        protected int[] PinIndex;

        [DataMember]
        //State of connected pin - -1=low, 0=unknown, 1=high
        protected readonly State[] PinState;
        protected State[] oldState;

        //Whether the pin is input or output
        private readonly PinMode[] PinModes;

        //State for input if input is unknown
        protected readonly State[] DefaultState;

        protected readonly Dictionary<string, int> NodeToPin = new Dictionary<string, int>();
        
        public ICType IcType;    
        public string ModelName;
        public readonly int Pins;
        public bool OverwriteObjText;
        public bool WriteToNodes;

        protected BreadBoard BreadBoardRef;
        protected GameObject GameObjectRef;
        protected int Vdd;
        protected int Gnd;

        public string[] PinNames;
        public bool PinNamesDisplayed;

        /// <summary>
        /// Can UI edit mode open up an edit form for this IC
        /// </summary>
        public bool isEditable;

        public IntegratedCircuit(int numPins)
        {
            Id = Guid.NewGuid();
            IcType = ICType.unknown;
            ModelName = "Unknown";
            OverwriteObjText = true;
            WriteToNodes = true;
            PinNames = new string[] { };
            PinNamesDisplayed = false;
            isEditable = false;

            Pins = numPins;
            Vdd  = numPins - 1;
            Gnd  = (numPins / 2) - 1;

            PinIndex     = Enumerable.Repeat(-1, Pins).ToArray();
            PinNodes     = Enumerable.Repeat("", Pins).ToArray();
            PinState     = Enumerable.Repeat(State.OFF, Pins).ToArray();
            PinModes     = Enumerable.Repeat(PinMode.Input, Pins).ToArray();
            DefaultState = Enumerable.Repeat(State.LOW, Pins).ToArray();

            if (Pins > 3)
            {
                DefaultState[Vdd] = 0;
                DefaultState[Gnd] = 0;
            }
        }

        public virtual void PostDeserialize()
        {
            // This gets called after object is deserialized on load.
            // Allows calling methods after constructer when poperties are set
        }

        public void AssignObjRef(GameObject obj)
        {
            GameObjectRef = obj;
        }

        public GameObject GetObjRef()
        {
            return GameObjectRef;
        }

        public Guid SetBreadBoard(BreadBoard newBoard)
        {
            BreadBoardRef = newBoard;
            return Id;
        }
        public Guid GetId()
        {
            return Id;
        }
        public void SetPinIndex(int index, int value)
        {
            PinIndex[index] = value;
        }
        public void SetPinNode(int index, string node)
        {
            PinNodes[index] = node;
            NodeToPin.Add(node, index);
        }
        public string GetPinNode(int index)
        {
            return PinNodes[index];
        }
        public int GetPinNodeIndex(int index)
        {
            return PinIndex[index];
        }

        public int GetPinState(int index)
        {
            if(PinModes[index] == PinMode.Ouput)
            {
                return (int)PinState[index];
            }
            return 0;
        }

        /// <summary>
        /// Gets the PinMode for a specific node on the breadborad
        /// </summary>
        /// <param name="node">Breadboard node id</param>
        /// <returns>PinMode of chip for specific node</returns>
        public PinMode GetPinMode(string node)
        {
            return PinModes[NodeToPin[node]];
        }

        /// <summary>
        /// Gets the PinMode for a specific index
        /// </summary>
        /// <param name="index">Index of Pin to lookup</param>
        /// <returns>PinMode of chip for a specific index</returns>
        public PinMode GetPinMode(int index)
        {
            return PinModes[index];
        }

        /// <summary>
        /// Allow setting the pin mode directly with PinMode enum
        /// </summary>
        /// <param name="index">Pin index to set</param>
        /// <param name="mode">PinMode enum to use</param>
        public void SetPinModes(int index, PinMode mode)
        {
            if (index < PinModes.Length)
            {
                PinModes[index] = mode;
            }
        }

        /// <summary>
        /// Allow setting a range of pin modes to the same values
        /// </summary>
        /// <param name="indexStart">Pin index to start (inclusive)</param>
        /// <param name="indexEnd">Pin index to end (inclusive)</param>
        /// <param name="mode">PinMode enum to use</param>
        public void SetPinModes(int indexStart, int indexEnd, PinMode mode)
        {
            for (int i = indexStart; i <= indexEnd; i++)
            {
                SetPinModes(i, mode);
            }
        }

        public void Update(bool disable = false)
        {
            //Debug.Log("Update: " + Id.ToString());            

            //Update all input states
            UpdateState(disable, false);

            //Update internal
            if(PinState[Vdd] == State.HIGH && PinState[Gnd] == State.LOW)
            {
                InternalUpdate(oldState);
            }
            else
            {
                InternalReset(disable);
            }


            //Propagate out output states if changed
            for (int i = 0; i < Pins; i++)
            {
                //Debug.Log("Pin " + i.ToString() + ", old: " + oldState[i].ToString() + ", new: " + PinState[i].ToString());                
                if (PinModes[i] == PinMode.Ouput)
                {
                    if(PinState[i] == State.HIGH && oldState[i] == State.OFF)
                    {
                        BreadBoardRef.PropagateValue(PinNodes[i], Id.ToString(), i, 1);
                    }
                    else if(PinState[i] == State.HIGH && oldState[i] == State.LOW)
                    {
                        BreadBoardRef.PropagateValue(PinNodes[i], Id.ToString(), i, 2);
                    }
                    else if (PinState[i] == State.OFF && oldState[i] == State.LOW)
                    {
                        BreadBoardRef.PropagateValue(PinNodes[i], Id.ToString(), i, 3);
                    }
                    else if (PinState[i] == State.OFF && oldState[i] == State.HIGH)
                    {
                        BreadBoardRef.PropagateValue(PinNodes[i], Id.ToString(), i, -3);
                    }
                    else if (PinState[i] == State.LOW && oldState[i] == State.HIGH)
                    {
                        BreadBoardRef.PropagateValue(PinNodes[i], Id.ToString(), i, -2);
                    }
                    else if (PinState[i] == State.LOW && oldState[i] == State.OFF)
                    {
                        BreadBoardRef.PropagateValue(PinNodes[i], Id.ToString(), i, -1);
                    }
                }
            }

        }

        protected void UpdateState(bool disable, bool updateOutputs)
        {
            oldState = (State[])PinState.Clone();

            for (int i = 0; i < Pins; i++)
            {
                if (!PinNodes[i].Equals("")) {
                    if (PinModes[i] == PinMode.Input)
                    {
                        int inputState = disable ? 0 : BreadBoardRef.GetNodeState(PinNodes[i]);
                        if (inputState != 0)
                        {
                            PinState[i] = (State)inputState;
                        }
                        else
                        {
                            PinState[i] = DefaultState[i];
                        }
                    }
                    else if(updateOutputs)
                    {
                        PinState[i] = State.OFF;
                    }
                }
            }
        }

        protected void CopyOutputsToOldState()
        {
            for (int i = 0; i < Pins; i++)
            {
                if (PinModes[i] == PinMode.Ouput)
                {
                    oldState[i] = PinState[i];
                }
            }
        }

        public virtual void CustomMethod()
        {
            //Overridable method for custom calls
        }

        protected virtual void InternalUpdate(State[] oldState)
        {
            InternalUpdate(false);
        }
        protected virtual void InternalUpdate(bool reset)
        {
            //Override this
            Debug.Log("Internal Update");
        }

        protected virtual void InternalReset(bool disable)
        {
            //Override this
            //Debug.Log("Internal Reset");

            for (int i = 0; i < Pins; i++)
            {
                if (PinModes[i] == PinMode.Ouput)
                {
                    PinState[i] = State.OFF;
                }
            }
        }

    }

}