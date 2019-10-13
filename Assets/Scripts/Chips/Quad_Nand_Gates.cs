using Gates;

/**
 * Quad And Gates based on the 74LS00
 * Used in the PQ011G_NandGate
 */

namespace Chips
{
    public class Quad_Nand_Gates : Chip
    {
        public Quad_Nand_Gates() : base(8, 4)
        {
            Gate gateA = new NANDGate();
            int indexA = AddGate(gateA);

            Gate gateB = new NANDGate();
            int indexB = AddGate(gateB);

            Gate gateC = new NANDGate();
            int indexC = AddGate(gateC);

            Gate gateD = new NANDGate();
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
