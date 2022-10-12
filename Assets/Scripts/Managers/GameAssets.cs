using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets instance;
    public Sprite[] tileSprites;
    public Sprite[] rocketSprites;
    public Sprite[] bombSprites;
    public Sprite[] colorSprites;
    public Sprite rocketVerticalSprite;
    public Sprite rocketHorizontalSprite;
    public Sprite bombSprite;
    public Sprite colorBombSprite;
    public GameObject questPanel;


    private void Awake()
    {
        instance = this;
    }
}