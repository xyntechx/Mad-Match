using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject reviewPanel;
    [SerializeField] GameObject scorePanel;

    public void ToggleReviewPanel()
    {
        reviewPanel.SetActive(!reviewPanel.activeSelf);
    }

    public void ToggleScorePanel()
    {
        scorePanel.SetActive(!scorePanel.activeSelf);
    }
}
