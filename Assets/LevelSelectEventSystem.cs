using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectEventSystem : MonoBehaviour
{

    public Button BackBtn;
    public Button Lvl1Btn;
    public Button Lvl2Btn;
    public Button Lvl3Btn;
    public Button Lvl4Btn;
    public Button Lvl5Btn;

    // Start is called before the first frame update
    void Start()
    {
        BackBtn.GetComponent<Button>().onClick.AddListener(delegate{
            Redirect("Home");
        });

        // should probably implement some better logic for this sometime later...
        Button[] buttons = {Lvl1Btn, Lvl2Btn, Lvl3Btn, Lvl4Btn, Lvl5Btn};
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].GetComponent<Button>().onClick.AddListener(delegate{
                Redirect("Game");
            });
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Redirect(String scene) // just Redirects to a new scene
    {
        SceneManager.LoadScene(scene);
    }
}
