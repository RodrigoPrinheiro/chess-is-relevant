using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private HighscoreEntryUI[] _entries;
    private HighscoresManager _manager;
    List<Highscore> highscores;
    private void Awake() 
    {
        _manager = FindObjectOfType<HighscoresManager>();
        highscores = _manager.GetHighscores();
    }

    private void OnDisable() 
    {
        highscores = _manager.GetHighscores();
    }

    public void UpdateHighscores()
    {
        if (highscores == null) return;
        for (int i = 0; i < _entries.Length; i++)
        {
            if (i > highscores.Count - 1) break;

            _entries[i].UpdateUI(highscores[i], i);
        }
    }
}
