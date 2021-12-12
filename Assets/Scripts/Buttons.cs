using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
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
            case "PlayBtn":
                SceneManager.LoadSceneAsync("LevelSelect"); // LevelSelect Scene
                break;
            case "HowToPlayBtn":
                SceneManager.LoadSceneAsync("HowToPlay"); // HowToPlay Scene
                break;
            case "LeaderboardBtn":
                SceneManager.LoadSceneAsync("Leaderboard"); // Leaderboard Scene
                break;
            case "BackBtn":
                SceneManager.LoadSceneAsync("Home"); // Home Scene
                break;
            default:
                break;
        }
    }
}
