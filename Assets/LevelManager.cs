using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public void Awake()
    {
        if (LevelManager.instance == null) instance = this; // make sure there's only 1 LevelManager
        else Destroy(gameObject);
    }

    public void GameOver()
    {
        // switch to the review scene when the game is completed

        UIManager _ui = GetComponent<UIManager>();
        if (_ui != null)
        {
            _ui.ToggleReviewPanel();
            GameObject c = GameObject.Find("LevelManager");
            cardSpawner lvlman_script = c.GetComponent<cardSpawner>();
            lvlman_script.set_game_stage("review");
        }
    }

    public void SeeScore()
    {
        UIManager _ui = GetComponent<UIManager>();
        if (_ui != null)
        {
            _ui.ToggleReviewPanel();
            _ui.ToggleScorePanel();
        }
    }
}
