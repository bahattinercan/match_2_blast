using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlackBomb : MonoBehaviour, IBlock
{
    public GameObject GetObject
    { get { return gameObject; } }
    public BlockType blockType { get; set; }
    public BlockType colorType { get; set; }

    public Vector3 GetTransform
    {
        get { return gameObject.transform.position; }
        set { gameObject.transform.position = value; }
    }

    private void Start()
    {
        blockType = BlockType.bomb;
    }

    public void Activate(GameGrid grid)
    {
        if (grid == null)
            throw new InvalidGridException("Cannot activate block on a null grid object.");

        var toBeDestroyed = new List<IBlock>();
        var queue = new Queue<IBlock>();
        queue.Enqueue(this);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (!toBeDestroyed.Contains(current) && current.blockType == this.blockType)
            {
                toBeDestroyed.Add(current);

                var surroundingBlocks = grid.GetSurroundingBlocks(current, true);
                var surroundingNonBombs = surroundingBlocks.Where(b => b.blockType != BlockType.bomb).ToList();
                var surroundingBombs = surroundingBlocks.Where(b => b.blockType == this.blockType).ToList();

                foreach (var nonBomb in surroundingNonBombs)
                    if (!toBeDestroyed.Contains(nonBomb))
                        toBeDestroyed.Add(nonBomb);

                foreach (var bomb in surroundingBombs)
                    queue.Enqueue(bomb);
            }
        }

        grid.DestroyBlocks(toBeDestroyed);
    }

    public List<IBlock> CheckOtherTiles(GameGrid grid)
    {
        return null;
    }

    public void ChangeBlockSprite(BlockType blockType)
    {
    }

    public void ChangeBlockType(BlockType blockType)
    {
    }

    public bool IsNormalBlock()
    {
        return false;
    }
}