using UnityEngine;
using System.Collections;

public class ButtonPush : MonoBehaviour 
{
	
	bool click;
	
	// Use this for initialization
	void Start () 
	{
		click = false;
		float newColor = 0.0f;
        
	}
	
	void OnMouseDown()
	{
		click = true;
	}
	
	// Update is called once per frame
	void Update()
	{
		if(level == "Options" && click)
		{
			m_camera.SendMessage("RotateRight");
			m_light.SendMessage ("RotateRight");
			click = false;
		}
		else if(level == "Credits" && click)
		{
			//Holder for moving to credits not loading game
			m_camera.SendMessage("RotateLeft");
			m_light.SendMessage ("RotateLeft");
			click = false;
		}
		else if(click)
			Application.LoadLevel(level);
			click = false;
	}
	
	[SerializeField]
	string level;
	
	[SerializeField]
	private GameObject m_camera;
	
	[SerializeField]
	private GameObject m_light;
}
