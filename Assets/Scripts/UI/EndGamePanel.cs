using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private Image[] stars;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private Button nextLevelButton;

    public void SetupPanel(int star,float score, int coin,bool win)
    {
        for (int i = 0; i < star; i++)
        {
            stars[i].color = Color.white;
        }
        scoreText.text = ((int)score).ToString();
        coinText.text = coin.ToString();
        if (!win)
            nextLevelButton.interactable = false;
    }
}