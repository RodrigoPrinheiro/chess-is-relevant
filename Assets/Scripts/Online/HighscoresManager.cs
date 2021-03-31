using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HighscoresManager : MonoBehaviour
{
    const string privateCode = "jTBh7rRfXEO9DJFovGcWJAZSuZFYZygUOuVwqDsFK4WQ";
    const string publicCode = "60648cf18f421366b058e41a";
    const string webURL = "http://dreamlo.com/lb/";

    private List<Highscore> _highscores;

    public void AddNewScore(string name, int score, float time)
    {
        StartCoroutine(UploadHighscore(name, score, time));
    }

    public List<Highscore> GetHighscores()
    {
        StartCoroutine(DownloadScores());

        return _highscores;
    }

    private IEnumerator UploadHighscore(string name, int score, float time)
    {
        UnityWebRequest www = new UnityWebRequest(webURL + privateCode + "/add/" + WWW.EscapeURL(name) + "/" + score + "/" + time);
        yield return www.SendWebRequest();

        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log($"New Score success: {name} with {score}");
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    private IEnumerator DownloadScores()
    {
        WWW www = new WWW(webURL + publicCode + "/pipe/");
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            FormatHighscores(www.text);
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    private void FormatHighscores(string text)
    {
        string[] entries = text.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
        _highscores = new List<Highscore>(entries.Length);
        for (int i = 0; i < entries.Length; i++)
        {
            string[] info = entries[i].Split('|');
            string name = info[0];
            int score = int.Parse(info[1]);
            float time = float.Parse(info[2]);
            
            Highscore entry;
            entry.name = name;
            entry.waves = score;
            entry.time = time;

            _highscores.Add(entry);

            Debug.Log($"{_highscores[i].name} : {_highscores[i].waves} : {_highscores[i].time}");
        }
    }
}

public struct Highscore
{
    public string name;
    public int waves;
    public float time;
}