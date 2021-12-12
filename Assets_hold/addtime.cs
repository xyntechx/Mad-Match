using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class addtime : MonoBehaviour
{
    Text text;
    string msg;
    bool leaving = false;
    float leaving_vel = 0.8f;

    // Start is called before the first frame update
    void Awake()
    {
        leaving = false;
        text = GetComponent<Text>();
    }

    public void start_anim(float time)
    {
        //print("starting");
        msg = "+" + time.ToString("0.00") + "s";
        text.text = msg;
        text.transform.position = GameObject.Find("timer").GetComponent<Text>().transform.position + new Vector3(0.85f, 0f, 0f);
        StartCoroutine(fade_anim());
    }

    IEnumerator fade_anim()
    {
        leaving = true;
        const int limit = 50;
        for (int x = 0; x < limit; x++)
        {
            yield return new WaitForSeconds(0.013f);
            Color tmpcolor = text.color;
            tmpcolor.a -= 1f / limit;
            //tmpcolor.a = 0f;
            text.color = tmpcolor;
        }
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        timer timer_script = GameObject.Find("timer").GetComponent<timer>();
        timer_script.add_time_vanish();
    }

    // Update is called once per frame
    void Update()
    {
        if (leaving)
        {
            text.transform.position += new Vector3(0f, leaving_vel * Time.deltaTime, 0f);
        }
    }
}
