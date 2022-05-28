using Gates;
using Wires;

namespace Chips
{

    // TODO: FIX: SRCLR not updating properly
    public class Shift_Register_8Bit : Chip
    {
        /// <summary>
        /// OE = 0,
        /// RCLK = 1,
        /// SRCLR = 2,
        /// SRCLK = 3,
        /// SER = 4
        /// </summary>
        public Shift_Register_8Bit() : base(6, 9)
        {
            KeepDirty = true;

            // Shift Latches
            Chip S1 = AddChipIndexed(new D_FlipFlop_Re());
            Chip S2 = AddChipIndexed(new D_FlipFlop_Re());
            Chip S3 = AddChipIndexed(new D_FlipFlop_Re());
            Chip S4 = AddChipIndexed(new D_FlipFlop_Re());
            Chip S5 = AddChipIndexed(new D_FlipFlop_Re());
            Chip S6 = AddChipIndexed(new D_FlipFlop_Re());
            Chip S7 = AddChipIndexed(new D_FlipFlop_Re());
            Chip S8 = AddChipIndexed(new D_FlipFlop_Re());

            // Register Latches
            Chip R1 = AddChipIndexed(new D_FlipFlop_Re());
            Chip R2 = AddChipIndexed(new D_FlipFlop_Re());
            Chip R3 = AddChipIndexed(new D_FlipFlop_Re());
            Chip R4 = AddChipIndexed(new D_FlipFlop_Re());
            Chip R5 = AddChipIndexed(new D_FlipFlop_Re());
            Chip R6 = AddChipIndexed(new D_FlipFlop_Re());
            Chip R7 = AddChipIndexed(new D_FlipFlop_Re());
            Chip R8 = AddChipIndexed(new D_FlipFlop_Re());

            // Output Enables
            Gate O1 = AddGateIndexed(new ANDGate());
            Gate O2 = AddGateIndexed(new ANDGate());
            Gate O3 = AddGateIndexed(new ANDGate());
            Gate O4 = AddGateIndexed(new ANDGate());
            Gate O5 = AddGateIndexed(new ANDGate());
            Gate O6 = AddGateIndexed(new ANDGate());
            Gate O7 = AddGateIndexed(new ANDGate());
            Gate O8 = AddGateIndexed(new ANDGate());

            // INs
            Gate OE = AddGateIndexed(new NotGate());
            Gate RCLK = AddGateIndexed(new BufferGate());

            // SER
            AddInputWire(new Wire(4, 1, S1));

            // SRCLR            
            AddInputWires(Wire.Create(2, 2, true, S1, S2, S3, S4, S5, S6, S7, S8));

            AddInputWires(Wire.Create(5, 3, false, S1, S2, S3, S4, S5, S6, S7, S8, R1, R2, R3, R4, R5, R6, R7, R8));

            // SRCLK
            AddInputWires(Wire.Create(3, 0, false, S1, S2, S3, S4, S5, S6, S7, S8));

            // RCLK
            AddInputWire(new Wire(1, 0, RCLK));
            AddWires(RCLK, Wire.Create(0, 0, false, R1, R2, R3, R4, R5, R6, R7, R8));

            // OE
            AddInputWire(new Wire(0, 0, OE));
            AddWires(OE, Wire.Create(0, 1, false, O1, O2, O3, O4, O5, O6, O7, O8));

            // OEs            
            AddWire(O1, new Wire(0, 0, -1, true));
            AddWire(O2, new Wire(0, 1, -1, true));
            AddWire(O3, new Wire(0, 2, -1, true));
            AddWire(O4, new Wire(0, 3, -1, true));
            AddWire(O5, new Wire(0, 4, -1, true));
            AddWire(O6, new Wire(0, 5, -1, true));
            AddWire(O7, new Wire(0, 6, -1, true));
            AddWire(O8, new Wire(0, 7, -1, true));
            //AddWire(S1, new Wire(0, 0, -1, true));
            //AddWire(S2, new Wire(0, 1, -1, true));
            //AddWire(S3, new Wire(0, 2, -1, true));
            //AddWire(S4, new Wire(0, 3, -1, true));
            //AddWire(S5, new Wire(0, 4, -1, true));
            //AddWire(S6, new Wire(0, 5, -1, true));
            //AddWire(S7, new Wire(0, 6, -1, true));
            //AddWire(S8, new Wire(0, 7, -1, true));

            //SR->R
            AddWires(S1, Wire.Create(0, 1, false, R1, S2));
            AddWires(S2, Wire.Create(0, 1, false, R2, S3));
            AddWires(S3, Wire.Create(0, 1, false, R3, S4));
            AddWires(S4, Wire.Create(0, 1, false, R4, S5));
            AddWires(S5, Wire.Create(0, 1, false, R5, S6));
            AddWires(S6, Wire.Create(0, 1, false, R6, S7));
            AddWires(S7, Wire.Create(0, 1, false, R7, S8));

            AddWire(S8, new Wire(0, 1, R8));
            AddWire(S8, new Wire(1, 8, -1, true));

            //R->OE
            AddWire(R1, new Wire(0, 0, O1));
            AddWire(R2, new Wire(0, 0, O2));
            AddWire(R3, new Wire(0, 0, O3));
            AddWire(R4, new Wire(0, 0, O4));
            AddWire(R5, new Wire(0, 0, O5));
            AddWire(R6, new Wire(0, 0, O6));
            AddWire(R7, new Wire(0, 0, O7));
            AddWire(R8, new Wire(0, 0, O8));
        }
    }
}