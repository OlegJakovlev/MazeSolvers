using System.Collections.Generic;
using UnityEngine;

namespace Solvers
{
    public class RandomMazeSolver : MazeSolver
    {
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

            PerformRandom();
        }

        private void PerformRandom()
        {
            currentNode.ResetColor();
            List<Node> neighbours = currentNode.GetNonWallNeighbours();
            currentNode = neighbours[Random.Range(0, neighbours.Count)];
            currentNode.Check();
        }

        public override void Solve()
        {
            startTime = Time.time;
        
            currentNode = MazeGenerator.GetStartNode();
            currentNode.Check();

            canStart = true;
        }
    }
}