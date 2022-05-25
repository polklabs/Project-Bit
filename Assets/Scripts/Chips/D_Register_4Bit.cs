using Gates;

namespace Chips
{
    class D_Register_4Bit : Chip
    {

        public D_Register_4Bit(): base(10, 4)
        {
            // Output control
            Gate gateA = AddGateIndexed(new ANDGate());

            // Data Enable
            Gate gateB = AddGateIndexed(new ANDGate());

            Gate gateC = AddGateIndexed(new NotGate());
            Gate gateD = AddGateIndexed(new NotGate());
            Gate gateE = AddGateIndexed(new NotGate());

            // Q1
            Gate gateF = AddGateIndexed(new ANDGate());
            Gate gateG = AddGateIndexed(new ANDGate());
            Gate gateR = AddGateIndexed(new ORGate());
            Chip chipH = AddChipIndexed(new D_FlipFlop_Re());
            Gate gateV = AddGateIndexed(new TriBufferGate());

            // Q2
            Gate gateI = AddGateIndexed(new ANDGate());
            Gate gateJ = AddGateIndexed(new ANDGate());
            Gate gateS = AddGateIndexed(new ORGate());
            Chip chipK = AddChipIndexed(new D_FlipFlop_Re());
            Gate gateW = AddGateIndexed(new TriBufferGate());

            // Q3
            Gate gateL = AddGateIndexed(new ANDGate());
            Gate gateM = AddGateIndexed(new ANDGate());
            Gate gateT = AddGateIndexed(new ORGate());
            Chip chipN = AddChipIndexed(new D_FlipFlop_Re());
            Gate gateX = AddGateIndexed(new TriBufferGate());

            // Q4
            Gate gateO = AddGateIndexed(new ANDGate());
            Gate gateP = AddGateIndexed(new ANDGate());
            Gate gateU = AddGateIndexed(new ORGate());
            Chip chipQ = AddChipIndexed(new D_FlipFlop_Re());
            Gate gateY = AddGateIndexed(new TriBufferGate());

            // Output control
            AddInputWire(new Wire(0, 0, gateA, true));
            AddInputWire(new Wire(1, 1, gateA, true));
            AddWire(gateA.ID, new Wire(0, 1, gateV));
            AddWire(gateA.ID, new Wire(0, 1, gateW));
            AddWire(gateA.ID, new Wire(0, 1, gateX));
            AddWire(gateA.ID, new Wire(0, 1, gateY));

            // Data Enable
            AddInputWire(new Wire(3, 0, gateB, true));
            AddInputWire(new Wire(4, 1, gateB, true));
            AddWire(gateB, new Wire(0, 0, gateC));
            AddWire(gateB, new Wire(0, 1, gateG));
            AddWire(gateB, new Wire(0, 1, gateJ));
            AddWire(gateB, new Wire(0, 1, gateM));
            AddWire(gateB, new Wire(0, 1, gateP));
            AddWire(gateC, new Wire(0, 1, gateF));
            AddWire(gateC, new Wire(0, 1, gateI));
            AddWire(gateC, new Wire(0, 1, gateL));
            AddWire(gateC, new Wire(0, 1, gateO));

            // CLK
            AddInputWire(new Wire(6, 0, gateD));
            AddWire(gateD, new Wire(0, 0, chipH, true));
            AddWire(gateD, new Wire(0, 0, chipK, true));
            AddWire(gateD, new Wire(0, 0, chipN, true));
            AddWire(gateD, new Wire(0, 0, chipQ, true));

            // CLR
            AddInputWire(new Wire(9, 0, gateE));
            AddWire(gateE, new Wire(0, 2, chipH, true));
            AddWire(gateE, new Wire(0, 2, chipK, true));
            AddWire(gateE, new Wire(0, 2, chipN, true));
            AddWire(gateE, new Wire(0, 2, chipQ, true));

            // Q1
            AddInputWire(new Wire(2, 0, gateG));
            AddWire(gateF, new Wire(0, 0, gateR));
            AddWire(gateG, new Wire(0, 1, gateR));
            AddWire(gateR, new Wire(0, 1, chipH));
            AddWire(chipH, new Wire(0, 0, gateF));
            AddWire(chipH, new Wire(0, 0, gateV));
            AddWire(gateV, new Wire(0, 0, -1, true));

            // Q2
            AddInputWire(new Wire(5, 0, gateJ));
            AddWire(gateI, new Wire(0, 0, gateS));
            AddWire(gateJ, new Wire(0, 1, gateS));
            AddWire(gateS, new Wire(0, 1, chipK));
            AddWire(chipK, new Wire(0, 0, gateI));
            AddWire(chipK, new Wire(0, 0, gateW));
            AddWire(gateW, new Wire(0, 1, -1, true));

            // Q3
            AddInputWire(new Wire(7, 0, gateM));
            AddWire(gateL, new Wire(0, 0, gateT));
            AddWire(gateM, new Wire(0, 1, gateT));
            AddWire(gateT, new Wire(0, 1, chipN));
            AddWire(chipN, new Wire(0, 0, gateL));
            AddWire(chipN, new Wire(0, 0, gateX));
            AddWire(gateX, new Wire(0, 2, -1, true));

            // Q4
            AddInputWire(new Wire(8, 0, gateP));
            AddWire(gateO, new Wire(0, 0, gateU));
            AddWire(gateP, new Wire(0, 1, gateU));
            AddWire(gateU, new Wire(0, 1, chipQ));
            AddWire(chipQ, new Wire(0, 0, gateO));
            AddWire(chipQ, new Wire(0, 0, gateY));
            AddWire(gateY, new Wire(0, 3, -1, true));
        }

    }
}
