using Gates;

namespace Chips
{
    public class Dual_JK_FlipFlop : Chip
    {

        public Dual_JK_FlipFlop() : base(8, 4)
        {
            Chip chipA = new JK_FlipFlop_Clr();
            int indexA = AddChip(chipA);

            Chip chipB = new JK_FlipFlop_Clr();
            int indexB = AddChip(chipB);

            AddWire(ID, new Wire(0, 0, indexA, true));
            AddWire(ID, new Wire(1, 1, indexA, true));
            AddWire(ID, new Wire(2, 2, indexA, true));
            AddWire(ID, new Wire(3, 3, indexA, true));

            AddWire(ID, new Wire(4, 0, indexB, true));
            AddWire(ID, new Wire(5, 1, indexB, true));
            AddWire(ID, new Wire(6, 2, indexB, true));
            AddWire(ID, new Wire(7, 3, indexB, true));

            AddWire(chipA.ID, new Wire(0, 0, -1, true));
            AddWire(chipA.ID, new Wire(1, 1, -1, true));

            AddWire(chipB.ID, new Wire(0, 2, -1, true));
            AddWire(chipB.ID, new Wire(1, 3, -1, true));
        }

    }
}
