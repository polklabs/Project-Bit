using System.Runtime.Serialization;
using UnityEngine;

namespace IntegratedCircuits
{
    [DataContract]
    public class Wire : IntegratedCircuit
    {
        [DataMember]
        public Vector3[] points;

        public Wire() : base(2)
        {
            IcType = ICType.wire;
            ModelName = "Wire";
        }
    }
}