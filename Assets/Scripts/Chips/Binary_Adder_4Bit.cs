 using Gates;

/**
 * 4-Bit Binary Adder output based on the 74LS283
 * Used in the PQ130_Adder4Bit
 */

namespace Chips
{
    public class Binary_Adder_4Bit : Chip
    {
        public Binary_Adder_4Bit() : base(9, 5)
        {
            Full_Adder chipA = new Full_Adder();
            int indexA = AddChip(chipA);

            Full_Adder chipB = new Full_Adder();
            int indexB = AddChip(chipB);

            Full_Adder chipC = new Full_Adder();
            int indexC = AddChip(chipC);

            Full_Adder chipD = new Full_Adder();
            int indexD = AddChip(chipD);

            // Chip A
            AddWire(ID, new Wire(8, 2, indexA, true));
            AddWire(ID, new Wire(7, 0, indexA, true));
            AddWire(ID, new Wire(6, 1, indexA, true));
            AddWire(chipA.ID, new Wire(0, 4, -1, true));
            AddWire(chipA.ID, new Wire(1, 2, indexB, true));

            // Chip B            
            AddWire(ID, new Wire(5, 0, indexB, true));
            AddWire(ID, new Wire(4, 1, indexB, true));
            AddWire(chipB.ID, new Wire(0, 3, -1, true));
            AddWire(chipB.ID, new Wire(1, 2, indexC, true));

            // Chip C
            AddWire(ID, new Wire(3, 0, indexC, true));
            AddWire(ID, new Wire(2, 1, indexC, true));
            AddWire(chipC.ID, new Wire(0, 2, -1, true));
            AddWire(chipC.ID, new Wire(1, 2, indexD, true));

            // Chip D   
            AddWire(ID, new Wire(1, 0, indexD, true));
            AddWire(ID, new Wire(0, 1, indexD, true));
            AddWire(chipD.ID, new Wire(0, 1, -1, true));
            AddWire(chipD.ID, new Wire(1, 0, -1, true));
        }
    }
}