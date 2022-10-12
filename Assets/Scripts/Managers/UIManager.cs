using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private TextMeshProUGUI moveText;
    [SerializeField] private Transform questHolder;
    [SerializeField] private Image progressSlider;
    private List<QuestPanel> questPanels= new List<QuestPanel>();
    public EndGamePanel endGamePanel;

    private void Awake()
    {
        instance = this;
    }

    public void SetupUI(Level level)
    {        
        progressSlider.fillAmount = 0;
        moveText.text = level.maxMove.ToString();
        foreach (Quest quest in level.quests)
        {
            QuestPanel questPanel = Instantiate(GameAssets.instance.questPanel, questHolder).GetComponent<QuestPanel>();
            questPanel.SetupQuest(GameAssets.instance.tileSprites[(int)quest.blockType], quest.targetNumber);
            questPanels.Add(questPanel);
        }
    }

    public void DecreaseMove(int move)
    {
        moveText.text = move.ToString();
    }

    public void UpdateQuestHud(int index,int number)
    {
        questPanels[index].SetupText(number);
    }

    public void UpdateProgressSlider(float current,float max)
    {
        progressSlider.fillAmount = current / max;
    }
}