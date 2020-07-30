using Newtonsoft.Json;
using System.Runtime.Serialization;
using System;
using UnityEngine;

public class Vector3Converter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Vector3);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        Vector3Struct value = serializer.Deserialize<Vector3Struct>(reader);
        return value.ToVector3();
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        Vector3 vector3 = (Vector3)value;
        Vector3Struct result = new Vector3Struct(vector3);        
        serializer.Serialize(writer, result);
    }
}

[DataContract]
public struct Vector3Struct
{
    [DataMember]
    public float x;
    [DataMember]
    public float y;
    [DataMember]
    public float z;

    public Vector3Struct(Vector3 vector3)
    {
        x = vector3.x;
        y = vector3.y;
        z = vector3.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}
