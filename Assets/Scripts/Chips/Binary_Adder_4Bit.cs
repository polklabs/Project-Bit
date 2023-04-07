using Gates;
using Wires;

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
            Chip chipA = AddChipIndexed(new Full_Adder());
            Chip chipB = AddChipIndexed(new Full_Adder());
            Chip chipC = AddChipIndexed(new Full_Adder());
            Chip chipD = AddChipIndexed(new Full_Adder());

            // Chip A
            AddInputWire(new Wire(8, 2, chipA, true));
            AddInputWire(new Wire(7, 0, chipA, true));
            AddInputWire(new Wire(6, 1, chipA, true));
            AddWire(chipA, new Wire(0, 4, -1, true));
            AddWire(chipA, new Wire(1, 2, chipB, true));

            // Chip B            
            AddInputWire(new Wire(5, 0, chipB, true));
            AddInputWire(new Wire(4, 1, chipB, true));
            AddWire(chipB, new Wire(0, 3, -1, true));
            AddWire(chipB, new Wire(1, 2, chipC, true));

            // Chip C
            AddInputWire(new Wire(3, 0, chipC, true));
            AddInputWire(new Wire(2, 1, chipC, true));
            AddWire(chipC, new Wire(0, 2, -1, true));
            AddWire(chipC, new Wire(1, 2, chipD, true));

            // Chip D   
            AddInputWire(new Wire(1, 0, chipD, true));
            AddInputWire(new Wire(0, 1, chipD, true));
            AddWire(chipD, new Wire(0, 1, -1, true));
            AddWire(chipD, new Wire(1, 0, -1, true));
        }
    }
}