using UnityEngine;
using System.Collections;

public class ButtonPush : MonoBehaviour 
{
	bool click;
    bool hasName, getName;
	
	// Use this for initialization
	void Start () 
	{
		click = false;
        hasName = false;
        getName = false;
	}
	
	void OnMouseDown()
	{
		audio.Play ();
		click = true;
	}
	
	void setClick()
	{
		click = true;
	}
	
	// Update is called once per frame
	void Update()
	{
		if(level == "Options" && click)
		{
			m_camera.SendMessage("RotateRight");
			m_light.SendMessage("RotateRight");
			click = false;
		}
		else if(level == "Credits" && click)
		{
			m_camera.SendMessage("RotateLeft");
			m_light.SendMessage("RotateLeft");
			click = false;
		}
        else if (level == "MainScreenScene" && click)
        {
            UnityEngine.Application.LoadLevel(level);
            click = false;
        }
        else if (click)
        {
            if (hasName == false)
            {
                getName = true;
            }

            click = false;
        }

        if (hasName == true)
        {
            ScreenName.name = name;
            UnityEngine.Application.LoadLevel(level);
        }
	}

    void OnGUI()
    {
        if (hasName == false && getName == true)
        {
            int top = 100;
            int left = 100;
            int right = Screen.width - 100 - left;
            int bottom = Screen.height - 100 - top;

            float fiftyX = 50 * Screen.width / 1024;
            float fiftyY = 50 * Screen.height / 768;

            Rect group = new Rect(left, top, right, bottom);
            GUI.Box(group, new GUIContent(""), m_style);

            GUI.BeginGroup(group);
            m_font.fontSize = 48 * Screen.height / 768;
            GUI.Label(new Rect(fiftyX, fiftyY, right - (fiftyX * 2), (fiftyY * 2)), "Enter Name: ", m_font);

            GUI.SetNextControlName("Text");
            name = GUI.TextField(new Rect(fiftyX, fiftyY * 3, right - (fiftyX * 2), bottom - (fiftyY * 7)), name, 40, m_font);

            if (GUI.Button(new Rect(fiftyX * 2, bottom - (fiftyY * 3), fiftyX * 4, fiftyY * 2), "Go!", m_font) && name.Length > 0)
            {
                hasName = true;
                getName = false;
            }
            else if (GUI.Button(new Rect(right - (fiftyX * 6), bottom - (fiftyY * 3), fiftyX * 4, fiftyY * 2), "Cancel", m_font))
            {
                hasName = false;
                getName = false;
            }

            GUI.EndGroup();

            if (GUI.GetNameOfFocusedControl() == string.Empty)
            {
                GUI.FocusControl("Text");
            }
        }
    }
	
	[SerializeField]
	string level;

    [SerializeField]
    string name;
	
	[SerializeField]
	private GameObject m_camera;
	
	[SerializeField]
	private GameObject m_light;

    [SerializeField]
    private GUIStyle m_style;

    [SerializeField]
    private GUIStyle m_font;
}
