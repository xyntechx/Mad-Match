using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class populatepairings : MonoBehaviour
{
    //public GameObject cardPrefab;
    public RectTransform content;

    // Start is called before the first frame update
    void Start()
    {
        //content = gameObject.GetComponent<RectTransform>();
        print(content);
        Texture2D texture = (Texture2D)Resources.Load("mycard/fronts/1_image");
        GameObject imageObject = new GameObject("Image");
        imageObject.transform.SetParent(content);
        imageObject.AddComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, 1f, 2.5f), Vector2.zero);

        //ScrollRect scrollrect = gameObject.GetComponent<ScrollRect>();
        //GameObject c = Instantiate(cardPrefab, new Vector3(0, 12, 0), Quaternion.identity) as GameObject;
        //RectTransform rectTransform = c.GetComponent<RectTransform>();
        //rectTransform.parent = scrollrect.content;
        ////scrollrect.flexibleHeight += 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
