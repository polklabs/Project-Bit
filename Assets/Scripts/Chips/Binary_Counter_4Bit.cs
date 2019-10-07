namespace Chips
{
    public class Binary_Counter_4Bit : Chip
    {
        public Binary_Counter_4Bit() : base(1, 4)
        {
            D_FlipFlop_Re chipA = new D_FlipFlop_Re();
            int indexA = AddChip(chipA);

            D_FlipFlop_Re chipB = new D_FlipFlop_Re();
            int indexB = AddChip(chipB);

            D_FlipFlop_Re chipC = new D_FlipFlop_Re();
            int indexC = AddChip(chipC);

            D_FlipFlop_Re chipD = new D_FlipFlop_Re();
            int indexD = AddChip(chipD);

            //Inputs
            AddWire(ID, new Wire(0, 0, indexA, true));

            //Chip A
            AddWire(chipA.ID, new Wire(0, 0, -1, true));
            AddWire(chipA.ID, new Wire(1, 1, indexA, true));
            AddWire(chipA.ID, new Wire(1, 0, indexB, true));

            //Chip B
            AddWire(chipB.ID, new Wire(0, 1, -1, true));
            AddWire(chipB.ID, new Wire(1, 1, indexB, true));
            AddWire(chipB.ID, new Wire(1, 0, indexC, true));

            //Chip C
            AddWire(chipC.ID, new Wire(0, 2, -1, true));
            AddWire(chipC.ID, new Wire(1, 1, indexC, true));
            AddWire(chipC.ID, new Wire(1, 0, indexD, true));

            //Chip D
            AddWire(chipD.ID, new Wire(0, 3, -1, true));
            AddWire(chipD.ID, new Wire(1, 1, indexD, true));            
        }
    }
}