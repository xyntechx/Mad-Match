using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class setEntry : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void set(string name, string userid, int score, string flag = "", int pos = 0)
	{
        gameObject.GetComponentInChildren<Text>().text = pos.ToString() + ". " + name + ": " + score.ToString();
        if (flag == "score" || flag == "highscore" || flag == "new highscore")
		{
            gameObject.GetComponentInChildren<Text>().text = flag.ToUpper() + " " + gameObject.GetComponentInChildren<Text>().text;
        }
    }

    public void seterror(string error)
	{
        gameObject.GetComponentInChildren<Text>().text = error;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
