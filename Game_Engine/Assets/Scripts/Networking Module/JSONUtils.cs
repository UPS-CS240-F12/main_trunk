using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class JSONUtils
{
    public static bool TryJSONToInt(JSONObject jsonObj, ref int value)
    {
        if (jsonObj == null)
        {
            return false;
        }
        if (jsonObj.type == JSONObject.Type.NUMBER)
        {
            value = (int)jsonObj.n;
            return true;
        }
        else if (jsonObj.type == JSONObject.Type.STRING)
        {
            return int.TryParse(jsonObj.str, out value);
        }
        else
        {
            return false;
        }
    }

    public static bool TryJSONToFloat(JSONObject jsonObj, ref float value)
    {
        if (jsonObj == null)
        {
            return false;
        }
        if (jsonObj.type == JSONObject.Type.NUMBER)
        {
            value = (float)jsonObj.n;
            return true;
        }
        else if (jsonObj.type == JSONObject.Type.STRING)
        {
            return float.TryParse(jsonObj.str, out value);
        }
        else
        {
            return false;
        }
    }

    public static KeyValuePair<bool, Vector3> ParseXYZTriplet(JSONObject json)
    {
        if (json == null)
            return new KeyValuePair<bool, Vector3>(false, Vector3.zero);

        if (json.type == JSONObject.Type.OBJECT)
        {
            float coord = 0.0f;
            Vector3 position = new Vector3();

            if (TryJSONToFloat(json[XString], ref coord) == false)
                return new KeyValuePair<bool, Vector3>(false, Vector3.zero);
            position.x = coord;

            if (TryJSONToFloat(json[YString], ref coord) == false)
                return new KeyValuePair<bool, Vector3>(false, Vector3.zero);
            position.y = coord;

            if (TryJSONToFloat(json[ZString], ref coord) == false)
                return new KeyValuePair<bool, Vector3>(false, Vector3.zero);
            position.z = coord;

            return new KeyValuePair<bool, Vector3>(true, position);
        }
        else
            return new KeyValuePair<bool, Vector3>(false, Vector3.zero);
    }

    public static JSONObject XYZTripletToJSON(float x, float y, float z)
    {
        return XYZTripletToJSON(new Vector3(x, y, z));
    }

    public static JSONObject XYZTripletToJSON(Vector3 xyz)
    {
        JSONObject obj = new JSONObject();
        obj.type = JSONObject.Type.OBJECT;
        obj.AddField(XString, xyz.x);
        obj.AddField(YString, xyz.y);
        obj.AddField(ZString, xyz.z);

        return obj;
    }

    public delegate void JSONObjectFunc<T>(T obj);

    public static void ParseJSONArray<T>(JSONObject arrayObj, JSONObjectFunc<T> func)
    {
        if (arrayObj.type != JSONObject.Type.ARRAY)
            return;

        foreach (T obj in arrayObj.list)
            func(obj);
    }

    public const string XString = "x";
    public const string YString = "y";
    public const string ZString = "z";
}
