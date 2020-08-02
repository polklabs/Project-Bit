using Gates;
using UnityEngine;

namespace Chips
{
    // https://www.electricaltechnology.org/wp-content/uploads/2018/05/schematic-of-BCD-to-7-Segment-Decoder.png
    public class Bcd_To_Seg : Chip
    {
        public Bcd_To_Seg() : base(4, 7)
        {
            // Column 1
            Gate gateA = new NotGate();
            int indexA = AddGate(gateA);

            Gate gateB = new NotGate();
            int indexB = AddGate(gateB);

            Gate gateC = new NotGate();
            int indexC = AddGate(gateC);

            // Column 2
            Gate gateD = new ANDGate();
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

            Gate gateJ = new ANDGate(3);
            int indexJ = AddGate(gateJ);

            Gate gateK = new ANDGate();
            int indexK = AddGate(gateK);

            Gate gateL = new ANDGate();
            int indexL = AddGate(gateL);

            // Column 3
            Gate gateM = new ORGate(4);
            int indexM = AddGate(gateM);

            Gate gateN = new ORGate(4);
            int indexN = AddGate(gateN);

            Gate gateO = new ORGate(4);
            int indexO = AddGate(gateO);

            Gate gateP = new ORGate(5);
            int indexP = AddGate(gateP);

            Gate gateQ = new ORGate();
            int indexQ = AddGate(gateQ);

            Gate gateR = new ORGate(4);
            int indexR = AddGate(gateR);

            Gate gateS = new ORGate(4);
            int indexS = AddGate(gateS);

            //Input A
            AddWire(ID, new Wire(0, 0, indexM, false));
            AddWire(ID, new Wire(0, 0, indexN, false));
            AddWire(ID, new Wire(0, 0, indexO, false));
            AddWire(ID, new Wire(0, 0, indexP, false));
            AddWire(ID, new Wire(0, 0, indexR, false));
            AddWire(ID, new Wire(0, 0, indexS, false));

            //Input B
            AddWire(ID, new Wire(1, 0, indexA, false));
            AddWire(ID, new Wire(1, 0, indexE, false));
            AddWire(ID, new Wire(1, 0, indexF, false));
            AddWire(ID, new Wire(1, 0, indexJ, false));
            AddWire(ID, new Wire(1, 0, indexL, false));
            AddWire(ID, new Wire(1, 1, indexO, false));

            //Input C
            AddWire(ID, new Wire(2, 0, indexB, false));
            AddWire(ID, new Wire(2, 0, indexG, false));
            AddWire(ID, new Wire(2, 0, indexI, false));            
            AddWire(ID, new Wire(2, 1, indexK, false));
            AddWire(ID, new Wire(2, 3, indexM, false));

            //Input D
            AddWire(ID, new Wire(3, 0, indexC, false));
            AddWire(ID, new Wire(3, 1, indexE, false));
            AddWire(ID, new Wire(3, 1, indexG, false));
            AddWire(ID, new Wire(3, 2, indexJ, false));
            AddWire(ID, new Wire(3, 2, indexO, false));

            //Gate A            
            AddWire(gateA.ID, new Wire(0, 0, indexD, false));
            AddWire(gateA.ID, new Wire(0, 1, indexN, false));
            AddWire(gateA.ID, new Wire(0, 0, indexK, false));

            //Gate B            
            AddWire(gateB.ID, new Wire(0, 0, indexH, false));
            AddWire(gateB.ID, new Wire(0, 1, indexJ, false));
            AddWire(gateB.ID, new Wire(0, 1, indexL, false));

            //Gate C            
            AddWire(gateC.ID, new Wire(0, 1, indexD, false));
            AddWire(gateC.ID, new Wire(0, 1, indexF, false));
            AddWire(gateC.ID, new Wire(0, 1, indexH, false));
            AddWire(gateC.ID, new Wire(0, 1, indexI, false));

            //Gate D
            AddWire(gateD.ID, new Wire(0, 1, indexM, false));
            AddWire(gateD.ID, new Wire(0, 1, indexP, false));
            AddWire(gateD.ID, new Wire(0, 0, indexQ, false));

            //Gate E                      
            AddWire(gateE.ID, new Wire(0, 2, indexM, false));

            //Gate F            
            AddWire(gateF.ID, new Wire(0, 1, indexR, false));

            //Gate G                       
            AddWire(gateG.ID, new Wire(0, 2, indexN, false));

            //Gate H
            AddWire(gateH.ID, new Wire(0, 3, indexN, false));
            AddWire(gateH.ID, new Wire(0, 3, indexO, false));
            AddWire(gateH.ID, new Wire(0, 2, indexR, false));

            //Gate I            
            AddWire(gateI.ID, new Wire(0, 2, indexP, false));
            AddWire(gateI.ID, new Wire(0, 1, indexQ, false));
            AddWire(gateI.ID, new Wire(0, 1, indexS, false));

            //Gate J                        
            AddWire(gateJ.ID, new Wire(0, 3, indexP, false));

            //Gate K            
            AddWire(gateK.ID, new Wire(0, 4, indexP, false));
            AddWire(gateK.ID, new Wire(0, 2, indexS, false));

            //Gate L            
            AddWire(gateL.ID, new Wire(0, 3, indexR, false));
            AddWire(gateL.ID, new Wire(0, 3, indexS, false));

            //Gate M                        
            AddWire(gateM.ID, new Wire(0, 0, -1, true));

            //Gate N            
            AddWire(gateN.ID, new Wire(0, 1, -1, true));

            //Gate O                        
            AddWire(gateO.ID, new Wire(0, 2, -1, true));

            //Gate P            
            AddWire(gateP.ID, new Wire(0, 3, -1, true));

            //Gate Q            
            AddWire(gateQ.ID, new Wire(0, 4, -1, true));

            //Gate R            
            AddWire(gateR.ID, new Wire(0, 5, -1, true));

            //Gate S            
            AddWire(gateS.ID, new Wire(0, 6, -1, true));           
        }
    }
}
