using Chips;
using Gates;
using System.Runtime.Serialization;

namespace Wires
{
    [DataContract]
    public class Wire
    {
        [DataMember]
        public bool IsChip { get; set; }
        [DataMember]
        public int CircuitIndex { get; set; }
        [DataMember]
        public int FromIndex { get; set; }
        [DataMember]
        public int ToIndex { get; set; }
        [DataMember]
        public bool InvertValue { get; set; }

        /// <summary>
        /// Creates a "wire" connecting two inputs/outputs and or gates/chips
        /// </summary>
        /// <param name="fromIndex">Index of output to grab value from (Input for .this)</param>
        /// <param name="toIndex">Index of input to send value to (Output for .this)</param>
        /// <param name="circuitIndex">Index of circuit to connect to (-1 for .this)</param>
        /// <param name="isChip">If the circuit is of type Chip</param>
        public Wire(int fromIndex, int toIndex, int circuitIndex, bool isChip = false, bool invert = false)
        {
            IsChip = isChip;
            CircuitIndex = circuitIndex;
            FromIndex = fromIndex;
            ToIndex = toIndex;
            InvertValue = invert;
        }

        /// <summary>
        /// Creates a "wire" connecting two inputs/outputs and or gates/chips
        /// </summary>
        /// <param name="fromIndex">Index of output to grab value from (Input for .this)</param>
        /// <param name="toIndex">Index of input to send value to (Output for .this)</param>
        /// <param name="gateWithIndex">Gate of circuit created by AddGateIndexed</param>
        /// <param name="invert">If the wire should invert it's value while propagating</param>
        public Wire(int fromIndex, int toIndex, Gate gateWithIndex, bool invert = false) : this(fromIndex, toIndex, gateWithIndex.index, false, invert) { }

        /// <summary>
        /// Creates a "wire" connecting two inputs/outputs and or gates/chips
        /// </summary>
        /// <param name="fromIndex">Index of output to grab value from (Input for .this)</param>
        /// <param name="toIndex">Index of input to send value to (Output for .this)</param>
        /// <param name="chipWithIndex">Chip of circuit created by AddChipIndexed</param>
        /// <param name="invert">If the wire should invert it's value while propagating</param>
        public Wire(int fromIndex, int toIndex, Chip chipWithIndex, bool invert = false) : this(fromIndex, toIndex, chipWithIndex.index, true, invert) { }

        /// <summary>
        /// Generates a list of wires if all parameters are identical except for the chips
        /// </summary>
        /// <param name="fromIndex">Index of output to grab value from (Input for .this)</param>
        /// <param name="toIndex">Index of input to send value to (Output for .this)</param>
        /// <param name="chipsWithIndex">List of chips of circuit created by AddChipIndexed</param>
        /// <param name="invert">If the wire should invert it's value while propagating</param>
        public static Wire[] Create(int fromIndex, int toIndex, bool invert = false, params Chip[] chipsWithIndex)
        {
            Wire[] wires = new Wire[chipsWithIndex.Length];
            for (int i = 0; i < chipsWithIndex.Length; i++)
            {
                wires[i] = new Wire(fromIndex, toIndex, chipsWithIndex[i], invert);
            }
            return wires;
        }

        /// <summary>
        /// Generates a list of wires if all parameters are identical except for the gates
        /// </summary>
        /// <param name="fromIndex">Index of output to grab value from (Input for .this)</param>
        /// <param name="toIndex">Index of input to send value to (Output for .this)</param>
        /// <param name="gatesWithIndex">List of gates of circuit created by AddGateIndexed</param>
        /// <param name="invert">If the wire should invert it's value while propagating</param>
        public static Wire[] Create(int fromIndex, int toIndex, bool invert, params Gate[] gatesWithIndex)
        {
            Wire[] wires = new Wire[gatesWithIndex.Length];
            for (int i = 0; i < gatesWithIndex.Length; i++)
            {
                wires[i] = new Wire(fromIndex, toIndex, gatesWithIndex[i], invert);
            }
            return wires;
        }
    }
}
