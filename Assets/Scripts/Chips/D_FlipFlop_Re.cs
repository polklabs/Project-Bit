﻿using Gates;

namespace Chips
{
    public class D_FlipFlop_Re : Chip
    {
        /// <summary>
        /// Clk = 0,
        /// Data = 1,
        /// Clr = 2,
        /// Pr = 3
        /// 
        /// Q = 0
        /// Q' = 1
        /// </summary>
        public D_FlipFlop_Re() : base(4, 2)
        {
            ScrubOutput = true;

            Gate gateA = new NANDGate(3);
            int indexA = AddGate(gateA);

            Gate gateB = new NANDGate(3);
            int indexB = AddGate(gateB);

            Gate gateC = new NANDGate(3);
            int indexC = AddGate(gateC);

            Gate gateD = new NANDGate(3);
            int indexD = AddGate(gateD);

            Gate gateE = new NANDGate();
            int indexE = AddGate(gateE);

            Gate gateF = new NANDGate();
            int indexF = AddGate(gateF);

            //Clock
            AddWire(ID, new Wire(0, 1, indexB, false));
            AddWire(ID, new Wire(0, 1, indexC, false));

            //Data
            AddWire(ID, new Wire(1, 1, indexD, false));

            //Clr
            AddWire(ID, new Wire(2, 2, indexB, false, true));
            AddWire(ID, new Wire(2, 2, indexD, false, true));

            //Pr
            AddWire(ID, new Wire(3, 2, indexA, false, true));
            AddWire(ID, new Wire(3, 2, indexC, false, true));

            //Gate A
            AddWire(gateA.ID, new Wire(0, 0, indexB, false));

            //Gate B
            AddWire(gateB.ID, new Wire(0, 1, indexA, false));
            AddWire(gateB.ID, new Wire(0, 0, indexE, false));
            AddWire(gateB.ID, new Wire(0, 0, indexC, false));

            //Gate C
            AddWire(gateC.ID, new Wire(0, 1, indexF, false));
            AddWire(gateC.ID, new Wire(0, 0, indexD, false));

            //Gate D
            AddWire(gateD.ID, new Wire(0, 2, indexC, false));
            AddWire(gateD.ID, new Wire(0, 0, indexA, false));

            //Gate E
            AddWire(gateE.ID, new Wire(0, 0, indexF, false));
            //Q
            AddWire(gateE.ID, new Wire(0, 0, -1, true));

            //Gate F
            AddWire(gateF.ID, new Wire(0, 1, indexE, false));
            //Q'
            AddWire(gateF.ID, new Wire(0, 1, -1, true));
        }
    }
}