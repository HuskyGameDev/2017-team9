using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KeyBindScript : MonoBehaviour {


    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    public Text up, left, down, right, pause;
    private GameObject currentKey;
    private int Pvalue = 0;
    public Color normal = new Color(0, 255, 45, 255);
    public Color selected = new Color(255,255,255,255);
    // Use this for initialization
    public Button StartButton;
    public Button Options;
    public Button Quit;
    public Button Resume; 
    public GameObject Main_Menu;
    public GameObject Options_Menu;
    public GameObject Player;
    private PlayerControls.PlayerState PreviousState;
	private Scene scene;

    void Start () {
		scene = SceneManager.GetActiveScene();

		if (scene == SceneManager.GetSceneByName("dev_Richy"))
        {
            Main_Menu.SetActive(true);
            Options_Menu.SetActive(false);
            Button start = StartButton.GetComponent<Button>();
            start.onClick.AddListener(StartGame);
        }
        else
        {
            Main_Menu.SetActive(false);
            Options_Menu.SetActive(false);
        }
        
        Button options = Options.GetComponent<Button>();
        options.onClick.AddListener(OptionsMenu);
        Button exit = Quit.GetComponent<Button>();
        exit.onClick.AddListener(GetOut);
        Button resume = Resume.GetComponent<Button>();
        resume.onClick.AddListener(PausePlease);

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
        if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Pause"))))
        {
			if (!(Options_Menu.activeSelf)) {
				PausePlease ();
			}
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

    void StartGame()
    {
        SceneManager.LoadScene("PauseMenu");
    }
    void OptionsMenu()
    {
        Main_Menu.SetActive(false);
        Options_Menu.SetActive(true);
        
    }
    void GetOut()
    {
        Debug.Log("GET THE FUCK OUT OF OUR GAME");
        Debug.Log("Quit");
        Application.Quit();
    }

    void PausePlease()
    {
        if (Pvalue == 0)
        {
            PreviousState = Player.GetComponent<PlayerControls>().state;
            Player.GetComponent<PlayerControls>().state = PlayerControls.PlayerState.LockedWithMouse;
            Main_Menu.SetActive(true);
            Pvalue = 1;
        }
        else
        {
            Pvalue = 0;
            Main_Menu.SetActive(false);
            Options_Menu.SetActive(false);
            //Player.GetComponent<GridMovementController>().StartCoroutine("TransitionToPlayer");
            Player.GetComponent<PlayerControls>().state = PreviousState;
        }
    }

    public void SaveKeys()
    {
        foreach(var key in keys)
        {
            PlayerPrefs.SetString(key.Key, key.Value.ToString());
        }
		Options_Menu.SetActive(false);
		Main_Menu.SetActive(true);
        PlayerPrefs.Save(); 
    }


}
