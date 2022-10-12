using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Square : MonoBehaviour, IBlock
{
    public GameObject GetObject
    { get { return gameObject; } }

    public BlockType blockType { get; set; }
    public BlockType colorType { get; set; }
    [SerializeField] private SpriteRenderer spriteRenderer;

    public Vector3 GetTransform
    {
        get { return gameObject.transform.position; }
        set { gameObject.transform.position = value; }
    }

    private void Start()
    {
        blockType = (BlockType)Random.Range(0, GameManager.instance.level.maxBlockType);
        colorType = blockType;
        spriteRenderer.sprite = GameAssets.instance.tileSprites[(int)blockType];
        InvokeRepeating("SetSortingOrder", .1f, .1f);
    }

    private void SetSortingOrder()
    {
        spriteRenderer.sortingOrder = (int)(transform.position.y * 10f);
    }

    public void Activate(GameGrid grid)
    {
        if (GameManager.instance.HasMoveChance())
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

                    var surroundingBlocks = grid.GetSurroundingBlocks(current, false);

                    if (IsNormalBlock())
                    {
                        foreach (var block in surroundingBlocks)
                            queue.Enqueue(block);
                    }
                    else if (!IsNormalBlock())
                    {
                        var surroundingNonBombs = surroundingBlocks.Where(b => b.IsNormalBlock()).ToList();
                        var surroundingHorizontalBombs = surroundingBlocks.Where(b => b.blockType == BlockType.rocketHorizontal).ToList();
                        var surroundingVerticalBombs = surroundingBlocks.Where(b => b.blockType == BlockType.rocketVertical).ToList();
                        var surroundingColorBombs = surroundingBlocks.Where(b => b.blockType == BlockType.color).ToList();
                        var surroundingBombs = surroundingBlocks.Where(b => b.blockType == BlockType.bomb).ToList();

                        foreach (var nonBomb in surroundingNonBombs)
                            if (!toBeDestroyed.Contains(nonBomb))
                                toBeDestroyed.Add(nonBomb);

                        foreach (var bomb in surroundingHorizontalBombs)
                            queue.Enqueue(bomb);
                        foreach (var bomb in surroundingVerticalBombs)
                            queue.Enqueue(bomb);
                        foreach (var bomb in surroundingColorBombs)
                            queue.Enqueue(bomb);
                        foreach (var bomb in surroundingBombs)
                            queue.Enqueue(bomb);
                    }
                }
            }
            // minimum 2 block can be blast
            // check for matched block
            //GameManager.instance.DecreaseMove();
            if (toBeDestroyed.Count <= 1)
                AudioManager.instance.PlaySFX(EGameSoundType.cubePressError);

            if (toBeDestroyed.Count > 1 && toBeDestroyed.Count <= 4 && IsNormalBlock())
            {
                // normal
                GameManager.instance.DecreaseMove();
                AudioManager.instance.PlaySFX(EGameSoundType.cubePress);
                grid.DestroyBlocks(toBeDestroyed);
                grid.CheckForSpriteTypes();

            }
            else if (toBeDestroyed.Count > 4 && toBeDestroyed.Count < 8 && IsNormalBlock())
            {
                // rocket
                GameManager.instance.DecreaseMove();
                AudioManager.instance.PlaySFX(EGameSoundType.cubePress);
                toBeDestroyed.Remove(this);
                if (Random.Range(0, 101) < 50)
                    ChangeBlockType(BlockType.rocketVertical);
                else
                    ChangeBlockType(BlockType.rocketHorizontal);

                grid.DestroyBlocks(toBeDestroyed);
                grid.CheckForSpriteTypes();
            }
            else if (toBeDestroyed.Count > 7 && toBeDestroyed.Count < 10 && IsNormalBlock())
            {
                // bomb
                GameManager.instance.DecreaseMove();
                AudioManager.instance.PlaySFX(EGameSoundType.cubePress);
                toBeDestroyed.Remove(this);
                ChangeBlockType(BlockType.bomb);
                grid.DestroyBlocks(toBeDestroyed);
                grid.CheckForSpriteTypes();
                
            }
            else if (toBeDestroyed.Count >= 10 && IsNormalBlock())
            {
                // color
                GameManager.instance.DecreaseMove();
                AudioManager.instance.PlaySFX(EGameSoundType.cubePress);
                toBeDestroyed.Remove(this);
                ChangeBlockType(BlockType.color);
                grid.DestroyBlocks(toBeDestroyed);
                grid.CheckForSpriteTypes();
                
            }
            else if (!IsNormalBlock())
            {
                GameManager.instance.DecreaseMove();
                switch (blockType)
                {
                    case BlockType.rocketVertical:
                        AudioManager.instance.PlaySFX(EGameSoundType.bomb);
                        break;
                    case BlockType.rocketHorizontal:
                        AudioManager.instance.PlaySFX(EGameSoundType.bomb);
                        break;
                    case BlockType.bomb:
                        AudioManager.instance.PlaySFX(EGameSoundType.dynamite);
                        break;
                    case BlockType.color:
                        AudioManager.instance.PlaySFX(EGameSoundType.colorBomb);
                        break;
                    default:
                        break;
                }
                
                grid.DestroyBlocks(toBeDestroyed);
                grid.CheckForSpriteTypes();
            }
        }
        else
        {
            GameManager.instance.CheckGameWinLose();
        }
    }

    public List<IBlock> CheckOtherTiles(GameGrid grid)
    {
        if (grid == null)
            throw new InvalidGridException("Cannot activate block on a null grid object.");
        var sameTypeBlocks = new List<IBlock>();
        var queue = new Queue<IBlock>();
        queue.Enqueue(this);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (!sameTypeBlocks.Contains(current) && current.blockType == this.blockType)
            {
                sameTypeBlocks.Add(current);

                var surroundingBlocks = grid.GetSurroundingBlocks(current, false);
                foreach (var block in surroundingBlocks)
                    queue.Enqueue(block);
            }
        }
        if (blockType != BlockType.bomb || blockType != BlockType.rocketHorizontal || blockType != BlockType.rocketVertical || blockType != BlockType.color)
        {
            // check for matched block
            if (sameTypeBlocks.Count <= 4 && IsNormalBlock())
            {
                foreach (IBlock block in sameTypeBlocks)
                {
                    block.ChangeBlockSprite(blockType);
                }
            }
            else if (sameTypeBlocks.Count > 4 && sameTypeBlocks.Count < 8 && IsNormalBlock())
            {
                foreach (IBlock block in sameTypeBlocks)
                {
                    block.ChangeBlockSprite(BlockType.rocketVertical);
                }
            }
            else if (sameTypeBlocks.Count > 7 && sameTypeBlocks.Count < 10 && IsNormalBlock())
            {
                foreach (IBlock block in sameTypeBlocks)
                {
                    block.ChangeBlockSprite(BlockType.bomb);
                }
            }
            else if (sameTypeBlocks.Count >= 10 && IsNormalBlock())
            {
                foreach (IBlock block in sameTypeBlocks)
                {
                    block.ChangeBlockSprite(BlockType.color);
                }
            }
        }
        return sameTypeBlocks;
    }

    public void ChangeBlockSprite(BlockType blockType)
    {
        switch (blockType)
        {
            case BlockType.red:
            case BlockType.blue:
            case BlockType.yellow:
            case BlockType.green:
            case BlockType.purple:
            case BlockType.pink:
                spriteRenderer.sprite = GameAssets.instance.tileSprites[(int)this.blockType];
                break;

            case BlockType.rocketVertical:
            case BlockType.rocketHorizontal:
                spriteRenderer.sprite = GameAssets.instance.rocketSprites[(int)this.blockType];
                break;

            case BlockType.bomb:
                spriteRenderer.sprite = GameAssets.instance.bombSprites[(int)this.blockType];
                break;

            case BlockType.color:
                spriteRenderer.sprite = GameAssets.instance.colorSprites[(int)this.blockType];
                break;

            default:
                break;
        }
    }

    public void ChangeBlockType(BlockType blockType)
    {
        this.blockType = blockType;
        switch (blockType)
        {
            case BlockType.rocketVertical:
                spriteRenderer.sprite = GameAssets.instance.rocketVerticalSprite;
                break;

            case BlockType.rocketHorizontal:
                spriteRenderer.sprite = GameAssets.instance.rocketHorizontalSprite;
                break;

            case BlockType.bomb:
                spriteRenderer.sprite = GameAssets.instance.bombSprite;
                break;

            case BlockType.color:
                spriteRenderer.sprite = GameAssets.instance.colorBombSprite;
                break;

            default:
                break;
        }
    }

    public bool IsNormalBlock()
    {
        if (blockType == BlockType.red || blockType == BlockType.blue || blockType == BlockType.yellow ||
            blockType == BlockType.green || blockType == BlockType.purple || blockType == BlockType.pink)
        {
            return true;
        }
        return false;
    }
}