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
            Gate gateV = AddGateIndexed(new BufferGate());

            // Q2
            Gate gateI = AddGateIndexed(new ANDGate());
            Gate gateJ = AddGateIndexed(new ANDGate());
            Gate gateS = AddGateIndexed(new ORGate());
            Chip chipK = AddChipIndexed(new D_FlipFlop_Re());
            Gate gateW = AddGateIndexed(new BufferGate());

            // Q3
            Gate gateL = AddGateIndexed(new ANDGate());
            Gate gateM = AddGateIndexed(new ANDGate());
            Gate gateT = AddGateIndexed(new ORGate());
            Chip chipN = AddChipIndexed(new D_FlipFlop_Re());
            Gate gateX = AddGateIndexed(new BufferGate());

            // Q4
            Gate gateO = AddGateIndexed(new ANDGate());
            Gate gateP = AddGateIndexed(new ANDGate());
            Gate gateU = AddGateIndexed(new ORGate());
            Chip chipQ = AddChipIndexed(new D_FlipFlop_Re());
            Gate gateY = AddGateIndexed(new BufferGate());

            // Output control
            AddInputWire(new Wire(0, 0, gateA, false, true));
            AddInputWire(new Wire(1, 1, gateA, false, true));
            //AddWire(gateA.ID, new Wire(0, 1, gateV, false));
            //AddWire(gateA.ID, new Wire(0, 1, gateW, false));
            //AddWire(gateA.ID, new Wire(0, 1, gateX, false));
            //AddWire(gateA.ID, new Wire(0, 1, gateY, false));

            // Data Enable
            AddInputWire(new Wire(3, 0, gateB, false, true));
            AddInputWire(new Wire(4, 1, gateB, false, true));
            AddWire(gateB, new Wire(0, 0, gateC, false));
            AddWire(gateB, new Wire(0, 1, gateG, false));
            AddWire(gateB, new Wire(0, 1, gateJ, false));
            AddWire(gateB, new Wire(0, 1, gateM, false));
            AddWire(gateB, new Wire(0, 1, gateP, false));
            AddWire(gateC, new Wire(0, 1, gateF, false));
            AddWire(gateC, new Wire(0, 1, gateI, false));
            AddWire(gateC, new Wire(0, 1, gateL, false));
            AddWire(gateC, new Wire(0, 1, gateO, false));

            // CLK
            AddInputWire(new Wire(6, 0, gateD, false));
            AddWire(gateD, new Wire(0, 0, chipH, true, true));
            AddWire(gateD, new Wire(0, 0, chipK, true, true));
            AddWire(gateD, new Wire(0, 0, chipN, true, true));
            AddWire(gateD, new Wire(0, 0, chipQ, true, true));

            // CLR
            AddInputWire(new Wire(9, 0, gateE, false));
            AddWire(gateE, new Wire(0, 2, chipH, true, true));
            AddWire(gateE, new Wire(0, 2, chipK, true, true));
            AddWire(gateE, new Wire(0, 2, chipN, true, true));
            AddWire(gateE, new Wire(0, 2, chipQ, true, true));

            // Q1
            AddInputWire(new Wire(2, 0, gateG, false));
            AddWire(gateF, new Wire(0, 0, gateR, false));
            AddWire(gateG, new Wire(0, 1, gateR, false));
            AddWire(gateR, new Wire(0, 1, chipH, true));
            AddWire(chipH, new Wire(0, 0, gateF, false));
            AddWire(chipH, new Wire(0, 0, gateV, false));
            AddWire(gateV, new Wire(0, 0, -1, true));

            // Q2
            AddInputWire(new Wire(5, 0, gateJ, false));
            AddWire(gateI, new Wire(0, 0, gateS, false));
            AddWire(gateJ, new Wire(0, 1, gateS, false));
            AddWire(gateS, new Wire(0, 1, chipK, true));
            AddWire(chipK, new Wire(0, 0, gateI, false));
            AddWire(chipK, new Wire(0, 0, gateW, false));
            AddWire(gateW, new Wire(0, 1, -1, true));

            // Q3
            AddInputWire(new Wire(7, 0, gateM, false));
            AddWire(gateL, new Wire(0, 0, gateT, false));
            AddWire(gateM, new Wire(0, 1, gateT, false));
            AddWire(gateT, new Wire(0, 1, chipN, true));
            AddWire(chipN, new Wire(0, 0, gateL, false));
            AddWire(chipN, new Wire(0, 0, gateX, false));
            AddWire(gateX, new Wire(0, 2, -1, true));

            // Q4
            AddInputWire(new Wire(8, 0, gateP, false));
            AddWire(gateO, new Wire(0, 0, gateU, false));
            AddWire(gateP, new Wire(0, 1, gateU, false));
            AddWire(gateU, new Wire(0, 1, chipQ, true));
            AddWire(chipQ, new Wire(0, 0, gateO, false));
            AddWire(chipQ, new Wire(0, 0, gateY, false));
            AddWire(gateY, new Wire(0, 3, -1, true));
        }

    }
}
