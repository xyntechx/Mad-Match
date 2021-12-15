using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickbanner : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject c = GameObject.Find("LevelManager");
            cardSpawner lvlman_script = c.GetComponent<cardSpawner>();
            lvlman_script.start_pregame_anim();
        }
    }
}
