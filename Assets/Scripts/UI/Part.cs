﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace userInterface
{

    public enum PartGroups { Breadboard, Wires, Inputs, Outputs, BasicCircuit, Memory, SpecialCircuit };

    public class Part
    {
        public static Dictionary<string, List<Part>> PartList = new Dictionary<string, List<Part>>();

        // Breadboards ------------------------------------------------------------------------------
        public static Part BREAD_BOARD  = new Part("Bread Board", "BreadBoardTemp", PartGroups.Breadboard);
        public static Part POWER_RAIL   = new Part("Power Rail", "PowerRailTemp", PartGroups.Breadboard);

        // Wires ------------------------------------------------------------------------------------
        public static Part VDD          = new Part("Power", "Vdd", PartGroups.Wires);
        public static Part GND          = new Part("Ground", "Gnd", PartGroups.Wires);
        public static Part WIRE_GREEN   = new Part("Wire, Green", "Wire_Green", PartGroups.Wires).SetMultiPoint();
        public static Part WIRE_RED     = new Part("Wire, Red", "Wire_Red", PartGroups.Wires).SetMultiPoint();
        public static Part WIRE_BLUE    = new Part("Wire, Blue", "Wire_Blue", PartGroups.Wires).SetMultiPoint();
        public static Part WIRE_YELLOW  = new Part("Wire, Yellow", "Wire_Yellow", PartGroups.Wires).SetMultiPoint();
        public static Part WIRE_BLACK   = new Part("Wire, Black", "Wire_Black", PartGroups.Wires).SetMultiPoint();
        public static Part WIRE_WHITE   = new Part("Wire, White", "Wire_White", PartGroups.Wires).SetMultiPoint();
        public static Part WIRE_PURPLE  = new Part("Wire, Purple", "Wire_Purple", PartGroups.Wires).SetMultiPoint();
        public static Part WIRE_ORANGE  = new Part("Wire, Orange", "Wire_Orange", PartGroups.Wires).SetMultiPoint();

        // Basic ------------------------------------------------------------------------------------
        public static Part QUAD_AND     = new Part("And Gates, Quad 2-Input (PQ010G)", "PQ010G_AndGate", PartGroups.BasicCircuit); // 74LS08
        public static Part QUAD_NAND    = new Part("Nand Gates, Quad 2-Input (PQ011G)", "PQ011G_NandGate", PartGroups.BasicCircuit); // 74LS00
        public static Part QUAD_OR      = new Part("Or Gates, Quad 2-Input (PQ020G)", "PQ020G_OrGate", PartGroups.BasicCircuit); // 74LS32
        public static Part QUAD_NOR     = new Part("Nor Gates, Quad 2-Input (PQ021G)", "PQ021G_NorGate", PartGroups.BasicCircuit); // 74LS02
        public static Part QUAD_XOR     = new Part("Xor Gates, Quad 2-Input (PQ030G)", "PQ030G_XorGate", PartGroups.BasicCircuit); // 74LS86
        public static Part QUAD_XNOR    = new Part("Xnor Gates, Quad 2-Input (PQ031G)", "PQ031G_XnorGate", PartGroups.BasicCircuit); // 74LS??
        public static Part OCTAL_BUS    = new Part("Buffer Gates, Octal 1-Input (PQ040G)", "PQ040G_BufferGate", PartGroups.BasicCircuit); // 74LS245
        public static Part HEX_NOT      = new Part("Not Gates, Hex 1-Input (PQ041G)", "PQ041G_NotGate", PartGroups.BasicCircuit); // 74LS04

        // Special ----------------------------------------------------------------------------------
        public static Part CLOCK            = new Part("Clock (PQ160)", "PQ160_Clock", PartGroups.SpecialCircuit);
        public static Part BCD_DEC_DECODE   = new Part("Bcd to decimal decoder (PQ110)", "PQ110_BcdToDec", PartGroups.SpecialCircuit);
        public static Part BCD_7SEG_DECODE  = new Part("Bcd to 7 segment decoder (PQ111)", "PQ111_BcdToSeg", PartGroups.SpecialCircuit);
        public static Part DUAL_JK_FF       = new Part("Dual JK Flip Flop (PQ120)", "PQ120_JkFf", PartGroups.SpecialCircuit); // 74LS107
        public static Part DUAL_D_FF        = new Part("Dual D Flip Flop (PQ121)", "PQ121_DFf", PartGroups.SpecialCircuit);
        public static Part OCTAL_D_FF       = new Part("Octal D Flip Flop (PQ122)", "PQ122_DFf", PartGroups.SpecialCircuit); // 74LS273
        public static Part ADDER_4_BIT      = new Part("4-Bit Binary Adder (PQ130)", "PQ130_Adder4Bit", PartGroups.SpecialCircuit); // 74LS283
        public static Part ADDER_4_BIT_GATE = new Part("4-Bit Binary Adder (PQ130G)", "PQ130G_Adder4Bit", PartGroups.SpecialCircuit); // 74LS283
        public static Part DECODE_3_8       = new Part("3-To-8 Line Decoder (PQ140)", "PQ140_Decode3To8", PartGroups.SpecialCircuit); // 74LS138
        public static Part DUAL_DECODE_2_4  = new Part("Dual 2-To-4 Line Decoder (PQ141)", "PQ141_Decode2To4", PartGroups.SpecialCircuit); // 74LS139
        public static Part QUAD_SEL_2_1     = new Part("Quad 2-To-1 Data Selector (PQ142)", "PQ142_Sel2To1", PartGroups.SpecialCircuit); // 74LS157
        public static Part BIN_COUNT_4      = new Part("4-Bit Binary Counter (PQ150G)", "PQ150G", PartGroups.SpecialCircuit); // 74LS161

        // Memory -----------------------------------------------------------------------------------
        public static Part SHIFT_REG_8  = new Part("8-Bit Shift Register (PQ210)", "PQ210_SftRegister8Bit", PartGroups.Memory); // 74HC595
        //public static Part REGISTER_4   = new Part("4-Bit D-type Register (PQ211)",       "PQ211_Register4Bit",       PartGroups.Memory); // 74LS173
        //public static Part EEPROM_16K   = new Part("EEPROM 16K (2K x 8) (PQ220)",         "PQ220_EEPROM16K",          PartGroups.Memory); // 28C16
        public static Part RAM_64       = new Part("64-Bit Random Access Memory (PQ230)", "PQ230_Ram64Bit", PartGroups.Memory); // 74189

        // Inputs -----------------------------------------------------------------------------------        
        public static Part SWITCH_TOGGLE    = new Part("Switch, Toggle", "Switch_Toggle", PartGroups.Inputs);
        public static Part SWITCH_MOMENT    = new Part("Switch, Momentary", "Switch_Momentary", PartGroups.Inputs);
        public static Part SWITCH_PULSE     = new Part("Switch, Pulse", "Switch_Pulse", PartGroups.Inputs);
        public static Part SWITCH_DIP3      = new Part("Switch, Dip 3", "Switch_Dip_3", PartGroups.Inputs);
        public static Part SWITCH_DIP4      = new Part("Switch, Dip 4", "Switch_Dip_4", PartGroups.Inputs);
        public static Part SWITCH_DIP8      = new Part("Switch, Dip 8", "Switch_Dip_8", PartGroups.Inputs);

        // Outputs ----------------------------------------------------------------------------------
        public static Part LED_GREEN    = new Part("LED, Green", "Led_Green", PartGroups.Outputs);
        public static Part LED_RED      = new Part("LED, Red", "Led_Red", PartGroups.Outputs);
        public static Part LED_YELLOW   = new Part("LED, Yellow", "Led_Yellow", PartGroups.Outputs);
        public static Part LED_BLUE     = new Part("LED, Blue", "Led_Blue", PartGroups.Outputs);
        public static Part SEG_DISP     = new Part("7-Segment Display", "Seg_Disp", PartGroups.Outputs);

        public string PartName { get; }
        public string ClassName { get; }
        public PartGroups PartGroup { get; }
        public bool IsMultiPoint { get; set; }

        public Part(string partName, string className, PartGroups partGroup)
        {
            PartName = partName;
            ClassName = className;
            PartGroup = partGroup;
            IsMultiPoint = false;

            string partGroupString = PartGroupToString(partGroup);
            if (!PartList.ContainsKey(partGroupString))
            {
                PartList.Add(PartGroupToString(partGroup), new List<Part>());
            }
            PartList[partGroupString].Add(this);
        }

        public Part SetMultiPoint()
        {
            IsMultiPoint = true;
            return this;
        }

        public static int GroupCount()
        {
            return Enum.GetNames(typeof(PartGroups)).Length;
        }

        public static string IntToGroupString(int i)
        {
            PartGroups partGroups = (PartGroups)i;
            return PartGroupToString(partGroups);
        }

        public static string PartGroupToString(PartGroups partGroup)
        {
            switch (partGroup)
            {
                case PartGroups.Breadboard:
                    return "Bread Boards";
                case PartGroups.BasicCircuit:
                    return "Basic Chips";
                case PartGroups.SpecialCircuit:
                    return "Special Chips";
                case PartGroups.Memory:
                    return "Memory Chips";
                case PartGroups.Inputs:
                    return "Inputs";
                case PartGroups.Outputs:
                    return "Outputs";
                case PartGroups.Wires:
                    return "Wires";
                default:
                    return "Unknown group";
            }
        }
    }
}