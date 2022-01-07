using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public Button button;
    GameObject speedrunManager;
    SpeedRun speedrunManagerScript;
    int thisLvl;

    void Awake()
	{
        // intialise online variables
        if (PlayerPrefs.GetString("myID") == "")
		{
            System.Random rd = new System.Random(Guid.NewGuid().GetHashCode());
            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789!@$?_-";
            int stringLength = 32;
            char[] chars = new char[stringLength];

            for (int i = 0; i < stringLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }
            
            PlayerPrefs.SetString("myID", new string(chars));
            PlayerPrefs.Save();
        }
        print(PlayerPrefs.GetString("myID"));
    }

    void Start()
    {
        button.GetComponent<Button>().onClick.AddListener(Navigate);
        speedrunManager = GameObject.Find("SpeedrunManager");
        speedrunManagerScript = speedrunManager.GetComponent<SpeedRun>();
        thisLvl = Convert.ToInt16(this.name[3]) - 48; // ASCII things, this just converts the button name to a level. If there's a better method, by all means.
    }

    void Update()
    {
        // TODO: prevent user from playing if locked
        // speedrunmode and is unlocked
        if (speedrunManagerScript.speedrunMode && DataController.getLock(thisLvl)) {
            button.GetComponent<Image>().color = Color.red;
        }
        // speedrunmode and is locked
        if (speedrunManagerScript.speedrunMode && !DataController.getLock(thisLvl)) {
            button.GetComponent<Image>().color = Color.gray;
        }
        // not speedrunmode
        if (!speedrunManagerScript.speedrunMode) {
            float r = 6.0f/256.0f;
            float g = 167.0f/256.0f;
            float b = 132.0f/256.0f;
            button.GetComponent<Image>().color = new Color(r, g, b);
        }
    }

    void Navigate()
    {
        bool srm = speedrunManagerScript.speedrunMode;
        bool unlocked = DataController.getLock(thisLvl);
        
        if (!srm) 
        {
            switch (this.name)
            {
                case "Lvl1Btn":
                    DataController.LevelSelected = 1;
                    PlayerPrefs.SetInt("lvl", 1);
                    PlayerPrefs.Save();
                    break;
                case "Lvl2Btn":
                    DataController.LevelSelected = 2;
                    PlayerPrefs.SetInt("lvl", 2);
                    PlayerPrefs.Save();
                    break;
                case "Lvl3Btn":
                    DataController.LevelSelected = 3;
                    PlayerPrefs.SetInt("lvl", 3);
                    PlayerPrefs.Save();
                    break;
                case "Lvl4Btn":
                    DataController.LevelSelected = 4;
                    PlayerPrefs.SetInt("lvl", 4);
                    PlayerPrefs.Save();
                    break;
                case "Lvl5Btn":
                    DataController.LevelSelected = 5;
                    PlayerPrefs.SetInt("lvl", 5);
                    PlayerPrefs.Save();
                    break;
                default:
                    print("Error: Somewhere, somehow, a button has been assigned the wrong script...");
                    break;
            }
        }
        
        if (srm && unlocked)
        {
            switch (this.name)
            {
                case "Lvl1Btn":
                    DataController.LevelSelected = 6;
                    PlayerPrefs.SetInt("lvl", 6);
                    PlayerPrefs.Save();
                    break;
                case "Lvl2Btn":
                    DataController.LevelSelected = 7;
                    PlayerPrefs.SetInt("lvl", 7);
                    PlayerPrefs.Save();
                    break;
                case "Lvl3Btn":
                    DataController.LevelSelected = 8;
                    PlayerPrefs.SetInt("lvl", 8);
                    PlayerPrefs.Save();
                    break;
                case "Lvl4Btn":
                    DataController.LevelSelected = 9;
                    PlayerPrefs.SetInt("lvl", 9);
                    PlayerPrefs.Save();
                    break;
                case "Lvl5Btn":
                    DataController.LevelSelected = 10;
                    PlayerPrefs.SetInt("lvl", 10);
                    PlayerPrefs.Save();
                    break;
                default:
                    print("Error: Somewhere, somehow, a button has been assigned the wrong script...");
                    break;
            }
        }

        SceneManager.LoadSceneAsync("Game");
    }
}
