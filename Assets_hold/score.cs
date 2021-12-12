using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class score : MonoBehaviour
{
    Text t;
    // Start is called before the first frame update
    void Awake()
    {
        t = gameObject.GetComponentInChildren<Text>();
        GameObject c = GameObject.Find("LevelManager");
        cardSpawner lvlman_script = c.GetComponent<cardSpawner>();
        t.text = lvlman_script.get_gameover_score().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
