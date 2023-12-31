using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestPathfinder
{
    private GridManager Grid;
    private Tile StartPoint;
    private Tile EndPoint;
    private Vector2 EndPointPosition;
    private Tile CurrentTile;

    public BestPathfinder(GridManager grid, Vector2 startPointPosition, Vector2 endPointPosition)
    {
        Grid = grid;

        EndPointPosition = endPointPosition;
        EndPoint = Grid.GetTileAtPosition(endPointPosition);
        StartPoint = Grid.GetTileAtPosition(startPointPosition);
    }

    public List<Tile> GetBestPath()
    {
        List<BestTileLogic> from = new List<BestTileLogic>();
        List<BestTileLogic> openSet = new List<BestTileLogic>();
        openSet.Add(StartPoint.BestPathLogic);

        while (openSet.Count > 0)
        {
            int win = 0;

            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].F > openSet[win].F)
                {
                    win = i;
                }

                if (openSet[i].F == openSet[win].F && openSet[i].G > openSet[win].G)
                {
                    win = i;
                }
            }

            BestTileLogic currentTile = openSet[win];
            currentTile.IsVisited = true;

            if (currentTile.Position == EndPointPosition)
            {
                return RecoveredPath();
            }

            openSet.RemoveAt(win);
            from.Add(currentTile);
            List<Tile> neighbors = Grid.GetNeighbors(currentTile.Position);
            List<BestTileLogic> logicNeighbors = ToLogicNeighbors(currentTile.AccScore, neighbors);

            foreach (BestTileLogic neighborLogic in logicNeighbors)
            {
                if (!from.Contains(neighborLogic))
                {
                    int g = currentTile.G + CalculateNeighborHeuristic(currentTile, neighborLogic);

                    if (!openSet.Contains(neighborLogic))
                    {
                        openSet.Add(neighborLogic);
                    }
                    else if (g >= neighborLogic.G)
                    {
                        continue;
                    }

                    neighborLogic.G = g;
                    neighborLogic.H = CalculateHeuristic(neighborLogic, EndPoint.BestPathLogic);
                    neighborLogic.F = neighborLogic.G + neighborLogic.H;
                    neighborLogic.PrevBestTileLogic = currentTile;
                    neighborLogic.AccScore = currentTile.AccScore + neighborLogic.ParentTile.Score;
                }
            }
        }

        return new List<Tile>();
    }

    private List<BestTileLogic> ToLogicNeighbors(int accScore, List<Tile> neighbors)
    {
        List<BestTileLogic> logicNeighbors = new List<BestTileLogic>();

        foreach (Tile neighbor in neighbors)
        {
            if ((accScore + neighbor.Score) >= 0)
            {
                logicNeighbors.Add(neighbor.BestPathLogic);
            }
        }

        return logicNeighbors;
    }

    private int CalculateNeighborHeuristic(BestTileLogic from, BestTileLogic to)
    {
        int xShift = Math.Abs((int)(from.Position.x - to.Position.x));
        int yShift = Math.Abs((int)(from.Position.y - to.Position.y));

        return xShift * to.ParentTile.Score + yShift * to.ParentTile.Score;
    }

    private int CalculateHeuristic(BestTileLogic from, BestTileLogic to)
    {
        int xShift = Math.Abs((int)(from.Position.x - to.Position.x));
        int yShift = Math.Abs((int)(from.Position.y - to.Position.y));

        int allShifts = xShift + yShift;

        return allShifts == 0 ? int.MaxValue : 1 / allShifts;
    }

    private List<Tile> RecoveredPath()
    {
        List<Tile> path = new List<Tile>();
        Tile currentTile = EndPoint;

        while (currentTile != StartPoint)
        {
            path.Add(currentTile);

            BestTileLogic prevTileLogic = currentTile.BestPathLogic.PrevBestTileLogic;
            Tile prevTile = Grid.GetTileAtPosition(prevTileLogic.Position);

            currentTile = prevTile;
        }

        path.Add(StartPoint);

        return path;
    }
}
