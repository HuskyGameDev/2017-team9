﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KeyBindScript : MonoBehaviour {


    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    public Text up, left, down, right, pause;
    private GameObject currentKey;
    public Color32 normal = new Color(5, 5, 5, 5);
    public Color32 selected = new Color(255,255,255,255);
    // Use this for initialization
    public Button StartButton;
    public Button Options;
    public Button Quit;
    public GameObject Main_Menu;
    public GameObject Options_Menu; 

    void Start () {

        Main_Menu.SetActive(true);
        Options_Menu.SetActive(false);

        Button start = StartButton.GetComponent<Button>();
        start.onClick.AddListener(StartGame);
        Button options = Options.GetComponent<Button>();
        options.onClick.AddListener(OptionsMenu);
        Button exit = Quit.GetComponent<Button>();
        exit.onClick.AddListener(GetOut);
        

        keys.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up","W")));
        keys.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
        keys.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A")));
        keys.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));
        keys.Add("Pause", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Pause", "Escape")));
        up.text = keys["Up"].ToString();
        down.text = keys["Down"].ToString();
        left.text = keys["Left"].ToString();
        right.text = keys["Right"].ToString();
        pause.text = keys["Pause"].ToString();

    }

   
	// Update is called once per frame
	void Update () {
		
       
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

    void StartGame()
    {
        Debug.Log("LOAD THE FUCKING SCENE");
        Debug.Log("Loading Scene ");
        SceneManager.LoadScene("PlayTesting");
    }
    void OptionsMenu()
    {
        Debug.Log("LOAD THE FUCKING OPTIONS SCREEN");
        Main_Menu.SetActive(false);
        Options_Menu.SetActive(true);
        
    }
    void GetOut()
    {
        Debug.Log("GET THE FUCK OUT OF OUR GAME");
        Debug.Log("Quit");
        Application.Quit();
    }

    public void SaveKeys()
    {
        foreach(var key in keys)
        {
            PlayerPrefs.SetString(key.Key, key.Value.ToString());
            Options_Menu.SetActive(false);
            Main_Menu.SetActive(true);
        }

        PlayerPrefs.Save(); 
    }


}
