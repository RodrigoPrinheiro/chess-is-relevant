using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndWaveText : MonoBehaviour
{
    [SerializeField] private string _initialWaveText = "become#relevant";
    [SerializeField] private string[] _endWaveTexts;
    [SerializeField, Range(1f, 4f)] private float _defaultTextTime;
    [SerializeField] private TextMeshProUGUI _endWaveCenterText;
    [SerializeField] private TextMeshProUGUI _waveText;
    private WaveManager _waves;

    private void Awake() {
        _waves = FindObjectOfType<WaveManager>();
        _endWaveCenterText.gameObject.SetActive(false);
        _waveText.gameObject.SetActive(false);
    }

    private void OnEnable() {
        _waves.newWaveEvent += ShowTextAnimation;
    }

    private void OnDisable() 
    {
        _waves.newWaveEvent += ShowTextAnimation;
    }

    private void Start() {
        ShowTextAnimation(1);
    }

    public void ShowTextAnimation(int wave)
    {
        _waveText.gameObject.SetActive(true);
        _endWaveCenterText.gameObject.SetActive(true);

        _waveText.text = $"Wave {wave}";
        string[] textGroup;
        if (wave == 1)
        {
            textGroup = _initialWaveText.Split('#');
        }
        else
            textGroup = _endWaveTexts[Random.Range(0, _endWaveTexts.Length)].Split('#');
        
        StartCoroutine(TextAnimation(textGroup));
        
    }

    //! WARNING, ENTERING SPAGHETTI CODE SECTION
    private IEnumerator TextAnimation(string[] textGroup)
    {
        LeanTween.alphaCanvas(_waveText.GetComponent<CanvasGroup>(), 1f, 0.8f);

        // Pick Random Text
        // Time per text line
        float time = _defaultTextTime / textGroup.Length; // Get a time per line so it matches default time
        WaitForSeconds waitTime = new WaitForSeconds(time);
        for (int i = 0; i < textGroup.Length; i++)
        {
            _endWaveCenterText.rectTransform.localScale = Vector3.one;
            LeanTween.alphaCanvas(_endWaveCenterText.GetComponent<CanvasGroup>(), 1f, 0f);
            _endWaveCenterText.text = textGroup[i];
            // Per new line
            _endWaveCenterText.rectTransform.LeanScale(Vector3.one * 3f, time);
            LeanTween.alphaCanvas(_endWaveCenterText.GetComponent<CanvasGroup>(), 0f, time);
            yield return waitTime;
        }

        LeanTween.alphaCanvas(_waveText.GetComponent<CanvasGroup>(), 0f, 0.8f).
            setOnComplete(() => _waveText.gameObject.SetActive(false));

        _endWaveCenterText.gameObject.SetActive(false);
    }
}
