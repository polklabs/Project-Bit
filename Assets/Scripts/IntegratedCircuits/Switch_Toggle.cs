namespace IntegratedCircuits
{
    public class Switch_Toggle : Switch
    {
        public Switch_Toggle() : base(1)
        {
            SwitchType = SwitchTypes.Toggle;
            ModelName = "Switch_Toggle";
            NeedsObjRef = true;
        }

        public override void CustomMethod()
        {
            if (GameObjectRef != null)
            {
                GameObjectRef.GetComponent<Mono_Toggle>().FlipSwitch(SwitchState[0]);
            }
        }
    }
}