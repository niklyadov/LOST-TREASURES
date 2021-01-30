using UnityEngine;

public class Player
{
    #region Singleton
    
    private static Player _player;
    public static Player GetCurrentPlayer()
    {
        if (_player == null) 
            _player = new Player();

        return _player;
    }
    
    #endregion
}