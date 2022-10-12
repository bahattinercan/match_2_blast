using System.Collections.Generic;
using UnityEngine;

public interface IBlock
{
	GameObject GetObject { get; }
	Vector3 GetTransform { get; set; }
	BlockType blockType { get; set; }
	BlockType colorType { get; set; }
	void Activate(GameGrid grid);
	public List<IBlock> CheckOtherTiles(GameGrid grid);
	public void ChangeBlockSprite(BlockType blockType);
	public void ChangeBlockType(BlockType blockType);
	public bool IsNormalBlock();
}

public enum BlockType
{
	red,
	blue,
	yellow,
	green,
	purple,
	pink,
	rocketVertical,
	rocketHorizontal,
	bomb,
	color
}