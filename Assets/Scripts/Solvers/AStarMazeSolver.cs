using System;
using System.Collections.Generic;
using UnityEngine;

namespace Solvers
{
    public class AStarMazeSolver : MazeSolver
    {
        private HashSet<Node> unvisitedNodes = new HashSet<Node>();
        
        private void Update()
        {
            if (!canStart)
            {
                lastUpdateTime = Time.time;
                return;
            }
            
            if (currentNode.IsFinish)
            {
                Finish();
                return;
            }
            
            if (Time.time - lastUpdateTime < floatFixedStepDeltaTime)
            {
                return;
            }
                
            lastUpdateTime = Time.time;
            solvingSteps++;

            PerformAStar();
        }

        private void PerformAStar()
        {
            if (unvisitedNodes.Count > 0)
            {
                Node bestCostNode = null;
                
                //Debug.Log($"START Node ID: {currentNode.NodeID} Cost: {currentNode.Cost} EBHSA: {EBHSA(currentNode)}");

                // Find the unvisited node with the smallest known distance
                foreach (Node node in unvisitedNodes)
                {
                    /*
                    string log = $"Unvisited Start ID: {node.NodeID}\tHeuritics: {EBHSA(node)}";

                    if (bestCostNode != null)
                    {
                        log += $"\nUnvisited Best ID: {bestCostNode.NodeID}\tHeuritics: {EBHSA(bestCostNode)}";
                    }
                    
                    Debug.Log(log);
                    */
                    
                    if (bestCostNode == null || node.Cost + EBHSA(node) < bestCostNode.Cost + EBHSA(bestCostNode))
                    {
                        bestCostNode = node;
                    }
                }
                
                //Debug.Log($"Best ID: {bestCostNode.NodeID} Cost: {bestCostNode.Cost} EBHSA: {EBHSA(bestCostNode)}");

                currentNode = bestCostNode;
                currentNode.Check();

                List<Node> allValidNeighbours = currentNode.GetAllValidNeighbours();

                foreach (Node node in allValidNeighbours)
                {
                    UpdateNeighbourCost(currentNode, node);
                    unvisitedNodes.Add(node);
                }

                unvisitedNodes.Remove(currentNode);
            }
        }

        private static void UpdateNeighbourCost(Node currentNode, Node neighbour)
        {
            if (!neighbour.IsChecked)
            {
                var targetNodeCost = neighbour.Cost;
                var potentialNodeCost = currentNode.Cost + 1;

                if (targetNodeCost > potentialNodeCost)
                {
                    neighbour.Cost = potentialNodeCost;
                }
            }
        }

        public override void Solve()
        {
            startTime = Time.time;
            currentNode = MazeGenerator.GetStartNode();
            currentNode.Cost = 0;
            unvisitedNodes.Add(currentNode);
            canStart = true;
        }

        public static float EBHSA(Node node)
        {
            return Euclidean(node) + Chebyshev(node) * 2;
        }

        protected override void Finish()
        {
            base.Finish();

            Node startNode = MazeGenerator.GetStartNode();

            while (currentNode != startNode)
            {
                if (!currentNode.IsFinish)
                {
                    currentNode.MarkAsPartOfSolution();
                }

                List<Node> checkedNeighbours = currentNode.GetCheckedNeighbours();
                currentNode = checkedNeighbours[0];
                
                for (var index = 1; index < checkedNeighbours.Count; index++)
                {
                    var node = checkedNeighbours[index];
                    if (node.Cost < currentNode.Cost)
                    {
                        currentNode = node;
                    }
                }
            }
        }

        private static float Chebyshev(Node node)
        {
            Node targetNode = MazeGenerator.GetFinishNode();
            float dx = Math.Abs(targetNode.NodeID % 100 - node.NodeID % 100);
            float dy = Math.Abs(targetNode.RowID - node.RowID);
            return Math.Max(dx, dy);
        }
        
        private static float Euclidean(Node node)
        {
            Node targetNode = MazeGenerator.GetFinishNode();
            
            float dx = Math.Abs(targetNode.NodeID % 100 - node.NodeID % 100);
            float dy = Math.Abs(targetNode.RowID - node.RowID);

            return Mathf.Sqrt(dx * dx + dy * dy);
        }
    }
}

