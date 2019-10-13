namespace IntegratedCircuits
{
    public class Switch_Dip_8 : Switch
    {
        public Switch_Dip_8() : base(16)
        {
            SwitchType = SwitchTypes.Toggle;
            ModelName = "DIP8";
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
