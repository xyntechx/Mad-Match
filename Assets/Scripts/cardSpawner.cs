using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class cardSpawner : MonoBehaviour
{
    public GameObject cardPrefab;
    public Sprite[] fronts; // ALL imported cards' sprites
    public string[] fronts_names; // their corresponding filename, without the extension, aka card name

    public List<GameObject> cards; // ALL spawned cards' gameobjects
    public List<string> cards_names; // spawned cards' corresponding names

    // game state variables
    int num_cards_flipped = 0; // number of cards flipped
    List<string> flipped_cards_names; // flipped cards' corresponding names
    int num_cards_solved = 0;
    int num_cards_spawned = 0;
    bool level_active = true;
    string game_stage = "clicktostart";
    bool game_stage_changing = false;

    timer timer_script;
    GameObject timer_obj;

    clickbanner clicktostart_banner_script;
    GameObject clicktostart_banner_obj;

    // Start is called before the first frame update
    void Awake()
    {
        fronts = Resources.LoadAll<Sprite>("mycard/fronts");
        fronts_names = new string[fronts.Length];
        for (int x = 0; x < fronts.Length; x++)
        {
            fronts_names[x] = fronts[x].name;
        }

        cards = new List<GameObject>();
        cards_names = new List<string>();
        flipped_cards_names = new List<string>();

        //print("start: " + fronts.Length);

        int count = 0;
        bool flag = true;
        // change x to 5, y to 4 and count == 11
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                num_cards_spawned++;
                count++;
                if (count == 4)
                {
                    flag = false;
                    count = 1;
                }
                SpawnCard(count, flag, -5f + x*2.5f, -3.8f + y*2.5f);
            }
        }

        // external scripts
        timer_obj = GameObject.Find("timer");
        timer_script = timer_obj.GetComponent<timer>();

        clicktostart_banner_obj = GameObject.Find("clickbanner");
        clicktostart_banner_script = clicktostart_banner_obj.GetComponent<clickbanner>();
    }

    void Start()
    {
        prep_level();
        //gameover();
    }

    void SpawnCard(int card_id, bool picture, float x, float y)
    {
        //print("spawncard:" + card_id);
        GameObject c = Instantiate(cardPrefab) as GameObject;
        card card_script = c.GetComponent<card>();
        c.transform.position = new Vector3(x, y, 0);

        string card_name = card_id.ToString() + "_" + (picture ? "image" : "text");
        int fronts_idx = Array.IndexOf(fronts_names, card_name);
        card_script.setSprite(fronts[fronts_idx], card_id, picture);
        cards.Add(c);
        cards_names.Add(card_name);
    }

    public void flip_completed(int card_id, bool picture)
    {
        //print("flip_completed:" + card_id + picture);
        //print(flipped_cards_names.Count);
        string card_name = card_id.ToString() + "_" + (picture ? "image" : "text");
        num_cards_flipped += 1;
        flipped_cards_names.Add(card_name);
    }

    void shuffle_cards(int idx1, int idx2, float shuffle_time) // TODO: 3 card adaptation of this
    { // TODO: add wiggle animation before commencing shuffle
        shuffle_time *= 100;
        card card_script1 = cards[idx1].GetComponent<card>();
        card card_script2 = cards[idx2].GetComponent<card>();
        card_script1.set_immobile(true);
        card_script2.set_immobile(true);
        Vector3 pos1 = cards[idx1].transform.position;
        Vector3 pos2 = cards[idx2].transform.position;
        Vector3 pos1to2 = pos2 - pos1;
        Vector3 pos2to1 = pos1 - pos2;
        StartCoroutine(calc_shuffle_cards(idx1, idx2, pos1to2, pos2to1, shuffle_time));
    }

    IEnumerator calc_shuffle_cards(int idx1, int idx2, Vector3 pos1to2, Vector3 pos2to1, float shuffle_time)
    {
        for (int i = 0; i < shuffle_time; i++)
        {
            yield return new WaitForSeconds(0.01f);
            cards[idx1].transform.Translate(pos1to2 / shuffle_time);
            cards[idx2].transform.Translate(pos2to1 / shuffle_time);
        }
        card card_script1 = cards[idx1].GetComponent<card>();
        card card_script2 = cards[idx2].GetComponent<card>();
        card_script1.set_immobile(false);
        card_script2.set_immobile(false);
    }

    public void gameover()
    {
        set_game_stage("gameover");
        level_active = false;
        for (int x = 0; x < cards.Count; x++)
        {
            cards[x].SetActive(false);
        }
        LevelManager.instance.GameOver();
    }

    int gameover_score;
    public int get_gameover_score()
    {
        return gameover_score;
    }

    public void set_game_stage(string game_stage)
    {
        this.game_stage = game_stage;
    }

    public void start_pregame_anim()
    {
        if (!game_stage_changing && game_stage == "clicktostart") 
        {
            game_stage_changing = true;
            StartCoroutine(clicktostart_delay());
        }
    }

    bool review_next_flag = false;
    public void review_next() // to review the next error
    {
        review_next_flag = true;
    }

    // Update is called once per frame
    void Update()
    {
        // clicktostart -> pregame -> ingame -> gameover -> review -> score -> clicktostart
        if (!game_stage_changing && game_stage == "pregame")
        {
            game_stage_changing = true;
            StartCoroutine(calc_reveal());
        }

        if (!game_stage_changing && game_stage == "review" && review_next_flag)
        {
            review_next_flag = false;
            // TODO: spawn the relevant images to review on the review panel
            // TODO: call the relevant function to notify the panel when all reviews are completed
        }

        if (!game_stage_changing && game_stage == "score")
        {

        }

        if (level_active && num_cards_solved == num_cards_spawned)
        {
            // game has been won!
            print("You win!");
            timer_script.pause();
            gameover_score = (int)Math.Ceiling(timer_script.get_time() * 100);
            timer_script.destroy_addtime();
            timer_obj.SetActive(false);

            gameover();
        }
        // game master logic
        if (num_cards_flipped >= 2)
        {
            if (flipped_cards_names[0].Substring(0, 2) == flipped_cards_names[1].Substring(0, 2))
            {
                print("good");
                for (int x = 0; x < flipped_cards_names.Count; x++)
                {
                    GameObject c = cards[cards_names.IndexOf(flipped_cards_names[x])];
                    card card_script = c.GetComponent<card>();
                    card_script.set_immobile(true);
                }
                num_cards_solved += 2;
                timer_script.add_time(10f);
            } else
            {
                print("bad");
                for (int x = 0; x < flipped_cards_names.Count; x++)
                {
                    GameObject c = cards[cards_names.IndexOf(flipped_cards_names[x])];
                    card card_script = c.GetComponent<card>();
                    card_script.WrongAnim();
                    //card_script.ShowBack();
                }
            }
            
            num_cards_flipped = 0;
            flipped_cards_names.Clear();
        }
    }

    void prep_level()
    {
        // TODO: level specific stats should come here, including spawning of cards
        timer_script.set_time(20f);
        for (int x = 0; x < cards.Count; x++)
        {
            card card_script = cards[x].GetComponent<card>();
            card_script.set_immobile(true);
        }
        clicktostart_banner_obj.SetActive(true);
        GameObject.Find("clickbannertext").SetActive(true);
        GameObject.Find("timer").SetActive(true);
    }

    IEnumerator clicktostart_delay()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject.Find("clickbannertext").SetActive(false);
        clicktostart_banner_obj.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        for (int x = 0; x < cards.Count; x++)
        {
            card card_script = cards[x].GetComponent<card>();
            card_script.set_immobile(false);
        }

        game_stage_changing = false;
        game_stage = "pregame";
    }

    IEnumerator calc_reveal()
    {
        // basically our pregame animation, involving revealing the cards (flipping over), pausing for user to memorise, flipping over again and then shuffling
        for (int x = 0; x < cards.Count; x++)
        {
            //print("here");
            card card_script = cards[x].GetComponent<card>();
            card_script.ShowFront(false);
            card_script.set_immobile(true);
        }
        yield return new WaitForSeconds(2f);
        for (int x = 0; x < cards.Count; x++)
        {
            card card_script = cards[x].GetComponent<card>();
            card_script.set_immobile(false);
            card_script.ShowBack();
            card_script.set_immobile(true);
        }
        yield return new WaitForSeconds(1f);
        shuffle_cards(1, 2, 0.5f);
        shuffle_cards(3, 5, 0.5f);
        yield return new WaitForSeconds(1f);
        for (int x = 0; x < cards.Count; x++)
        {
            card card_script = cards[x].GetComponent<card>();
            card_script.set_immobile(false);
        }
        timer_script.resume();
        game_stage_changing = false;
        game_stage = "ingame";
    }
}
