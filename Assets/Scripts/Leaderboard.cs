using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    GameObject[] scores = new GameObject[9]; // change 3 to total number of scores shown

    void Awake()
    {
        for (int i = 1; i <= scores.Length; i++) {
            string name = i + "_place";
            scores[i-1] = GameObject.Find(name);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        getScores();
    }

    void getScores()
    {
        List<KeyValuePair<string, int>> data = new List<KeyValuePair<string, int>>();
        // Faek data - request to server should go here
        data.Add(new KeyValuePair<string, int>("A", 9001));
        data.Add(new KeyValuePair<string, int>("B", 7554));
        data.Add(new KeyValuePair<string, int>("C", 3441));
        data.Sort((x, y) => y.Value.CompareTo(x.Value));

        int idx = 0;
        foreach (GameObject score in scores) {
            KeyValuePair<string, int> kvp = data[idx];
            score.GetComponent<Text>().text = $"{kvp.Key}\n{kvp.Value}";
            idx++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
