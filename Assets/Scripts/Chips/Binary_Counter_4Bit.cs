using Gates;

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
            JK_FlipFlop_Clr chip0 = new JK_FlipFlop_Clr();
            chip0.SetInputBit(0, true);
            chip0.SetInputBit(1, true);
            int index0 = AddChip(chip0);

            JK_FlipFlop_Clr chip1 = new JK_FlipFlop_Clr();
            chip1.SetInputBit(0, true);
            chip1.SetInputBit(1, true);
            int index1 = AddChip(chip1);

            JK_FlipFlop_Clr chip2 = new JK_FlipFlop_Clr();
            chip2.SetInputBit(0, true);
            chip2.SetInputBit(1, true);
            int index2 = AddChip(chip2);

            JK_FlipFlop_Clr chip3 = new JK_FlipFlop_Clr();
            chip3.SetInputBit(0, true);
            chip3.SetInputBit(1, true);
            int index3 = AddChip(chip3);

            AddWire(ID, new Wire(0, 2, index0, true));            

            AddWire(ID, new Wire(1, 3, index0, true));
            AddWire(ID, new Wire(1, 3, index1, true));
            AddWire(ID, new Wire(1, 3, index2, true));
            AddWire(ID, new Wire(1, 3, index3, true));

            AddWire(chip0, new Wire(0, 2, index1, true));
            AddWire(chip1, new Wire(0, 2, index2, true));
            AddWire(chip2, new Wire(0, 2, index3, true));

            AddWire(chip0, new Wire(0, 0, -1, true));
            AddWire(chip1, new Wire(0, 1, -1, true));
            AddWire(chip2, new Wire(0, 2, -1, true));
            AddWire(chip3, new Wire(0, 3, -1, true));
        }
    }
}