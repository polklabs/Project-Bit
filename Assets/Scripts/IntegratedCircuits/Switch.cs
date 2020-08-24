using System.Runtime.Serialization;
using System.Linq;

namespace IntegratedCircuits
{   
    [DataContract]
    public class Switch : IntegratedCircuit
    {        

        [DataMember]
        public bool[] SwitchState;

        protected readonly int SwitchPins;       
        public SwitchTypes SwitchType;

        public Switch(int pins) : base(pins == 1 ? 6 : pins)
        {
            SwitchPins = pins;
            SwitchState = Enumerable.Repeat(false, pins == 1 ? 1 : pins / 2).ToArray();
            SwitchType = SwitchTypes.Momentary;

            IcType = ICType.ic4;
            ModelName = "momentary_switch";
            WriteToNodes = false;
        }

        protected override void InternalReset(bool disable)
        {
            if (disable)
            {
                if (SwitchPins == 1)
                {
                    BreadBoardRef.UnlinkNodes(GetPinNode(0), GetPinNodeIndex(0), GetPinNode(2), GetPinNodeIndex(2));
                    BreadBoardRef.UnlinkNodes(GetPinNode(5), GetPinNodeIndex(5), GetPinNode(3), GetPinNodeIndex(3));
                }
                else
                {
                    for (int i = 0; i < SwitchPins / 2; i++)
                    {
                        BreadBoardRef.UnlinkNodes(GetPinNode(i), GetPinNodeIndex(i), GetPinNode(SwitchPins - 1 - i), GetPinNodeIndex(SwitchPins - 1 - i));
                    }
                }
                return;
            }
            else
            {
                InternalUpdate(true);
            }
        }

        protected override void InternalUpdate(bool reset)
        {
            if (SwitchPins == 1)
            {
                if (SwitchState[0])
                {
                    BreadBoardRef.LinkNodes(GetPinNode(0), GetPinNodeIndex(0), GetPinNode(2), GetPinNodeIndex(2));
                    BreadBoardRef.LinkNodes(GetPinNode(5), GetPinNodeIndex(5), GetPinNode(3), GetPinNodeIndex(3));
                } else
                {
                    BreadBoardRef.UnlinkNodes(GetPinNode(0), GetPinNodeIndex(0), GetPinNode(2), GetPinNodeIndex(2));
                    BreadBoardRef.UnlinkNodes(GetPinNode(5), GetPinNodeIndex(5), GetPinNode(3), GetPinNodeIndex(3));
                }
            }
            else
            {
                for (int i = 0; i < SwitchPins / 2; i++)
                {
                    if (SwitchState[SwitchState.Length - 1 - i])
                    {
                        BreadBoardRef.LinkNodes(GetPinNode(i), GetPinNodeIndex(i), GetPinNode(SwitchPins - 1 - i), GetPinNodeIndex(SwitchPins - 1 - i));
                    }
                    else
                    {
                        BreadBoardRef.UnlinkNodes(GetPinNode(i), GetPinNodeIndex(i), GetPinNode(SwitchPins - 1 - i), GetPinNodeIndex(SwitchPins - 1 - i));
                    }
                }
            }
        }
    }
}