using Gates;
using Wires;

/**
 * Octal Bus Gates based on the 74LS245
 * Used in the PQ040G_BufferGate
 */

namespace Chips
{
    public class Octal_Bus_Gates : Chip
    {
        /// <summary>
        /// DIR = 0,
        /// E' = 1,
        /// A1-9 = 2-9
        /// B1-9 = 10-17
        /// </summary>
        public Octal_Bus_Gates() : base(18, 16)
        {
            // Control Gates
            Gate gateA = AddGateIndexed(new ANDGate());
            Gate gateB = AddGateIndexed(new ANDGate());

            // Buffers
            Gate gateE = AddGateIndexed(new TriBufferGate());
            Gate gateF = AddGateIndexed(new TriBufferGate());
            Gate gateG = AddGateIndexed(new TriBufferGate());
            Gate gateH = AddGateIndexed(new TriBufferGate());
            Gate gateI = AddGateIndexed(new TriBufferGate());
            Gate gateJ = AddGateIndexed(new TriBufferGate());
            Gate gateK = AddGateIndexed(new TriBufferGate());
            Gate gateL = AddGateIndexed(new TriBufferGate());
            Gate gateM = AddGateIndexed(new TriBufferGate());
            Gate gateN = AddGateIndexed(new TriBufferGate());
            Gate gateO = AddGateIndexed(new TriBufferGate());
            Gate gateP = AddGateIndexed(new TriBufferGate());
            Gate gateQ = AddGateIndexed(new TriBufferGate());
            Gate gateR = AddGateIndexed(new TriBufferGate());
            Gate gateS = AddGateIndexed(new TriBufferGate());
            Gate gateT = AddGateIndexed(new TriBufferGate());

            //Inputs
            AddInputWire(new Wire(0, 0, gateA));
            AddInputWire(new Wire(0, 0, gateB, true));
            AddInputWire(new Wire(1, 1, gateA, true));
            AddInputWire(new Wire(1, 1, gateB, true));
            AddInputWire(new Wire(2, 0, gateE));
            AddInputWire(new Wire(3, 0, gateF));
            AddInputWire(new Wire(4, 0, gateG));
            AddInputWire(new Wire(5, 0, gateH));
            AddInputWire(new Wire(6, 0, gateI));
            AddInputWire(new Wire(7, 0, gateJ));
            AddInputWire(new Wire(8, 0, gateK));
            AddInputWire(new Wire(9, 0, gateL));
            AddInputWire(new Wire(10, 0, gateM));
            AddInputWire(new Wire(11, 0, gateN));
            AddInputWire(new Wire(12, 0, gateO));
            AddInputWire(new Wire(13, 0, gateP));
            AddInputWire(new Wire(14, 0, gateQ));
            AddInputWire(new Wire(15, 0, gateR));
            AddInputWire(new Wire(16, 0, gateS));
            AddInputWire(new Wire(17, 0, gateT));

            //Gate A
            AddWires(gateA, Wire.Create(0, 1, false, gateE, gateF, gateG, gateH, gateI, gateJ, gateK, gateL));

            //Gate B
            AddWires(gateB, Wire.Create(0, 1, false, gateM, gateN, gateO, gateP, gateQ, gateR, gateS, gateT));

            //Outputs
            AddWire(gateE, new Wire(0, 8, -1, true));
            AddWire(gateF, new Wire(0, 9, -1, true));
            AddWire(gateG, new Wire(0, 10, -1, true));
            AddWire(gateH, new Wire(0, 11, -1, true));
            AddWire(gateI, new Wire(0, 12, -1, true));
            AddWire(gateJ, new Wire(0, 13, -1, true));
            AddWire(gateK, new Wire(0, 14, -1, true));
            AddWire(gateL, new Wire(0, 15, -1, true));
            AddWire(gateM, new Wire(0, 0, -1, true));
            AddWire(gateN, new Wire(0, 1, -1, true));
            AddWire(gateO, new Wire(0, 2, -1, true));
            AddWire(gateP, new Wire(0, 3, -1, true));
            AddWire(gateQ, new Wire(0, 4, -1, true));
            AddWire(gateR, new Wire(0, 5, -1, true));
            AddWire(gateS, new Wire(0, 6, -1, true));
            AddWire(gateT, new Wire(0, 7, -1, true));

        }
    }
}