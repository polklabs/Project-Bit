namespace IntegratedCircuits
{
    public class Gnd : IntegratedCircuit
    {
        public Gnd() : base(1)
        {
            IcType = ICType.solo;
            ModelName = "Gnd";
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
            PinState[0] = State.LOW;
        }
    }
}