using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour, IDataPersistence
{
    [SerializeField] private TextMeshProUGUI healthText, coinText;
    private int levelIndex;

    public void LoadData(GameData data)
    {
        healthText.text = data.health.ToString();
        coinText.text = data.coin.ToString();
        levelIndex = data.levelIndex;
    }

    private void Start()
    {
        if (MyGameDatas.instance.gameData != null)
        {
            healthText.text = MyGameDatas.instance.gameData.health.ToString();
            coinText.text = MyGameDatas.instance.gameData.coin.ToString();
            levelIndex = MyGameDatas.instance.gameData.levelIndex;
        }
        Time.timeScale = 1;
    }

    public void SaveData(GameData data)
    {
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("Game");
    }
}