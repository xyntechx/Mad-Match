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
                SceneManager.LoadSceneAsync(7);
                break;
            case "Lvl2Btn":
                SceneManager.LoadSceneAsync(4);
                break;
            case "Lvl3Btn":
                SceneManager.LoadSceneAsync(8);
                break;
            case "Lvl4Btn":
                SceneManager.LoadSceneAsync(6);
                break;
            case "Lvl5Btn":
                SceneManager.LoadSceneAsync(5);
                break;
            default:
                break;
        }
    }
}
