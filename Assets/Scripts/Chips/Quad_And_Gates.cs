using Gates;
using Wires;

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
            Gate gateA = AddGateIndexed(new ANDGate());
            Gate gateB = AddGateIndexed(new ANDGate());
            Gate gateC = AddGateIndexed(new ANDGate());
            Gate gateD = AddGateIndexed(new ANDGate());

            // Gate A
            AddInputWire(new Wire(0, 0, gateA));
            AddInputWire(new Wire(1, 1, gateA));
            AddWire(gateA, new Wire(0, 0, -1, true));

            // Gate B
            AddInputWire(new Wire(2, 0, gateB));
            AddInputWire(new Wire(3, 1, gateB));
            AddWire(gateB, new Wire(0, 1, -1, true));

            // Gate C
            AddInputWire(new Wire(4, 0, gateC));
            AddInputWire(new Wire(5, 1, gateC));
            AddWire(gateC, new Wire(0, 2, -1, true));

            // Gate D
            AddInputWire(new Wire(6, 0, gateD));
            AddInputWire(new Wire(7, 1, gateD));
            AddWire(gateD, new Wire(0, 3, -1, true));
        }
    }
}
