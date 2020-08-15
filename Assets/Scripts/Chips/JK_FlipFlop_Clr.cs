using Gates;

namespace Chips
{
    public class JK_FlipFlop_Clr : Chip
    {
        /// <summary>
        /// J = 0,
        /// K = 1,
        /// CLK = 2,
        /// CLR = 3
        /// 
        /// Q = 0,
        /// Q' = 1
        /// </summary>
        public JK_FlipFlop_Clr() : base(4, 2)
        {
            ScrubOutput = true;

            Gate Not = new NotGate();
            int indexNot = AddGate(Not);

            Gate GateA = new ANDGate(3);
            int indexA = AddGate(GateA);

            Gate GateB = new ANDGate(3);
            int indexB = AddGate(GateB);

            Gate GateC = new NORGate();
            int indexC = AddGate(GateC);

            Gate GateD = new NORGate(3);
            int indexD = AddGate(GateD);

            Gate GateE = new ANDGate();
            int indexE = AddGate(GateE);

            Gate GateF = new ANDGate();
            int indexF = AddGate(GateF);

            Gate GateG = new NORGate();
            int indexG = AddGate(GateG);

            Gate GateH = new NORGate();
            int indexH = AddGate(GateH);

            // Not
            AddWire(ID, new Wire(2, 0, indexNot));
            AddWire(Not.ID, new Wire(0, 1, indexE));
            AddWire(Not.ID, new Wire(0, 0, indexF));

            // J
            AddWire(ID, new Wire(0, 1, indexA));

            // K 
            AddWire(ID, new Wire(1, 1, indexB));

            // CLK
            AddWire(ID, new Wire(2, 2, indexA));
            AddWire(ID, new Wire(2, 0, indexB));            

            // Clr
            AddWire(ID, new Wire(3, 2, indexD));          

            // A
            AddWire(GateA.ID, new Wire(0, 0, indexC));

            // B
            AddWire(GateB.ID, new Wire(0, 1, indexD));

            // C
            AddWire(GateC.ID, new Wire(0, 0, indexD));
            AddWire(GateC.ID, new Wire(0, 0, indexE));

            // D
            AddWire(GateD.ID, new Wire(0, 1, indexC));
            AddWire(GateD.ID, new Wire(0, 1, indexF));

            // E
            AddWire(GateE.ID, new Wire(0, 0, indexG));

            // F
            AddWire(GateF.ID, new Wire(0, 1, indexH));

            // G
            AddWire(GateG.ID, new Wire(0, 0, indexH));
            AddWire(GateG.ID, new Wire(0, 2, indexB));
            AddWire(GateG.ID, new Wire(0, 0, -1, true));

            // H
            AddWire(GateH.ID, new Wire(0, 1, indexG));
            AddWire(GateH.ID, new Wire(0, 0, indexA));
            AddWire(GateH.ID, new Wire(0, 1, -1, true));
        }

    }
}
