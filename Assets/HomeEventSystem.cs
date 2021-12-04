using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeEventSystem : MonoBehaviour
{
    public Button playBtn;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = playBtn.GetComponent<Button>();
        btn.onClick.AddListener(delegate{
            Redirect();
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Redirect() 
    {
        SceneManager.LoadScene("LevelSelect");
    }
}
