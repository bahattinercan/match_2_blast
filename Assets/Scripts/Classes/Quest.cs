[System.Serializable]
public class Quest
{
    public int targetNumber;
    public BlockType blockType;

    public Quest(int targetNumber,BlockType blockType)
    {
        this.targetNumber = targetNumber;
        this.blockType = blockType;
    }
}
