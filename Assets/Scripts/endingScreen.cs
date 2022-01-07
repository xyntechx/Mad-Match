using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;

public class endingScreen : MonoBehaviour
{
	const string HOST = "https://ohgames.herokuapp.com";

	public Button PlayAgain;
	public Button MainMenu;
	public Text DispScore;
	public Text ScoreText;
	public GameObject LBEntryPrefab;

	public GameObject HSName;
	public GameObject HSSubmit;

	GameObject LBGrid;
	List<LBEntry> LBEntries;

	struct LBEntry
	{
		public string name;
		public string userid;
		public int score;
	}

	// Start is called before the first frame update
	void Awake()
	{
		// for testing
		//PlayerPrefs.SetFloat("GameScore", 4000f);
		//PlayerPrefs.SetFloat("myScore", 4000f);
		//PlayerPrefs.SetFloat("myHighscore", 1000f);
		//PlayerPrefs.SetString("myID", "5555");
		string slvl = DataController.LevelSelected.ToString();
		LBGrid = GameObject.Find("LBGrid");
		LBEntries = new List<LBEntry>();
		StartCoroutine(GetRequest(HOST + "/api/jigsaw/getlb"
			+ "?mode=" + PlayerPrefs.GetInt("songchoice").ToString(), "GetLBInfo"));
		if (PlayerPrefs.GetFloat("myScore") > PlayerPrefs.GetFloat("myHighscore" + slvl))
		{
			// new high score
			PlayerPrefs.SetFloat("myHighscore" + slvl, PlayerPrefs.GetFloat("myScore"));
			PlayerPrefs.Save();
			StartCoroutine(GetRequest(HOST + "/api/jigsaw/getrank?myScore="
				+ PlayerPrefs.GetFloat("myHighscore" + slvl).ToString("0")
				+ "&mode=" + PlayerPrefs.GetInt("songchoice").ToString(), "GetRank", "new highscore"));
			HSName.SetActive(true);
			HSSubmit.SetActive(true);
			ScoreText.text = "NEW HIGHSCORE!!!";
		}
		else
		{
			StartCoroutine(GetRequest(HOST + "/api/jigsaw/getrank?myScore="
				+ PlayerPrefs.GetFloat("myScore").ToString("0")
				+ "&mode=" + PlayerPrefs.GetInt("songchoice").ToString(), "GetRank", "score"));
			StartCoroutine(GetRequest(HOST + "/api/jigsaw/getrank?myScore="
				+ PlayerPrefs.GetFloat("myHighscore" + slvl).ToString("0")
				+ "&mode=" + PlayerPrefs.GetInt("songchoice").ToString(), "GetRank", "highscore"));
		}

		//StartCoroutine(GetRequest(HOST + "/jigsaw/submitscore?myID=" + PlayerPrefs.GetString("myID") 
		//	+ "&myScore=" + PlayerPrefs.GetFloat("myScore").ToString("0"), "GetRank"));
	}

	void Start()
	{
		Button restartBtn = PlayAgain.GetComponent<Button>();
		restartBtn.onClick.AddListener(restartGame);
		Button exitBtn = MainMenu.GetComponent<Button>();
		exitBtn.onClick.AddListener(exitGame);
		DispScore.text = PlayerPrefs.GetFloat("myScore").ToString("0");
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void restartGame()
	{
		SceneManager.LoadScene("SampleScene");
	}

	public void exitGame()
	{
		SceneManager.LoadScene("LandingScreen");
	}

	IEnumerator GetRequest(string uri, string type, string arg1 = "", string arg2 = "")
	{
		using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
		{
			// Request and wait for the desired page.
			yield return webRequest.SendWebRequest();

			string[] pages = uri.Split('/');
			int page = pages.Length - 1;

			switch (webRequest.result)
			{
				case UnityWebRequest.Result.ConnectionError:
				case UnityWebRequest.Result.DataProcessingError:
					Debug.LogError(pages[page] + ": Error: " + webRequest.error);
					{
						GameObject b = Instantiate(LBEntryPrefab) as GameObject;
						b.transform.parent = LBGrid.transform;
						setEntry entry_script = b.GetComponent<setEntry>();
						entry_script.seterror("Error fetching the leaderboard.");
					}
					break;
				case UnityWebRequest.Result.ProtocolError:
					Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
					{
						GameObject b = Instantiate(LBEntryPrefab) as GameObject;
						b.transform.parent = LBGrid.transform;
						setEntry entry_script = b.GetComponent<setEntry>();
						entry_script.seterror("Error fetching the leaderboard.");
					}
					break;
				case UnityWebRequest.Result.Success:
					Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
					string res = webRequest.downloadHandler.text;
					if (type == "GetLBInfo")
					{
						string[] separated = res.Split('+');
						for (int x = 0; x < separated.Length / 3; x++)
						{
							LBEntry entry = new LBEntry();
							entry.name = separated[3 * x];
							entry.userid = separated[3 * x + 1];
							entry.score = Int32.Parse(separated[3 * x + 2]);
							LBEntries.Add(entry);
						}
						for (int x = 0; x < LBEntries.Count; x++)
						{
							GameObject b = Instantiate(LBEntryPrefab) as GameObject;
							b.transform.parent = LBGrid.transform;
							setEntry entry_script = b.GetComponent<setEntry>();
							entry_script.set(LBEntries[x].name, LBEntries[x].userid, LBEntries[x].score, "", x + 1);
						}
					}
					else if (type == "GetRank")
					{

						if (arg1 == "score" || arg1 == "highscore" || arg1 == "new highscore")
						{
							int myScoreRank = Int32.Parse(res);
							// update rank for score
							GameObject b = Instantiate(LBEntryPrefab) as GameObject;
							b.transform.parent = LBGrid.transform;
							b.transform.SetSiblingIndex(1);
							setEntry entry_script = b.GetComponent<setEntry>();
							entry_script.set("", PlayerPrefs.GetString("myID"),
								(int)PlayerPrefs.GetFloat("myScore"), arg1, myScoreRank);
						}
						//else if (arg1 == "highscore")
						//{
						//	int myHighscoreRank = Int32.Parse(res);
						//	// update rank for highscore
						//	GameObject b = Instantiate(LBEntryPrefab) as GameObject;
						//	b.transform.parent = LBGrid.transform;
						//	b.transform.SetSiblingIndex(0);
						//	setEntry entry_script = b.GetComponent<setEntry>();
						//	entry_script.set("", PlayerPrefs.GetString("myID"),
						//		(int)PlayerPrefs.GetFloat("myHighscore"), "highscore", myHighscoreRank);
						//}
					}

					break;
			}
		}
	}

}
