using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    public GameObject addtimePrefab;

    Text text;
    float timeLeft;
    bool paused = true;

    Queue<GameObject> addtime_objs;

    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<Text>();
        paused = true;
        addtime_objs = new Queue<GameObject>();
    }

    public void toggle_pause()
    {
        paused = !paused;
    }

    public void pause()
    {
        paused = true;
    }

    public void resume()
    {
        paused = false;
    }

    public float get_time() { 
        return timeLeft;
    }

    public void set_time(float time)
    {
        //print("where");
        timeLeft = time;
        text.text = "Time left: " + timeLeft.ToString("0.00") + "s";
    }

    public void add_time(float time)
    {
        // TODO: add time animation
        timeLeft += time;
        GameObject atp = Instantiate(addtimePrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform) as GameObject;
        addtime atp_script = atp.GetComponent<addtime>();
        atp_script.start_anim(time);
        addtime_objs.Enqueue(atp);
    }

    public void add_time_vanish()
    {
        addtime_objs.Dequeue();
    }

    public void destroy_addtime()
    {
        int limit = addtime_objs.Count;
        for (int x = 0; x < limit; x++)
        {
            print("destroying");
            Destroy(addtime_objs.Dequeue());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            timeLeft -= Time.deltaTime;
            text.text = "Time left: " + timeLeft.ToString("0.00") + "s";
        }
        if (!paused && timeLeft <= 0)
        {
            timeLeft = 0f;
            paused = true;
            text.text = "Time left: 0.00s";

            print("You lose!");

            destroy_addtime();

            gameObject.SetActive(false); // TODO: replace all setactive(false) with destroy() since scene is reloaded anyways
            GameObject c = GameObject.Find("LevelManager");
            cardSpawner lvlman_script = c.GetComponent<cardSpawner>();
            lvlman_script.gameover();
        }
    }
}
