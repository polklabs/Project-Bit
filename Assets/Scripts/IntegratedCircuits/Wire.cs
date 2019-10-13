using System.Runtime.Serialization;
using UnityEngine;

namespace IntegratedCircuits
{
    [DataContract]
    public class Wire : IntegratedCircuit
    {
        [DataMember]
        public WirePoint[] points;

        public Wire() : base(2)
        {
            IcType = ICType.wire;
            ModelName = "Wire";
        }
    }

    [DataContract]
    public class WirePoint
    {
        [DataMember]
        public float x;
        [DataMember]
        public float y;
        [DataMember]
        public float z;

        public WirePoint()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public WirePoint(Vector3 point)
        {
            x = point.x;
            y = point.y;
            z = point.z;
        }

        public WirePoint(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3 GetVector3()
        {
            return new Vector3(x, y, z);
        }
    }
}