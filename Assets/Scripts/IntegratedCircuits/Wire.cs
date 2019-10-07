namespace IntegratedCircuits
{
    public class Wire : IntegratedCircuit
    {
        public Wire() : base(2)
        {
            IcType = ICType.wire;
            ModelName = "Wire";
        }
    }
}