using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;

public class NetworkInterface
{
    private delegate string ErrorHandler();

    private void SendGETRequest(string url)
    {
        using (WebClient wc = new WebClient())
        {
            wc.DownloadStringAsync(new Uri(url));
        }
    }

    private void SendPOSTRequest(string url, string data)
    {
        using (WebClient wc = new WebClient())
        {
            wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            wc.UploadStringAsync(new Uri(url), "POST", data);
        }
    }

    private string PositionToPOSTData(Vector3 vec)
    {
        return "position=" + vec.x + "," + vec.y + "," + vec.z;
    }

    public void SendCharacterData(Vector3 position)
    {
        SendPOSTRequest("url", position.ToString());
    }

    public void SendData()
    {
    }
}
