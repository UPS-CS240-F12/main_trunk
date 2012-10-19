using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;

public class NetworkInterface : MonoBehaviour
{
    void Start()
    {
        JSONObject obj = new JSONObject("{\"turret\":{\"ID\":\"1\",\"position\":\"1400,0,1400\"}}");

        //foreach (JSONObject turret in obj["turrets"].list)
        {
            HandleJSON(obj);
            /*GameObject newT = Instantiate(m_turretClone, ParseVector3(turret["position"].str), Quaternion.identity) as GameObject;
            newT.name = ("turret" + turret["ID"].str);*/
        }

        //StartCoroutine("PollData");
    }

    private IEnumerator PollData()
    {
        float startTime;
        while (true)
        {
            startTime = Time.realtimeSinceStartup;
            WWW get = new WWW(m_serverURL);
            yield return get;
            Debug.Log(get.text);
            HandleJSON(get.text);
            yield return new WaitForSeconds(Mathf.Clamp((1.0f / m_pollRate) - (Time.realtimeSinceStartup - startTime), 0, 1));
        }
    }

    private void HandleJSONArray(string name, JSONObject arrayObj)
    {
        if (arrayObj.type != JSONObject.Type.ARRAY)
            return;

        foreach (JSONObject obj in arrayObj.list)
        {
            HandleJSONObject(name, obj);
        }
    }

    private bool TryJSONToInt(JSONObject jsonObj, ref int value)
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

    private bool TryJSONToFloat(JSONObject jsonObj, ref float value)
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

    KeyValuePair<bool, Vector3> ParseJSONPosition(JSONObject jsonPosition)
    {
        if (jsonPosition == null) 
            return new KeyValuePair<bool,Vector3>(false, Vector3.zero);

        Vector3 position;

        switch (jsonPosition.type)
        {
            case JSONObject.Type.STRING:
                try
                {
                    position = ParseVector3(jsonPosition.str);
                }
                catch (InvalidOperationException)
                {
                    return new KeyValuePair<bool,Vector3>(false, Vector3.zero);
                }
                break;

            case JSONObject.Type.OBJECT:
                float coord = 0.0f;

                if (TryJSONToFloat(jsonPosition[XString], ref coord) == false)
                    return new KeyValuePair<bool, Vector3>(false, Vector3.zero);
                position.x = coord;

                if (TryJSONToFloat(jsonPosition[YString], ref coord) == false)
                    return new KeyValuePair<bool, Vector3>(false, Vector3.zero);
                position.y = coord;

                if (TryJSONToFloat(jsonPosition[ZString], ref coord) == false)
                    return new KeyValuePair<bool, Vector3>(false, Vector3.zero);
                position.z = coord;
                break;

            default: // Invalid JSON
                return new KeyValuePair<bool,Vector3>(false, Vector3.zero);
        }

        return new KeyValuePair<bool, Vector3>(true, position);
    }

    private void HandleJSON(string s)
    {
        HandleJSON(new JSONObject(s));
    }

    private void HandleJSON(JSONObject obj)
    {
        if (obj == null)
            return;

        if (obj[TurretString] != null)
            HandleJSONObject(TurretString, obj[TurretString]);
    }

    private void HandleJSONObject(string name, JSONObject obj)
    {
        if (obj == null || obj.type == JSONObject.Type.NULL)
            return;

        int id = 0;
        if (TryJSONToInt(obj[IDString], ref id) == false)
            return;

        KeyValuePair<bool, Vector3> position = ParseJSONPosition(obj[PositionString]);

        if (position.Key == false)
            return;

        GameObject cloneObject;
        if (name == TurretString)
            cloneObject = m_turretClone;
        else
        {
            Debug.LogWarning("Invalid JSON object: " + name);
            return;
        }

        UpdateObject(name, id, position.Value, cloneObject);
    }

    private static Vector3 ParseVector3(string s)
    {
        char[] comma = { ',' };
        string[] nums = s.Split(comma, 3);

        if (nums.Length != 3)
            throw new InvalidOperationException();
        else
        {
            Vector3 retVec = Vector3.zero;
            float coord;

            if (float.TryParse(nums[0], out coord) == false)
                throw new InvalidOperationException();
            retVec.x = coord;

            if (float.TryParse(nums[1], out coord) == false)
                throw new InvalidOperationException();
            retVec.y = coord;

            if (float.TryParse(nums[2], out coord) == false)
                throw new InvalidOperationException();
            retVec.z = coord;

            return retVec;
        }
    }

    private static void UpdateObject(string name, int id, Vector3 position, GameObject objectClone)
    {
        UpdateObject(name + id, position, objectClone);
    }

    private static void UpdateObject(string name, string id, Vector3 position, GameObject objectClone)
    {
        UpdateObject(name + id, position, objectClone);
    }

    private static void UpdateObject(string nameAndID, Vector3 position, GameObject objectClone)
    {
        GameObject obj = GameObject.Find(nameAndID);
        if (obj == null)
        {
            obj = Instantiate(objectClone, position, Quaternion.identity) as GameObject;
            obj.name = nameAndID;
        }
        else
        {
            obj.transform.position = position;
        }
    }

    [SerializeField]
    private string m_serverURL;

    [SerializeField]
    private int m_pollRate;

    [SerializeField]
    private GameObject m_turretClone;

    private const string TurretString = "turret";
    private const string IDString = "ID";
    private const string PositionString = "position";

    private const string XString = "x";
    private const string YString = "y";
    private const string ZString = "z";
}
