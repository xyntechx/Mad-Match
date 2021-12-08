using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card : MonoBehaviour
{
    public Sprite cardFront;
    public Sprite cardBack;
    public bool cardBackIsActive = true;
    public int flip_timer;
    public int flip_half;
    public int flip_status;
    public bool flip_started, flip_done;
    

    Sprite my_sprite;
    int my_card_id;
    bool my_card_picture;
    bool immobile = false;

    // Start is called before the first frame update
    void Awake()
    {
        flip_timer = 0;
        flip_half = 1;
        flip_status = 1;
        cardBackIsActive = true; // BUG: for some reason, this has to be manually ticked under the prefab inspector as well.
        flip_started = false;
        flip_done = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSprite(Sprite s, int card_id, bool picture)
    {
        my_sprite = s;
        my_card_id = card_id;
        my_card_picture = picture;
    }

    void OnMouseDown()
    {
        if (!flip_started && flip_done && cardBackIsActive) // prevents user from undoing their click on the card
        {
            StartFlip();
        }
    }

    public void set_immobile(bool flag)
    {
        immobile = flag;
    }

    void StartFlip(bool init = true, bool setDefaultColor = false)
    {
        if (immobile) return;
        StartCoroutine(CalculateFlip(init, setDefaultColor));
        flip_started = true;
        flip_done = false;
    }

    public void ShowBack(bool init = false, bool setDefaultColor = true)
    {
        if (cardBackIsActive == false)
        {
            StartFlip(init, setDefaultColor);
        }
    }

    public void ShowFront(bool init = true)
    {
        if (cardBackIsActive == true)
        {
            StartFlip(init);
        }
    }

    void Flip()
    {
        if (cardBackIsActive == true)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = my_sprite;
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            cardBackIsActive = false;
        } else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = cardBack;
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            cardBackIsActive = true;
        }
    }

    IEnumerator CalculateFlip(bool init = true, bool setDefaultColor = false)
    {
        for (int i = 0; i < 180; i++)
        {
            yield return new WaitForSeconds(0.003f);
            // rotation is 180deg - arctan(w/h)
            //transform.Rotate(new Vector3(0, 0, 1), 1);
            transform.Rotate(0, 0, flip_half * 110f / 180);
            transform.Rotate(new Vector3(400, 596, 0), flip_status * 1);
            flip_timer++;
            if (flip_timer == 90 || flip_timer == -90)
            {
                Flip();
                flip_half = -1;
                if (setDefaultColor)
                {
                    gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                }
            }
        }

        flip_started = false;
        flip_done = true;
        flip_timer = 0;
        flip_half = 1;
        flip_status *= -1;

        if (init) // false when startflip is initiated by start of game revealing animation
        {
            GameObject c = GameObject.Find("LevelManager");
            cardSpawner lvlman_script = c.GetComponent<cardSpawner>();
            lvlman_script.flip_completed(my_card_id, my_card_picture);
        }
        
    }

    public void WrongAnim()
    {
        // TODO: prettier wrong animation
        StartCoroutine(CalculateWrongAnim());
    }

    IEnumerator CalculateWrongAnim()
    {
        for (int i = 0; i < 200; i++)
        {
            //print(i);
            yield return new WaitForSeconds(0.01f);
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.6f, 0.6f, 1f);
            if (i == 179)
            {
                ShowBack(false, true);
            }
        }
    }

}
