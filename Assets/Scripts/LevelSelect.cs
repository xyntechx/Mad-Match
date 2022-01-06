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

    void Start()
    {
        button.GetComponent<Button>().onClick.AddListener(Navigate);
        speedrunManager = GameObject.Find("SpeedrunManager");
        speedrunManagerScript = speedrunManager.GetComponent<SpeedRun>();
        thisLvl = Convert.ToInt16(this.name[3]) - 48; // ASCII things, this just converts the button name to a level. If there's a better method, by all means.
    }

    void Update()
    {
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
                    break;
                case "Lvl2Btn":
                    DataController.LevelSelected = 2;
                    break;
                case "Lvl3Btn":
                    DataController.LevelSelected = 3;
                    break;
                case "Lvl4Btn":
                    DataController.LevelSelected = 4;
                    break;
                case "Lvl5Btn":
                    DataController.LevelSelected = 5;
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
                    break;
                case "Lvl2Btn":
                    DataController.LevelSelected = 7;
                    break;
                case "Lvl3Btn":
                    DataController.LevelSelected = 8;
                    break;
                case "Lvl4Btn":
                    DataController.LevelSelected = 9;
                    break;
                case "Lvl5Btn":
                    DataController.LevelSelected = 10;
                    break;
                default:
                    print("Error: Somewhere, somehow, a button has been assigned the wrong script...");
                    break;
            }
        }

        SceneManager.LoadSceneAsync("Game");
    }
}
