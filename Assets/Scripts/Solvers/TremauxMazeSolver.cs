using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Solvers
{
    public class TremauxMazeSolver : MazeSolver
    {
        private Stack<Node> stack = new Stack<Node>();
        private Node prevNode;
        
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
    
            PerformTremaux();
        }
    
        private void PerformTremaux()
        {
            prevNode = currentNode;
            prevNode.ResetColor();
            currentNode = stack.Pop();
            currentNode.Check();
    
            List<Node> neighbours = currentNode.GetNonWallNeighbours()
                .Where(node => node != prevNode)
                .Where(node => node.MarkCount < 2)
                .OrderBy(node => node.MarkCount)
                .ToList();
    
            if (neighbours.Count > 1)
            {
                prevNode?.Mark();
    
                // Check if junction
                if (neighbours[0].GetAllValidNeighbours().Count - 1 > 1)
                {
                    currentNode.Mark();
                }
                else
                {
                    currentNode.ResetColor();
                    neighbours[0].Mark();
                }
            }
    
            if (neighbours.Count >= 1)
            {
                stack.Push(neighbours[0]);
            }
    
            if (neighbours.Count == 0)
            {
                currentNode.Mark();
                stack.Push(prevNode);
            }
        }
    
        public override void Solve()
        {
            startTime = Time.time;
            
            currentNode = MazeGenerator.GetStartNode();
            currentNode.Check();
            
            stack.Push(currentNode);
            
            canStart = true;
        }
    }
}

