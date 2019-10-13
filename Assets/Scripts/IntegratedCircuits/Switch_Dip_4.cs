namespace IntegratedCircuits
{
    public class Switch_Dip_4 : Switch
    {
        public Switch_Dip_4() : base(8)
        {
            SwitchType = SwitchTypes.Toggle;
            ModelName = "DIP4";
            NeedsObjRef = true;
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
