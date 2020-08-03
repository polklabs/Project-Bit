﻿using System;
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
    public class IntegratedCircuit
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
        protected readonly PinMode[] PinModes;

        //State for input if input is unknown
        protected readonly State[] DefaultState;

        protected readonly Dictionary<string, int> NodeToPin = new Dictionary<string, int>();
        
        public ICType IcType;    
        public string ModelName;
        public readonly int Pins;
        public bool OverwriteObjText;

        protected BreadBoard BreadBoardRef;
        protected GameObject GameObjectRef;
        protected int Vdd;
        protected int Gnd;

        public IntegratedCircuit(int numPins)
        {
            Id = Guid.NewGuid();
            IcType = ICType.unknown;
            ModelName = "Unknown";
            OverwriteObjText = true;

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

        public PinMode GetPinMode(string node)
        {
            return PinModes[NodeToPin[node]];
        }

        public void Update()
        {
            Update(false);
        }

        public void Update(bool disable)
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

        public virtual void CustomMethod()
        {
            //Overridable method for custom calls
        }

        protected virtual void InternalUpdate(State[] oldState)
        {
            InternalUpdate();
        }
        protected virtual void InternalUpdate()
        {
            //Override this
            Debug.Log("Internal Update");
        }

        protected virtual void InternalReset(bool disable)
        {
            //Override this
            Debug.Log("Internal Reset");

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