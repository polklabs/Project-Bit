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
