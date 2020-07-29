using Gates;

namespace Chips
{
    public class Full_Adder : Chip
    {
        public Full_Adder() : base(3, 2)
        {
            Gate gateA = new XORGate();
            int indexA = AddGate(gateA);

            Gate gateB = new XORGate();
            int indexB = AddGate(gateB);

            Gate gateC = new ANDGate();
            int indexC = AddGate(gateC);

            Gate gateD = new ANDGate();
            int indexD = AddGate(gateD);

            Gate gateE = new ORGate();
            int indexE = AddGate(gateE);

            // Gate A
            AddWire(ID, new Wire(0, 0, indexA));
            AddWire(ID, new Wire(1, 1, indexA));
            AddWire(gateA.ID, new Wire(0, 0, indexB));
            AddWire(gateA.ID, new Wire(0, 0, indexC));

            // Gate B
            AddWire(ID, new Wire(2, 1, indexB));
            AddWire(gateB.ID, new Wire(0, 0, -1, true));

            // Gate C
            AddWire(ID, new Wire(2, 1, indexC));
            AddWire(gateC.ID, new Wire(0, 0, indexE));

            // Gate D
            AddWire(ID, new Wire(0, 0, indexD));
            AddWire(ID, new Wire(1, 1, indexD));
            AddWire(gateD.ID, new Wire(0, 1, indexE));

            // Gate E
            AddWire(gateE.ID, new Wire(0, 1, -1, true));
        }
    }
}
