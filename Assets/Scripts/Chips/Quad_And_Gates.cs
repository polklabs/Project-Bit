using Gates;

/**
 * Quad And Gates based on the 74LS08
 * Used in the PQ010G_AndGate
 */

namespace Chips
{
    public class Quad_And_Gates : Chip
    {
        public Quad_And_Gates() : base(8, 4)
        {
            Gate gateA = new ANDGate();
            int indexA = AddGate(gateA);

            Gate gateB = new ANDGate();
            int indexB = AddGate(gateB);

            Gate gateC = new ANDGate();
            int indexC = AddGate(gateC);

            Gate gateD = new ANDGate();
            int indexD = AddGate(gateD);

            // Gate A
            AddWire(ID, new Wire(0, 0, indexA, false));
            AddWire(ID, new Wire(1, 1, indexA, false));
            AddWire(gateA.ID, new Wire(0, 0, -1, true));

            // Gate B
            AddWire(ID, new Wire(2, 0, indexB, false));
            AddWire(ID, new Wire(3, 1, indexB, false));
            AddWire(gateB.ID, new Wire(0, 1, -1, true));

            // Gate C
            AddWire(ID, new Wire(4, 0, indexC, false));
            AddWire(ID, new Wire(5, 1, indexC, false));
            AddWire(gateC.ID, new Wire(0, 2, -1, true));

            // Gate D
            AddWire(ID, new Wire(6, 0, indexD, false));
            AddWire(ID, new Wire(7, 1, indexD, false));
            AddWire(gateD.ID, new Wire(0, 3, -1, true));
        }
    }
}
