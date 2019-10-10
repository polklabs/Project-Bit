using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

public class BitArrayConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(BitArray);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var bytes = serializer.Deserialize<byte[]>(reader);
        return bytes == null ? null : new BitArray(bytes);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, ((BitArray)value).BitArrayToByteArray());
    }

}

public static class BitArrayExtensions
{
    public static byte[] BitArrayToByteArray(this BitArray bits)
    {
        byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
        bits.CopyTo(ret, 0);
        return ret;
    }
}
