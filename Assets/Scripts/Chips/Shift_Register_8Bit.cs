using Gates;

namespace Chips
{
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
            // Shift Latches
            D_FlipFlop_Re S1 = new D_FlipFlop_Re();
            int indexS1 = AddChip(S1);

            D_FlipFlop_Re S2 = new D_FlipFlop_Re();
            int indexS2 = AddChip(S2);

            D_FlipFlop_Re S3 = new D_FlipFlop_Re();
            int indexS3 = AddChip(S3);

            D_FlipFlop_Re S4 = new D_FlipFlop_Re();
            int indexS4 = AddChip(S4);

            D_FlipFlop_Re S5 = new D_FlipFlop_Re();
            int indexS5 = AddChip(S5);

            D_FlipFlop_Re S6 = new D_FlipFlop_Re();
            int indexS6 = AddChip(S6);

            D_FlipFlop_Re S7 = new D_FlipFlop_Re();
            int indexS7 = AddChip(S7);

            D_FlipFlop_Re S8 = new D_FlipFlop_Re();
            int indexS8 = AddChip(S8);

            // Register Latches
            D_FlipFlop_Re R1 = new D_FlipFlop_Re();
            int indexR1 = AddChip(R1);

            D_FlipFlop_Re R2 = new D_FlipFlop_Re();
            int indexR2 = AddChip(R2);

            D_FlipFlop_Re R3 = new D_FlipFlop_Re();
            int indexR3 = AddChip(R3);

            D_FlipFlop_Re R4 = new D_FlipFlop_Re();
            int indexR4 = AddChip(R4);

            D_FlipFlop_Re R5 = new D_FlipFlop_Re();
            int indexR5 = AddChip(R5);

            D_FlipFlop_Re R6 = new D_FlipFlop_Re();
            int indexR6 = AddChip(R6);

            D_FlipFlop_Re R7 = new D_FlipFlop_Re();
            int indexR7 = AddChip(R7);

            D_FlipFlop_Re R8 = new D_FlipFlop_Re();
            int indexR8 = AddChip(R8);

            // Output Enables
            ANDGate O1 = new ANDGate();
            int indexO1 = AddGate(O1);

            ANDGate O2 = new ANDGate();
            int indexO2 = AddGate(O2);

            ANDGate O3 = new ANDGate();
            int indexO3 = AddGate(O3);

            ANDGate O4 = new ANDGate();
            int indexO4 = AddGate(O4);

            ANDGate O5 = new ANDGate();
            int indexO5 = AddGate(O5);

            ANDGate O6 = new ANDGate();
            int indexO6 = AddGate(O6);

            ANDGate O7 = new ANDGate();
            int indexO7 = AddGate(O7);

            ANDGate O8 = new ANDGate();
            int indexO8 = AddGate(O8);

            // INs
            NotGate OE = new NotGate();
            int indexOE = AddGate(OE);

            BufferGate SRCLR = new BufferGate();
            int indexSRCLR = AddGate(SRCLR);

            BufferGate SRCLK = new BufferGate();
            int indexSRCLK = AddGate(SRCLK);

            BufferGate RCLK = new BufferGate();
            int indexRCLK = AddGate(RCLK);

            // SER
            AddWire(ID, new Wire(4, 1, indexS1, true));

            // SRCLR
            AddWire(ID, new Wire(2, 0, indexSRCLR));
            AddWire(SRCLR, new Wire(0, 2, indexS1, true));
            AddWire(SRCLR, new Wire(0, 2, indexS2, true));
            AddWire(SRCLR, new Wire(0, 2, indexS3, true));
            AddWire(SRCLR, new Wire(0, 2, indexS4, true));
            AddWire(SRCLR, new Wire(0, 2, indexS5, true));
            AddWire(SRCLR, new Wire(0, 2, indexS6, true));
            AddWire(SRCLR, new Wire(0, 2, indexS7, true));
            AddWire(SRCLR, new Wire(0, 2, indexS8, true));

            AddWire(ID, new Wire(5, 3, indexS1, true));
            AddWire(ID, new Wire(5, 3, indexS2, true));
            AddWire(ID, new Wire(5, 3, indexS3, true));
            AddWire(ID, new Wire(5, 3, indexS4, true));
            AddWire(ID, new Wire(5, 3, indexS5, true));
            AddWire(ID, new Wire(5, 3, indexS6, true));
            AddWire(ID, new Wire(5, 3, indexS7, true));
            AddWire(ID, new Wire(5, 3, indexS8, true));
            AddWire(ID, new Wire(5, 3, indexR1, true));
            AddWire(ID, new Wire(5, 3, indexR2, true));
            AddWire(ID, new Wire(5, 3, indexR3, true));
            AddWire(ID, new Wire(5, 3, indexR4, true));
            AddWire(ID, new Wire(5, 3, indexR5, true));
            AddWire(ID, new Wire(5, 3, indexR6, true));
            AddWire(ID, new Wire(5, 3, indexR7, true));
            AddWire(ID, new Wire(5, 3, indexR8, true));

            // SRCLK
            AddWire(ID, new Wire(3, 0, indexSRCLK));
            AddWire(SRCLK, new Wire(0, 0, indexS1, true));
            AddWire(SRCLK, new Wire(0, 0, indexS2, true));
            AddWire(SRCLK, new Wire(0, 0, indexS3, true));
            AddWire(SRCLK, new Wire(0, 0, indexS4, true));
            AddWire(SRCLK, new Wire(0, 0, indexS5, true));
            AddWire(SRCLK, new Wire(0, 0, indexS6, true));
            AddWire(SRCLK, new Wire(0, 0, indexS7, true));
            AddWire(SRCLK, new Wire(0, 0, indexS8, true));

            // RCLK
            AddWire(ID, new Wire(1, 0, indexRCLK));
            AddWire(RCLK, new Wire(0, 0, indexR1, true));
            AddWire(RCLK, new Wire(0, 0, indexR2, true));
            AddWire(RCLK, new Wire(0, 0, indexR3, true));
            AddWire(RCLK, new Wire(0, 0, indexR4, true));
            AddWire(RCLK, new Wire(0, 0, indexR5, true));
            AddWire(RCLK, new Wire(0, 0, indexR6, true));
            AddWire(RCLK, new Wire(0, 0, indexR7, true));
            AddWire(RCLK, new Wire(0, 0, indexR8, true));

            // OE
            AddWire(ID, new Wire(0, 0, indexOE));
            AddWire(OE, new Wire(0, 1, indexO1));
            AddWire(OE, new Wire(0, 1, indexO2));
            AddWire(OE, new Wire(0, 1, indexO3));
            AddWire(OE, new Wire(0, 1, indexO4));
            AddWire(OE, new Wire(0, 1, indexO5));
            AddWire(OE, new Wire(0, 1, indexO6));
            AddWire(OE, new Wire(0, 1, indexO7));
            AddWire(OE, new Wire(0, 1, indexO8));

            // OEs            
            AddWire(R1, new Wire(0, 0, -1, true));
            AddWire(R2, new Wire(0, 1, -1, true));
            AddWire(R3, new Wire(0, 2, -1, true));
            AddWire(R4, new Wire(0, 3, -1, true));
            AddWire(R5, new Wire(0, 4, -1, true));
            AddWire(R6, new Wire(0, 5, -1, true));
            AddWire(R7, new Wire(0, 6, -1, true));
            AddWire(R8, new Wire(0, 7, -1, true));

            //SR->R
            AddWire(S1, new Wire(0, 1, indexR1, true));
            AddWire(S1, new Wire(0, 1, indexS2, true));

            AddWire(S2, new Wire(0, 1, indexR2, true));
            AddWire(S2, new Wire(0, 1, indexS3, true));

            AddWire(S3, new Wire(0, 1, indexR3, true));
            AddWire(S3, new Wire(0, 1, indexS4, true));

            AddWire(S4, new Wire(0, 1, indexR4, true));
            AddWire(S4, new Wire(0, 1, indexS5, true));

            AddWire(S5, new Wire(0, 1, indexR5, true));
            AddWire(S5, new Wire(0, 1, indexS6, true));

            AddWire(S6, new Wire(0, 1, indexR6, true));
            AddWire(S6, new Wire(0, 1, indexS7, true));

            AddWire(S7, new Wire(0, 1, indexR7, true));
            AddWire(S7, new Wire(0, 1, indexS8, true));

            AddWire(S8, new Wire(0, 1, indexR8, true));
            AddWire(S8, new Wire(1, 8, -1, true));

            //R->OE
            AddWire(R1, new Wire(0, 0, indexO1));
            AddWire(R2, new Wire(0, 0, indexO2));
            AddWire(R3, new Wire(0, 0, indexO3));
            AddWire(R4, new Wire(0, 0, indexO4));
            AddWire(R5, new Wire(0, 0, indexO5));
            AddWire(R6, new Wire(0, 0, indexO6));
            AddWire(R7, new Wire(0, 0, indexO7));
            AddWire(R8, new Wire(0, 0, indexO8));
        }
    }
}