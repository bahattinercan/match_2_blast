using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class QuestPanel : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI text;

    public void SetupQuest(Sprite sprite,int number)
    {
        image.sprite = sprite;
        text.text = number.ToString();
    }

    public void SetupText(int number)
    {
        text.text = number.ToString();
    }
}