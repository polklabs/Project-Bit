using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace userInterface
{

    public enum PartGroup { None, Breadboard, Wires, Inputs, Outputs, BasicCircuit, Memory, SpecialCircuit };

    public class Part
    {
        public static Dictionary<PartGroup, List<Part>> PartList = new Dictionary<PartGroup, List<Part>>();

        // Breadboards ------------------------------------------------------------------------------
        public static Part BREAD_BOARD  = new Part("Bread Board", "BreadBoardTemp", PartGroup.Breadboard);
        public static Part POWER_RAIL   = new Part("Power Rail", "PowerRailTemp", PartGroup.Breadboard);

        // Wires ------------------------------------------------------------------------------------
        public static Part VDD          = new Part("Power", "Vdd", PartGroup.Wires);
        public static Part GND          = new Part("Ground", "Gnd", PartGroup.Wires);
        public static Part WIRE_GREEN   = new Part("Wire, Green", "Wire_Green", PartGroup.Wires).SetMultiPoint();
        public static Part WIRE_RED     = new Part("Wire, Red", "Wire_Red", PartGroup.Wires).SetMultiPoint();
        public static Part WIRE_BLUE    = new Part("Wire, Blue", "Wire_Blue", PartGroup.Wires).SetMultiPoint();
        public static Part WIRE_YELLOW  = new Part("Wire, Yellow", "Wire_Yellow", PartGroup.Wires).SetMultiPoint();
        public static Part WIRE_BLACK   = new Part("Wire, Black", "Wire_Black", PartGroup.Wires).SetMultiPoint();
        public static Part WIRE_WHITE   = new Part("Wire, White", "Wire_White", PartGroup.Wires).SetMultiPoint();
        public static Part WIRE_PURPLE  = new Part("Wire, Purple", "Wire_Purple", PartGroup.Wires).SetMultiPoint();
        public static Part WIRE_ORANGE  = new Part("Wire, Orange", "Wire_Orange", PartGroup.Wires).SetMultiPoint();

        // Basic ------------------------------------------------------------------------------------
        public static Part QUAD_AND     = new Part("And Gates, Quad 2-Input (PQ010G)", "PQ010G_AndGate", PartGroup.BasicCircuit); // 74LS08
        public static Part QUAD_NAND    = new Part("Nand Gates, Quad 2-Input (PQ011G)", "PQ011G_NandGate", PartGroup.BasicCircuit); // 74LS00
        public static Part QUAD_OR      = new Part("Or Gates, Quad 2-Input (PQ020G)", "PQ020G_OrGate", PartGroup.BasicCircuit); // 74LS32
        public static Part QUAD_NOR     = new Part("Nor Gates, Quad 2-Input (PQ021G)", "PQ021G_NorGate", PartGroup.BasicCircuit); // 74LS02
        public static Part QUAD_XOR     = new Part("Xor Gates, Quad 2-Input (PQ030G)", "PQ030G_XorGate", PartGroup.BasicCircuit); // 74LS86
        public static Part QUAD_XNOR    = new Part("Xnor Gates, Quad 2-Input (PQ031G)", "PQ031G_XnorGate", PartGroup.BasicCircuit); // 74LS??
        public static Part OCTAL_BUS    = new Part("Buffer Gates, Octal 1-Input (PQ040G)", "PQ040G_BufferGate", PartGroup.BasicCircuit); // 74LS245
        public static Part HEX_NOT      = new Part("Not Gates, Hex 1-Input (PQ041G)", "PQ041G_NotGate", PartGroup.BasicCircuit); // 74LS04

        // Special ----------------------------------------------------------------------------------
        public static Part CLOCK            = new Part("Clock (PQ160)", "PQ160_Clock", PartGroup.SpecialCircuit);
        public static Part BCD_DEC_DECODE   = new Part("Bcd to decimal decoder (PQ110)", "PQ110_BcdToDec", PartGroup.SpecialCircuit);
        public static Part BCD_7SEG_DECODE  = new Part("Bcd to 7 segment decoder (PQ111)", "PQ111_BcdToSeg", PartGroup.SpecialCircuit);
        // public static Part ADDER_4_BIT   = new Part("4-Bit Binary Adder (PQ130)", "PQ130_Adder4Bit", PartGroup.SpecialCircuit); // 74LS283
        public static Part ADDER_4_BIT_GATE = new Part("4-Bit Binary Adder (PQ130G)", "PQ130G_Adder4Bit", PartGroup.SpecialCircuit); // 74LS283
        public static Part DECODE_3_8       = new Part("3-To-8 Line Decoder (PQ140)", "PQ140_Decode3To8", PartGroup.SpecialCircuit); // 74LS138
        public static Part DUAL_DECODE_2_4  = new Part("Dual 2-To-4 Line Decoder (PQ141)", "PQ141_Decode2To4", PartGroup.SpecialCircuit); // 74LS139
        public static Part QUAD_SEL_2_1     = new Part("Quad 2-To-1 Data Selector (PQ142)", "PQ142_Sel2To1", PartGroup.SpecialCircuit); // 74LS157
        public static Part BIN_COUNT_4      = new Part("4-Bit Binary Counter (PQ150G)", "PQ150G", PartGroup.SpecialCircuit); // 74LS161

        // Memory -----------------------------------------------------------------------------------
        public static Part DUAL_JK_FF   = new Part("Dual JK Flip Flop (PQ120)", "PQ120_JkFf", PartGroup.Memory); // 74LS107
        public static Part DUAL_D_FF    = new Part("Dual D Flip Flop (PQ121)", "PQ121_DFf", PartGroup.Memory);
        public static Part OCTAL_D_FF   = new Part("Octal D Flip Flop (PQ122)", "PQ122_DFf", PartGroup.Memory); // 74LS273
        public static Part SHIFT_REG_8  = new Part("8-Bit Shift Register (PQ210)", "PQ210G_SftRegister8Bit", PartGroup.Memory); // 74HC595
        public static Part REGISTER_4   = new Part("4-Bit D-type Register (PQ211)", "PQ211_Register4Bit", PartGroup.Memory); // 74LS173
        public static Part EEPROM_16K = new Part("EEPROM 16K (2K x 8) (PQ220)", "PQ220_EEPROM16K", PartGroup.Memory); // 28C16
        public static Part RAM_64       = new Part("64-Bit Random Access Memory (PQ230)", "PQ230_Ram64Bit", PartGroup.Memory); // 74189

        // Inputs -----------------------------------------------------------------------------------        
        public static Part SWITCH_TOGGLE    = new Part("Switch, Toggle", "Switch_Toggle", PartGroup.Inputs);
        public static Part SWITCH_MOMENT    = new Part("Switch, Momentary", "Switch_Momentary", PartGroup.Inputs);
        public static Part SWITCH_MOMENT_I  = new Part("Switch, Momentary, Inverted", "Switch_Momentary_Inverted", PartGroup.Inputs);
        public static Part SWITCH_PULSE     = new Part("Switch, Pulse", "Switch_Pulse", PartGroup.Inputs);
        public static Part SWITCH_DIP3      = new Part("Switch, Dip 3", "Switch_Dip_3", PartGroup.Inputs);
        public static Part SWITCH_DIP4      = new Part("Switch, Dip 4", "Switch_Dip_4", PartGroup.Inputs);
        public static Part SWITCH_DIP8      = new Part("Switch, Dip 8", "Switch_Dip_8", PartGroup.Inputs);

        // Outputs ----------------------------------------------------------------------------------
        public static Part LED_GREEN    = new Part("LED, Green", "Led_Green", PartGroup.Outputs);
        public static Part LED_RED      = new Part("LED, Red", "Led_Red", PartGroup.Outputs);
        public static Part LED_YELLOW   = new Part("LED, Yellow", "Led_Yellow", PartGroup.Outputs);
        public static Part LED_BLUE     = new Part("LED, Blue", "Led_Blue", PartGroup.Outputs);
        public static Part SEG_DISP     = new Part("7-Segment Display", "Seg_Disp", PartGroup.Outputs);

        public string PartName { get; }
        public string ClassName { get; }
        public PartGroup PartGroup { get; }
        public bool IsMultiPoint { get; set; }

        public Part(string partName, string className, PartGroup partGroup)
        {
            PartName = partName;
            ClassName = className;
            PartGroup = partGroup;
            IsMultiPoint = false;

            if (!PartList.ContainsKey(partGroup))
            {
                PartList.Add(partGroup, new List<Part>());
            }
            PartList[partGroup].Add(this);
        }

        public Part SetMultiPoint()
        {
            IsMultiPoint = true;
            return this;
        }

        public static int GroupCount()
        {
            return Enum.GetNames(typeof(PartGroup)).Length;
        }

        public static string IntToGroupString(int i)
        {
            PartGroup partGroups = (PartGroup)i;
            return PartGroupToString(partGroups);
        }

        public static string PartGroupToString(PartGroup partGroup)
        {
            switch (partGroup)
            {
                case PartGroup.Breadboard:
                    return "Bread Boards";
                case PartGroup.BasicCircuit:
                    return "Basic Chips";
                case PartGroup.SpecialCircuit:
                    return "Special Chips";
                case PartGroup.Memory:
                    return "Memory Chips";
                case PartGroup.Inputs:
                    return "Inputs";
                case PartGroup.Outputs:
                    return "Outputs";
                case PartGroup.Wires:
                    return "Wires";
                default:
                    return "Unknown group";
            }
        }
    }
}