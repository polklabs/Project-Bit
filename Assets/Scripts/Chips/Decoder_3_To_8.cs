using Gates;
using Wires;

namespace Chips
{
    public class Decoder_3_To_8 : Chip
    {
        public Decoder_3_To_8() : base(4, 8)
        {
            Gate notA = AddGateIndexed(new NotGate());
            Gate notB = AddGateIndexed(new NotGate());
            Gate notC = AddGateIndexed(new NotGate());
            Gate gate0 = AddGateIndexed(new ANDGate(4));
            Gate gate1 = AddGateIndexed(new ANDGate(4));
            Gate gate2 = AddGateIndexed(new ANDGate(4));
            Gate gate3 = AddGateIndexed(new ANDGate(4));
            Gate gate4 = AddGateIndexed(new ANDGate(4));
            Gate gate5 = AddGateIndexed(new ANDGate(4));
            Gate gate6 = AddGateIndexed(new ANDGate(4));
            Gate gate7 = AddGateIndexed(new ANDGate(4));

            AddInputWire(new Wire(0, 0, notA));
            AddInputWire(new Wire(1, 0, notB));
            AddInputWire(new Wire(2, 0, notC));

            AddInputWires(Wire.Create(0, 0, false, gate1, gate3, gate5, gate7));
            AddInputWires(Wire.Create(1, 1, false, gate2, gate3, gate6, gate7));
            AddInputWires(Wire.Create(2, 2, false, gate4, gate5, gate6, gate7));

            // Enable
            AddInputWires(Wire.Create(3, 3, false, gate0, gate1, gate2, gate3, gate4, gate5, gate6, gate7));

            AddWires(notA, Wire.Create(0, 0, false, gate0, gate2, gate4, gate6));
            AddWires(notB, Wire.Create(0, 1, false, gate0, gate1, gate4, gate5));
            AddWires(notC, Wire.Create(0, 2, false, gate0, gate1, gate2, gate3));

            AddWire(gate0, new Wire(0, 0, -1, true));
            AddWire(gate1, new Wire(0, 1, -1, true));
            AddWire(gate2, new Wire(0, 2, -1, true));
            AddWire(gate3, new Wire(0, 3, -1, true));
            AddWire(gate4, new Wire(0, 4, -1, true));
            AddWire(gate5, new Wire(0, 5, -1, true));
            AddWire(gate6, new Wire(0, 6, -1, true));
            AddWire(gate7, new Wire(0, 7, -1, true));
        }
    }
}