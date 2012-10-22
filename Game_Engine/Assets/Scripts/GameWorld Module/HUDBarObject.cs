using UnityEngine;
using System.Collections;

public class HUDBarObject : HUDObject
{
    void OnGUI()
    {
        int x = 0, y = 0;
        switch (Orientation.location)
        {
            case HUDLocation.TopLeft: x = Orientation.margins.left + Orientation.xOffset; y = Orientation.margins.top + Orientation.yOffset; break;
            case HUDLocation.TopMiddle: x = (Screen.width - Orientation.width) / 2 + Orientation.xOffset; y = Orientation.margins.top + Orientation.yOffset; break;
            case HUDLocation.TopRight: x = Screen.width - Orientation.width - Orientation.margins.right + Orientation.xOffset; y = Orientation.margins.top + Orientation.yOffset; break;
            case HUDLocation.CenterLeft: x = Orientation.margins.left + Orientation.xOffset; y = (Screen.height - Orientation.height) / 2 + Orientation.yOffset; break;
            case HUDLocation.CenterMiddle: x = (Screen.width - Orientation.width) / 2 + Orientation.xOffset; y = (Screen.height - Orientation.height) / 2 + Orientation.yOffset; break;
            case HUDLocation.CenterRight: x = Screen.width - Orientation.width - Orientation.margins.right + Orientation.xOffset; y = (Screen.height - Orientation.height) / 2; break;
            case HUDLocation.BottomLeft: x = Orientation.margins.left + Orientation.xOffset; y = Screen.height - Orientation.height - Orientation.margins.bottom + Orientation.yOffset; break;
            case HUDLocation.BottomMiddle: x = (Screen.width - Orientation.width) / 2 + Orientation.xOffset; y = Screen.height - Orientation.height - Orientation.margins.bottom + Orientation.yOffset; break;
            case HUDLocation.BottomRight: x = Screen.width - Orientation.width - Orientation.margins.right + Orientation.xOffset; y = Screen.height - Orientation.height - Orientation.margins.bottom + Orientation.yOffset; break;
        }

        GUI.depth = DrawOrder;
		int width = Mathf.Clamp(Orientation.width * m_currentValue / m_maxValue, 0, Orientation.width);
        GUI.Box(new Rect(x, y, width, Orientation.height), new GUIContent(""), Style);
        GUIStyle textStyle = new GUIStyle(Style);
        textStyle.normal.background = null;
        GUI.Label(new Rect(x, y, Orientation.width, Orientation.height), m_currentValue + " / " + m_maxValue, textStyle);
    }

    public int MaxValue
    {
        get { return m_maxValue; }
        set { m_maxValue = value; }
    }

    public int CurrentValue
    {
        get { return m_currentValue; }
        set { m_currentValue = value; }
    }
    
    [SerializeField]
    private int m_maxValue;
    [SerializeField]
    private int m_currentValue;
}
