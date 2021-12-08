using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickbanner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print("here");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("clicked");
            GameObject c = GameObject.Find("LevelManager");
            cardSpawner lvlman_script = c.GetComponent<cardSpawner>();
            lvlman_script.start_pregame_anim();
        }
    }
}
