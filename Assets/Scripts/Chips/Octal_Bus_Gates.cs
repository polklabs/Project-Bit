using Gates;

/**
 * Octal Bus Gates based on the 74LS245
 * Used in the PQ040G_BufferGate
 */

namespace Chips
{
    public class Octal_Bus_Gates : Chip
    {
        public Octal_Bus_Gates() : base(18, 16)
        {
            Gate gateA = new ANDGate();
            int indexA = AddGate(gateA);

            Gate gateB = new ANDGate();
            int indexB = AddGate(gateB);

            Gate gateC = new NotGate();
            int indexC = AddGate(gateC);

            Gate gateD = new NotGate();
            int indexD = AddGate(gateD);

            Gate gateE = new ANDGate();
            int indexE = AddGate(gateE);

            Gate gateF = new ANDGate();
            int indexF = AddGate(gateF);

            Gate gateG = new ANDGate();
            int indexG = AddGate(gateG);

            Gate gateH = new ANDGate();
            int indexH = AddGate(gateH);

            Gate gateI = new ANDGate();
            int indexI = AddGate(gateI);

            Gate gateJ = new ANDGate();
            int indexJ = AddGate(gateJ);

            Gate gateK = new ANDGate();
            int indexK = AddGate(gateK);

            Gate gateL = new ANDGate();
            int indexL = AddGate(gateL);

            Gate gateM = new ANDGate();
            int indexM = AddGate(gateM);

            Gate gateN = new ANDGate();
            int indexN = AddGate(gateN);

            Gate gateO = new ANDGate();
            int indexO = AddGate(gateO);

            Gate gateP = new ANDGate();
            int indexP = AddGate(gateP);

            Gate gateQ = new ANDGate();
            int indexQ = AddGate(gateQ);

            Gate gateR = new ANDGate();
            int indexR = AddGate(gateR);

            Gate gateS = new ANDGate();
            int indexS = AddGate(gateS);

            Gate gateT = new ANDGate();
            int indexT = AddGate(gateT);

            //Inputs
            AddWire(ID, new Wire(0, 0, indexA, false));
            AddWire(ID, new Wire(0, 0, indexC, false));
            AddWire(ID, new Wire(1, 0, indexD, false));
            AddWire(ID, new Wire(2, 1, indexE, false));
            AddWire(ID, new Wire(3, 1, indexF, false));
            AddWire(ID, new Wire(4, 1, indexG, false));
            AddWire(ID, new Wire(5, 1, indexH, false));
            AddWire(ID, new Wire(6, 1, indexI, false));
            AddWire(ID, new Wire(7, 1, indexJ, false));
            AddWire(ID, new Wire(8, 1, indexK, false));
            AddWire(ID, new Wire(9, 1, indexL, false));
            AddWire(ID, new Wire(10, 1, indexM, false));
            AddWire(ID, new Wire(11, 1, indexN, false));
            AddWire(ID, new Wire(12, 1, indexO, false));
            AddWire(ID, new Wire(13, 1, indexP, false));
            AddWire(ID, new Wire(14, 1, indexQ, false));
            AddWire(ID, new Wire(15, 1, indexR, false));
            AddWire(ID, new Wire(16, 1, indexS, false));
            AddWire(ID, new Wire(17, 1, indexT, false));

            //Gate A
            AddWire(gateA.ID, new Wire(0, 0, indexE, false));
            AddWire(gateA.ID, new Wire(0, 0, indexF, false));
            AddWire(gateA.ID, new Wire(0, 0, indexG, false));
            AddWire(gateA.ID, new Wire(0, 0, indexH, false));
            AddWire(gateA.ID, new Wire(0, 0, indexI, false));
            AddWire(gateA.ID, new Wire(0, 0, indexJ, false));
            AddWire(gateA.ID, new Wire(0, 0, indexK, false));
            AddWire(gateA.ID, new Wire(0, 0, indexL, false));

            //Gate B
            AddWire(gateB.ID, new Wire(0, 0, indexM, false));
            AddWire(gateB.ID, new Wire(0, 0, indexN, false));
            AddWire(gateB.ID, new Wire(0, 0, indexO, false));
            AddWire(gateB.ID, new Wire(0, 0, indexP, false));
            AddWire(gateB.ID, new Wire(0, 0, indexQ, false));
            AddWire(gateB.ID, new Wire(0, 0, indexR, false));
            AddWire(gateB.ID, new Wire(0, 0, indexS, false));
            AddWire(gateB.ID, new Wire(0, 0, indexT, false));

            //Gate C
            AddWire(gateC.ID, new Wire(0, 0, indexB, false));

            //Gate D
            AddWire(gateD.ID, new Wire(0, 1, indexB, false));
            AddWire(gateD.ID, new Wire(0, 1, indexA, false));

            //Outputs
            AddWire(gateE.ID, new Wire(0, 8, -1, true));
            AddWire(gateF.ID, new Wire(0, 9, -1, true));
            AddWire(gateG.ID, new Wire(0, 10, -1, true));
            AddWire(gateH.ID, new Wire(0, 11, -1, true));
            AddWire(gateI.ID, new Wire(0, 12, -1, true));
            AddWire(gateJ.ID, new Wire(0, 13, -1, true));
            AddWire(gateK.ID, new Wire(0, 14, -1, true));
            AddWire(gateL.ID, new Wire(0, 15, -1, true));
            AddWire(gateM.ID, new Wire(0, 0, -1, true));
            AddWire(gateN.ID, new Wire(0, 1, -1, true));
            AddWire(gateO.ID, new Wire(0, 2, -1, true));
            AddWire(gateP.ID, new Wire(0, 3, -1, true));
            AddWire(gateQ.ID, new Wire(0, 4, -1, true));
            AddWire(gateR.ID, new Wire(0, 5, -1, true));
            AddWire(gateS.ID, new Wire(0, 6, -1, true));
            AddWire(gateT.ID, new Wire(0, 7, -1, true));

        }
    }
}