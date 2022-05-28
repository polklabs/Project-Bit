using Gates;
using Wires;

namespace Chips
{
    public class Full_Adder : Chip
    {
        public Full_Adder() : base(3, 2)
        {
            Gate gateA = AddGateIndexed(new XORGate());
            Gate gateB = AddGateIndexed(new XORGate());
            Gate gateC = AddGateIndexed(new ANDGate());
            Gate gateD = AddGateIndexed(new ANDGate());
            Gate gateE = AddGateIndexed(new ORGate());

            // Gate A
            AddInputWire(new Wire(0, 0, gateA));
            AddInputWire(new Wire(1, 1, gateA));
            AddWires(gateA, Wire.Create(0, 0, false, gateB, gateC));

            // Gate B
            AddInputWire(new Wire(2, 1, gateB));
            AddWire(gateB, new Wire(0, 0, -1, true));

            // Gate C
            AddInputWire(new Wire(2, 1, gateC));
            AddWire(gateC, new Wire(0, 0, gateE));

            // Gate D
            AddInputWire(new Wire(0, 0, gateD));
            AddInputWire(new Wire(1, 1, gateD));
            AddWire(gateD, new Wire(0, 1, gateE));

            // Gate E
            AddWire(gateE, new Wire(0, 1, -1, true));
        }
    }
}
