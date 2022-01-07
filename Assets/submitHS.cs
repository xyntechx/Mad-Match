using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class submitHS : MonoBehaviour
{
	const string HOST = "https://ohgames.herokuapp.com";

	public GameObject HSname;
    // Start is called before the first frame update
    void Start()
    {
        
    }

	public void sendHS()
	{
		StartCoroutine(GetRequest(HOST + "/api/matching/submitscore?myID=" + PlayerPrefs.GetString("myID")
			+ "&myScore=" + PlayerPrefs.GetFloat("myScore").ToString("0") + "&myName=" + HSname.GetComponent<InputField>().text
			+ "&mode=" + PlayerPrefs.GetInt("lvl").ToString()));
		gameObject.SetActive(false);
		HSname.SetActive(false);

	}

	IEnumerator GetRequest(string uri)
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
					break;
				case UnityWebRequest.Result.ProtocolError:
					Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
					break;
				case UnityWebRequest.Result.Success:
					Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
					// HS submitted successfully
					break;
			}
		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
