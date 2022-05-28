using System;
using System.Collections;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

public class BitArrayConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(BitArray);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        string bytes = serializer.Deserialize<string>(reader);
        bool[] values = Enumerable.Repeat(false, bytes.Length).ToArray();
        for(int i = 0; i < bytes.Length; i++)
        {
            if (bytes[i].Equals('1'))
            {
                values[i] = true;
            }
        }
        return new BitArray(values);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, ((BitArray)value).ToDigitString());
    }

}

public class BitArrayArrayConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(BitArray[]);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        string[] data = serializer.Deserialize<string>(reader).Split('x');

        int arrayLength = int.Parse(data[0]);
        int wordLength = int.Parse(data[1]);
        string hexCodes = data[2];

        return ToBitArrays(hexCodes, arrayLength, wordLength);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        BitArray[] arrays = (BitArray[])value;
        int wordLength = arrays[0].Length / 4;
        serializer.Serialize(writer, $"{arrays.Length}x{wordLength}x{ToString(arrays)}");
    }

    public static string ToString(BitArray[] arrays)
    {
        if (arrays[0].Length % 4 != 0)
        {
            throw new Exception("BitArrays must be multiples of 4 bits");
        }
        int wordLength = arrays[0].Length / 4;

        StringBuilder sb = new StringBuilder();

        foreach (BitArray bits in arrays)
        {
            int[] array = new int[1];
            bits.CopyTo(array, 0);
            sb.Append(array[0].ToString($"X{wordLength}"));
        }

        return sb.ToString();
    }

    public static BitArray[] ToBitArrays(string hexCodes, int arrayLength, int wordLength)
    {
        BitArray[] arrays = new BitArray[arrayLength];

        for (int i = 0; i < arrayLength; i++)
        {
            var value = int.Parse(hexCodes.Substring(i * wordLength, wordLength), System.Globalization.NumberStyles.HexNumber);

            var bits = new bool[wordLength * 4];
            for (int b = 0; b < wordLength * 4; b++)
            {
                bits[b] = (value & 1) == 1;
                value >>= 1;
            }

            arrays[i] = new BitArray(bits);
        }

        return arrays;
    }

}

public static class BitArrayExtensions
{
    public static string ToDigitString(this BitArray array)
    {
        var builder = new StringBuilder();
        foreach(bool bit in array.Cast<bool>())
        {
            builder.Append(bit ? "1" : "0");
        }
        return builder.ToString();
    }

}
