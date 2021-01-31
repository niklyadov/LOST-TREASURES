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

        public int Duration;
        public int Time;
        public Team BlueTeam;
        public Team RedTeam;

        private bool _matchStarted;

        [Command]
        public void CmdJoinTeam(NetworkInstanceId playerId)
        {
            if (BlueTeam.Members.Count <= RedTeam.Members.Count)
                BlueTeam.CmdJoinTeam(playerId);
            else
                RedTeam.CmdJoinTeam(playerId);
            GameObject.FindGameObjectWithTag("UI").GetComponent<OverlayUI>().RpcUpdateScore(BlueTeam.Score, RedTeam.Score);
        }

        [Command]
        public void CmdLeaveTeam(NetworkInstanceId playerId)
        {
            var player = ClientScene.FindLocalObject(playerId).GetComponent<Submarine>();
            BlueTeam.Members.Remove(player);
            RedTeam.Members.Remove(player);
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
                GameObject.FindGameObjectWithTag("UI").GetComponent<OverlayUI>().RpcUpdateScore(BlueTeam.Score, RedTeam.Score);
            }
        }

        public void StartMatch()
        {
            _matchStarted = true;
        }

        private IEnumerator Countdown(float seconds)
        {
            while (true)
            {
                yield return new WaitForSeconds(seconds);

                if (_matchStarted)
                {
                    if (Time == 0)
                    {
                        // match stopped
                    }

                    Time--;
                }
                else
                {
                    if (BlueTeam.Members.Count > 0 && RedTeam.Members.Count > 0)
                    {
                        StartMatch();
                    }
                    else
                    {
                        //Text = недостаточно игроков
                    }
                }
            }
        }
    }
}
