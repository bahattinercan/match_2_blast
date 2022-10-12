using UnityEngine;

public class MyGameDatas : MonoBehaviour, IDataPersistence
{
    public static MyGameDatas instance;
    public GameData gameData;
    public LevelListSO levelList;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void LoadData(GameData data)
    {
        gameData = data;
    }

    public void SaveData(GameData data)
    {
        data.health = gameData.health;
        data.coin = gameData.coin;
        data.levelIndex = gameData.levelIndex;
    }

    public LevelSO GetLevel()
    {
        return levelList.levels[gameData.levelIndex];
    }
}