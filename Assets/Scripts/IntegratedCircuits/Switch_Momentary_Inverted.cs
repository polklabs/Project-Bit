using IntegratedCircuits;

namespace IntegratedCircuits
{
    public class Switch_Momentary_Inverted : Switch
    {
        public Switch_Momentary_Inverted() : base(1)
        {
            SwitchType = SwitchTypes.Momentary;
            OnState = false;
            SwitchState[0] = true;

            ModelName = "momentary_switch";
        }
    }
}
