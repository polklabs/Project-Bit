using Gates;

namespace Chips
{
    public class Binary_Adder_4Bit : Chip
    {
        public Binary_Adder_4Bit() : base(9, 5)
        {
            Gate gateA = new NANDGate();
            int indexA = AddGate(gateA);

            Gate gateB = new NORGate();
            int indexB = AddGate(gateB);

            Gate gateC = new NANDGate();
            int indexC = AddGate(gateC);

            Gate gateD = new NORGate();
            int indexD = AddGate(gateD);

            Gate gateE = new NANDGate();
            int indexE = AddGate(gateE);

            Gate gateF = new NORGate();
            int indexF = AddGate(gateF);

            Gate gateG = new NANDGate();
            int indexG = AddGate(gateG);

            Gate gateH = new NORGate();
            int indexH = AddGate(gateH);

            Gate gateI = new NotGate();
            int indexI = AddGate(gateI);

            Gate gateJ = new BufferGate();
            int indexJ = AddGate(gateJ);

            Gate gateK = new ANDGate();
            int indexK = AddGate(gateK);

            Gate gateL = new ANDGate(3);
            int indexL = AddGate(gateL);

            Gate gateM = new ANDGate(4);
            int indexM = AddGate(gateM);

            Gate gateN = new ANDGate(5);
            int indexN = AddGate(gateN);

            Gate gateO = new NotGate();
            int indexO = AddGate(gateO);

            Gate gateP = new BufferGate();
            int indexP = AddGate(gateP);

            Gate gateQ = new ANDGate();
            int indexQ = AddGate(gateQ);

            Gate gateR = new ANDGate(3);
            int indexR = AddGate(gateR);

            Gate gateS = new ANDGate(4);
            int indexS = AddGate(gateS);

            Gate gateT = new NotGate();
            int indexT = AddGate(gateT);

            Gate gateU = new BufferGate();
            int indexU = AddGate(gateU);

            Gate gateV = new ANDGate();
            int indexV = AddGate(gateV);

            Gate gateW = new ANDGate(3);
            int indexW = AddGate(gateW);

            Gate gateX = new NotGate();
            int indexX = AddGate(gateX);

            Gate gateY = new BufferGate();
            int indexY = AddGate(gateY);

            Gate gateZ = new NANDGate();
            int indexZ = AddGate(gateZ);

            Gate gateAA = new NotGate();
            int indexAA = AddGate(gateAA);

            Gate gateAB = new NotGate();
            int indexAB = AddGate(gateAB);

            Gate gateAC = new ANDGate();
            int indexAC = AddGate(gateAC);

            Gate gateAD = new ANDGate();
            int indexAD = AddGate(gateAD);

            Gate gateAE = new ANDGate();
            int indexAE = AddGate(gateAE);

            Gate gateAF = new ANDGate();
            int indexAF = AddGate(gateAF);

            Gate gateAG = new NORGate();
            int indexAG = AddGate(gateAG);

            Gate gateAH = new NORGate();
            int indexAH = AddGate(gateAH);

            Gate gateAI = new NORGate();
            int indexAI = AddGate(gateAI);

            Gate gateAJ = new NORGate(5);
            int indexAJ = AddGate(gateAJ);

            Gate gateAK = new XORGate();
            int indexAK = AddGate(gateAK);

            Gate gateAL = new XORGate();
            int indexAL = AddGate(gateAL);

            Gate gateAM = new XORGate();
            int indexAM = AddGate(gateAM);

            Gate gateAN = new XORGate();
            int indexAN = AddGate(gateAN);

            //INPUTS
            //B4
            AddWire(ID, new Wire(0, 0, indexA, false));
            AddWire(ID, new Wire(0, 0, indexB, false));

            //A4
            AddWire(ID, new Wire(1, 1, indexA, false));
            AddWire(ID, new Wire(1, 1, indexB, false));

            //B3
            AddWire(ID, new Wire(2, 0, indexC, false));
            AddWire(ID, new Wire(2, 0, indexD, false));

            //A3
            AddWire(ID, new Wire(3, 1, indexC, false));
            AddWire(ID, new Wire(3, 1, indexD, false));

            //B2
            AddWire(ID, new Wire(4, 0, indexE, false));
            AddWire(ID, new Wire(4, 0, indexF, false));

            //A2
            AddWire(ID, new Wire(5, 1, indexE, false));
            AddWire(ID, new Wire(5, 1, indexF, false));

            //B1
            AddWire(ID, new Wire(6, 0, indexG, false));
            AddWire(ID, new Wire(6, 0, indexH, false));

            //A1
            AddWire(ID, new Wire(7, 1, indexG, false));
            AddWire(ID, new Wire(7, 1, indexH, false));

            //C0
            AddWire(ID, new Wire(8, 0, indexI, false));

            //Gate A
            AddWire(gateA.ID, new Wire(0, 1, indexK, false));
            AddWire(gateA.ID, new Wire(0, 1, indexL, false));
            AddWire(gateA.ID, new Wire(0, 1, indexM, false));
            AddWire(gateA.ID, new Wire(0, 0, indexN, false));
            AddWire(gateA.ID, new Wire(0, 0, indexAC, false));

            //Gate B
            AddWire(gateB.ID, new Wire(0, 0, indexJ, false));
            AddWire(gateB.ID, new Wire(0, 0, indexO, false));

            //Gate C
            AddWire(gateC.ID, new Wire(0, 2, indexL, false));
            AddWire(gateC.ID, new Wire(0, 2, indexM, false));
            AddWire(gateC.ID, new Wire(0, 1, indexN, false));
            AddWire(gateC.ID, new Wire(0, 1, indexQ, false));
            AddWire(gateC.ID, new Wire(0, 1, indexR, false));
            AddWire(gateC.ID, new Wire(0, 0, indexS, false));
            AddWire(gateC.ID, new Wire(0, 0, indexAD, false));

            //Gate D
            AddWire(gateD.ID, new Wire(0, 0, indexK, false));
            AddWire(gateD.ID, new Wire(0, 0, indexP, false));
            AddWire(gateD.ID, new Wire(0, 0, indexT, false));

            //Gate E
            AddWire(gateE.ID, new Wire(0, 3, indexM, false));
            AddWire(gateE.ID, new Wire(0, 2, indexN, false));
            AddWire(gateE.ID, new Wire(0, 2, indexR, false));
            AddWire(gateE.ID, new Wire(0, 1, indexS, false));
            AddWire(gateE.ID, new Wire(0, 1, indexV, false));
            AddWire(gateE.ID, new Wire(0, 0, indexW, false));
            AddWire(gateE.ID, new Wire(0, 0, indexAE, false));

            //Gate F
            AddWire(gateF.ID, new Wire(0, 0, indexL, false));
            AddWire(gateF.ID, new Wire(0, 0, indexQ, false));
            AddWire(gateF.ID, new Wire(0, 0, indexU, false));
            AddWire(gateF.ID, new Wire(0, 0, indexX, false));

            //Gate G
            AddWire(gateG.ID, new Wire(0, 3, indexN, false));
            AddWire(gateG.ID, new Wire(0, 2, indexS, false));
            AddWire(gateG.ID, new Wire(0, 1, indexW, false));
            AddWire(gateG.ID, new Wire(0, 0, indexZ, false));
            AddWire(gateG.ID, new Wire(0, 0, indexAF, false));

            //Gate H
            AddWire(gateH.ID, new Wire(0, 0, indexM, false));
            AddWire(gateH.ID, new Wire(0, 0, indexR, false));
            AddWire(gateH.ID, new Wire(0, 0, indexV, false));
            AddWire(gateH.ID, new Wire(0, 0, indexY, false));
            AddWire(gateH.ID, new Wire(0, 0, indexAA, false));

            //Gate I
            AddWire(gateI.ID, new Wire(0, 4, indexN, false));
            AddWire(gateI.ID, new Wire(0, 3, indexS, false));
            AddWire(gateI.ID, new Wire(0, 2, indexW, false));
            AddWire(gateI.ID, new Wire(0, 1, indexZ, false));
            AddWire(gateI.ID, new Wire(0, 0, indexAB, false));

            //Gate J
            AddWire(gateJ.ID, new Wire(0, 0, indexAJ, false));

            //Gate K
            AddWire(gateK.ID, new Wire(0, 1, indexAJ, false));

            //Gate L
            AddWire(gateL.ID, new Wire(0, 2, indexAJ, false));

            //Gate M
            AddWire(gateM.ID, new Wire(0, 3, indexAJ, false));

            //Gate N
            AddWire(gateN.ID, new Wire(0, 4, indexAJ, false));

            //Gate O
            AddWire(gateO.ID, new Wire(0, 1, indexAC, false));

            //Gate P
            AddWire(gateP.ID, new Wire(0, 0, indexAG, false));

            //Gate Q
            AddWire(gateQ.ID, new Wire(0, 1, indexAG, false));

            //Gate R
            AddWire(gateR.ID, new Wire(0, 2, indexAG, false));

            //Gate S
            AddWire(gateS.ID, new Wire(0, 3, indexAG, false));

            //Gate T
            AddWire(gateT.ID, new Wire(0, 1, indexAD, false));

            //Gate U
            AddWire(gateU.ID, new Wire(0, 0, indexAH, false));

            //Gate V
            AddWire(gateV.ID, new Wire(0, 1, indexAH, false));

            //Gate W
            AddWire(gateW.ID, new Wire(0, 2, indexAH, false));

            //Gate X
            AddWire(gateX.ID, new Wire(0, 1, indexAE, false));

            //Gate Y
            AddWire(gateY.ID, new Wire(0, 0, indexAI, false));

            //Gate Z
            AddWire(gateZ.ID, new Wire(0, 1, indexAI, false));

            //Gate AA
            AddWire(gateAA.ID, new Wire(0, 1, indexAF, false));

            //Gate AB
            AddWire(gateAB.ID, new Wire(0, 1, indexAN, false));

            //Gate AC
            AddWire(gateAC.ID, new Wire(0, 0, indexAK, false));

            //Gate AD
            AddWire(gateAD.ID, new Wire(0, 0, indexAL, false));

            //Gate AE
            AddWire(gateAE.ID, new Wire(0, 0, indexAM, false));

            //Gate AF
            AddWire(gateAF.ID, new Wire(0, 0, indexAN, false));

            //Gate AG
            AddWire(gateAG.ID, new Wire(0, 1, indexAK, false));

            //Gate AH
            AddWire(gateAH.ID, new Wire(0, 1, indexAL, false));

            //Gate AI
            AddWire(gateAI.ID, new Wire(0, 1, indexAM, false));

            //Gate AJ
            AddWire(gateAJ.ID, new Wire(0, 0, -1, true));

            //Gate AK
            AddWire(gateAK.ID, new Wire(0, 1, -1, true));

            //Gate AL
            AddWire(gateAL.ID, new Wire(0, 2, -1, true));

            //Gate AM
            AddWire(gateAM.ID, new Wire(0, 3, -1, true));

            //Gate AN
            AddWire(gateAN.ID, new Wire(0, 4, -1, true));
        }
    }
}