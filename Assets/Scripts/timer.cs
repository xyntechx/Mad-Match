using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    public GameObject addtimePrefab;

    Text text; // the text to show on screen
    float timeLeft;
    bool paused = true;

    Queue<GameObject> addtime_objs; // a queue of addtime objects, which are created whenever the user selects a correct pair

    void Awake()
    {
        text = GetComponent<Text>();
        paused = true;
        addtime_objs = new Queue<GameObject>();
    }

    // pause and resume the timer
    public void pause()
    {
        paused = true;
    }

    public void resume()
    {
        paused = false;
    }

    public void toggle_pause()
    {
        paused = !paused;
    }

    // getter and setter for time
    public float get_time() { 
        return timeLeft;
    }

    public void set_time(float time)
    {
        timeLeft = time;
        text.text = timeLeft.ToString("0.00") + "s";
    }

    // procedure for adding time: add_time, add_time_vanish and destroy_addtime
    // add_time creates an addtime object which contains a text and an animation
    public void add_time(float time)
    {
        timeLeft += time;
        GameObject atp = Instantiate(addtimePrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform) as GameObject;
        addtime atp_script = atp.GetComponent<addtime>();
        atp_script.start_anim(time);
        addtime_objs.Enqueue(atp); // this keeps track of all the addtime objects created
        // we use queues here because the addtime objects all have the same animation duration so the queue is strictly sequential
    }

    // add_time_vanish is called by the addtime objects themselves when the animation is over
    // the addtime objects will also destroy themselves when the animation is over so we don't do that here
    public void add_time_vanish()
    {
        addtime_objs.Dequeue(); // dequeueing allows us to track which addtime objects are still active
    }

    // destroy_addtime is manually called when the game is over to destroy any addtime objects that are still active
    // this happens typically when the player completes the level and addtime is called to reward the player with time
    // this function then clears all the addtime objects still left in the queue to prevent showing them on the score page
    // which happens in the same scene as the level
    public void destroy_addtime()
    {
        int limit = addtime_objs.Count;
        for (int x = 0; x < limit; x++)
        {
            Destroy(addtime_objs.Dequeue());
        }
    }

    // called every frame
    void Update()
    {
        if (!paused) // runs while the timer is not paused
        {
            timeLeft -= Time.deltaTime;
            text.text = timeLeft.ToString("0.00") + "s";
        }

        if (!paused && timeLeft <= 0) // lose condition is met
        {
            timeLeft = 0f;
            paused = true; // prevent timer from further ticking
            text.text = "Time left: 0.00s";

            print("You lose!");

            destroy_addtime(); // manually clear the addtime objects

            gameObject.SetActive(false); // TODO: replace all setactive(false) with destroy() since scene is reloaded anyways
            GameObject c = GameObject.Find("LevelManager");
            cardSpawner lvlman_script = c.GetComponent<cardSpawner>();
            lvlman_script.gameover(); // call the gameover function in the cardspawner script
        }
    }
}
