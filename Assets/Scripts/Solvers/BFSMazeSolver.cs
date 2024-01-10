using System.Collections.Generic;
using UnityEngine;

namespace Solvers
{
    public class BFSMazeSolver : MazeSolver
    {
        private Queue<Node> queue = new Queue<Node>();

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
            
            PerformBFS();
        }

        private void PerformBFS()
        {
            currentNode = queue.Dequeue();

            if (currentNode.IsChecked || currentNode.IsWall)
            {
                return;
            }

            // Mark node as visited
            currentNode.Check();

            List<Node> validNeighbours = currentNode.GetAllValidNeighbours();

            // Add all neighbors to the queue
            foreach (Node neighbor in validNeighbours)
            {
                queue.Enqueue(neighbor);
            }
        }

        public override void Solve()
        {
            startTime = Time.time;
            currentNode = MazeGenerator.GetStartNode();
        
            queue.Enqueue(currentNode);
            canStart = true;
        }
    }
}