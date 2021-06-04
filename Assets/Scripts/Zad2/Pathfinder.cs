using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pathfinder : MonoBehaviour
{

    private Node startNode;
    private Node endNode;

    private Graph graph;

    private PriorityQueue<Node> nodesToExplore;
    private List<Node> exploredNodes;
    private List<Node> pathNodes;

    private bool hasFoundGoal;

    public Node StartNode => startNode;
    public Node GoalNode => endNode;
    public PriorityQueue<Node> NodesToExplore => nodesToExplore;
    public List<Node> ExploredNodes => exploredNodes;
    public List<Node> PathNodes => pathNodes;
    public bool HasFoundGoal => hasFoundGoal;

    public Action initLevelAction;
    public Action drawIterationAction;
    public Action foundPathAction;

    public bool showIterations = true;
    public bool exitOnGoal = true;
    private bool isComplete = false;

    int iterations = 0;

    public void Init(Graph graph, Node start, Node goal)
    {
        if (start.nodeType == NodeType.Closed || goal.nodeType == NodeType.Closed)
        {
            Debug.LogWarning("PATHFINDER Init error: start and goal nodes must be unblocked!");
            return;
        }

        this.graph = graph;
        startNode = start;
        endNode = goal;

        nodesToExplore = new PriorityQueue<Node>();
        nodesToExplore.Enqueue(start);

        exploredNodes = new List<Node>();
        pathNodes = new List<Node>();


        // default the nodes
        for (int x = 0; x < this.graph.width; x++)
        {
            for (int y = 0; y < this.graph.height; y++)
            {
                this.graph.nodes[x, y].ResetPrevious();
            }
        }

        isComplete = false;
        iterations = 0;
        startNode.distanceTraveled = 0;
        hasFoundGoal = false;

        initLevelAction?.Invoke();
    }

    public IEnumerator SearchRoutine(float timeStep = 0.1f)
    {
        yield return null;

        while (!isComplete && nodesToExplore != null)
        {
            if (nodesToExplore.Count > 0)
            {
                // getting top node from prio que
                Node currentNode = nodesToExplore.Dequeue();
                iterations++;

                // this node is explored
                if (!exploredNodes.Contains(currentNode))
                {
                    exploredNodes.Add(currentNode);
                }

                // main algorithm
                AStarAddNodesToExplore(currentNode);

                if (showIterations)
                {
                    drawIterationAction?.Invoke();
                    yield return new WaitForSeconds(timeStep);
                }

                // point of reaching end node
                if (nodesToExplore.Contains(endNode))
                {
                    pathNodes = GetPathNodes(endNode);

                    if (exitOnGoal)
                    {
                        isComplete = true;
                        foundPathAction.Invoke();
                    }
                }
            }
            else
            {
                isComplete = true;
                foundPathAction.Invoke();
            }
        }
    }

    // expand the frontier nodes using AStar search from a single node
    private void AStarAddNodesToExplore(Node node)
    {
        for (int i = 0; i < node.neighbours.Count; i++)
        {
            // if the current neighbor has not been explored and the node is not closed (wall)
            if (!exploredNodes.Contains(node.neighbours[i]) && node.neighbours[i].nodeType != NodeType.Closed)
            {
                // calculate distance to current neighbor (basically 10 or 14)
                int distanceToNeighbor = graph.GetNodeDistance(node, node.neighbours[i]);

                // total distance of the current path + distanceToNeighbor
                int newDistanceTraveled = distanceToNeighbor + node.distanceTraveled;

                // if a shorter path exists to the neighbor through this node, re-route and recalc distance
                if ((node.neighbours[i].distanceTraveled == -1) || newDistanceTraveled < node.neighbours[i].distanceTraveled)
                {
                    node.neighbours[i].previous = node;
                    node.neighbours[i].distanceTraveled = newDistanceTraveled;
                }

                // if the neighbor is not in the nodes to explore, add it (to the priority queue)
                if (!nodesToExplore.Contains(node.neighbours[i]) && graph != null)
                {
                    int distanceToGoal = graph.GetNodeDistance(node.neighbours[i], endNode);

                    // fScore(total distance = gScore(distance from start) + hScore(distance to goal)
                    node.neighbours[i].priority = node.neighbours[i].distanceTraveled + distanceToGoal;

                    // add to priority queue using the F score
                    nodesToExplore.Enqueue(node.neighbours[i]);
                }
            }
        }
    }

    private List<Node> GetPathNodes(Node endNode)
    {
        List<Node> path = new List<Node>();
        if (endNode == null)
        {
            return path;
        }

        path.Add(endNode);

        Node currentNode = endNode.previous;
        while (currentNode != null)
        {
            // insert the previous node at the first position in the path
            path.Insert(0, currentNode);
            currentNode = currentNode.previous;
        }

        return path;
    }
}