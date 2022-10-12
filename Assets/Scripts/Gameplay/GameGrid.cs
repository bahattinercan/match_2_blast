using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    private int width = 10;
    private int height = 10;
    private const float CascadeSpeed = 12.5f;
    private List<List<IBlock>> Columns = new List<List<IBlock>>();
    [SerializeField] private GameObject Square;
    [SerializeField] private GameObject BlackBomb;

    private const float CheckForSpriteTypesDelay = .05f;
    public int Height { get => height; set => height = value; }
    public int Width { get => width; set => width = value; }

    public GameGrid()
    {
        LevelSO level = MyGameDatas.instance.GetLevel();
        width = level.width;
        height = level.height;
        for (var x = 0; x < Width; x++)
            this.Columns.Add(new List<IBlock>());
    }

    public void FillGrid()
    {
        foreach (var column in this.Columns)
            while (column.Count < Height)
            {
                var spawnPoint = new Vector3(Columns.IndexOf(column), 10, 0);
                var instantiatedBlock = Instantiate(Square, spawnPoint, Quaternion.identity);
                instantiatedBlock.transform.parent = gameObject.transform;
                column.Add(instantiatedBlock.GetComponent<IBlock>());
            }
    }

    private void Start()
    {
        Invoke("CheckForSpriteTypes", CheckForSpriteTypesDelay*2);
    }

    public void CheckForSpriteTypes()
    {
        List<IBlock> blocks = new List<IBlock>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (Columns[i][j] != null &&!blocks.Contains(Columns[i][j]))
                {
                    blocks.AddRange(Columns[i][j].CheckOtherTiles(this));
                    Columns[i][j].CheckOtherTiles(this);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
                Gizmos.DrawCube(new Vector3(x, y + 0.5f, 0), new Vector3(.5f, .5f, .5f));
    }

    public void Cascade()
    {
        foreach (var column in this.Columns)
            foreach (var block in column)
            {
                var destination = new Vector3(Columns.IndexOf(column), column.IndexOf(block));
                block.GetTransform = Vector3.MoveTowards(block.GetTransform, destination, CascadeSpeed * Time.deltaTime);
            }
    }

    public void DestroyBlocks(List<IBlock> blocks)
    {
        foreach (var block in blocks)
        {
            GameManager.instance.UpdateHud(block);
            this.Columns.ForEach(b => { b.Remove(block); Destroy(block.GetObject); });
        }
        GameManager.instance.CheckGameWinLose();
        Invoke("CheckForSpriteTypes", CheckForSpriteTypesDelay);
    }

    public List<IBlock> GetSurroundingBlocks(IBlock target, bool isHexagonal)
    {
        if (target == null)
            throw new InvalidBlockException("Cannot find surrounding blocks for a null object.");

        var targetX = this.Columns.Where(b => b.Contains(target)).Select(b => this.Columns.IndexOf(b)).SingleOrDefault();
        var targetY = this.Columns[targetX].IndexOf(target);

        var positions = new List<Point>();
        if (target.IsNormalBlock())
        {
            positions.Add(new Point { x = targetX + 1, y = targetY });
            positions.Add(new Point { x = targetX - 1, y = targetY });
            positions.Add(new Point { x = targetX, y = targetY + 1 });
            positions.Add(new Point { x = targetX, y = targetY - 1 });
        }
        else if (target.blockType == BlockType.bomb)
        {
            positions.Add(new Point { x = targetX + 1, y = targetY });
            positions.Add(new Point { x = targetX - 1, y = targetY });
            positions.Add(new Point { x = targetX, y = targetY + 1 });
            positions.Add(new Point { x = targetX, y = targetY - 1 });
            positions.Add(new Point { x = targetX + 1, y = targetY + 1 });
            positions.Add(new Point { x = targetX - 1, y = targetY + 1 });
            positions.Add(new Point { x = targetX + 1, y = targetY - 1 });
            positions.Add(new Point { x = targetX - 1, y = targetY - 1 });
        }
        else if (target.blockType == BlockType.rocketVertical)
        {
            for (int i = 0; i < height; i++)
            {
                positions.Add(new Point { x = targetX, y = i });
            }
        }
        else if (target.blockType == BlockType.rocketHorizontal)
        {
            for (int i = 0; i < width; i++)
            {
                positions.Add(new Point { x = i, y = targetY });
            }
        }
        else if (target.blockType == BlockType.color)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (Columns[i][j] != null)
                    {
                        IBlock block = Columns[i][j];
                        if (block.colorType == target.colorType)
                            positions.Add(new Point { x = i, y = j });
                    }
                }
            }
        }

        var surrounding = new List<IBlock>();
        foreach (var position in positions)
            if ((position.x >= 0 && position.x < Width) && (position.y >= 0 && position.y < Height)
                && Columns[position.x].Count > position.y)
                surrounding.Add(this.Columns[position.x][position.y]);

        return surrounding;
    }

    private IBlock RandomizeBlock()
    {
        var blocks = new List<IBlock>();
        blocks.Add(Square.GetComponent<IBlock>());
        return blocks[Random.Range(0, blocks.Count)];
    }
}