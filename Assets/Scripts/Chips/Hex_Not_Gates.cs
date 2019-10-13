using Gates;

namespace Chips
{
    public class Hex_Not_Gates : Chip
    {
        public Hex_Not_Gates() : base(6, 6)
        {
            Gate gateA = new NotGate();
            int indexA = AddGate(gateA);

            Gate gateB = new NotGate();
            int indexB = AddGate(gateB);

            Gate gateC = new NotGate();
            int indexC = AddGate(gateC);

            Gate gateD = new NotGate();
            int indexD = AddGate(gateD);

            Gate gateE = new NotGate();
            int indexE = AddGate(gateE);

            Gate gateF = new NotGate();
            int indexF = AddGate(gateF);

            //A
            AddWire(ID, new Wire(0, 0, indexA, false));
            AddWire(gateA.ID, new Wire(0, 0, -1, true));

            //B
            AddWire(ID, new Wire(1, 0, indexB, false));
            AddWire(gateB.ID, new Wire(0, 1, -1, true));

            //C
            AddWire(ID, new Wire(2, 0, indexC, false));
            AddWire(gateC.ID, new Wire(0, 2, -1, true));

            //D
            AddWire(ID, new Wire(3, 0, indexD, false));
            AddWire(gateD.ID, new Wire(0, 3, -1, true));

            //E
            AddWire(ID, new Wire(4, 0, indexE, false));
            AddWire(gateE.ID, new Wire(0, 4, -1, true));

            //F
            AddWire(ID, new Wire(5, 0, indexF, false));
            AddWire(gateF.ID, new Wire(0, 5, -1, true));

        }
    }
}
