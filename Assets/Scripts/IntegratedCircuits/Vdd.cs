namespace IntegratedCircuits
{
    public class Vdd : IntegratedCircuit
    {
        public Vdd() : base(1)
        {
            IcType = ICType.solo;
            ModelName = "Vdd";
            Vdd = 0;
            Gnd = 0;
            PinModes[0] = PinMode.Ouput;
        }

        protected override void InternalReset(bool disable)
        {
            if (disable)
            {
                PinState[0] = State.OFF;
                return;
            }
            InternalUpdate(true);
        }

        protected override void InternalUpdate(bool reset)
        {
            PinState[0] = State.HIGH;
        }
    }
}