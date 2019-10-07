namespace IntegratedCircuits
{
    public class Switch_Pulse : Switch
    {
        public Switch_Pulse() : base(1)
        {
            SwitchType = SwitchTypes.Pulse;

            ModelName = "momentary_switch";
        }
    }
}