using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card : MonoBehaviour
{
    // TODO: TOFIX: bug that prevents the card from spawning face-up.
    // this is fine for now as we have managed to use work-arounds to avoid needing to spawn them face-up
    // eg. spawning face down and then playing the flip over animation
    // ultimately we can just leave this bug if we want to

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
    bool immobile = false; // controls whether the card will flip when the user clicks on it

    void Awake()
    {
        flip_timer = 0;
        flip_half = 1;
        flip_status = 1;
        cardBackIsActive = true; // BUG: for some reason, this has to be manually ticked under the prefab inspector as well.
        flip_started = false;
        flip_done = true;
    }

    void OnMouseDown()
    {
        if (!flip_started && flip_done && cardBackIsActive) // prevents user from undoing their click on the card
        {
            StartFlip();
        }
    }

    public void setSprite(Sprite s, int card_id, bool picture) // called by cardspawner when creating a card object (spawncard and review_next)
    {
        my_sprite = s;
        my_card_id = card_id;
        my_card_picture = picture;
    }

    public bool set_immobile(bool flag) // returns original value of immobile
    {
        bool orig_immobile = immobile;
        immobile = flag;
        return orig_immobile;
    }

    public void ShowBack(bool init = false, bool setDefaultColor = true, bool force = false)
    {
        // init: false when startflip is initiated by start of game revealing animation etc
        // setting to false prevents flipcomplete (in cardspawner) from being called
        // setting to true (default) indicates that the card should be flipped back over when more than 2 cards are flipped face up
        // this may be an undesired behaviour for certain visual tasks
        if (cardBackIsActive == false)
        {
            StartFlip(init, setDefaultColor, force);
        }
    }

    public void ShowFront(bool init = true)
    {
        // init: false when startflip is initiated by start of game revealing animation etc
        // setting to false prevents flipcomplete (in cardspawner) from being called
        // setting to true (default) indicates that the card should be flipped back over when more than 2 cards are flipped face up
        // this may be an undesired behaviour for certain visual tasks
        if (cardBackIsActive == true)
        {
            StartFlip(init);
        }
    }

    public void WrongAnim()
    {
        // TODO: prettier wrong animation
        StartCoroutine(CalculateWrongAnim());
    }

    void StartFlip(bool init = true, bool setDefaultColor = false, bool force = false)
    {
        // init: false when startflip is initiated by start of game revealing animation etc
        // setting to false prevents flipcomplete (in cardspawner) from being called
        // setting to true (default) indicates that the card should be flipped back over when more than 2 cards are flipped face up
        // this may be an undesired behaviour for certain visual tasks
        if (!force && immobile) return;
        StartCoroutine(CalculateFlip(init, setDefaultColor));
        flip_started = true;
        flip_done = false;
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
        // init: false when startflip is initiated by start of game revealing animation etc
        // setting to false prevents flipcomplete (in cardspawner) from being called
        // setting to true (default) indicates that the card should be flipped back over when more than 2 cards are flipped face up
        // this may be an undesired behaviour for certain visual tasks
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

    IEnumerator CalculateWrongAnim() // 179 * 0.01f + 180 * 0.003f = 2.33s
    {
        for (int i = 0; i < 200; i++)
        {
            //print(i);
            yield return new WaitForSeconds(0.01f);
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.6f, 0.6f, 1f);
            if (i == 179)
            {
                ShowBack(false, true, true);
            }
        }
    }

}
