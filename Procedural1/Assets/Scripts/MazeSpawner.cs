using UnityEngine;

public class MazeSpawner : MonoBehaviour
{
    public Maze maze;
    public HintRenderer HintRenderer;
    
    public Cell CellPrefab;
    public Vector3 CellSize = new Vector3(1,1,0);

    public int Width = 23;
    public int Height = 15;
    GameObject[] gameObjects;

    private void Start()
    {
        MazeGenerating();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            gameObjects = GameObject.FindGameObjectsWithTag("MazeTag");
            if (gameObjects.Length > 0) {
                for (int i = 0; i < gameObjects.Length; i++) {
                    Destroy(gameObjects[i]);
                }
                MazeGenerating();
            } else {
                return;
            }
        }
    }

    private void MazeGenerating () {
        MazeGenerator generator = new MazeGenerator();
        maze = generator.GenerateMaze(Width, Height);

        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int y = 0; y < maze.cells.GetLength(1); y++)
            {
                Cell c = Instantiate(CellPrefab, new Vector3(x * CellSize.x, y * CellSize.y, y * CellSize.z), Quaternion.identity);

                c.WallLeft.SetActive(maze.cells[x, y].WallLeft);
                c.WallBottom.SetActive(maze.cells[x, y].WallBottom);
            }
        }

        HintRenderer.DrawPath();
    }
}