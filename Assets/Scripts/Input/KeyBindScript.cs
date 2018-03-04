using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindScript : MonoBehaviour {


    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    public Text up, left, down, right, pause;
    private GameObject currentKey;
    public Color32 normal = new Color(5, 5, 5, 5);
    public Color32 selected = new Color(255,255,255,255);
    // Use this for initialization

    void Start () {
        keys.Add("Up", KeyCode.W);
        keys.Add("Down", KeyCode.S);
        keys.Add("Left", KeyCode.A);
        keys.Add("Right", KeyCode.D);
        keys.Add("Pause", KeyCode.Escape);

        up.text = keys["Up"].ToString();
        down.text = keys["Down"].ToString();
        left.text = keys["Left"].ToString();
        right.text = keys["Right"].ToString();
        pause.text = keys["Pause"].ToString();

    }
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetKeyDown(keys["Up"]))
        {
            Debug.Log("Up");
        }

        if (Input.GetKeyDown(keys["Down"]))
        {
            Debug.Log("Down");
        }

        if (Input.GetKeyDown(keys["Left"]))
        {
            Debug.Log("Left");
        }

        if (Input.GetKeyDown(keys["Right"]))
        {
            Debug.Log("Right");
        }

        if (Input.GetKeyDown(keys["Pause"]))
        {
            Debug.Log("Pause");
        }
    }

    private void OnGUI()
    {
        if(currentKey != null)
        {
            Event e = Event.current; 
            if(e.isKey)
            {
                keys[currentKey.name] = e.keyCode;
                currentKey.transform.GetChild(0).GetComponent<Text>().text = e.keyCode.ToString();
                currentKey.GetComponent<Image>().color = normal;
                currentKey = null; 
            }
        }
    }

    public void ChangeKey(GameObject clicked)
    {
        if(currentKey != null)
        {
            currentKey.GetComponent<Image>().color = normal;
        }

        currentKey = clicked;
        currentKey.GetComponent<Image>().color = selected; 
    }


}
