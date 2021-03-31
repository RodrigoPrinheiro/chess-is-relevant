using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreCreator : MonoBehaviour
{
    private WaveManager _waves;
    private HighscoresManager _highscores;

    private void Awake() 
    {
        _waves = FindObjectOfType<WaveManager>();
        _highscores = GetComponent<HighscoresManager>();
    }

    private void OnEnable() 
    {
        GameManager.gameEndEvent += AddScore;
    }

    private void OnDisable()
    {
        GameManager.gameEndEvent -= AddScore;
    }

    private void AddScore()
    {
        if (string.IsNullOrEmpty(GameManager.Instance.PlayerName)) return;
        _highscores.AddNewScore(GameManager.Instance.PlayerName, _waves.Waves, _waves.GameTime);
    }
}
