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
        t.text = lvlman_script.get_gameover_score().ToString();
    }
}
