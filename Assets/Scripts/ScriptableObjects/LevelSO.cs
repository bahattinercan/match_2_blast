using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Level")]
public class LevelSO : ScriptableObject
{
    public int width, height;
    public List<Quest> quests;
    public int maxMove;
    [Range(0, 6)] public int maxBlockType;
    public float scoreMultiplier;
}

[System.Serializable]
public struct Level
{
    public int width, height;
    public List<Quest> quests;
    public int maxMove;
    [Range(0, 6)] public int maxBlockType;
    public float scoreMultiplier;
    public Level(LevelSO level)
    {
        width = level.width;
        height = level.height;
        quests = new List<Quest>();
        foreach (Quest quest in level.quests)
        {
            Quest newQuest = new Quest(quest.targetNumber,quest.blockType);
            quests.Add(newQuest);
        }
        maxMove = level.maxMove;
        maxBlockType = level.maxBlockType;
        scoreMultiplier = level.scoreMultiplier;
    }
}