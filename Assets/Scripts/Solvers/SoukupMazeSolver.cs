using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Solvers
{
    public class SoukupMazeSolver : MazeSolver
    {
        private Queue<Node> queue = new Queue<Node>();
        private List<Node> unvisitedNodes = new List<Node>();

        private bool useAStar = true;
        private Node bestCostNode;
        
        private void Update()
        {
            if (!canStart)
            {
                lastUpdateTime = Time.time;
                return;
            }
        
            if (currentNode != null && currentNode.IsFinish)
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

            PerformSoukup();
        }

        private void PerformSoukup()
        {
            if (useAStar)
            {
                AStarWithExit();
            }
            else
            {
                BFS();
            }
        }

        private void AStarWithExit()
        {
            // Get best neighbour using heuritics
            List<Node> allUncheckedNeighbours = currentNode.GetAllNeighbours().FindAll(node => !node.IsChecked);

            Node bestNeighbour = null;
            Node bestNeighbourNonWall = null;
            
            foreach (Node neighbour in allUncheckedNeighbours)
            {
                if (bestNeighbour == null || AStarMazeSolver.EBHSA(neighbour) < AStarMazeSolver.EBHSA(bestNeighbour))
                {
                    bestNeighbour = neighbour;
                }

                if (!neighbour.IsWall)
                {
                    if (bestNeighbourNonWall == null || AStarMazeSolver.EBHSA(neighbour) < AStarMazeSolver.EBHSA(bestNeighbourNonWall))
                    {
                        bestNeighbourNonWall = neighbour;
                    }
                    
                    unvisitedNodes.Add(neighbour);
                }
            }

            if (bestNeighbour.IsWall && !currentNode.IsChecked)
            {
                useAStar = false;
                bestCostNode = currentNode;
                queue.Enqueue(currentNode);
                unvisitedNodes.Remove(currentNode);
            }
            else if (bestNeighbour.IsWall && currentNode.IsChecked)
            {
                Node localBest = null;
                List<Node> unvisitedNodesDummy = new List<Node>(unvisitedNodes);
                
                foreach (Node node in unvisitedNodesDummy)
                {
                    if (node.IsChecked)
                    {
                        unvisitedNodes.Remove(node);
                        continue;
                    }
                    
                    if (localBest == null || AStarMazeSolver.EBHSA(node) < AStarMazeSolver.EBHSA(localBest))
                    {
                        localBest = node;
                    }
                }

                bestCostNode = localBest;
                currentNode = localBest;
            }
            else
            {
                currentNode.Check();
                unvisitedNodes.Remove(currentNode);
                currentNode = bestNeighbour;
            }
        }
        
        private void BFS()
        {
            if (queue.Count == 0)
            {
                useAStar = true;
                return;
            }
            
            currentNode = queue.Dequeue();

            if (currentNode.IsWall || currentNode.IsChecked)
            {
                return;
            }
                        
            // Mark node as visited
            currentNode.Check();

            List<Node> validNeighbours = currentNode.GetAllValidNeighbours();

            // Add all neighbors to the queue
            Node bestNeighbour = null;
            foreach (Node neighbour in validNeighbours)
            {
                if (bestNeighbour == null || AStarMazeSolver.EBHSA(neighbour) < AStarMazeSolver.EBHSA(bestNeighbour))
                {
                    bestNeighbour = neighbour;
                }

                queue.Enqueue(neighbour);
            }

            // Determine if we start A* again
            if (bestNeighbour != null && AStarMazeSolver.EBHSA(bestNeighbour) <= AStarMazeSolver.EBHSA(bestCostNode))
            {
                bestCostNode = bestNeighbour;
                currentNode = bestNeighbour;
                useAStar = true;
                queue.Clear();
            }
        }

        public override void Solve()
        {
            startTime = Time.time;
            currentNode = MazeGenerator.GetStartNode();
            unvisitedNodes.Add(currentNode);
            canStart = true;
        }
    }
}