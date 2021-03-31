using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DeadScreenUI : MonoBehaviour
{
    [SerializeField] private RectTransform _text;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _wavesText;
    private CanvasGroup _group;
    private WaveManager _waves;

    private bool _open;

    private void Awake()
    {
        _group = GetComponent<CanvasGroup>();
        _waves = FindObjectOfType<WaveManager>();
        _group.alpha = 0f;
    }

    private void OnEnable()
    {
        GameManager.gameEndEvent += OpenUI;
    }

    private void OnDisable()
    {
        GameManager.gameEndEvent -= OpenUI;
    }

    private void OpenUI()
    {
        _open = true;
        _group.LeanAlpha(1f, 1f);

        _wavesText.text = _waves.Waves.ToString();
        TimeSpan t = TimeSpan.FromSeconds(_waves.GameTime);

        _timeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                        t.Hours,
                        t.Minutes,
                        t.Seconds);
    
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private IEnumerator Loop()
    {
        bool loop = true;
        Vector3 end = (Vector3.one * 1.5f);
        while (true)
        {
            if (_text.localScale.magnitude >= end.magnitude)
                loop = !loop;

            if (loop)
            {
                _text.localScale = Vector3.Lerp(Vector3.one, end, Time.deltaTime * 3f);
            }
            else
            {
                _text.localScale = Vector3.Lerp(end, Vector3.one, Time.deltaTime * 3f);
            }
            yield return null;
        }
    }
}
