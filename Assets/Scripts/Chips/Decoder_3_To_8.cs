﻿using Gates;

namespace Chips
{
    public class Decoder_3_To_8 : Chip
    {
        public Decoder_3_To_8() : base(4, 8)
        {
            Gate notA = new NotGate();
            int indexNotA = AddGate(notA);

            Gate notB = new NotGate();
            int indexNotB = AddGate(notB);

            Gate notC = new NotGate();
            int indexNotC = AddGate(notC);

            Gate gate0 = new ANDGate(4);
            int index0 = AddGate(gate0);

            Gate gate1 = new ANDGate(4);
            int index1 = AddGate(gate1);

            Gate gate2 = new ANDGate(4);
            int index2 = AddGate(gate2);

            Gate gate3 = new ANDGate(4);
            int index3 = AddGate(gate3);

            Gate gate4 = new ANDGate(4);
            int index4 = AddGate(gate4);

            Gate gate5 = new ANDGate(4);
            int index5 = AddGate(gate5);

            Gate gate6 = new ANDGate(4);
            int index6 = AddGate(gate6);

            Gate gate7 = new ANDGate(4);
            int index7 = AddGate(gate7);

            AddWire(ID, new Wire(0, 0, indexNotA));
            AddWire(ID, new Wire(1, 0, indexNotB));
            AddWire(ID, new Wire(2, 0, indexNotC));

            AddWire(ID, new Wire(0, 0, index1));
            AddWire(ID, new Wire(0, 0, index3));
            AddWire(ID, new Wire(0, 0, index5));
            AddWire(ID, new Wire(0, 0, index7));

            AddWire(ID, new Wire(1, 1, index2));
            AddWire(ID, new Wire(1, 1, index3));
            AddWire(ID, new Wire(1, 1, index6));
            AddWire(ID, new Wire(1, 1, index7));

            AddWire(ID, new Wire(2, 2, index4));
            AddWire(ID, new Wire(2, 2, index5));
            AddWire(ID, new Wire(2, 2, index6));
            AddWire(ID, new Wire(2, 2, index7));

            // Enable
            AddWire(ID, new Wire(3, 3, index0));
            AddWire(ID, new Wire(3, 3, index1));
            AddWire(ID, new Wire(3, 3, index2));
            AddWire(ID, new Wire(3, 3, index3));
            AddWire(ID, new Wire(3, 3, index4));
            AddWire(ID, new Wire(3, 3, index5));
            AddWire(ID, new Wire(3, 3, index6));
            AddWire(ID, new Wire(3, 3, index7));

            AddWire(notA.ID, new Wire(0, 0, index0));
            AddWire(notA.ID, new Wire(0, 0, index2));
            AddWire(notA.ID, new Wire(0, 0, index4));
            AddWire(notA.ID, new Wire(0, 0, index6));

            AddWire(notB.ID, new Wire(0, 1, index0));
            AddWire(notB.ID, new Wire(0, 1, index1));
            AddWire(notB.ID, new Wire(0, 1, index4));
            AddWire(notB.ID, new Wire(0, 1, index5));

            AddWire(notC.ID, new Wire(0, 2, index0));
            AddWire(notC.ID, new Wire(0, 2, index1));
            AddWire(notC.ID, new Wire(0, 2, index2));
            AddWire(notC.ID, new Wire(0, 2, index3));

            AddWire(gate0.ID, new Wire(0, 0, -1, true));
            AddWire(gate1.ID, new Wire(0, 1, -1, true));
            AddWire(gate2.ID, new Wire(0, 2, -1, true));
            AddWire(gate3.ID, new Wire(0, 3, -1, true));
            AddWire(gate4.ID, new Wire(0, 4, -1, true));
            AddWire(gate5.ID, new Wire(0, 5, -1, true));
            AddWire(gate6.ID, new Wire(0, 6, -1, true));
            AddWire(gate7.ID, new Wire(0, 7, -1, true));
        }
    }
}