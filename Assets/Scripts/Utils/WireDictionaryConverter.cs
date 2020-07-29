using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Chips;
using System.Linq;

public class WireDictionaryConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Dictionary<Guid, List<Wire>>);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        Dictionary<Guid, List<Wire>> value = (Dictionary<Guid, List<Wire>>)existingValue;
        Guid[] values = serializer.Deserialize<Guid[]>(reader);        

        Guid[] valueKeys = value.Keys.ToArray();

        for (int i = 0; i < valueKeys.Length; i++)
        {
            Guid key = valueKeys[i];
            List<Wire> wires = value[key];

            value.Remove(key);
            value.Add(values[i], wires);
        }
        return value;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        Dictionary<Guid, List<Wire>> wireDict = (Dictionary<Guid, List<Wire>>)value;        
        serializer.Serialize(writer, wireDict.Keys.ToArray());
    }
}