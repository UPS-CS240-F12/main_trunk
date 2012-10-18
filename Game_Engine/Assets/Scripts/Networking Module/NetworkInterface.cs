using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;

public class NetworkInterface : MonoBehaviour
{
    void Start()
    {
        JSONObject obj = new JSONObject("{\"turrets\":[{\"ID\":\"1\",\"position\":\"700,75,700\"},{\"ID\":\"2\",\"position\":\"200,75,200\"},{\"ID\":\"3\",\"position\":\"1400,75,1400\"}]}");
        foreach (JSONObject turret in obj["turrets"].list)
        {
            GameObject newT = Instantiate(m_turretClone, ParseVector3(turret["position"].str), Quaternion.identity) as GameObject;
            newT.name = ("turret" + turret["ID"].str);
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
            HandleJSONTurret(get.text);
            yield return new WaitForSeconds(Mathf.Clamp((1.0f / m_pollRate) - (Time.realtimeSinceStartup - startTime), 0, 1));
        }
    }

    private void HandleJSONTurret(string s)
    {
        JSONObject obj = new JSONObject(s);
        if (obj == null || obj.type == JSONObject.Type.NULL)
            return;

        JSONObject jsonTurret = obj["turret"];

        if (jsonTurret == null || jsonTurret.type == JSONObject.Type.NULL)
            return;

        JSONObject jsonID = jsonTurret["ID"];
        JSONObject.Type jsonIDType = jsonID.type;
        if (jsonID == null || jsonIDType == JSONObject.Type.NULL)
            return;

        JSONObject jsonPosition = jsonTurret["position"];
        if (jsonPosition == null || jsonPosition.type != JSONObject.Type.STRING)
            return;

        Vector3 position;
        try
        {
            position = ParseVector3(jsonPosition.str);
        }
        catch (InvalidOperationException)
        {
            return;
        }

        string turretNameAndID;
        if (jsonIDType == JSONObject.Type.NUMBER)
            turretNameAndID = "turret" + jsonID.n.ToString();
        else
            turretNameAndID = "turret" + jsonID.str;

        GameObject turret;
        if (TryGetObject(turretNameAndID, out turret) == true)
        {
            turret.transform.position = position;
        }
        else
        {
            turret = Instantiate(m_turretClone, position, Quaternion.identity) as GameObject;
            turret.name = turretNameAndID;
        }
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

    private static bool TryGetObject(string name, string id, out GameObject foundObject)
    {
        return TryGetObject(name + id, out foundObject);
    }

    private static bool TryGetObject(string nameAndID, out GameObject foundObject)
    {
        foundObject = GameObject.Find(nameAndID);
        return (foundObject != null);
    }

    [SerializeField]
    private string m_serverURL;

    [SerializeField]
    private int m_pollRate;

    [SerializeField]
    private GameObject m_turretClone;
}
