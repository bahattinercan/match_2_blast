using System;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
	[SerializeField] private GameGrid grid;
	private IPlayerControls controls = new Mouse();
	public bool canInteract;
	public IPlayerControls Controls { get => controls; set => controls = value; }
    private void Start()
    {
		canInteract = true;
    }
    private void Update() 
	{
		try
		{
			if (Controls.GetInteraction() != null && canInteract==true)
			Controls.GetInteraction().GetComponent<IBlock>().Activate(grid);

			grid.FillGrid();
			
			grid.Cascade();
		}
		catch (InvalidGridException gridException)
		{
			Debug.Log("An unexpected error has occurred with the game grid. " + gridException);
		}
		catch (InvalidBlockException blockException)
		{
			Debug.Log("An unexpected error has occurred with one of the blocks. " + blockException);
		}
		catch (Exception exception)
		{
			Debug.Log("An unexpected error has occurred. " + exception);
		}
	}

	public void SetInteract(bool canInteract)
    {
		this.canInteract = canInteract;
    }
}