using System;
using System.Collections;
using System.Collections.Generic;

public class DataSheet
{
    public static Dictionary<string, DataSheet> DataSheetList = new Dictionary<string, DataSheet>();

    // Wires ------------------------------------------------------------------------------------
    public static DataSheet VDD = new DataSheet("Power", "Vdd").SetText("Provides a positive voltage to the pin it is connected to.");
    public static DataSheet GND = new DataSheet("Ground", "Gnd").SetText("Provides a negative voltage to the pin it is connected to.");
    public static DataSheet WIRE_GREEN = new DataSheet("Wire, Green", "Wire_Green");
    public static DataSheet WIRE_RED = new DataSheet("Wire, Red", "Wire_Red");
    public static DataSheet WIRE_BLUE = new DataSheet("Wire, Blue", "Wire_Blue");
    public static DataSheet WIRE_YELLOW = new DataSheet("Wire, Yellow", "Wire_Yellow");
    public static DataSheet WIRE_BLACK = new DataSheet("Wire, Black", "Wire_Black");
    public static DataSheet WIRE_WHITE = new DataSheet("Wire, White", "Wire_White");
    public static DataSheet WIRE_PURPLE = new DataSheet("Wire, Purple", "Wire_Purple");
    public static DataSheet WIRE_ORANGE = new DataSheet("Wire, Orange", "Wire_Orange");

    // Basic ------------------------------------------------------------------------------------
    public static DataSheet QUAD_AND = new DataSheet("And Gates, Quad 2-Input (PQ010G)", "PQ010G_AndGate").SetLayers("IC14_PQ010", "IC14_outline", "IC14_VG", "IC14_dip_2");
    public static DataSheet QUAD_NAND = new DataSheet("Nand Gates, Quad 2-Input (PQ011G)", "PQ011G_NandGate").SetLayers("IC14_PQ011", "IC14_outline", "IC14_VG", "IC14_dip_2");
    public static DataSheet QUAD_OR = new DataSheet("Or Gates, Quad 2-Input (PQ020G)", "PQ020G_OrGate").SetLayers("IC14_PQ020", "IC14_outline", "IC14_VG", "IC14_dip_2");
    public static DataSheet QUAD_NOR = new DataSheet("Nor Gates, Quad 2-Input (PQ021G)", "PQ021G_NorGate").SetLayers("IC14_PQ021", "IC14_outline", "IC14_VG", "IC14_dip_2");
    public static DataSheet QUAD_XOR = new DataSheet("Xor Gates, Quad 2-Input (PQ030G)", "PQ030G_XorGate").SetLayers("IC14_PQ030", "IC14_outline", "IC14_VG", "IC14_dip_2");
    public static DataSheet QUAD_XNOR = new DataSheet("Xnor Gates, Quad 2-Input (PQ031G)", "PQ031G_XnorGate").SetLayers("IC14_PQ031", "IC14_outline", "IC14_VG", "IC14_dip_2");
    public static DataSheet OCTAL_BUS = new DataSheet("Buffer Gates, Octal 1-Input (PQ040G)", "PQ040G_BufferGate"); // 74LS245
    public static DataSheet HEX_NOT = new DataSheet("Not Gates, Hex 1-Input (PQ041G)", "PQ041G_NotGate"); // 74LS04

    // Special ----------------------------------------------------------------------------------
    public static DataSheet CLOCK = new DataSheet("Clock (PQ160)", "PQ160_Clock");    
    public static DataSheet BCD_7SEG_DECODE = new DataSheet("Bcd to 7 segment decoder (PQ111)", "PQ111_BcdToSeg");
    public static DataSheet ADDER_4_BIT = new DataSheet("4-Bit Binary Adder (PQ130)", "PQ130_Adder4Bit"); // 74LS283
    public static DataSheet ADDER_4_BIT_GATE = new DataSheet("4-Bit Binary Adder (PQ130G)", "PQ130G_Adder4Bit"); // 74LS283
    public static DataSheet BIN_COUNT_4 = new DataSheet("4-Bit Binary Counter (PQ150G)", "PQ150G"); // 74LS161

    // Memory -----------------------------------------------------------------------------------

    // Inputs -----------------------------------------------------------------------------------        
    public static DataSheet SWITCH_TOGGLE = new DataSheet("Switch, Toggle", "Switch_Toggle");
    public static DataSheet SWITCH_MOMENT = new DataSheet("Switch, Momentary", "Switch_Momentary");
    public static DataSheet SWITCH_PULSE = new DataSheet("Switch, Pulse", "Switch_Pulse");
    public static DataSheet SWITCH_DIP3 = new DataSheet("Switch, Dip 3", "Switch_Dip_3");
    public static DataSheet SWITCH_DIP4 = new DataSheet("Switch, Dip 4", "Switch_Dip_4");
    public static DataSheet SWITCH_DIP8 = new DataSheet("Switch, Dip 8", "Switch_Dip_8");

    // Outputs ----------------------------------------------------------------------------------
    public static DataSheet LED_GREEN = new DataSheet("LED, Green", "Led_Green");
    public static DataSheet LED_RED = new DataSheet("LED, Red", "Led_Red");
    public static DataSheet LED_YELLOW = new DataSheet("LED, Yellow", "Led_Yellow");
    public static DataSheet LED_BLUE = new DataSheet("LED, Blue", "Led_Blue");
    public static DataSheet SEG_DISP = new DataSheet("7-Segment Display", "Seg_Disp");

    public string PartName { get; }
    public string ClassName { get; }
    public string Text { get; set; }
    public string[] DiagramLayers { get; set; }

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
        if (text.Length > 230)
        {
            throw new Exception("Max text length is 230 characters (" + text.Length + ")");
        }

        Text = text;
        return this;
    }

    public DataSheet SetLayers(params string[] layers)
    {
        DiagramLayers = layers;
        return this;
    }

}
