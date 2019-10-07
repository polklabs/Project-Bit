namespace IntegratedCircuits
{
    public class Seg_Disp : IntegratedCircuit
    {
        public Seg_Disp() : base(10)
        {
            IcType = ICType.ic6;
            ModelName = "7SegmentDisplay";
        }

        protected override void InternalUpdate()
        {

        }
    }
}