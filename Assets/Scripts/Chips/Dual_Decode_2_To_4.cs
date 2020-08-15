using Gates;

namespace Chips
{
    public class Dual_Decode_2_To_4 : Chip
    {
        public Dual_Decode_2_To_4() : base(6, 8)
        {
            // 2 to 4 Decoder ----------------------------------

            NotGate notA = new NotGate();
            int indexA = AddGate(notA);

            NotGate notB = new NotGate();
            int indexB = AddGate(notB);

            ANDGate gate0 = new ANDGate(3);
            int index0 = AddGate(gate0);

            ANDGate gate1 = new ANDGate(3);
            int index1 = AddGate(gate1);

            ANDGate gate2 = new ANDGate(3);
            int index2 = AddGate(gate2);

            ANDGate gate3 = new ANDGate(3);
            int index3 = AddGate(gate3);

            AddWire(ID, new Wire(0, 0, indexA));
            AddWire(ID, new Wire(1, 0, indexB));

            // 0
            AddWire(notA, new Wire(0, 0, index0));
            AddWire(notB, new Wire(0, 1, index0));
            AddWire(gate0, new Wire(0, 0, -1, true, true));

            // 1
            AddWire(ID, new Wire(0, 0, index1));
            AddWire(notB, new Wire(0, 1, index1));
            AddWire(gate1, new Wire(0, 1, -1, true, true));

            // 2
            AddWire(notA, new Wire(0, 0, index2));
            AddWire(ID, new Wire(1, 1, index2));
            AddWire(gate2, new Wire(0, 2, -1, true, true));

            // 3
            AddWire(ID, new Wire(0, 0, index3));
            AddWire(ID, new Wire(1, 1, index3));
            AddWire(gate3, new Wire(0, 3, -1, true, true));

            // Enable
            AddWire(ID, new Wire(2, 2, index0, false, true));
            AddWire(ID, new Wire(2, 2, index1, false, true));
            AddWire(ID, new Wire(2, 2, index2, false, true));
            AddWire(ID, new Wire(2, 2, index3, false, true));

            // 2 to 4 Decoder ----------------------------------

            NotGate notC = new NotGate();
            int indexC = AddGate(notC);

            NotGate notD = new NotGate();
            int indexD = AddGate(notD);

            ANDGate gate4 = new ANDGate(3);
            int index4 = AddGate(gate4);

            ANDGate gate5 = new ANDGate(3);
            int index5 = AddGate(gate5);

            ANDGate gate6 = new ANDGate(3);
            int index6 = AddGate(gate6);

            ANDGate gate7 = new ANDGate(3);
            int index7 = AddGate(gate7);

            AddWire(ID, new Wire(3, 0, indexC));
            AddWire(ID, new Wire(4, 0, indexD));

            // 0
            AddWire(notC, new Wire(0, 0, index4));
            AddWire(notD, new Wire(0, 1, index4));
            AddWire(gate4, new Wire(0, 4, -1, true, true));

            // 1
            AddWire(ID, new Wire(3, 0, index5));
            AddWire(notD, new Wire(0, 1, index5));
            AddWire(gate5, new Wire(0, 5, -1, true, true));

            // 2
            AddWire(notC, new Wire(0, 0, index6));
            AddWire(ID, new Wire(4, 1, index6));
            AddWire(gate6, new Wire(0, 6, -1, true, true));

            // 3
            AddWire(ID, new Wire(3, 0, index7));
            AddWire(ID, new Wire(4, 1, index7));
            AddWire(gate7, new Wire(0, 7, -1, true, true));

            // Enable
            AddWire(ID, new Wire(5, 2, index4, false, true));
            AddWire(ID, new Wire(5, 2, index5, false, true));
            AddWire(ID, new Wire(5, 2, index6, false, true));
            AddWire(ID, new Wire(5, 2, index7, false, true));
        }
    }
}