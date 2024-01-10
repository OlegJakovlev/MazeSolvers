using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Solvers
{
    public class DFSMazeSolver : MazeSolver
    {
        private Stack<Node> stack = new Stack<Node>();

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
            
            PerformDFS();
        }

        private void PerformDFS()
        {
            currentNode = stack.Pop();

            if (currentNode.IsChecked || currentNode.IsWall)
            {
                return;
            }

            // Mark node as visited
            currentNode.Check();

            List<Node> validNeighbours = currentNode.GetAllValidNeighbours()
                .OrderByDescending(AStarMazeSolver.EBHSA).ToList();

            // Add all neighbors to the queue
            foreach (Node neighbor in validNeighbours)
            {
                stack.Push(neighbor);
            }
        }

        public override void Solve()
        {
            startTime = Time.time;
            currentNode = MazeGenerator.GetStartNode();
        
            stack.Push(currentNode);
            canStart = true;
        }
    }
}