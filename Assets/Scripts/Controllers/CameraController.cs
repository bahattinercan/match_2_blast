using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private GameGrid gameGrid;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        ViewSetup();
    }

    private void ViewSetup()
    {
        float neededXMove = (gameGrid.Width - 6) / 2f;
        float neededYMove = 0;
        if (gameGrid.Height < 6)
        {
            neededYMove = (6 - gameGrid.Height) / -2;
        }
        else if (gameGrid.Height > 6)
        {
            neededYMove = (gameGrid.Height - 6) / 2f;
        }
        int newCameraSize = (int)mainCamera.orthographicSize + gameGrid.Width - 6;
        mainCamera.orthographicSize = newCameraSize;
        transform.position = new Vector3(transform.position.x + neededXMove, transform.position.y + neededYMove, transform.position.z);
    }
}