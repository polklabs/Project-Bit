namespace IntegratedCircuits
{
    public class Switch_Dip_3 : Switch
    {
        public Switch_Dip_3() : base(6)
        {
            SwitchType = SwitchTypes.Toggle;
            ModelName = "DIP3";
            OverwriteObjText = false;
        }

        public override void CustomMethod()
        {
            if (GameObjectRef != null)
            {
                for (int i = 0; i < SwitchPins / 2; i++)
                {
                    GameObjectRef.GetComponent<Mono_Dip>().FlipSwitch(i, SwitchState[i]);
                }
            }
        }

    }
}
