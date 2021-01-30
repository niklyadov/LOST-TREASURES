﻿using UnityEngine;
using UnityEngine.UI;

public class OverlayUI : MonoBehaviour
{
    [SerializeField] Text time;
    [SerializeField] Text blueScore;
    [SerializeField] Text redScore;
    [SerializeField] Transform blueScoreBar;
    [SerializeField] Transform CurrentHPBar;
    [SerializeField] GameObject Pause;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause.SetActive(!Pause.activeSelf);
    }

    public void SetScore(int blue, int red)
    {
        blueScore.text = blue.ToString();
        redScore.text = red.ToString();
        blueScoreBar.localScale.Set((float)blue / (blue + red), 1, 1);
    }

    public void SetTime(int seconds)
    {
        time.text = (seconds / 60).ToString() + ":" + (seconds % 60).ToString();
    }

    public void SetHP(int total, int current)
    {
        CurrentHPBar.localScale.Set((float)current / total, 1, 1);
    }

    /// <summary>
    /// Устанавливает HP в процентах [0 .. 1]. Значение <paramref name="rate"/> обрезается по границам [0 .. 1]
    /// </summary>
    /// <param name="rate"></param>
    public void SetHP(float rate)
    {
        CurrentHPBar.localScale.Set(Mathf.Clamp(rate,0,1), 1, 1);
    }
}
