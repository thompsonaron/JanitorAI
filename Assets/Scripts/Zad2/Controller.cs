using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public MapData mapData;
    public Graph graph;
    public Pathfinder pathfinder;

    public int startX = 0;
    public int startY = 0;

    public int goalX = 15;
    public int goalY = 1;

    public float iterationStepTime = 0.1f;

    void Start()
    {
        if (mapData != null && graph != null)
        {
            InitMapAndGraph();
            FindPath();
        }
    }

    private void InitMapAndGraph()
    {
        // generate the map from text file
        int[,] mapInstance = mapData.MakeMap();

        // initialize the graph
        graph.Init(mapInstance);
    }

    // initialize the Pathfinder and begin the graph search
    private void FindPath()
    {
        if (!graph.IsWithinBounds(startX, startY) || !graph.IsWithinBounds(goalX, goalY))
        {
            Debug.LogWarning("CONTROLLER_Start or goal out of bounds!");
            return;
        }

        Node startNode = graph.nodes[startX, startY];
        Node goalNode = graph.nodes[goalX, goalY];

        pathfinder.Init(graph, startNode, goalNode);
        StartCoroutine(pathfinder.SearchRoutine(iterationStepTime));
    }
}