using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public Button button;

    void Start()
    {
        button.GetComponent<Button>().onClick.AddListener(Navigate);
    }

    void Navigate()
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

        SceneManager.LoadSceneAsync("Game");
    }
}
