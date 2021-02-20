using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator
{
    public Maze GenerateMaze(int w, int h)
    {
        MazeGeneratorCell[,] cells = new MazeGeneratorCell[w, h];

        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                cells[x, y] = new MazeGeneratorCell {X = x, Y = y};
            }
        }

        //удаление боковых стен
        for (int x = 0; x < cells.GetLength(0); x++)
        {
            cells[x, h - 1].WallLeft = false;
        }   
        //удаление боковых стен
        for (int y = 0; y < cells.GetLength(1); y++)
        {
            cells[w - 1, y].WallBottom = false;
        }

        RemoveWallsWithBacktracker(cells, w, h);

        Maze maze = new Maze();

        maze.cells = cells;
        maze.finishPosition = PlaceMazeExit(cells, w, h);

        return maze;
    }

    private void RemoveWallsWithBacktracker(MazeGeneratorCell[,] maze, int w, int h)
    {
        MazeGeneratorCell current = maze[0, 0];
        current.Visited = true;
        current.DistanceFromStart = 0;

        Stack<MazeGeneratorCell> stack = new Stack<MazeGeneratorCell>();
        do
        {
            List<MazeGeneratorCell> unvisitedNeighbours = new List<MazeGeneratorCell>();

            int x = current.X;
            int y = current.Y;

            if (x > 0 && !maze[x - 1, y].Visited) unvisitedNeighbours.Add(maze[x - 1, y]);
            if (y > 0 && !maze[x, y - 1].Visited) unvisitedNeighbours.Add(maze[x, y - 1]);
            if (x < w - 2 && !maze[x + 1, y].Visited) unvisitedNeighbours.Add(maze[x + 1, y]);
            if (y < h - 2 && !maze[x, y + 1].Visited) unvisitedNeighbours.Add(maze[x, y + 1]);

            if (unvisitedNeighbours.Count > 0)
            {
                MazeGeneratorCell chosen = unvisitedNeighbours[UnityEngine.Random.Range(0, unvisitedNeighbours.Count)];
                RemoveWall(current, chosen);

                chosen.Visited = true;
                stack.Push(chosen);
                chosen.DistanceFromStart = current.DistanceFromStart + 1;
                current = chosen;
            }
            else
            {
                current = stack.Pop();
            }
        } while (stack.Count > 0);
    }
    
    private void RemoveWall(MazeGeneratorCell a, MazeGeneratorCell b)
    {
        if (a.X == b.X)
        {
            if (a.Y > b.Y) a.WallBottom = false;
            else b.WallBottom = false;
        }
        else
        {
            if (a.X > b.X) a.WallLeft = false;
            else b.WallLeft = false;
        }
    }

    private Vector2Int PlaceMazeExit(MazeGeneratorCell[,] maze, int w, int h)
    {
        MazeGeneratorCell furthest = maze[0, 0];

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            if (maze[x, h - 2].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[x, h - 2];
            if (maze[x, 0].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[x, 0];
        }

        for (int y = 0; y < maze.GetLength(1); y++)
        {
            if (maze[w - 2, y].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[w - 2, y];
            if (maze[0, y].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[0, y];
        }

        if (furthest.X == 0) furthest.WallLeft = false;
        else if (furthest.Y == 0) furthest.WallBottom = false;
        else if (furthest.X == w - 2) maze[furthest.X + 1, furthest.Y].WallLeft = false;
        else if (furthest.Y == h - 2) maze[furthest.X, furthest.Y + 1].WallBottom = false;

        return new Vector2Int(furthest.X, furthest.Y);
    }
}