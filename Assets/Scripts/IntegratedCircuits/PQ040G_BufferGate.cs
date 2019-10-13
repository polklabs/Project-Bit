using System.Runtime.Serialization;

namespace IntegratedCircuits
{
    [DataContract]
    public class PQ040G_BufferGate : IntegratedCircuit
    {
        [DataMember]
        private readonly Chips.Octal_Bus_Gates Chip;
        [DataMember]
        private bool ChangeDir;

        public PQ040G_BufferGate() : base(20)
        {
            IcType = ICType.ic4;
            ModelName = "IC20";

            Chip = new Chips.Octal_Bus_Gates();
            ChangeDir = false;

            SetInOut(false);
        }

        private void SetInOut(bool dir)
        {
            PinModes[1] = dir ? PinMode.Input : PinMode.Ouput;
            PinModes[2] = dir ? PinMode.Input : PinMode.Ouput;
            PinModes[3] = dir ? PinMode.Input : PinMode.Ouput;
            PinModes[4] = dir ? PinMode.Input : PinMode.Ouput;
            PinModes[5] = dir ? PinMode.Input : PinMode.Ouput;
            PinModes[6] = dir ? PinMode.Input : PinMode.Ouput;
            PinModes[7] = dir ? PinMode.Input : PinMode.Ouput;
            PinModes[8] = dir ? PinMode.Input : PinMode.Ouput;

            PinModes[10] = !dir ? PinMode.Input : PinMode.Ouput;
            PinModes[11] = !dir ? PinMode.Input : PinMode.Ouput;
            PinModes[12] = !dir ? PinMode.Input : PinMode.Ouput;
            PinModes[13] = !dir ? PinMode.Input : PinMode.Ouput;
            PinModes[14] = !dir ? PinMode.Input : PinMode.Ouput;
            PinModes[15] = !dir ? PinMode.Input : PinMode.Ouput;
            PinModes[16] = !dir ? PinMode.Input : PinMode.Ouput;
            PinModes[17] = !dir ? PinMode.Input : PinMode.Ouput;
        }

        private void SetOutputOff()
        {
            for(int i = 0; i < Pins; i++)
            {
                if(PinModes[i] == PinMode.Ouput)
                {
                    PinState[i] = State.OFF;
                }
            }
        }

        protected override void InternalUpdate(State[] oldState)
        {
            if(PinState[0] != oldState[0])
            {
                SetOutputOff();
                ChangeDir = true;
                BreadBoardRef.UpdateComponent(Id);
                return;
            }
            else if(ChangeDir)
            {
                ChangeDir = false;
                SetInOut(PinState[0] == State.HIGH ? true : false);
                UpdateState(false, true);
                this.oldState = (State[])PinState.Clone();
            }

            InternalUpdate();

        }

        protected override void InternalUpdate()
        {
            Chip.Input[0] = PinState[0]   == State.HIGH ? true : false;
            Chip.Input[1] = PinState[18]  == State.HIGH ? true : false;

            Chip.Input[2] = PinState[1]   == State.HIGH ? true : false;
            Chip.Input[3] = PinState[2]   == State.HIGH ? true : false;
            Chip.Input[4] = PinState[3]   == State.HIGH ? true : false;
            Chip.Input[5] = PinState[4]   == State.HIGH ? true : false;
            Chip.Input[6] = PinState[5]   == State.HIGH ? true : false;
            Chip.Input[7] = PinState[6]   == State.HIGH ? true : false;
            Chip.Input[8] = PinState[7]   == State.HIGH ? true : false;
            Chip.Input[9] = PinState[8]   == State.HIGH ? true : false;
            Chip.Input[10] = PinState[17] == State.HIGH ? true : false;
            Chip.Input[11] = PinState[16] == State.HIGH ? true : false;
            Chip.Input[12] = PinState[15] == State.HIGH ? true : false;
            Chip.Input[13] = PinState[14] == State.HIGH ? true : false;
            Chip.Input[14] = PinState[13] == State.HIGH ? true : false;
            Chip.Input[15] = PinState[12] == State.HIGH ? true : false;
            Chip.Input[16] = PinState[11] == State.HIGH ? true : false;
            Chip.Input[17] = PinState[10] == State.HIGH ? true : false;

            Chip.Update();

            if(PinState[0] == State.HIGH ? true : false)
            {
                PinState[17] = Chip.Output[8]  ? State.HIGH : State.OFF;
                PinState[16] = Chip.Output[9]  ? State.HIGH : State.OFF;
                PinState[15] = Chip.Output[10] ? State.HIGH : State.OFF;
                PinState[14] = Chip.Output[11] ? State.HIGH : State.OFF;
                PinState[13] = Chip.Output[12] ? State.HIGH : State.OFF;
                PinState[12] = Chip.Output[13] ? State.HIGH : State.OFF;
                PinState[11] = Chip.Output[14] ? State.HIGH : State.OFF;
                PinState[10] = Chip.Output[15] ? State.HIGH : State.OFF;
            }
            else
            {
                PinState[1] = Chip.Output[0] ? State.HIGH : State.OFF;
                PinState[2] = Chip.Output[1] ? State.HIGH : State.OFF;
                PinState[3] = Chip.Output[2] ? State.HIGH : State.OFF;
                PinState[4] = Chip.Output[3] ? State.HIGH : State.OFF;
                PinState[5] = Chip.Output[4] ? State.HIGH : State.OFF;
                PinState[6] = Chip.Output[5] ? State.HIGH : State.OFF;
                PinState[7] = Chip.Output[6] ? State.HIGH : State.OFF;
                PinState[8] = Chip.Output[7] ? State.HIGH : State.OFF;
            }
        }

        protected override void InternalReset(bool disable)
        {
            InternalUpdate();
            base.InternalReset(disable);
        }

    }
}
