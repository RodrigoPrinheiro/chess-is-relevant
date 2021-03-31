using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HighscoreEntryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _waves;
    [SerializeField] private TextMeshProUGUI _time;

    public void UpdateUI(Highscore score, int position)
    {
        _name.text = $"{position}.{score.name}";
        _waves.text = $"{score.waves}";
        TimeSpan t = TimeSpan.FromSeconds(score.time);
        
        _time.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                        t.Hours,
                        t.Minutes,
                        t.Seconds);
    }
}
