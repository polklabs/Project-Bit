using Gates;
using Wires;

namespace Chips
{
    public class Bcd_To_Dec : Chip
    {
        public Bcd_To_Dec() : base (4, 10)
        {
            Gate notA = AddGateIndexed(new NotGate());
            Gate notB = AddGateIndexed(new NotGate());
            Gate notC = AddGateIndexed(new NotGate());
            Gate notD = AddGateIndexed(new NotGate());
            Gate gate0 = AddGateIndexed(new ANDGate(4));
            Gate gate1 = AddGateIndexed(new ANDGate(4));
            Gate gate2 = AddGateIndexed(new ANDGate(4));
            Gate gate3 = AddGateIndexed(new ANDGate(4));
            Gate gate4 = AddGateIndexed(new ANDGate(4));
            Gate gate5 = AddGateIndexed(new ANDGate(4));
            Gate gate6 = AddGateIndexed(new ANDGate(4));
            Gate gate7 = AddGateIndexed(new ANDGate(4));
            Gate gate8 = AddGateIndexed(new ANDGate(4));
            Gate gate9 = AddGateIndexed(new ANDGate(4));

            AddInputWire(new Wire(0, 0, notA));
            AddInputWire(new Wire(1, 0, notB));
            AddInputWire(new Wire(2, 0, notC));
            AddInputWire(new Wire(3, 0, notD));

            AddInputWire(new Wire(0, 0, gate1));
            AddInputWire(new Wire(0, 0, gate3));
            AddInputWire(new Wire(0, 0, gate5));
            AddInputWire(new Wire(0, 0, gate7));
            AddInputWire(new Wire(0, 0, gate9));

            AddInputWire(new Wire(1, 1, gate2));
            AddInputWire(new Wire(1, 1, gate3));
            AddInputWire(new Wire(1, 1, gate6));
            AddInputWire(new Wire(1, 1, gate7));

            AddInputWire(new Wire(2, 2, gate4));
            AddInputWire(new Wire(2, 2, gate5));
            AddInputWire(new Wire(2, 2, gate6));
            AddInputWire(new Wire(2, 2, gate7));

            AddInputWire(new Wire(3, 3, gate8));
            AddInputWire(new Wire(3, 3, gate9));

            AddWire(notA, new Wire(0, 0, gate0));
            AddWire(notA, new Wire(0, 0, gate2));
            AddWire(notA, new Wire(0, 0, gate4));
            AddWire(notA, new Wire(0, 0, gate6));
            AddWire(notA, new Wire(0, 0, gate8));

            AddWire(notB, new Wire(0, 1, gate0));
            AddWire(notB, new Wire(0, 1, gate1));
            AddWire(notB, new Wire(0, 1, gate4));
            AddWire(notB, new Wire(0, 1, gate5));
            AddWire(notB, new Wire(0, 1, gate8));
            AddWire(notB, new Wire(0, 1, gate9));

            AddWire(notC, new Wire(0, 2, gate0));
            AddWire(notC, new Wire(0, 2, gate1));
            AddWire(notC, new Wire(0, 2, gate2));
            AddWire(notC, new Wire(0, 2, gate3));
            AddWire(notC, new Wire(0, 2, gate8));
            AddWire(notC, new Wire(0, 2, gate9));

            AddWire(notD, new Wire(0, 3, gate0));
            AddWire(notD, new Wire(0, 3, gate1));
            AddWire(notD, new Wire(0, 3, gate2));
            AddWire(notD, new Wire(0, 3, gate3));
            AddWire(notD, new Wire(0, 3, gate4));
            AddWire(notD, new Wire(0, 3, gate5));
            AddWire(notD, new Wire(0, 3, gate6));
            AddWire(notD, new Wire(0, 3, gate7));

            AddWire(gate0, new Wire(0, 0, -1, true));
            AddWire(gate1, new Wire(0, 1, -1, true));
            AddWire(gate2, new Wire(0, 2, -1, true));
            AddWire(gate3, new Wire(0, 3, -1, true));
            AddWire(gate4, new Wire(0, 4, -1, true));
            AddWire(gate5, new Wire(0, 5, -1, true));
            AddWire(gate6, new Wire(0, 6, -1, true));
            AddWire(gate7, new Wire(0, 7, -1, true));
            AddWire(gate8, new Wire(0, 8, -1, true));
            AddWire(gate9, new Wire(0, 9, -1, true));
        }
    }
}