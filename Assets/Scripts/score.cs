using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class score : MonoBehaviour
{
    // this script sets the value of the score (in the text) under the score panel in the game scene
    Text t;
    void Awake()
    {
        GameObject c = GameObject.Find("LevelManager");
        cardSpawner lvlman_script = c.GetComponent<cardSpawner>();

        t = gameObject.GetComponentInChildren<Text>();
        float highscore = PlayerPrefs.GetFloat("highscore");
        float myscore = lvlman_script.get_gameover_score();
        if (highscore < myscore)
		{
            t.text = "NEW HIGHSCORE: " + myscore.ToString("0");
            PlayerPrefs.SetFloat("highscore", myscore);
            PlayerPrefs.Save();
        } else
		{
            t.text = "HIGHSCORE: " + highscore.ToString("0") + "\n";
            t.text += "YOUR SCORE: " + myscore.ToString("0");
		}
        
    }
}
