using Gates;

namespace Chips
{
    public class Octal_D_FlipFlop : Chip
    {
        /// <summary>
        /// Data = 0-7,
        /// Clk = 8,
        /// OE = 9
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

            ANDGate gate1 = new ANDGate();
            int i1 = AddGate(gate1);

            ANDGate gate2 = new ANDGate();
            int i2 = AddGate(gate2);

            ANDGate gate3 = new ANDGate();
            int i3 = AddGate(gate3);

            ANDGate gate4 = new ANDGate();
            int i4 = AddGate(gate4);

            ANDGate gate5 = new ANDGate();
            int i5 = AddGate(gate5);

            ANDGate gate6 = new ANDGate();
            int i6 = AddGate(gate6);

            ANDGate gate7 = new ANDGate();
            int i7 = AddGate(gate7);

            ANDGate gate8 = new ANDGate();
            int i8 = AddGate(gate8);

            NotGate gateA = new NotGate();
            int indexA = AddGate(gateA);

            // OE
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
            AddWire(gateA, new Wire(0, 1, i1));
            AddWire(gateA, new Wire(0, 1, i2));
            AddWire(gateA, new Wire(0, 1, i3));
            AddWire(gateA, new Wire(0, 1, i4));
            AddWire(gateA, new Wire(0, 1, i5));
            AddWire(gateA, new Wire(0, 1, i6));
            AddWire(gateA, new Wire(0, 1, i7));
            AddWire(gateA, new Wire(0, 1, i8));

            // D FF
            AddWire(chip1, new Wire(0, 0, i1));
            AddWire(chip2, new Wire(0, 0, i2));
            AddWire(chip3, new Wire(0, 0, i3));
            AddWire(chip4, new Wire(0, 0, i4));
            AddWire(chip5, new Wire(0, 0, i5));
            AddWire(chip6, new Wire(0, 0, i6));
            AddWire(chip7, new Wire(0, 0, i7));
            AddWire(chip8, new Wire(0, 0, i8));

            // Output
            AddWire(gate1, new Wire(0, 0, -1, true));
            AddWire(gate2, new Wire(0, 1, -1, true));
            AddWire(gate3, new Wire(0, 2, -1, true));
            AddWire(gate4, new Wire(0, 3, -1, true));
            AddWire(gate5, new Wire(0, 4, -1, true));
            AddWire(gate6, new Wire(0, 5, -1, true));
            AddWire(gate7, new Wire(0, 6, -1, true));
            AddWire(gate8, new Wire(0, 7, -1, true));

        }
    }
}