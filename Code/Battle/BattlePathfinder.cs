using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePathfinder
{
    public BattleMap map;
    private PathNode[,] grid;

    private int length;
    private int height;

    private List<PathNode> open_list;
    private List<PathNode> closed_list;

    private PathNode start_node;
    private PathNode current_node;
    private PathNode end_node;

    private class PathNode
    {
        public int f;
        public int g;
        public int h;

        public int x;
        public int y;

        public PathNode previous_node;

        public PathNode(int i, int I)
        {
            x = i;
            y = I;

            g = 0;
        }

        public void CalculateG(PathNode previous, MovePace move)
        {
            int change;
            if (move == MovePace.Full)
                change = 1;
            else if (move == MovePace.Half)
                change = 2;
            else
                change = 4;

            if (g == 0 || previous.g + change < g)
            {
                g = previous.g + change;

                previous_node = previous;

                f = g + h;
            }
        }

        public void CalculateH(PathNode goal)
        {
            h = 0;

            h += Mathf.Abs(x - goal.x);
            h += Mathf.Abs(y - goal.y);

            f = g + h;
        }

        public Coordinate GetCoordinate()
        {
            return new Coordinate(x, y);
        }
    }

    public class Path
    {
        private int value;
        private int[] cost;
        private Coordinate[] cords;
        public Path(Coordinate[] c, int[] I, int i)
        {
            value = i;
            cost = I;
            cords = c;
        }

        public int GetValue()
        {
            return value;
        }

        public Coordinate[] GetCoordinates()
        {
            return cords;
        }

        public Coordinate GetNodeAtValue(int i)
        {
            if (i > cost[cost.Length -1])
                return cords[cost.Length - 1];

            int I = -1;
            while (I+1 != cost.Length && cost[I + 1] <= i)
                ++I;

            return cords[I];
        }
    }

    public BattlePathfinder(BattleMap m)
    {
        map = m;

        length = map.length;
        height = map.height;

        grid = new PathNode[length, height];

        Reset();
    }

    public Path GetPath(Coordinate start, Coordinate end, BattleUnit u)
    {
        Reset();

        if (!map.Contains(start) || !map.Contains(end))
            return null;

        start_node = CoordinateToNode(start);
        end_node = CoordinateToNode(end);

        if (start_node == null || end_node == null)
            return null;

        open_list.Add(start_node);

        SetH();

        int i = 0;

        while (open_list.Count != 0)
        {
            NextNode();

            if (current_node == end_node)
            {
                return new Path(GetPathCoordinates(), GetPathCost(),end_node.g);
            }

            open_list.Remove(current_node);
            closed_list.Add(current_node);

            UpdateOpen(u);

            ++i;

            if (i > 1000)
                return null;
        }

        return null;
    }

    private void NextNode()
    {
        PathNode temp = open_list[0];

        foreach (PathNode n in open_list)
            if (n.f < temp.f)
                temp = n;

        current_node = temp;
    }

    private void UpdateOpen(BattleUnit u)
    {
        if (current_node.x > 0)
            AddNodeToOpen(grid[current_node.x - 1, current_node.y], u);

        if (current_node.y > 0)
            AddNodeToOpen(grid[current_node.x, current_node.y - 1], u);

        if (current_node.x < length - 1)
            AddNodeToOpen(grid[current_node.x + 1, current_node.y], u);

        if (current_node.y < height - 1)
            AddNodeToOpen(grid[current_node.x, current_node.y + 1], u);
    }

    private void AddNodeToOpen(PathNode node, BattleUnit u)
    {
        if (closed_list.Contains(node))
            return;

        MovePace temp = map.CheckTraversability(node.GetCoordinate(), u);

        if (temp == MovePace.None)
        {
            closed_list.Add(node);
            return;
        }

        node.CalculateG(current_node, temp);

        if (!open_list.Contains(node))
            open_list.Add(node);
    }

    private Coordinate[] GetPathCoordinates()
    {
        List<Coordinate> cords = new List<Coordinate>();

        PathNode tempnode = current_node;

        while (tempnode != null)
        {
            Coordinate temp = tempnode.GetCoordinate();
            cords.Add(temp);
            tempnode = tempnode.previous_node;
        }

        cords.Reverse();
        return cords.ToArray();
    }
    private int[] GetPathCost()
    {
        List<int> costs = new List<int>();

        PathNode tempnode = current_node;

        while (tempnode != null)
        {
            int temp = tempnode.g;
            costs.Add(temp);
            tempnode = tempnode.previous_node;
        }

        costs.Reverse();
        return costs.ToArray();
    }

    private PathNode CoordinateToNode(Coordinate point)
    {
        if (point.GetX() < 0 || point.GetY() < 0 || point.GetX() >= length || point.GetY() >= height)
            return null;

        return grid[point.GetX(), point.GetY()];
    }

    private void SetH()
    {
        for (int i = 0; i < length; ++i)
            for (int I = 0; I < height; ++I)
                grid[i, I].CalculateH(end_node);
    }

    private void Reset()
    {
        open_list = new List<PathNode>();
        closed_list = new List<PathNode>();

        for (int i = 0; i < length; ++i)
            for (int I = 0; I < height; ++I)
            {
                grid[i, I] = new PathNode(i, I);
            }
    }
}