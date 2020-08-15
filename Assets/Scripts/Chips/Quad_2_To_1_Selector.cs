using Gates;

namespace Chips
{
    public class Quad_2_To_1_Selector : Chip
    {
        public Quad_2_To_1_Selector() : base(10, 4)
        {
            ANDGate gateA0 = new ANDGate(3);
            int indexA0 = AddGate(gateA0);

            ANDGate gateA1 = new ANDGate(3);
            int indexA1 = AddGate(gateA1);

            ANDGate gateA2 = new ANDGate(3);
            int indexA2 = AddGate(gateA2);

            ANDGate gateA3 = new ANDGate(3);
            int indexA3 = AddGate(gateA3);

            ANDGate gateB0 = new ANDGate(3);
            int indexB0 = AddGate(gateB0);

            ANDGate gateB1 = new ANDGate(3);
            int indexB1 = AddGate(gateB1);

            ANDGate gateB2 = new ANDGate(3);
            int indexB2 = AddGate(gateB2);

            ANDGate gateB3 = new ANDGate(3);
            int indexB3 = AddGate(gateB3);

            ORGate gate0 = new ORGate();
            int index0 = AddGate(gate0);

            ORGate gate1 = new ORGate();
            int index1 = AddGate(gate1);

            ORGate gate2 = new ORGate();
            int index2 = AddGate(gate2);

            ORGate gate3 = new ORGate();
            int index3 = AddGate(gate3);

            AddWire(ID, new Wire(0, 0, indexA0));
            AddWire(ID, new Wire(1, 0, indexA1));
            AddWire(ID, new Wire(2, 0, indexA2));
            AddWire(ID, new Wire(3, 0, indexA3));
            AddWire(ID, new Wire(4, 0, indexB0));
            AddWire(ID, new Wire(5, 0, indexB1));
            AddWire(ID, new Wire(6, 0, indexB2));
            AddWire(ID, new Wire(7, 0, indexB3));

            AddWire(ID, new Wire(8, 1, indexA0, false, true));
            AddWire(ID, new Wire(8, 1, indexA1, false, true));
            AddWire(ID, new Wire(8, 1, indexA2, false, true));
            AddWire(ID, new Wire(8, 1, indexA3, false, true));
            AddWire(ID, new Wire(8, 1, indexB0));
            AddWire(ID, new Wire(8, 1, indexB1));
            AddWire(ID, new Wire(8, 1, indexB2));
            AddWire(ID, new Wire(8, 1, indexB3));

            AddWire(ID, new Wire(9, 2, indexA0, false, true));
            AddWire(ID, new Wire(9, 2, indexA1, false, true));
            AddWire(ID, new Wire(9, 2, indexA2, false, true));
            AddWire(ID, new Wire(9, 2, indexA3, false, true));
            AddWire(ID, new Wire(9, 2, indexB0, false, true));
            AddWire(ID, new Wire(9, 2, indexB1, false, true));
            AddWire(ID, new Wire(9, 2, indexB2, false, true));
            AddWire(ID, new Wire(9, 2, indexB3, false, true));

            AddWire(gateA0, new Wire(0, 0, index0));
            AddWire(gateA1, new Wire(0, 0, index1));
            AddWire(gateA2, new Wire(0, 0, index2));
            AddWire(gateA3, new Wire(0, 0, index3));
            AddWire(gateB0, new Wire(0, 1, index0));
            AddWire(gateB1, new Wire(0, 1, index1));
            AddWire(gateB2, new Wire(0, 1, index2));
            AddWire(gateB3, new Wire(0, 1, index3));

            AddWire(gate0, new Wire(0, 0, -1, true));
            AddWire(gate1, new Wire(0, 1, -1, true));
            AddWire(gate2, new Wire(0, 2, -1, true));
            AddWire(gate3, new Wire(0, 3, -1, true));
        }
    }
}