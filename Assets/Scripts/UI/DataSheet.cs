using System;
using System.Collections;
using System.Collections.Generic;

public class DataSheet
{
    public static Dictionary<string, DataSheet> DataSheetList = new Dictionary<string, DataSheet>();

    // Wires ------------------------------------------------------------------------------------
    public static DataSheet VDD = new DataSheet("Power", "Vdd").SetText("Provides a positive voltage to the pin it is connected to.");
    public static DataSheet GND = new DataSheet("Ground", "Gnd").SetText("Provides a negative voltage to the pin it is connected to.");

    // Basic ------------------------------------------------------------------------------------
    public static DataSheet QUAD_AND    = new DataSheet("And Gates, Quad 2-Input (PQ010G)", "PQ010G_AndGate").SetLayers("IC14_PQ010", "IC14_outline", "IC14_VG", "IC14_dip_2").SetText("This chip contains four AND gates and each gate has two inputs.\n\nInputs | Outputs\nA | B   | Y\n0 | 0    | 0\n0 | 1    | 0\n1 | 0    | 0\n1 | 1    | 1");
    public static DataSheet QUAD_NAND   = new DataSheet("Nand Gates, Quad 2-Input (PQ011G)", "PQ011G_NandGate").SetLayers("IC14_PQ011", "IC14_outline", "IC14_VG", "IC14_dip_2").SetText("This chip contains four NAND gates and each gate has two inputs.\n\nInputs | Outputs\nA | B   | Y\n0 | 0    | 1\n0 | 1    | 1\n1 | 0    | 1\n1 | 1    | 0");
    public static DataSheet QUAD_OR     = new DataSheet("Or Gates, Quad 2-Input (PQ020G)", "PQ020G_OrGate").SetLayers("IC14_PQ020", "IC14_outline", "IC14_VG", "IC14_dip_2").SetText("This chip contains four OR gates and each gate has two inputs.\n\nInputs | Outputs\nA | B   | Y\n0 | 0    | 0\n0 | 1    | 1\n1 | 0    | 1\n1 | 1    | 1");
    public static DataSheet QUAD_NOR    = new DataSheet("Nor Gates, Quad 2-Input (PQ021G)", "PQ021G_NorGate").SetLayers("IC14_PQ021", "IC14_outline", "IC14_VG", "IC14_dip_2").SetText("This chip contains four NOR gates and each gate has two inputs.\n\nInputs | Outputs\nA | B   | Y\n0 | 0    | 1\n0 | 1    | 0\n1 | 0    | 0\n1 | 1    | 0");
    public static DataSheet QUAD_XOR    = new DataSheet("Xor Gates, Quad 2-Input (PQ030G)", "PQ030G_XorGate").SetLayers("IC14_PQ030", "IC14_outline", "IC14_VG", "IC14_dip_2").SetText("This chip contains four XOR gates and each gate has two inputs.\n\nInputs | Outputs\nA | B   | Y\n0 | 0    | 0\n0 | 1    | 1\n1 | 0    | 1\n1 | 1    | 0");
    public static DataSheet QUAD_XNOR   = new DataSheet("Xnor Gates, Quad 2-Input (PQ031G)", "PQ031G_XnorGate").SetLayers("IC14_PQ031", "IC14_outline", "IC14_VG", "IC14_dip_2").SetText("This chip contains four XNOR gates and each gate has two inputs.\n\nInputs | Outputs\nA | B   | Y\n0 | 0    | 1\n0 | 1    | 0\n1 | 0    | 0\n1 | 1    | 1");
    public static DataSheet OCTAL_BUS   = new DataSheet("Buffer Gates, Octal 1-Input (PQ040G)", "PQ040G_BufferGate").SetLayers("IC20_PQ040", "IC20_outline", "IC20_VG", "IC20_dip_2").SetText("This chip contains four buffer gates and can output in one of two directions. If DIR is <b>HIGH</b> it will pass the value from A to B and vice versa if DIR is <b>LOW</b>\n\nInputs               | Outputs\nDIR | E | A | B   | A | B\n   0  | 0  | x | 0   | 0  | x\n   0  | 0  | x | 1   | 1  | x\n   1  |  0 | 0 | x   | x  | 0\n   1  |  0 | 1 | x   | x  | 1\n   x  |  1 | x | x   | 0  | 0");
    public static DataSheet HEX_NOT     = new DataSheet("Not Gates, Hex 1-Input (PQ041G)", "PQ041G_NotGate").SetLayers("IC14_PQ041", "IC14_outline", "IC14_VG", "IC14_dip_2").SetText("This chip contains six NOT gates and each gate has one input.\n\nInputs | Outputs\nA         | B\n0         | 1\n1         | 0");

    // Special ----------------------------------------------------------------------------------
    public static DataSheet CLOCK               = new DataSheet("Clock (PQ160)", "PQ160_Clock").SetLayers("IC6_PQ160", "IC6_outline", "IC6_VG", "IC6_dip_1").SetText("This chip provides a constant clock pulse from pin 5. The frequency of the clock is determined by the values of A, B, and C.\n\nA | B | C | Y\n0 | 0 | 0  | 1s\n0 | 0 | 1  | .5s\n0 | 1 | 0  | .25s\n0 | 1 | 1  | .125s\n1 | 0 | 0  | 62.5ms\n1 | 0 | 1  | 31.25ms\n1 | 1 | 0  | 15.6ms\n1 | 1 | 1  | 7.8ms");
    public static DataSheet BCD_DEC_DECODE      = new DataSheet("Bcd to decimal decoder (PQ110)", "PQ110_BcdToDec").SetLayers("IC16_PQ110", "IC16_outline", "IC16_VG", "IC16_dip_2").SetText("A Bcd (binary coded decimal) to decimal decoder converts 4 bits into one it's respective 10 outputs. This chip can covert to any of the 10 digits.\nA, B, C, D maps to the outputs 0-9 with A being the LSB and D being the MSB");
    public static DataSheet BCD_7SEG_DECODE     = new DataSheet("Bcd to 7 segment decoder (PQ111)", "PQ111_BcdToSeg").SetLayers("IC16_PQ111", "IC16_outline", "IC16_VG", "IC16_dip_2").SetText("A Bcd (binary coded decimal) to 7 segment decoder converts 4 bits into it's respective digit on a display. This chip can covert to any of the 10 digits.\nA, B, C, D maps to the outputs a, b, c, d, e, f, g with A being the LSB and D being the MSB");
    public static DataSheet DUAL_JK_FF          = new DataSheet("Dual JK Negative-edge-triggered Flip Flops (PQ120)", "PQ120_JkFf").SetLayers("IC14_PQ120", "IC14_outline", "IC14_VG", "IC14_dip_2");
    public static DataSheet DUAL_D_FF           = new DataSheet("Dual D Flip Flop (PQ121)", "PQ121_DFf").SetLayers("IC14_PQ121", "IC14_outline", "IC14_VG", "IC14_dip_2");
    public static DataSheet OCTAL_D_FF          = new DataSheet("Octal D Flip Flop (PQ122)", "PQ122_DFf").SetLayers("IC20_PQ122", "IC20_outline", "IC20_VG", "IC20_dip_2");
    public static DataSheet ADDER_4_BIT         = new DataSheet("4-Bit Binary Adder (PQ130)", "PQ130_Adder4Bit").SetLayers("IC16_PQ130", "IC16_outline", "IC16_VG", "IC16_dip_2").SetText("This 4-bit full adder is made up of 4 independent full adders. It takes in two 4-bit numbers A(1-4) and B(1-4) along with a carry in (C0). It outputs a 4-bit number Σ(1-4) along with a carry out (C4)");
    public static DataSheet ADDER_4_BIT_GATE    = new DataSheet("PQ130G_Adder4Bit", ADDER_4_BIT);
    public static DataSheet DECODE_3_8          = new DataSheet("3-To-8 Line Decoder (PQ140)", "PQ140_Decode3To8").SetLayers("IC14_PQ140", "IC14_outline", "IC14_VG", "IC14_dip_2");
    public static DataSheet DUAL_DECODE_2_4     = new DataSheet("Dual 2-To-4 Line Decoder (PQ141)", "PQ141_Decode2To4").SetLayers("IC16_PQ141", "IC16_outline", "IC16_VG", "IC16_dip_2");
    public static DataSheet BIN_COUNT_4         = new DataSheet("4-Bit Binary Counter (PQ150G)", "PQ150G").SetLayers("IC8_PQ150", "IC8_outline", "IC8_VG", "IC8_dip_2").SetText("This 4-bit binary counter is made up of 4 rising edge D flip-flops. The output changes when the clock (CK) changes from LOW to HIGH. The reset pin (RS) will return the counter to a zero count on the next rising edge.");

    // Memory -----------------------------------------------------------------------------------
    public static DataSheet RAM_64 = new DataSheet("64-Bit Random Access Memory (PQ230)", "PQ230_Ram64Bit").SetLayers("IC16_PQ230", "IC16_outline", "IC16_VG", "IC16_dip_2").SetText("The PQ230 is a fully decoded 64-bit RAM organized as 16 4-bit words. The memory addressed by applying a binary number to the four address onputs (A1-4). After addressing, information may be either written into or read from the memory. To write, both the memory enable (ME) and the write enable (WE) must be in the logical \"0\" state. Information applied to the four write inputs (D1-4) will be written into the addressed location. To read information from the memory (S1-4) the memory enable input must be in the logical \"0\" state and the write enable must be in the logical \"1\" state. When the memory enable is in the logical \"1\" state the outputs will be logical \"1\".\n\nME | WE | Operation | Output\n0 | 0 | Write | 1\n0 | 1 | Read | Word\n1 | x | Hold | 1\n");

    // Inputs -----------------------------------------------------------------------------------        
    public static DataSheet SWITCH_TOGGLE   = new DataSheet("Switch, Toggle", "Switch_Toggle").SetLayers("Switch").SetText("Connects two pins together based on the user's input.");
    public static DataSheet SWITCH_MOMENT   = new DataSheet("Switch, Momentary", "Switch_Momentary").SetLayers("Switch").SetText("Connects two pins together based on the user's input.");
    public static DataSheet SWITCH_PULSE    = new DataSheet("Switch, Pulse", "Switch_Pulse").SetLayers("Switch").SetText("Connects two pins together based on the user's input.");
    public static DataSheet SWITCH_DIP3     = new DataSheet("Switch, Dip 3", "Switch_Dip_3").SetLayers("IC6_Switch", "IC6_outline").SetText("Connects two pins together based on the user's input.");
    public static DataSheet SWITCH_DIP4     = new DataSheet("Switch, Dip 4", "Switch_Dip_4").SetLayers("IC8_Switch", "IC8_outline").SetText("Connects two pins together based on the user's input.");
    public static DataSheet SWITCH_DIP8     = new DataSheet("Switch, Dip 8", "Switch_Dip_8").SetLayers("IC16_Switch", "IC16_outline").SetText("Connects two pins together based on the user's input.");

    // Outputs ----------------------------------------------------------------------------------
    public static DataSheet LED_GREEN   = new DataSheet("LED, Green", "Led_Green");
    public static DataSheet LED_RED     = new DataSheet("LED, Red", "Led_Red");
    public static DataSheet LED_YELLOW  = new DataSheet("LED, Yellow", "Led_Yellow");
    public static DataSheet LED_BLUE    = new DataSheet("LED, Blue", "Led_Blue");
    public static DataSheet SEG_DISP    = new DataSheet("7-Segment Display", "Seg_Disp");

    public string PartName { get; }
    public string ClassName { get; }
    public string Text { get; set; }
    public string[] DiagramLayers { get; set; }

    public DataSheet(string className, DataSheet dataSheet)
    {
        if (!DataSheetList.ContainsKey(className))
        {
            DataSheetList.Add(className, dataSheet);
        }
        else
        {
            throw new Exception(ClassName + " already exists in DataSheetList");
        }
    }

    public DataSheet(string partName, string className)
    {
        PartName = partName;
        ClassName = className;
        Text = "";
        DiagramLayers = new string[0];
        
        if (!DataSheetList.ContainsKey(ClassName))
        {
            DataSheetList.Add(ClassName, this);
        } else
        {
            throw new Exception(ClassName + " already exists in DataSheetList");
        }
    }

    public DataSheet SetText(string text)
    {
        Text = text;
        return this;
    }

    public DataSheet SetLayers(params string[] layers)
    {
        DiagramLayers = layers;
        return this;
    }

    public static bool HasDataSheet(string className)
    {
        return DataSheetList.ContainsKey(className);
    }

}
