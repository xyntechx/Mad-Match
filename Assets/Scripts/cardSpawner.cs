using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class cardSpawner : MonoBehaviour
{
	[SerializeField] GameObject reviewPanel;
	[SerializeField] GameObject scorePanel;
	[SerializeField] GameObject LBPanel;

	public GameObject cardPrefab;
	public Sprite[] fronts; // ALL imported cards' sprites
	public string[] fronts_names; // their corresponding filename, without the extension, aka card name

	public List<GameObject> cards; // ALL spawned cards' gameobjects
	public List<string> cards_names; // spawned cards' corresponding names

	// game state variables
	int lvl_num;
	int num_cards_flipped = 0; // number of cards flipped
	List<string> flipped_cards_names; // flipped cards' corresponding names
	int num_cards_solved = 0;
	int num_cards_spawned = 0; // total number of cards spawned
	bool level_active = true;
	string game_stage = "clicktostart"; // clicktostart -> pregame -> ingame -> gameover -> review -> score -> clicktostart
	bool game_stage_changing = false;

	// mistakes and review variables
	int gameover_score;
	int[] mistakes;
	GameObject[] review_cards = new GameObject[2];
	bool review_next_flag = false;
	int review_idx = 0;
	List<int> cards_to_review = new List<int>();

	// external objects and scripts
	timer timer_script;
	GameObject timer_obj;
	clickbanner clicktostart_banner_script;
	GameObject clicktostart_banner_obj;
	Button PauseBtn;
	pause pause_btn_script;

	void OnEnable()
	{
		this.lvl_num = (DataController.LevelSelected != 0) ? DataController.LevelSelected : 1; // Level selected defaults to 1 if not set
	}

	void Awake()
	{
		// import the faces of the cards, located in the mycard directory
		fronts = Resources.LoadAll<Sprite>("mycard/fronts");
		fronts_names = new string[fronts.Length]; // a paired array containing the names of each corresponding face
		for (int x = 0; x < fronts.Length; x++)
		{
			fronts_names[x] = fronts[x].name;
		}

		cards = new List<GameObject>();
		cards_names = new List<string>();
		flipped_cards_names = new List<string>();

		// external scripts
		timer_obj = GameObject.Find("timer");
		timer_script = timer_obj.GetComponent<timer>();

		clicktostart_banner_obj = GameObject.Find("clickbanner");
		clicktostart_banner_script = clicktostart_banner_obj.GetComponent<clickbanner>();

		PauseBtn = GameObject.Find("PauseBtn").GetComponent<Button>();
		pause_btn_script = PauseBtn.GetComponent<pause>();
		pause_btn_script.set_immobile(true);
		PauseBtn.onClick.AddListener(() => // I'm not sure where to put this, so I'll just leave it here for now...
		{
			if (!pause_btn_script.immobile)
			{
				timer_script.toggle_pause();
				for (int x = 0; x < cards.Count; x++)
				{
					card card_script = cards[x].GetComponent<card>();
					card_script.set_immobile(timer_script.paused);
				}
			}
		});
	}

	void Start()
	{
		// order of calling: onEnable -> awake -> start
		// so the relevant variables would have been initalised and the level number would have been obtained already
		prep_level(lvl_num);
	}

	public int get_gameover_score()
	{
		return gameover_score;
	}

	public void set_game_stage(string game_stage)
	{
		this.game_stage = game_stage;
	}

	public void start_pregame_anim() // called by the clickbanner script
	{
		if (!game_stage_changing && game_stage == "clicktostart")
		{
			game_stage_changing = true;
			StartCoroutine(clicktostart_delay());
			// the coroutine changes the gamestate to "pregame" upon completion
		}
	}

	public void flip_completed(int card_id, bool picture)
	{ // this function is called when the animation for the card flipping is completed
		string card_name = card_id.ToString() + "_" + (picture ? "image" : "text");
		num_cards_flipped += 1;
		flipped_cards_names.Add(card_name);
	}

	public void gameover()
	{
		// this mistakes stuff is really dumb right now - scrapped together.
		// mistakes is an array. index 0 corresponds to card1_image and card1_text etc
		// when a mistake (wrong pairing) is made on either the image or the text card, the value is incremented by 1.
		// eg if the user selects card1_image and card3_text, mistakes[0] += 1; mistakes[2] += 1;
		// TODO: maximum of 3 mistakes, but if 0 mistakes were made it should not be shown

		int[] mistakes_copy = new int[mistakes.Length];
		mistakes.CopyTo(mistakes_copy, 0);

		// we want to find the cards with the most mistakes made so we sort in descending order
		Array.Sort(mistakes);
		Array.Reverse(mistakes);
		// next, we take the number of mistakes for the top 3 mistakes (now the first 3 elements in the mistakes array)
		// and we look for its position in the original mistakes array (stored in mistakes_copy)
		// its position/index would indicate what card the number of mistakes corresponds to
		// we add 1 to account for the difference in indexing/naming
		// TODO: this could probably be done better with some <pair> struct

		for (int i = 0; i < mistakes.Length; i++)
		{
			print(mistakes_copy[i]);
		}

		if (mistakes.Length > 0)
		{
			if (mistakes[0] > 0)
			{
				cards_to_review.Add(Array.IndexOf(mistakes_copy, mistakes[0]) + 1);
				mistakes_copy[Array.IndexOf(mistakes_copy, mistakes[0])] = -1;
				// prevents using the same card again
				// eg. if mistakes was [2, 2] and mistakes_copy was [2, 2]: the first card we find is mistakes_copy[0]
				// on the second pass (below) we would obtain that same card.
				// so, we set the first card's value to -1 so that the indexof does not find it again
			}
		}

		print("adsf");
		for (int i = 0; i < mistakes.Length; i++)
		{
			print(mistakes_copy[i]);
		}

		if (mistakes.Length > 1)
		{
			if (mistakes[1] > 0)
			{
				cards_to_review.Add(Array.IndexOf(mistakes_copy, mistakes[1]) + 1);
				mistakes_copy[Array.IndexOf(mistakes_copy, mistakes[1])] = -1;
			}
		}
		if (mistakes.Length > 2)
		{
			if (mistakes[2] > 0)
			{
				cards_to_review.Add(Array.IndexOf(mistakes_copy, mistakes[2]) + 1);
				mistakes_copy[Array.IndexOf(mistakes_copy, mistakes[2])] = -1;
			}
		}


		set_game_stage("gameover"); // this game stage is actually superfluous for now
		level_active = false;
		for (int x = 0; x < cards.Count; x++)
		{
			cards[x].SetActive(false);
		}

		reviewPanel.SetActive(!reviewPanel.activeSelf);
		set_game_stage("review");
		review_next();
	}


	public void review_next() // to review the next error
	{
		if (cards_to_review.Count == 0)
		{
			SeeScore();
			return;
		}

		// review_cards is an array of 2 cards containing the image and text pairing
		for (int x = 0; x < 2; x++)
		{
			// remove any cards that might have been set by a previous run of review_next
			if (review_cards[x] != null)
			{
				Destroy(review_cards[x]);
			}
		}

		review_next_flag = true;
		if (review_idx == 3 || review_idx >= cards_to_review.Count) // this means that the maximum of 3 reviews has been hit. redirects player to the score page.
		{
			SeeScore();
			return;
		}

		// some messy repeated code here to generate the image and text card pairing
		// create two new gameobjects for the two cards and flip them over face up
		{
			GameObject c = Instantiate(cardPrefab) as GameObject;
			card card_script = c.GetComponent<card>();
			c.transform.position = new Vector3(-1.25f, 0f, 0f);
			int card_id = cards_to_review[review_idx];
			string card_name = card_id.ToString() + "_image";
			int fronts_idx = Array.IndexOf(fronts_names, card_name);
			card_script.setSprite(fronts[fronts_idx], card_id, true);
			card_script.ShowFront(false);
			review_cards[0] = c;
		}
		{
			GameObject c = Instantiate(cardPrefab) as GameObject;
			card card_script = c.GetComponent<card>();
			c.transform.position = new Vector3(1.25f, 0f, 0f);
			int card_id = cards_to_review[review_idx];
			string card_name = card_id.ToString() + "_text";
			int fronts_idx = Array.IndexOf(fronts_names, card_name);
			card_script.setSprite(fronts[fronts_idx], card_id, false);
			card_script.ShowFront(false);
			review_cards[1] = c;
		}
		review_idx++;
	}

	public void SeeScore() // this function is also called by the seescore button under the review panel in the game scene
	{
		set_game_stage("score");
		// sets the review panel to be inactive as it was previously active before leading to the score panel
		reviewPanel.SetActive(!reviewPanel.activeSelf);
		// sets the score panel to be active
		if (DataController.LevelSelected > 5)
		{
			// speedrun mode
			PlayerPrefs.SetFloat("myScore", (float)get_gameover_score());
			PlayerPrefs.Save();
			SceneManager.LoadSceneAsync("EndingScreen");
			//LBPanel.SetActive(!LBPanel.activeSelf);
		}
		else
		{
			scorePanel.SetActive(!scorePanel.activeSelf);
		}
	}

	void prep_level(int lvl_num)
	{
		// level specific stats should come here, including spawning of cards
		//print("Preparing level " + lvl_num.ToString());
		int rows = 0;
		int cols = 0;
		int flag_switch = 0;
		float x_offset = 0f;
		float y_offset = 0f;
		bool randomCards = false; // if it's SPEEDRUN mode, cards will be randomly shown...

		if (lvl_num > 5) {
			randomCards = true;
		}

		// different levels
		switch (lvl_num)
		{
			case 1:
			case 6:
				cols = 5;
				rows = 2;
				flag_switch = 6;
				x_offset = -5f;
				y_offset = -1.25f;
				break;
			case 2: // a little bit long... can't really fit 7 pairs into neat rows, but it works.
			case 7:
				cols = 7;
				rows = 2;
				flag_switch = 8;
				x_offset = -7.5f;
				y_offset = -1.25f;
				break;
			case 3:
			case 8:
				cols = 6;
				rows = 3;
				flag_switch = 10;
				x_offset = -6.25f;
				y_offset = -2.6f;
				break;
			case 4: // case 4 and 5 are the same (10 pairs)
			case 5:
			case 9:
			case 10:
				cols = 5;
				rows = 4;
				flag_switch = 11;
				x_offset = 1f + -5f; // -5 to 5, interval 2.5, offset 1f because of timer and top left
				y_offset = -3.8f;
				break;
			default:
				print("Error: An unknown level was built");
				break;
		}

		timer_script.set_time(20f);
		int count = 0;
		int spawn_id = 0; // card being spawned on that iteration
		List<int> card_ids = new List<int>(); // accumulates a list of "image" card ids so their corresponding "text" cards can be spawned
		bool flag = true;
		for (int x = 0; x < cols; x++)
		{
			for (int y = 0; y < rows; y++)
			{
				num_cards_spawned++;
				count++;
				if (count == flag_switch)
				{
					flag = false;
					count = 1;
				}
				if (!randomCards)
				{
					spawn_id = count;
				} else 
				{
					if (flag) 
					{
						do 
						{
							spawn_id = UnityEngine.Random.Range(1, 11);
						} while (card_ids.Contains(spawn_id));
						card_ids.Add(spawn_id);
					} else 
					{
						spawn_id = card_ids[count-1];
					}
				}
				SpawnCard(spawn_id, flag, x_offset + x * 2.5f, y_offset + y * 2.5f);
			}
		}

		// initialise mistakes to an array of zeroes. index 0 corresponds to card1_image and card1_text
		mistakes = new int[cards.Count / 2];
		Array.Clear(mistakes, 0, mistakes.Length);
		for (int x = 0; x < cards.Count; x++)
		{
			card card_script = cards[x].GetComponent<card>();
			card_script.set_immobile(true); // cards will be unlocked in start_pregame_anim
		}
		clicktostart_banner_obj.SetActive(true);
		GameObject.Find("clickbannertext").SetActive(true);
		GameObject.Find("clickbannertext").GetComponent<Text>().text = "Level " + lvl_num.ToString() + "\nClick to start!";
		GameObject.Find("timer").SetActive(true);
	}

	void SpawnCard(int card_id, bool picture, float x, float y)
	{
		GameObject c = Instantiate(cardPrefab) as GameObject;
		card card_script = c.GetComponent<card>();
		c.transform.position = new Vector3(x, y, 0); // spawns the card at the input x and y coordinates

		// we get the card's face's sprite here by generating its card_name, looking for that name in the list of fronts_names
		// and then using the index of the name we find to locate the sprite in the array fronts
		// we can do this because fronts_names and fronts are "paired"
		string card_name = card_id.ToString() + "_" + (picture ? "image" : "text");
		int fronts_idx = Array.IndexOf(fronts_names, card_name);
		card_script.setSprite(fronts[fronts_idx], card_id, picture);

		// cards and cards_names will have corresponding card objects and their names
		cards.Add(c);
		cards_names.Add(card_name);
	}

	void shuffle_cards(int idx1, int idx2, float shuffle_time) // TODO: 3 card adaptation of this
	{ // TODO: add wiggle animation before commencing shuffle
		shuffle_time *= 100; // because of the for loop in calc_shuffle_cards
		card card_script1 = cards[idx1].GetComponent<card>();
		card card_script2 = cards[idx2].GetComponent<card>();

		card_script1.set_immobile(true);
		card_script2.set_immobile(true);
		Vector3 pos1 = cards[idx1].transform.position;
		Vector3 pos2 = cards[idx2].transform.position;
		Vector3 pos1to2 = pos2 - pos1;
		Vector3 pos2to1 = pos1 - pos2;

		StartCoroutine(calc_shuffle_cards(idx1, idx2, pos1to2, pos2to1, shuffle_time)); // start the animation
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
		// unlock flipping of the card again
		card_script1.set_immobile(false);
		card_script2.set_immobile(false);
	}

	void Update()
	{
		// game_stage: clicktostart -> pregame -> ingame -> gameover -> review -> score -> clicktostart
		if (!game_stage_changing && game_stage == "pregame")
		{
			game_stage_changing = true;
			StartCoroutine(pregame_reveal(this.lvl_num));
		}

		if (!game_stage_changing && game_stage == "review" && review_next_flag)
		{
			review_next_flag = false;
			// TODO: spawn the relevant images to review on the review panel
			// TODO: call the relevant function to notify the panel when all reviews are completed
		}

		if (!game_stage_changing && game_stage == "score")
		{
			for (int x = 0; x < 2; x++)
			{
				if (review_cards[x] != null)
				{
					Destroy(review_cards[x]); // same rationale as in review_next
				}
			}
		}

		if (level_active && num_cards_solved == num_cards_spawned)
		{
			// game has been won!
			print("You win!");
			timer_script.pause();
			gameover_score = (int)Math.Ceiling(timer_script.get_time() * 100);
			timer_script.destroy_addtime();
			timer_obj.SetActive(false);
			pause_btn_script.set_immobile(false);
			DataController.unlock(lvl_num);

			gameover();
		}

		// game master logic
		if (num_cards_flipped >= 2)
		{
			if (flipped_cards_names[0].Substring(0, 2) == flipped_cards_names[1].Substring(0, 2)) // compares to see if the cards are a matching pair
																								  // this substring-ing works because the first two characters will either be X_ or XX where X is a digit. this will work for total cards < 100.
			{
				// correct pair chosen
				// print("good");
				for (int x = 0; x < flipped_cards_names.Count; x++)
				{
					GameObject c = cards[cards_names.IndexOf(flipped_cards_names[x])];
					card card_script = c.GetComponent<card>();
					card_script.set_immobile(true); // prevent the correct pair from being flipped again
				}
				num_cards_solved += 2;
				timer_script.add_time(10f); // reward the player with extra time
			}
			else
			{
				// wrong pair chosen
				//print("bad");
				for (int x = 0; x < flipped_cards_names.Count; x++)
				{
					GameObject c = cards[cards_names.IndexOf(flipped_cards_names[x])];
					card card_script = c.GetComponent<card>();
					card_script.WrongAnim();
				}
				StartCoroutine(global_wrong_anim());
				// this global animation is called to prevent other cards from being flipped when the player chooses a wrong pair
				// this prevents the player from spam guessing

				int id1 = Int32.Parse(flipped_cards_names[0].Substring(0, flipped_cards_names[0].IndexOf('_')));
				int id2 = Int32.Parse(flipped_cards_names[1].Substring(0, flipped_cards_names[1].IndexOf('_')));
				// increase the count of mistakes made by one. mistakes is zero-indexed so we subtract one from the ids which start from 1.
				mistakes[id1 - 1] += 1;
				mistakes[id2 - 1] += 1;
			}

			num_cards_flipped = 0;
			flipped_cards_names.Clear();
		}
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

	IEnumerator pregame_reveal(int lvl_num)
	{
		// basically our pregame animation, involving revealing the cards (flipping over), pausing for user to memorise, flipping over again and then shuffling
		// setting up level stats
		System.Random rnd = new System.Random();
		int shuffles = 0;
		int max = 0;
		float shuffle_time = 0.5f;
		float memorise_time = 1f;

		switch (lvl_num)
		{
			case 1:
			case 6:
				break;
			case 2:
			case 7:
				shuffles = 3;
				max = 14;
				break;
			case 3:
			case 8:
				shuffles = 5;
				max = 18;
				break;
			case 4:
			case 9:
				shuffles = 5;
				max = 20;
				memorise_time = 5f;
				break;
			case 5:
			case 10:
				shuffles = 7;
				max = 20;
				memorise_time = 5f;
				break;
			default:
				print("Error: An unknown level was loaded.");
				break;
		}

		for (int x = 0; x < cards.Count; x++)
		{
			card card_script = cards[x].GetComponent<card>();
			card_script.ShowFront(false);
			card_script.set_immobile(true);
		}
		yield return new WaitForSeconds(memorise_time); // wait here to let players memorise cards
		for (int x = 0; x < cards.Count; x++)
		{
			card card_script = cards[x].GetComponent<card>();
			card_script.set_immobile(false);
			card_script.ShowBack();
			card_script.set_immobile(true);
		}
		yield return new WaitForSeconds(1f);
		// TODO: each level (those with shuffles) should have some pre-defined shuffles and shuffles randomly generated at runtime
		// So... I'm gonna go ahead and make random shuffles for all levels for now... not sure what this means lol
		/* Old shuffles in case anyone needs them
            level 1
                shuffle_cards(1, 2, 0.5f);
                yield return new WaitForSeconds(0.5f);
                shuffle_cards(3, 5, 0.5f);
            Level 2
                 shuffle_cards(1, 5, 0.5f);
                yield return new WaitForSeconds(0.5f);
                shuffle_cards(16, 10, 0.5f);
                yield return new WaitForSeconds(0.5f);
                shuffle_cards(8, 7, 0.5f);
                yield return new WaitForSeconds(0.5f);
                shuffle_cards(18, 19, 0.5f);
                yield return new WaitForSeconds(0.5f);
        */

		for (int x = 0; x < shuffles; x++)
		{
			int a = rnd.Next(0, max);
			int b = rnd.Next(0, max);
			while (b == a)
			{
				b = rnd.Next(0, max);
			}
			shuffle_cards(a, b, shuffle_time);
			yield return new WaitForSeconds(0.6f);
		}

		yield return new WaitForSeconds(1f);

		for (int x = 0; x < cards.Count; x++)
		{
			card card_script = cards[x].GetComponent<card>();
			card_script.set_immobile(false);
		}
		timer_script.resume();
		pause_btn_script.set_immobile(false);
		game_stage_changing = false;
		game_stage = "ingame";
	}

	IEnumerator global_wrong_anim()
	{
		// some cards are already immobile, eg. correct pairs, so we note these down to not restore mobility later
		List<int> nonimmobile_cards = new List<int>();
		for (int x = 0; x < cards.Count; x++)
		{
			card card_script = cards[x].GetComponent<card>();
			if (!card_script.set_immobile(true)) // set_immobile returns the original immobility value of the card
			{
				nonimmobile_cards.Add(x); // this adds only the cards that were not originally immobile
			};
		}
		yield return new WaitForSeconds(2.4f); // based on duration of CalculateWrongAnim
		for (int x = 0; x < nonimmobile_cards.Count; x++)
		{
			card card_script = cards[nonimmobile_cards[x]].GetComponent<card>();
			card_script.set_immobile(false); // restores mobility to only the cards that were not originally immobile
		}
	}
}
