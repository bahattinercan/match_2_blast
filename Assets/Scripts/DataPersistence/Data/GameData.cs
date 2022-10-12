[System.Serializable]
public class GameData
{
    public int health;
    public int coin;
    public int levelIndex;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData()
    {
        health = 5;
        coin = 100;
        levelIndex = 0;
    }
}