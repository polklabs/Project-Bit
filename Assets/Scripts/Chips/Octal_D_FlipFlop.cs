using Gates;

namespace Chips
{
    public class Octal_D_FlipFlop : Chip
    {
        /// <summary>
        /// Data = 0-7,
        /// Clk = 8,
        /// Clr = 9
        /// 
        /// Q = 0-7
        /// </summary>
        public Octal_D_FlipFlop() : base(10, 8)
        {
            D_FlipFlop_Re chip1 = new D_FlipFlop_Re();
            int index1 = AddChip(chip1);

            D_FlipFlop_Re chip2 = new D_FlipFlop_Re();
            int index2 = AddChip(chip2);

            D_FlipFlop_Re chip3 = new D_FlipFlop_Re();
            int index3 = AddChip(chip3);

            D_FlipFlop_Re chip4 = new D_FlipFlop_Re();
            int index4 = AddChip(chip4);

            D_FlipFlop_Re chip5 = new D_FlipFlop_Re();
            int index5 = AddChip(chip5);

            D_FlipFlop_Re chip6 = new D_FlipFlop_Re();
            int index6 = AddChip(chip6);

            D_FlipFlop_Re chip7 = new D_FlipFlop_Re();
            int index7 = AddChip(chip7);

            D_FlipFlop_Re chip8 = new D_FlipFlop_Re();
            int index8 = AddChip(chip8);

            BufferGate gateA = new BufferGate();
            int indexA = AddGate(gateA);

            // Clr
            AddWire(ID, new Wire(9, 0, indexA));

            // Clk
            AddWire(ID, new Wire(8, 0, index1, true));
            AddWire(ID, new Wire(8, 0, index2, true));
            AddWire(ID, new Wire(8, 0, index3, true));
            AddWire(ID, new Wire(8, 0, index4, true));
            AddWire(ID, new Wire(8, 0, index5, true));
            AddWire(ID, new Wire(8, 0, index6, true));
            AddWire(ID, new Wire(8, 0, index7, true));
            AddWire(ID, new Wire(8, 0, index8, true));

            // Data
            AddWire(ID, new Wire(0, 1, index1, true));
            AddWire(ID, new Wire(1, 1, index2, true));
            AddWire(ID, new Wire(2, 1, index3, true));
            AddWire(ID, new Wire(3, 1, index4, true));
            AddWire(ID, new Wire(4, 1, index5, true));
            AddWire(ID, new Wire(5, 1, index6, true));
            AddWire(ID, new Wire(6, 1, index7, true));
            AddWire(ID, new Wire(7, 1, index8, true));

            // Gate A
            AddWire(gateA.ID, new Wire(0, 2, index1, true));
            AddWire(gateA.ID, new Wire(0, 2, index2, true));
            AddWire(gateA.ID, new Wire(0, 2, index3, true));
            AddWire(gateA.ID, new Wire(0, 2, index4, true));
            AddWire(gateA.ID, new Wire(0, 2, index5, true));
            AddWire(gateA.ID, new Wire(0, 2, index6, true));
            AddWire(gateA.ID, new Wire(0, 2, index7, true));
            AddWire(gateA.ID, new Wire(0, 2, index8, true));

            // Output
            AddWire(chip1.ID, new Wire(0, 0, -1, true));
            AddWire(chip2.ID, new Wire(0, 1, -1, true));
            AddWire(chip3.ID, new Wire(0, 2, -1, true));
            AddWire(chip4.ID, new Wire(0, 3, -1, true));
            AddWire(chip5.ID, new Wire(0, 4, -1, true));
            AddWire(chip6.ID, new Wire(0, 5, -1, true));
            AddWire(chip7.ID, new Wire(0, 6, -1, true));
            AddWire(chip8.ID, new Wire(0, 7, -1, true));

        }
    }
}