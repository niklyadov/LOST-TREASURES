using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class Match: NetworkBehaviour
    {
        #region Singleton

        private static Match _match;
        public static Match GetInstance()
        {
            if (_match == null)
                _match = new Match();

            return _match;
        }

        #endregion

        public OverlayUI OverlayUI;

        public int MatchDuration = 300;
        public int PauseDuration = 20;
        
        [SyncVar]
        public int Time;
        
        public Team BlueTeam;
        public Team RedTeam;

        [SerializeField]
        private bool _matchStarted;
        
        [SerializeField]
        private bool _pause = true;

        private void Start()
        {
            if (isServer)
            {
                OverlayUI = GameObject.FindGameObjectWithTag("UI").GetComponent<OverlayUI>();
                
                StopAllCoroutines();
                
                Time = PauseDuration;
                _pause = true;
                
                BlueTeam.Score = 0;
                RedTeam.Score = 0;

                // start timer
                StartCoroutine(Countdown(1));
            }
        }

        public void StartMatch()
        {
            _matchStarted = true;
            
            OverlayUI.RpcResetUI();

            SendAllLock(false);
            
            Time = MatchDuration;
            _pause = false;
                
            BlueTeam.Score = 0;
            RedTeam.Score = 0;
        }

        [Command]
        public void CmdJoinTeam(NetworkInstanceId playerId)
        {
            if (BlueTeam.Members.Count <= RedTeam.Members.Count)
                BlueTeam.CmdJoinTeam(playerId);
            else
                RedTeam.CmdJoinTeam(playerId);
            
            OverlayUI.RpcUpdateScore(BlueTeam.Score, RedTeam.Score);
            
            ClientScene.FindLocalObject(playerId).GetComponent<Submarine>().RpcSetLock(true);
        }

        [Command]
        public void CmdLeaveTeam(NetworkInstanceId playerId)
        {
            BlueTeam.Members.RemoveAll(player => player == null || player.netId == playerId);
            RedTeam.Members.RemoveAll(player => player == null || player.netId == playerId);
        }

        [Command]
        public void CmdAddPoints(TeamColor color, int points)
        {
            if (isServer)
            {
                if (color == TeamColor.Blue)
                    BlueTeam.Score += points;
                else if (color == TeamColor.Red)
                    RedTeam.Score += points;
                
                OverlayUI.RpcUpdateScore(BlueTeam.Score, RedTeam.Score);
            }
        }

        private void PauseControl()
        {
            Time--;
                    
            if (Time <= 0)
            {
                _pause = false;
                _matchStarted = false;
                        
                OverlayUI.RpcSendBigMessageAll("Match started!");
            }
            else
            {
                if (Time % 5 == 0)
                {
                    OverlayUI.RpcSendBigMessageAll(Time + "seconds!");
                } else if (Time < 5)
                {
                    OverlayUI.RpcSendBigMessageAll(Time.ToString());
                } 
            }
        }

        private void MatchControl()
        {
            Time--;
                    
            if (Time <= 0)
            {
                var message = "";
                
                if (BlueTeam.Score > RedTeam.Score)
                    message = "Blue won!";
                else if (BlueTeam.Score < RedTeam.Score)
                    message = "Red won!";
                else
                    message = "Draw!";
                
                OverlayUI.RpcSendBigMessageAll($"{message}\nblue : {BlueTeam.Score}, red : {RedTeam.Score}");

                // match stopped
                
                SendAllLock(true);
                
                _matchStarted = false;
                _pause = true;
                        
                Time = PauseDuration;
                        
                BlueTeam.Score = 0;
                RedTeam.Score = 0;
            }
            else
            {
                OverlayUI.RpcUpdateTime(Time);
            }
        }

        private IEnumerator Countdown(float seconds)
        {
            while (true)
            {
                yield return new WaitForSeconds(seconds);


                if (_matchStarted)
                {
                    MatchControl();
                }
                else if (_pause)
                {
                    PauseControl();
                }
                else
                {
                    if (BlueTeam.Members.Count > 0 && RedTeam.Members.Count > 0)
                        StartMatch();
                    else OverlayUI.RpcSendBigMessageAll("1 player needed for start the match");
                }

            }                                                                                                                                                                                                                                                                                                    
        }

        // lock all players (control)
        private void SendAllLock(bool to)
        {
            foreach (var member in BlueTeam.Members)
                member.RpcSetLock(to);

            foreach (var member in RedTeam.Members)
                member.RpcSetLock(to);
        }
    }
}
