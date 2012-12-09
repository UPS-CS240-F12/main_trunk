using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
using System.Text;

public class NetworkInterface : MonoBehaviour
{
    public static IEnumerator AddPhoneScore(string phoneID, int score)
    {
        // Try 5 times
        for (uint i = 0; i < 5; ++i)
        {
            WWWForm form = new WWWForm();
            form.AddField(phoneID, score);
            WWW post = new WWW(m_serverURL + "?phone=" + phoneID + "&score=" + score, form);
            yield return post;
            if (post.error == null)
            {
                // No error
                break;
            }
        }
    }

    public static void ClearBattery(string batteryId)
	{
		JSONObject obj = new JSONObject("{\"engine\":{\"batteries\":{\"" + batteryId + "\":null}}}");
        SendSimpleUpdate(obj);
	}

    public static void ClearBullet(string id)
	{
		JSONObject obj = new JSONObject("{\"engine\":{\"turretBullets\":{\"" + id + "\":null}}}");
        SendSimpleUpdate(obj);
	}

    public static void ClearEyeball(string eyeballId)
	{
		JSONObject obj = new JSONObject("{\"engine\":{\"eyeballs\":{\"" + eyeballId + "\":null}}}");
        SendSimpleUpdate(obj);
	}

    public static void ClearFireball(string fireballId)
	{
		JSONObject obj = new JSONObject("{\"engine\":{\"fireballs\":{\"" + fireballId + "\":null}}}");
        SendSimpleUpdate(obj);
	}

    public static void ClearMinion(string minionId)
	{
		JSONObject obj = new JSONObject("{\"engine\":{\"minions\":{\"" + minionId + "\":null}}}");
        SendSimpleUpdate(obj);
	}

    public static void ClearTurret(string id)
	{
		JSONObject obj = new JSONObject("{\"engine\":{\"turrets\":{\"" + id + "\":null}}}");
        SendSimpleUpdate(obj);
	}

    public static void ClearPhoneFireballRequest(string id, string fireballID)
	{
		JSONObject obj = new JSONObject("{\"phones\":{\"" + id + "\":{\"requests\":{\"fireballs\":{\"" + fireballID + "\":null}}}}}");
        SendSimpleUpdate(obj);
	}

    public static void ClearPhoneMinionRequest(string id, string minionID)
	{
		JSONObject obj = new JSONObject("{\"phones\":{\"" + id + "\":{\"requests\":{\"minions\":{\"" + minionID + "\":null}}}}}");
		SendSimpleUpdate(obj);
	}

    private static IEnumerator SendSimpleUpdate(JSONObject obj)
	{
		//Debug.Log ("Sending Update: " + obj.ToString());
		for (uint i = 0; i < 5; ++i)
        {
            WWW post = new WWW(m_serverURL, System.Text.Encoding.UTF8.GetBytes(obj.ToString()));
            yield return post;
            if (post.error == null)
            {
                // No error
                break;
            }
        }
	}

    void ResetGameState(bool saveScores)
    {
        // Try 5 times
        for (uint i = 0; i < 5; ++i)
        {
            WWWForm form = new WWWForm();
            form.AddField("restartGame", true.ToString());
            string serverString = m_serverURL + "?restartGame=true";
            if (saveScores == true)
            {
                form.AddField("logScores", true.ToString());
                serverString += "&logScores=true";
            }

            WWW post = new WWW(serverString, form);

            if (post.error == null)
            {
                // No error
                break;
            }
            else
            {
                Debug.LogWarning("Could not reset game state: " + post.error);
            }
        }
    }

    void Start()
    {
        ResetGameState(false);

        //StartCoroutine("PollData");
        StartCoroutine(SendData());
    }

    void OnDestroy()
    {
        ResetGameState(true);
    }

    private IEnumerator PollData()
    {
        float startTime;
        while (true)
        {
            startTime = Time.realtimeSinceStartup;
            WWW get = new WWW(m_serverURL);
            yield return get;
			if (get.error == null)
			{
	            //Debug.Log(get.text);
                ParseJSON(new JSONObject(get.text));
			}
            yield return new WaitForSeconds(Mathf.Clamp((1.0f / m_pollRate) - (Time.realtimeSinceStartup - startTime), 0, 1));
        }
    }

    private IEnumerator SendData()
    {
        float startTime;
        while (true)
        {
            startTime = Time.realtimeSinceStartup;
            JSONObject newEngineData = new JSONObject();
            newEngineData.type = JSONObject.Type.OBJECT;
            newEngineData.AddField("engine", GetUpdatedEngineJSON());
            WWW post = new WWW(m_serverURL, System.Text.Encoding.UTF8.GetBytes(newEngineData.ToString()));
            yield return post;
            if (post.error == null)
            {
				//Debug.Log(post.text);
                ParseJSON(new JSONObject(post.text));
            }
            yield return new WaitForSeconds(Mathf.Clamp((1.0f / m_pollRate) - (Time.realtimeSinceStartup - startTime), 0, 1));
        }
    }

    private JSONObject GetUpdatedEngineJSON()
    {
        JSONObject engine = new JSONObject();
        engine.type = JSONObject.Type.OBJECT;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerCharacter playerScript = player.GetComponent<PlayerCharacter>();
            if (playerScript != null)
            {
                JSONObject jsonTemp = new JSONObject();
                jsonTemp.type = JSONObject.Type.OBJECT;
                jsonTemp.AddField("energy", playerScript.EnergyPoints);
                jsonTemp.AddField(PositionString, JSONUtils.XYZTripletToJSON(player.transform.position));
                jsonTemp.AddField(RotationString, JSONUtils.XYZTripletToJSON(player.transform.rotation.eulerAngles));
                engine.AddField("player", jsonTemp);
            }
        }

        JSONObject batteryGroup = new JSONObject();
        batteryGroup.type = JSONObject.Type.OBJECT;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Batteries"))
        {
            JSONObject jsonTemp = new JSONObject();
            jsonTemp.type = JSONObject.Type.OBJECT;
            jsonTemp.AddField(PositionString, JSONUtils.XYZTripletToJSON(obj.transform.position));
            batteryGroup.AddField(obj.GetInstanceID().ToString(), jsonTemp);
        }
        engine.AddField("batteries", batteryGroup);

        JSONObject bulletGroup = new JSONObject();
        bulletGroup.type = JSONObject.Type.OBJECT;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Bullets"))
        {
            JSONObject jsonTemp = new JSONObject();
            jsonTemp.type = JSONObject.Type.OBJECT;
            jsonTemp.AddField(PositionString, JSONUtils.XYZTripletToJSON(obj.transform.position));
            jsonTemp.AddField(RotationString, JSONUtils.XYZTripletToJSON(obj.transform.rotation.eulerAngles));
            bulletGroup.AddField(obj.GetInstanceID().ToString(), jsonTemp);
        }
        engine.AddField("turretBullets", bulletGroup);

        JSONObject fireballGroup = new JSONObject();
        fireballGroup.type = JSONObject.Type.OBJECT;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Fireballs"))
        {
            Fireball fireballScript = obj.GetComponent<Fireball>();
            if (fireballScript != null)
            {
                JSONObject jsonTemp = new JSONObject();
                jsonTemp.type = JSONObject.Type.OBJECT;
                jsonTemp.AddField(PositionString, JSONUtils.XYZTripletToJSON(obj.transform.position));
                jsonTemp.AddField(RotationString, JSONUtils.XYZTripletToJSON(obj.transform.rotation.eulerAngles));
                fireballGroup.AddField(fireballScript.ID, jsonTemp);
            }
        }
        engine.AddField("fireballs", fireballGroup);

        JSONObject minionGroup = new JSONObject();
        minionGroup.type = JSONObject.Type.OBJECT;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Minions"))
        {
            MinionObject minionScript = obj.GetComponent<MinionObject>();
            if (minionScript != null)
            {
                JSONObject jsonTemp = new JSONObject();
                jsonTemp.type = JSONObject.Type.OBJECT;
                jsonTemp.AddField(PositionString, JSONUtils.XYZTripletToJSON(obj.transform.position));
                jsonTemp.AddField(RotationString, JSONUtils.XYZTripletToJSON(obj.transform.rotation.eulerAngles));
                minionGroup.AddField(minionScript.ID, jsonTemp);
            }
        }
        engine.AddField("minions", minionGroup);

        JSONObject eyeballGroup = new JSONObject();
        eyeballGroup.type = JSONObject.Type.OBJECT;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Eyeballs"))
        {
            MobileCamera eyeScript = obj.GetComponent<MobileCamera>();
            if (eyeScript != null)
            {
                JSONObject jsonTemp = new JSONObject();
                jsonTemp.type = JSONObject.Type.OBJECT;
                jsonTemp.AddField(PositionString, JSONUtils.XYZTripletToJSON(obj.transform.position));
                jsonTemp.AddField(RotationString, JSONUtils.XYZTripletToJSON(obj.transform.rotation.eulerAngles));
                eyeballGroup.AddField(eyeScript.DeviceID, jsonTemp);
            }
        }
        engine.AddField("eyeballs", eyeballGroup);

        JSONObject towerGroup = new JSONObject();
        towerGroup.type = JSONObject.Type.OBJECT;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Towers"))
        {
            TowerObject script = obj.GetComponent<TowerObject>();
            if (script != null)
            {
                JSONObject jsonTemp = new JSONObject();
                jsonTemp.type = JSONObject.Type.OBJECT;
                jsonTemp.AddField(PositionString, JSONUtils.XYZTripletToJSON(obj.transform.position));
                jsonTemp.AddField(RotationString, JSONUtils.XYZTripletToJSON(obj.transform.Find("Top").rotation.eulerAngles));
                jsonTemp.AddField(OwnerString, script.OwnerID);
                towerGroup.AddField(script.ID, jsonTemp);
            }
        }
        engine.AddField("turrets", towerGroup);

        if (m_enableDeletedTiles == true)
        {
            TerrainGenerator script = m_terrainFactory.GetComponent<TerrainGenerator>();
            if (script != null)
            {
                JSONObject jsonTemp = new JSONObject();
                jsonTemp.type = JSONObject.Type.OBJECT;
                TerrainGenerator terrainData = m_terrainFactory.GetComponent<TerrainGenerator>();
                if (terrainData != null)
                {
                    jsonTemp.AddField("rows", terrainData.Rows);
                    jsonTemp.AddField("columns", terrainData.Columns);
                    List<Vector3> delTilesList = terrainData.GetDeletedTiles();
                    if (delTilesList != null)
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.Append("[");
                        foreach (Vector3 tile in delTilesList)
                        {
                            builder.Append("[");
                            builder.Append(tile.x + 4); // Goes from -4 to 3, add offset to go from 0 to 7
                            builder.Append(",");
                            builder.Append(tile.z + 4); // Same as above
                            builder.Append("],");
                        }
                        if (builder.Length > 1)
                            builder.Remove(builder.Length - 1, 1); // Remove last comma
                        builder.Append("]");
                        JSONObject delTiles = new JSONObject(builder.ToString());
                        jsonTemp.AddField("deletedTiles", delTiles);

                        engine.AddField("platforms", jsonTemp);
                    }
                }
            }
        }

        engine.AddField("gameRunning", true);
        engine.AddField("lastUpdated", (int)(Time.fixedTime * 1000));
        engine.AddField("timeElapsed", (int)(Time.realtimeSinceStartup * 1000));

        return engine;
    }

    private void ParseJSON(JSONObject obj)
    {
        if (obj == null || obj.type == JSONObject.Type.NULL)
            return;

        JSONObject phonesTag = obj[PhonesString];

        if (phonesTag == null || phonesTag.type != JSONObject.Type.OBJECT)
            return;

        GameObject[] turrets = GameObject.FindGameObjectsWithTag("Towers");
        bool[] existingTurrets = new bool[turrets.Length]; // Initializes as false
		
		GameObject[] eyeballs = GameObject.FindGameObjectsWithTag("Eyeballs");
		bool[] existingPhones = new bool[eyeballs.Length];

        for (int i = 0; i < phonesTag.keys.Count; ++i)
        {
            JSONObject phone = phonesTag[i];
            if (phone == null)
                continue;

            string phoneID = phonesTag.keys[i].ToString();
			
			for (int j = 0; j < eyeballs.Length; ++j)
			{
				MobileCamera eyeScript = eyeballs[j].GetComponent<MobileCamera>();
				if (eyeScript != null)
				{
					if (eyeScript.DeviceID == phoneID)
					{
						existingPhones[j] = true;
						break;
					}
				}
			}

            KeyValuePair<bool, Vector3> position = JSONUtils.ParseXYZTriplet(phone[PositionString]);
            KeyValuePair<bool, Vector3> rotation = JSONUtils.ParseXYZTriplet(phone[RotationString]);

            if (position.Key == true && rotation.Key == true)
            {
                GameObject eyeObj = UpdateObject("eyeball" + phoneID, position.Value, Quaternion.Euler(rotation.Value), m_eyeballClone);				
				MobileCamera eyeScript = eyeObj.GetComponent<MobileCamera>();
                if (eyeScript != null)
                    eyeScript.DeviceID = phoneID;
            }

            JSONObject requests = phone[RequestsString];
            if (requests != null)
            {	
                {
					JSONObject nextRequest = requests["fireballs"];
                    // If request is fireball creation
                    if (nextRequest != null)
                    {
                        foreach (string nextID in nextRequest.keys)
                        {
							JSONObject nextFireball = nextRequest[nextID];
                            position = JSONUtils.ParseXYZTriplet(nextFireball[PositionString]);
                            rotation = JSONUtils.ParseXYZTriplet(nextFireball[RotationString]);

                            if (position.Key == true && rotation.Key == true)
                            {
                                GameObject fireballObj = UpdateObject("fireball" + nextID, position.Value, Quaternion.Euler(rotation.Value), m_fireballClone);
                                Fireball fireballScript = fireballObj.GetComponent<Fireball>();
                                if (fireballScript != null)
                                {
                                    fireballScript.ID = nextID;
                                    fireballScript.PhoneSpawnerID = phoneID;
                                }
                            }
							
							ClearPhoneFireballRequest(phoneID, nextID);
                        }
                    }
				}
                    // If request is minion creation
                {
					JSONObject nextRequest = requests["minions"];
					if (nextRequest != null)
                    {
                        foreach (string nextID in nextRequest.keys)
                        {
							JSONObject nextMinion = nextRequest[nextID];
							
                            position = JSONUtils.ParseXYZTriplet(nextMinion[PositionString]);
                            rotation = JSONUtils.ParseXYZTriplet(nextMinion[RotationString]);

                            if (position.Key == true && rotation.Key == true)
                            {
                                GameObject minionObj = UpdateObject("minion" + nextID, position.Value, Quaternion.Euler(rotation.Value), m_minionClone);
                                MinionObject minionScript = minionObj.GetComponent<MinionObject>();
                                if (minionScript != null)
								{
                                    minionScript.ID = nextID;
									minionScript.PhoneOwner = phoneID;
								}
                            }
								
							ClearPhoneMinionRequest(phoneID, nextID);
                        }
                    }
				}
                    // Else if request is turret creation
                {
					JSONObject nextRequest = requests["objectPlacement"];
					if (nextRequest != null)
                    {
                        string type;
                        foreach (string nextID in nextRequest.keys)
                        {
							JSONObject nextObj = nextRequest[nextID];
                            position = JSONUtils.ParseXYZTriplet(nextObj[PositionString]);
                            rotation = JSONUtils.ParseXYZTriplet(nextObj[RotationString]);
                            type = nextObj["type"].str;
							
                            string jsonID = nextID;
                            if (jsonID == null)
                                continue;

                            // TODO: account for types that are not turrets

                            if (position.Key == true && rotation.Key == true)
                            {
                                switch (type)
                                {
                                case "turret":
                                    string turretName = "turret" + jsonID;
                                    string owner = GetOwner(turretName, turrets);
                                    if (owner == null || owner == phoneID || phonesTag.keys.Contains(owner) == false)
                                    {
                                        existingTurrets[i] = true;
                                        GameObject turret = UpdateObject(turretName, position.Value, Quaternion.Euler(rotation.Value), m_turretClone);
                                        TowerObject turretScript = turret.GetComponent<TowerObject>();
                                        if (turretScript != null)
                                        {
                                            turretScript.OwnerID = phoneID;
                                            turretScript.ID = jsonID;
                                        }
                                    }
                                    break;
                                default:
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
		
		for (int i = 0; i < existingPhones.Length; ++i)
		{
			if (existingPhones[i] == false)
			{
				MobileCamera script = eyeballs[i].GetComponent<MobileCamera>();
				if (script != null)
					ClearEyeball(script.DeviceID);
				
				Destroy(eyeballs[i]);
			}
		}

        for (int i = 0; i < existingTurrets.Length; ++i)
        {
            if (existingTurrets[i] == false)
            {
				TowerObject script = turrets[i].GetComponent<TowerObject>();
				if (script != null)
				{
					ClearTurret(script.ID);
				}
				
                Destroy(turrets[i]);
            }
        }

        JSONObject webTag = obj[WebString];
        if (webTag != null)
        {
            JSONObject twitterTag = webTag[TwitterString];
            if (twitterTag != null)
            {
                JSONObject activeEffectTag = twitterTag["activeEffect"];
                if (activeEffectTag != null)
                {
                    string effect = activeEffectTag.str;
                    switch (effect)
                    {
                        case "none":
                            break;
						case "robotBuff":
							break;
						case "eyeballBuff":
							break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    private static string GetOwner(string turretID, GameObject[] turrets)
    {
        foreach (GameObject nextTurret in turrets)
        {
            TowerObject script = nextTurret.GetComponent<TowerObject>();
            if (turretID == nextTurret.name && script != null)
                return script.OwnerID;
        }

        return null;
    }

    private static GameObject UpdateObject(string nameAndID, Vector3 position, GameObject objectClone)
    {
        return UpdateObject(nameAndID, position, Quaternion.identity, objectClone);
    }

    private static GameObject UpdateObject(string nameAndID, Vector3 position, Quaternion rotation, GameObject objectClone)
    {
        GameObject obj = GameObject.Find(nameAndID);
        if (obj == null)
        {
            obj = Instantiate(objectClone, position, rotation) as GameObject;
            obj.name = nameAndID;
        }
        else
        {
            obj.transform.position = position;
            obj.transform.rotation = rotation;
        }

        return obj;
    }

    [SerializeField]
    private static string m_serverURL = "http://puppetmaster.pugetsound.edu:1730/gameState.json";

    [SerializeField]
    private int m_pollRate;

    [SerializeField]
    private GameObject m_turretClone;
    [SerializeField]
    private GameObject m_eyeballClone;
    [SerializeField]
    private GameObject m_fireballClone;
    [SerializeField]
    private GameObject m_minionClone;

    [SerializeField]
    private GameObject m_terrainFactory;
    [SerializeField]
    private GameObject m_pointKeeper;
	
    private const string EngineString = "engine";
    private const string PhonesString = "phones";
    private const string TurretString = "turret";
    private const string WebString = "web";
    private const string TwitterString = "twitter";

    private const string IDString = "ID";
    private const string PositionString = "position";
    private const string RotationString = "rotation";
    private const string OwnerString = "ownerID";
    private const string RequestsString = "requests";

    private bool m_enableDeletedTiles = true;
}
