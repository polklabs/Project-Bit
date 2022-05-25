using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Chips;
using System.Linq;

public class WireConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Wire);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {        
        string[] value = serializer.Deserialize<string>(reader).Split('_');

        if (value.Length == 4)
        {
            return new Wire(int.Parse(value[0]), int.Parse(value[1]), int.Parse(value[2]), value[3] == "1");
        } 
        else
        {
            return new Wire(int.Parse(value[0]), int.Parse(value[1]), int.Parse(value[2]), value[3] == "1", value[4] == "1");
        }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        Wire wire = (Wire)value;
        serializer.Serialize(writer, string.Format("{0}_{1}_{2}_{3}_{4}", wire.FromIndex, wire.ToIndex, wire.CircuitIndex, wire.IsChip ? "1" : "0", wire.InvertValue ? "1" : "0"));
    }
}