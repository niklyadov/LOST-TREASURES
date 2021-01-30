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
}