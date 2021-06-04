using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public Node[,] nodes;

    int[,] mapData;
    public int width;
    public int height;

    public static readonly Vector2[] allDirections =
    {
        new Vector2(0f,1f),
        new Vector2(1f,1f),
        new Vector2(1f,0f),
        new Vector2(1f,-1f),
        new Vector2(0f,-1f),
        new Vector2(-1f,-1f),
        new Vector2(-1f,0f),
        new Vector2(-1f,1f)
    };

    public static readonly Vector2[] worldSides =
{
        new Vector2(0f,1f),
        new Vector2(1f,0f),
        new Vector2(0f,-1f),
        new Vector2(-1f,0f),
    };

    public void Init(int[,] mapData)
    {
        this.mapData = mapData;

        width = mapData.GetLength(0);
        height = mapData.GetLength(1);

        nodes = new Node[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                NodeType nodeType = (NodeType)mapData[x, y];
                Node newNode = new Node(x, y, nodeType);
                nodes[x, y] = newNode;

                newNode.position = new Vector3(x, y, 0);
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (nodes[x, y].nodeType != NodeType.Closed)
                {
                    nodes[x, y].neighbours = GetNeighbours(x, y);
                }
            }
        }
    }

    public bool IsWithinBounds(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0 && y < height);
    }

    private List<Node> GetNeighbours(int x, int y, Node[,] nodeArray, Vector2[] directions)
    {
        List<Node> neighbourNodes = new List<Node>();

        foreach (var dir in directions)
        {
            int neighbourX = x + (int)dir.x;
            int neighbourY = y + (int)dir.y;

            if (IsWithinBounds(neighbourX, neighbourY) && nodeArray[x,y].nodeType != NodeType.Closed)
            {
                neighbourNodes.Add(nodeArray[neighbourX, neighbourY]);
            }
        }

        return neighbourNodes;
    }

    private List<Node> GetNeighbours(int x, int y)
    {
        return GetNeighbours(x, y, nodes, allDirections);
    }

    public int GetNodeDistance(Node source, Node target)
    {
        int dx = Mathf.Abs(source.x - target.x);
        int dy = Mathf.Abs(source.y - target.y);

        int min = Mathf.Min(dx, dy);
        int max = Mathf.Max(dx, dy);

        int diagonalSteps = min;
        int straightSteps = max - min;

        return (14 * diagonalSteps + straightSteps * 10);
    }

    public int GetManhattanDistance(Node source, Node target)
    {
        int dx = Mathf.Abs(source.x - target.x);
        int dy = Mathf.Abs(source.y - target.y);
        return (dx + dy);
    }
}
