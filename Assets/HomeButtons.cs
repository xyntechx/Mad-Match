using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeButtons : MonoBehaviour {
    public Button button;

    void Start() {
		button.GetComponent<Button>().onClick.AddListener(Navigate);
	}

	void Navigate() {
        if (this.name == "PlayBtn") {
            SceneManager.LoadSceneAsync(1); // LevelSelect Scene
        } else if (this.name == "HowToPlayBtn") {
            SceneManager.LoadSceneAsync(2); // HowToPlay Scene
        } else if (this.name == "LeaderboardBtn") {
            SceneManager.LoadSceneAsync(3); // Leaderboard Scene
        }
	}
}
