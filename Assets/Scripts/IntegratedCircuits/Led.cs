﻿using System.Runtime.Serialization;
using UnityEngine;

namespace IntegratedCircuits
{
    [DataContract]
    public class Led : IntegratedCircuit
    {
        [DataMember]
        public bool On;

        private Mono_Led mono;

        public Led() : base(1)
        {
            IcType = ICType.solo;
            ModelName = "Led_Green";
            On = false;
            NeedsObjRef = true;
            Vdd = 0;
            Gnd = 0;
        }

        public override void CustomMethod()
        {
            if (mono == null)
            {
                mono = GameObjectRef.GetComponent<Mono_Led>();
            }
            mono.ToggleLed(On);
        }

        protected override void InternalUpdate()
        {
            if (PinState[0] == State.HIGH /*&& PinState[1] == -1*/)
            {
                //Debug.Log("LIGHT ON");
                On = true;                
            }
            else
            {
                //Debug.Log("LIGHT OFF");
                On = false;
            }

            CustomMethod();

        }

        protected override void InternalReset(bool disable)
        {
            InternalUpdate();
        }
    }
}