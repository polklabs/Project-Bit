using UnityEngine;

namespace IntegratedCircuits
{
    public class PQ001 : IntegratedCircuit
    {
        public PQ001() : base(8)
        {
            IcType = ICType.ic4;
            ModelName = "IC8";

            PinModes[4] = PinMode.Ouput;
            PinModes[5] = PinMode.Ouput;
            PinModes[6] = PinMode.Ouput;
        }

        protected override void InternalUpdate()
        {
            //Debug.Log("Update");
            PinState[4] = PinState[2] == State.HIGH ? State.LOW : State.HIGH;
            PinState[5] = PinState[1] == State.HIGH ? State.LOW : State.HIGH;
            PinState[6] = PinState[0] == State.HIGH ? State.LOW : State.HIGH;
        }
    }
}
