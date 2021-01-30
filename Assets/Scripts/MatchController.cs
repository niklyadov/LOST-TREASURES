using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchController : MonoBehaviour
{
    private GameController _controller;
    
    void Start()
    {
        var _controller = GameController.GetInstance();
        
        StartCoroutine(Countdown(1));
    }

    private IEnumerator Countdown(float seconds) {
        while(true) {
            yield return new WaitForSeconds(seconds);

            if (_controller.MatchStarted)
            {
                if (_controller.MatchSeconds == 0)
                {
                    // match stopped
                }
                
                _controller.MatchSeconds--;
            }
            else
            {
                if (_controller.BlueTeam.Count > 0 && _controller.RedTeam.Count > 0)
                {
                    _controller.StartMatch();
                }
            }
        }
    }
}