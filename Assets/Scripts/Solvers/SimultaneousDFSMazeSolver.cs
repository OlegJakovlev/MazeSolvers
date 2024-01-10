using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Solvers
{
    public class SimultaneousDFSMazeSolver : MazeSolver
    {
        Stack<Node> startStack = new Stack<Node>();
        Stack<Node> finishStack = new Stack<Node>();

        private void Update()
        {
            if (!canStart)
            {
                lastUpdateTime = Time.time;
                return;
            }

            if (Time.time - lastUpdateTime < floatFixedStepDeltaTime)
            {
                return;
            }
            
            lastUpdateTime = Time.time;
            solvingSteps++;
            
            PerformDFSStart();
            PerformDFSFinish();
        }

        private void PerformDFSStart()
        {
            currentNode = startStack.Pop();

            if (currentNode.IsWall)
            {
                return;
            }

            switch (currentNode.IsChecked)
            {
                case true when !currentNode.IsPartOfFinishPath:
                    return;
                
                case true when currentNode.IsPartOfFinishPath:
                    Finish();
                    return;
            }

            // Mark node as visited
            currentNode.Check();
            currentNode.IsPartOfStartPath = true;

            List<Node> validNeighbours = currentNode.GetAllNeighbours().Where(node => !node.IsWall).ToList();

            // Add all neighbors to the queue
            foreach (Node neighbor in validNeighbours)
            {
                startStack.Push(neighbor);
            }
        }
        
        private void PerformDFSFinish()
        {
            currentNode = finishStack.Pop();

            switch (currentNode.IsChecked)
            {
                case true when !currentNode.IsPartOfStartPath:
                    return;
                
                case true when currentNode.IsPartOfStartPath:
                    Finish();
                    return;
            }

            // Mark node as visited
            currentNode.Check();
            currentNode.IsPartOfFinishPath = true;

            List<Node> validNeighbours = currentNode.GetAllNeighbours().Where(node => !node.IsWall).ToList();

            // Add all neighbors to the queue
            foreach (Node neighbor in validNeighbours)
            {
                finishStack.Push(neighbor);
            }
        }

        public override void Solve()
        {
            startTime = Time.time;
            currentNode = MazeGenerator.GetStartNode();
        
            startStack.Push(currentNode);
            finishStack.Push(MazeGenerator.GetFinishNode());
            
            canStart = true;
        }
    }
}