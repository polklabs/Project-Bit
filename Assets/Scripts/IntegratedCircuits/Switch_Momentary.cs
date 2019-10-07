namespace IntegratedCircuits
{
    public class Switch_Momentary : Switch
    {
        public Switch_Momentary() : base(1)
        {
            SwitchType = SwitchTypes.Momentary;

            ModelName = "momentary_switch";
        }
    }
}