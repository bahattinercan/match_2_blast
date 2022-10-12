using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Level level;
    private float maxScore = 0;
    private float score = 0;
    [SerializeField] private GameLogic gameLogic;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        level = new Level(MyGameDatas.instance.GetLevel());
        UIManager.instance.SetupUI(level);
        foreach (Quest quest in level.quests)
        {
            maxScore += (quest.targetNumber * level.scoreMultiplier);
        }
        maxScore *= 2;
    }

    public void DecreaseMove()
    {
        level.maxMove--;
        UIManager.instance.DecreaseMove(level.maxMove);
    }

    public bool HasMoveChance()
    {
        if (level.maxMove >= 1)
        {
            return true;
        }
        return false;
    }

    public void UpdateHud(IBlock block)
    {
        for (int i = 0; i < level.quests.Count; i++)
        {
            Quest quest = level.quests[i];
            Vector3 vfxSpawnPos = new Vector3(block.GetTransform.x, block.GetTransform.y,5);
            Instantiate(GameAssets.instance.destroyVFX, vfxSpawnPos, Quaternion.identity);
            if (block.blockType == quest.blockType)
            {
                level.quests[i].targetNumber--;
                if (level.quests[i].targetNumber < 0)
                    level.quests[i].targetNumber = 0;

                UIManager.instance.UpdateQuestHud(i, level.quests[i].targetNumber);
            }
        }
        score += level.scoreMultiplier;
        UIManager.instance.UpdateProgressSlider(score, maxScore);
    }

    public void CheckGameWinLose()
    {
        int completedQuests = 0;
        for (int i = 0; i < level.quests.Count; i++)
        {
            Quest quest = level.quests[i];
            if (quest.targetNumber == 0)
                completedQuests++;
        }
        if (completedQuests == level.quests.Count)
        {
            // game win
            FinishTheGame(true, completedQuests);
        }
        else if (completedQuests != level.quests.Count && level.maxMove <= 0)
        {
            // game over
            FinishTheGame(false, completedQuests);
        }
    }

    private void FinishTheGame(bool win, int completedQuests)
    {
        int coin = (int)(score / 7.5);
        MyGameDatas.instance.gameData.coin += coin;
        gameLogic.canInteract = false;
        UIManager.instance.endGamePanel.gameObject.SetActive(true);
        UIManager.instance.endGamePanel.SetupPanel(completedQuests, score, coin, win);
        if (win)
            AudioManager.instance.PlaySFX(EUISoundType.win, false);
        else
            AudioManager.instance.PlaySFX(EUISoundType.lose, false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void NextLevel()
    {
        MyGameDatas.instance.gameData.levelIndex++;
        if (MyGameDatas.instance.gameData.levelIndex >= MyGameDatas.instance.levelList.levels.Count)
            MyGameDatas.instance.gameData.levelIndex = 0;
        SceneManager.LoadScene("Game");
    }
}