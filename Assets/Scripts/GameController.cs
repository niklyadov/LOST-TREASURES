using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController
{
    #region Singleton
    
    private static GameController _instance;
    public static GameController GetInstance()
    {
        if (_instance == null) 
            _instance = new GameController();

        return _instance;
    }
    
    #endregion
    
    public OverlayUI OverlayUi;

    public List<Submarine> BlueTeam = new List<Submarine>();
    public List<Submarine> RedTeam = new List<Submarine>();

    private int _redTeamScore;
    public int RedTeamScore
    {
        get
        {
            return _redTeamScore;
        }
        set
        {
            _redTeamScore = value;
            OverlayUi.SetScore(_blueTeamScore, _redTeamScore);
        }
    }
    
    private int _blueTeamScore;
    public int BlueTeamScore
    {
        get
        {
            return _blueTeamScore;
        }
        set
        {
            _redTeamScore = value;
            OverlayUi.SetScore(_blueTeamScore, _redTeamScore);
        }
    }
    
    private bool _matchStarted;
    public bool MatchStarted
    {
        get => _matchStarted;
    }

    public int MatchSeconds = 300;

    public void Reset()
    {
        BlueTeam = new List<Submarine>();
        RedTeam = new List<Submarine>();
        OverlayUi = null;
    }

    public Team PlayerJoin(Submarine submarine)
    {
        var team = Team.Blue;
        if (BlueTeam.Count >= RedTeam.Count)
        {
            BlueTeam.Add(submarine);
            team = Team.Blue;
        }
        else
        {
            RedTeam.Add(submarine);
            team = Team.Red;
        }

        return team;
    }

    public void PlayerLeave(Team team, Submarine submarine)
    {
        if (team == Team.Blue)
            BlueTeam.Remove(submarine);
        
        if (team == Team.Red)
            RedTeam.Remove(submarine);
    }

    public void StartMatch()
    {
        _matchStarted = true;
    }
}