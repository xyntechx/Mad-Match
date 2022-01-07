using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedRun : MonoBehaviour
{
    public bool speedrunMode = false;
    public Button SpeedrunBtn;

    // Start is called before the first frame update
    void Start()
    {
        SpeedrunBtn.GetComponent<Button>().onClick.AddListener(toggleSpeedrunMode);
    }

    // Setters
    public void setSpeedrunMode(bool val) {
        speedrunMode = val;
    }

    // Speedrunmode state toggler
    public void toggleSpeedrunMode() {
        setSpeedrunMode(!speedrunMode);
        if (speedrunMode)
		{
            SpeedrunBtn.GetComponent<Button>().GetComponentInChildren<Text>().text = "Return to normal mode";
        } else
		{
            SpeedrunBtn.GetComponent<Button>().GetComponentInChildren<Text>().text = "Speedrun Mode";
        }
    }
}
