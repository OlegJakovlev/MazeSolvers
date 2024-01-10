using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Solvers
{
    public class DeadEndFillerMazeSolver : MazeSolver
    {
        private List<Node> deadEndNodes = new List<Node>();
        private List<Node> bufferEndNodes = new List<Node>();

        private void Update()
        {
            if (!canStart)
            {
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
            
            PerformDeadEndFiller();
        }

        private void PerformDeadEndFiller()
        {
            if (deadEndNodes.Count > 0)
            {
                foreach (Node node in deadEndNodes)
                {
                    node.SetAsDeadEnd();
                    var validAliveNeighbours = node.GetAllValidNeighbours().Where(node => !node.IsDeadEnd);

                    foreach (Node neighbour in validAliveNeighbours)
                    {
                        if (neighbour.GetAllValidNeighbours().Count(node => !node.IsDeadEnd) <= 1)
                        {
                            bufferEndNodes.Add(neighbour);
                        }
                    }
                }

                deadEndNodes.Clear();
                deadEndNodes.AddRange(bufferEndNodes);
                bufferEndNodes.Clear();
            }
            else
            {
                currentNode.Check();
                currentNode = currentNode.GetAllValidNeighbours().Where(node => !node.IsDeadEnd).ToList()[0];
            }
        }

        public override void Solve()
        {
            startTime = Time.time;
            currentNode = MazeGenerator.GetStartNode();
            canStart = true;
            
            Node[,] allNodes = MazeGenerator.GetAllMazeNodes();

            for (int i = 0; i < MazeGenerator.Height; i++)
            {
                for (int j = 0; j < MazeGenerator.Width; j++)
                {
                    Node localNode = allNodes[i, j];

                    if (localNode.IsWall)
                    {
                        continue;
                    }
                    
                    if (localNode == MazeGenerator.GetStartNode() || localNode == MazeGenerator.GetFinishNode())
                    {
                        continue;
                    }
                    
                    if (localNode.GetAllValidNeighbours().Count(node => !node.IsDeadEnd) <= 1)
                    {
                        deadEndNodes.Add(localNode);
                    }
                }
            }
        }
    }
}