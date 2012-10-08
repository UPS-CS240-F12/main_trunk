using UnityEngine;
using UnityEditor;
using System.Collections;

public class HUDObject : MonoBehaviour
{
    [System.Serializable]
    public enum HUDLocation
    {
        TopLeft,
        TopMiddle,
        TopRight,
        CenterLeft,
        CenterMiddle,
        CenterRight,
        BottomLeft,
        BottomMiddle,
        BottomRight
    }

    [System.Serializable]
    public class HUDMargins
    {
        public int left, top, right, bottom;

        public HUDMargins(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }
    }

    [System.Serializable]
    public class HUDOrientation
    {
        public HUDLocation location;
        public int width, height, xOffset, yOffset;
        public HUDMargins margins = new HUDMargins(5, 5, 5, 5);
    }

    void OnGUI()
    {
        int x = 0, y = 0;
        switch (m_orientation.location)
        {
            case HUDLocation.TopLeft: x = m_orientation.margins.left + m_orientation.xOffset; y = m_orientation.margins.top + m_orientation.yOffset; break;
            case HUDLocation.TopMiddle: x = (Screen.width - m_orientation.width) / 2 + m_orientation.xOffset; y = m_orientation.margins.top + m_orientation.yOffset; break;
            case HUDLocation.TopRight: x = Screen.width - m_orientation.width - m_orientation.margins.right + m_orientation.xOffset; y = m_orientation.margins.top + m_orientation.yOffset; break;
            case HUDLocation.CenterLeft: x = m_orientation.margins.left + m_orientation.xOffset; y = (Screen.height - m_orientation.height) / 2 + m_orientation.yOffset; break;
            case HUDLocation.CenterMiddle: x = (Screen.width - m_orientation.width) / 2 + m_orientation.xOffset; y = (Screen.height - m_orientation.height) / 2 + m_orientation.yOffset; break;
            case HUDLocation.CenterRight: x = Screen.width - m_orientation.width - m_orientation.margins.right + m_orientation.xOffset; y = (Screen.height - m_orientation.height) / 2; break;
            case HUDLocation.BottomLeft: x = m_orientation.margins.left + m_orientation.xOffset; y = Screen.height - m_orientation.height - m_orientation.margins.bottom + m_orientation.yOffset; break;
            case HUDLocation.BottomMiddle: x = (Screen.width - m_orientation.width) / 2 + m_orientation.xOffset; y = Screen.height - m_orientation.height - m_orientation.margins.bottom + m_orientation.yOffset; break;
            case HUDLocation.BottomRight: x = Screen.width - m_orientation.width - m_orientation.margins.right + m_orientation.xOffset; y = Screen.height - m_orientation.height - m_orientation.margins.bottom + m_orientation.yOffset; break;
        }

        GUI.depth = m_drawOrder;
        GUI.Box(new Rect(x, y, m_orientation.width, m_orientation.height), m_content, m_style);
    }

    public int DrawOrder
    {
        get { return m_drawOrder; }
        set { m_drawOrder = value; }
    }

    public HUDOrientation Orientation
    {
        get { return m_orientation; }
        set { m_orientation = value; }
    }

    public GUIContent Content
    {
        get { return m_content; }
        set { m_content = value; }
    }

    public GUIStyle Style
    {
        get { return m_style; }
        set { m_style = value; }
    }

    [SerializeField]
    private int m_drawOrder;
    [SerializeField]
    private HUDOrientation m_orientation;
    [SerializeField]
    private GUIContent m_content;
    [SerializeField]
    private GUIStyle m_style;
}
