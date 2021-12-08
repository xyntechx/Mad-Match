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
                SceneManager.LoadSceneAsync(1); // LevelSelect Scene
                break;
            case "HowToPlayBtn":
                SceneManager.LoadSceneAsync(2); // HowToPlay Scene
                break;
            case "LeaderboardBtn":
                SceneManager.LoadSceneAsync(3); // Leaderboard Scene
                break;
            case "BackBtn":
                SceneManager.LoadSceneAsync(0); // Home Scene
                break;
            default:
                break;
        }
    }
}
