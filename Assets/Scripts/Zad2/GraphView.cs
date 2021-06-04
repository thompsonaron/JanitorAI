using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Graph))]
public class GraphView : MonoBehaviour
{
    public GameObject nodeViewPrefab;
    public NodeView[,] nodeViews;

    public Color startColor = Color.green;
    public Color goalColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploredColor = Color.gray;
    public Color pathColor = Color.cyan;
    public Color highlightColor = new Color(1f, 1f, 0.5f, 1f);

    private GameObject nodeParent;
    private Pathfinder pathfinder;
    private Graph graph;

    private void Awake()
    {
        pathfinder = FindObjectOfType<Pathfinder>();
        graph = GetComponent<Graph>();

        if (pathfinder)
        {
            pathfinder.initLevelAction += OnInitLevel;
            pathfinder.drawIterationAction += OnDraw;
            pathfinder.foundPathAction += OnFoundPath;
        }
    }

    private void OnDisable()
    {
        if (pathfinder)
        {
            pathfinder.initLevelAction -= OnInitLevel;
            pathfinder.drawIterationAction -= OnDraw;
            pathfinder.foundPathAction -= OnFoundPath;
        }
    }

    private void OnDestroy()
    {
        if (pathfinder)
        {
            pathfinder.initLevelAction -= OnInitLevel;
            pathfinder.drawIterationAction -= OnDraw;
            pathfinder.foundPathAction -= OnFoundPath;
        }
    }

    // blank map
    private void OnInitLevel()
    {
        InitGraphView();
    }

    private void OnDraw()
    {
        if (!pathfinder.HasFoundGoal)
        {
            ColorFrontierNodes();
            ColorExploredNodes();
            ColorPathNodes();
            ColorStartGoalNodes(pathfinder.StartNode, pathfinder.GoalNode);
        }
    }

    private void OnFoundPath()
    {
        ColorPathNodes();
        ColorStartGoalNodes(pathfinder.StartNode, pathfinder.GoalNode);
    }

    public void InitGraphView()
    {
        ColorMapNodes(graph);
        ColorStartGoalNodes(pathfinder.StartNode, pathfinder.GoalNode);
    }

    private void ColorMapNodes(Graph graph)
    {
        // setup array of NodeViews
        nodeViews = new NodeView[graph.width, graph.height];
        nodeParent = new GameObject("MapNodes");

        foreach (Node n in graph.nodes)
        {
            // create a NodeView for each corresponding Node
            GameObject instance = Instantiate(nodeViewPrefab, Vector3.zero, Quaternion.identity);
            NodeView nodeView = instance.GetComponent<NodeView>();

            nodeView.Init(n);
            nodeViews[n.x, n.y] = nodeView;

            Color nodeColor = MapData.GetColorFromNodeType(n.nodeType);
            nodeView.ColorNode(nodeColor);

            nodeView.transform.parent = nodeParent.transform;
        }
    }

    public void ColorStartGoalNodes(Node start, Node goal)
    {
        // color start NodeView and goal NodeView directly
        NodeView startNodeView = this.nodeViews[start.x, start.y];
        startNodeView.ColorNode(startColor);
        NodeView goalNodeView = this.nodeViews[goal.x, goal.y];
        goalNodeView.ColorNode(goalColor);
    }

    public void ColorFrontierNodes()
    {
        ColorNodes(pathfinder.NodesToExplore.ToList(), frontierColor);
    }

    public void ColorPathNodes()
    {
        if (pathfinder.PathNodes.Count > 0)
        {
            ColorNodes(pathfinder.PathNodes, pathColor);
        }
    }

    public void ColorExploredNodes()
    {
        if (pathfinder != null && pathfinder.ExploredNodes != null && pathfinder.ExploredNodes.Count > 0)
        {
            ColorNodes(pathfinder.ExploredNodes, exploredColor);
        }
    }

    public void ColorNodes(List<Node> nodes, Color color)
    {
        foreach (Node n in nodes)
        {
            if (n != null)
            {
                NodeView nodeView = nodeViews[n.x, n.y];
                Color newColor = color;
                nodeView.ColorNode(newColor);
            }
        }
    }
}
