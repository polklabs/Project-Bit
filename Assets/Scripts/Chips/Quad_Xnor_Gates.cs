using Gates;

/**
 * Quad And Gates based on the 74LS??
 * Used in the PQ010G_XnorGate
 */

namespace Chips
{
    public class Quad_Xnor_Gates : Chip
    {
        public Quad_Xnor_Gates() : base(8, 4)
        {
            Gate gateA = new XNORGate();
            int indexA = AddGate(gateA);

            Gate gateB = new XNORGate();
            int indexB = AddGate(gateB);

            Gate gateC = new XNORGate();
            int indexC = AddGate(gateC);

            Gate gateD = new XNORGate();
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
