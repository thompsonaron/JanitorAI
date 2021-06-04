using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    Open = 0,
    Closed = 1,
}

public class Node : IComparable<Node>
{
    public NodeType nodeType = NodeType.Open;

    public int x = -1;
    public int y = -1;

    public Vector2 position;

    public List<Node> neighbours = new List<Node>();
    // total distance traveled from the start Node
    public int distanceTraveled = -1;

    public Node previous = null;

    public int priority;

    public Node(int x, int y, NodeType nodeType)
    {
        this.x = x;
        this.y = y;
        this.nodeType = nodeType;
    }

    public int CompareTo(Node other)
    {
        if (priority < other.priority)
        {
            return -1;
        }
        else if (priority > other.priority)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public void ResetPrevious()
    {
        previous = null;
    }
}
