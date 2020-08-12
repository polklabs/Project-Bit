using Gates;

namespace Chips
{
    public class Dual_D_FlipFlop : Chip
    {
        /// <summary>
        /// Data = 0,1,
        /// Clk = 2,3,
        /// Clr = 4,5,
        /// Pr = 6,7,
        /// 
        /// Q = 0,1,
        /// Q' = 2,3
        /// </summary>
        public Dual_D_FlipFlop() : base(8, 4)
        {
            D_FlipFlop_Re chip1 = new D_FlipFlop_Re();
            int index1 = AddChip(chip1);

            D_FlipFlop_Re chip2 = new D_FlipFlop_Re();
            int index2 = AddChip(chip2);

            // Clr
            AddWire(ID, new Wire(4, 2, index1, true, true));
            AddWire(ID, new Wire(5, 2, index2, true, true));

            // Pr
            AddWire(ID, new Wire(6, 3, index1, true, true));
            AddWire(ID, new Wire(7, 3, index2, true, true));

            // Clk
            AddWire(ID, new Wire(2, 0, index1, true));
            AddWire(ID, new Wire(3, 0, index2, true));

            // Data
            AddWire(ID, new Wire(0, 1, index1, true));
            AddWire(ID, new Wire(1, 1, index2, true));

            // Output
            AddWire(chip1.ID, new Wire(0, 0, -1, true));
            AddWire(chip2.ID, new Wire(0, 1, -1, true));
            AddWire(chip1.ID, new Wire(1, 2, -1, true));
            AddWire(chip2.ID, new Wire(1, 3, -1, true));

        }
    }
}