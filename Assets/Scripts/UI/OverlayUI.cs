﻿using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OverlayUI : NetworkBehaviour
{
    [SerializeField] Text bigMessage;
    [SerializeField] Text time;
    [SerializeField] Text blueScore;
    [SerializeField] Text redScore;
    [SerializeField] RectTransform blueScoreBar;
    [SerializeField] RectTransform CurrentHPBar;
    [SerializeField] GameObject Pause;
    [SerializeField] private Image actionImage;

    Vector3 _blueScale;
    Vector3 _hpScale;

    //private int _blueScoreDelta = 0;
    //private int _redScoreDelta = 0;
    
    //private int _blueScore = 0;
    //private int _redScore = 0;

    //private float _numberUpdateTime = 0.005f;
    //private float _numberUpdateDelta = 0;

    private void Start()
    {
        Reset();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause.SetActive(!Pause.activeSelf);

        /*
        if (_numberUpdateDelta >= _numberUpdateTime)
        {
            _numberUpdateDelta = 0;

            if (_blueScoreDelta < _blueScore)
                _blueScoreDelta++;
            
            if (_redScoreDelta < _redScore)
                _redScoreDelta++;

            blueScore.text = _blueScoreDelta.ToString();
            redScore.text = _redScoreDelta.ToString();
        }
        else _numberUpdateDelta += Time.deltaTime;*/
    }

    public void SetScore(int blue, int red)
    {
        //_blueScore = blue;
        //_redScore = red;

        blueScore.text = blue.ToString();
        redScore.text = red.ToString();
        
        if (blue + red == 0)
            _blueScale.x = 0.5f;
        else
            _blueScale.x = blue / (float)(blue + red);
        blueScoreBar.localScale = _blueScale;
    }

    public void SetTime(int seconds)
    {
        time.text = (seconds / 60).ToString() + ":" + (seconds % 60).ToString();
    }

    public void SetActionPercentage(float percentage)
    {
        actionImage.fillAmount = percentage;
    }
    
    public void SetHP(float total, float current)
    {
        _hpScale.x = current / total;
        CurrentHPBar.localScale = _hpScale;
    }

    /// <summary>
    /// Устанавливает HP в процентах [0 .. 1]. Значение <paramref name="rate"/> обрезается по границам [0 .. 1]
    /// </summary>
    /// <param name="rate"></param>
    public void SetHP(float rate)
    {
        _hpScale.x = Mathf.Clamp(rate, 0, 1);
        CurrentHPBar.localScale = _hpScale;
    }

    public void DisplayBigMessage(string text)
    {
        bigMessage.text = text;
        bigMessage.gameObject.SetActive(true);
    }

    public void Reset()
    {
        blueScore.text = "0";
        redScore.text = "0";
        time.text = "00:00";
        _blueScale = Vector3.one;
        _hpScale = Vector3.one;
        SetScore(0, 0);
    }

    [ClientRpc]
    public void RpcUpdateScore(int blueScore, int redScore)
    {
        SetScore(blueScore, redScore);
    }

    [ClientRpc]
    public void RpcSendBigMessageAll(string message)
    {
        DisplayBigMessage(message);
    }
    
    
    [ClientRpc]
    public void RpcUpdateTime(int time)
    {
        SetTime(time);
    }
    
    [ClientRpc]
    public void RpcResetUI()
    {
        Reset();
    }
}
