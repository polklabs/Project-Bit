using Gates;
using Wires;

/**
 * Binary Counter based on the 74LS161
 * Used in the PQ150G
 */

namespace Chips
{
    public class Binary_Counter_4Bit : Chip
    {
        /// <summary>
        /// CLK = 0,
        /// 1 = CLR        
        /// </summary>
        public Binary_Counter_4Bit() : base(2, 4)
        {
            Chip chip0 = AddChipIndexed(new JK_FlipFlop_Clr());
            chip0.SetInputBit(0, true);
            chip0.SetInputBit(1, true);

            Chip chip1 = AddChipIndexed(new JK_FlipFlop_Clr());
            chip1.SetInputBit(0, true);
            chip1.SetInputBit(1, true);

            Chip chip2 = AddChipIndexed(new JK_FlipFlop_Clr());
            chip2.SetInputBit(0, true);
            chip2.SetInputBit(1, true);

            Chip chip3 = AddChipIndexed(new JK_FlipFlop_Clr());
            chip3.SetInputBit(0, true);
            chip3.SetInputBit(1, true);

            AddInputWire(new Wire(0, 2, chip0));

            AddInputWires(Wire.Create(1, 3, false, chip0, chip1, chip2, chip3));

            AddWire(chip0, new Wire(0, 2, chip1));
            AddWire(chip1, new Wire(0, 2, chip2));
            AddWire(chip2, new Wire(0, 2, chip3));

            AddWire(chip0, new Wire(0, 0, -1, true));
            AddWire(chip1, new Wire(0, 1, -1, true));
            AddWire(chip2, new Wire(0, 2, -1, true));
            AddWire(chip3, new Wire(0, 3, -1, true));
        }
    }
}