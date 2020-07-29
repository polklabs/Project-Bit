using System;
using Newtonsoft.Json;

public class BreadBoardConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(BreadBoardData);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        string[] data = serializer.Deserialize<string>(reader).Split('x');       
       
        return new BreadBoardData(data[0], int.Parse(data[1]), int.Parse(data[2]), data[3] == "1");
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        BreadBoardData bbd = (BreadBoardData)value;
        serializer.Serialize(writer, string.Format("{0}x{1}x{2}x{3}", bbd.t, bbd.x.ToString(), bbd.y.ToString(), (bbd.r ? "1" : "0")));
    }

}