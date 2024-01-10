using UnityEngine;

namespace Solvers
{
    public class RightHandMazeSolver : LeftHandMazeSolver
    {
        protected void Update()
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
            
            PerformRightHand();
        }
    
        private void PerformRightHand()
        {
            if (CanTurnRight())
            {
                TurnRight();
                MoveForward();
            }
            else if (CanMoveForward())
            {
                MoveForward();
            }
            else
            {
                TurnLeft();
            }
        }
    
        private bool CanTurnRight()
        {
            TurnRight();
            Node rightNode = currentNode.GetNeighbour(currentDirection);
            TurnLeft();
            
            return rightNode != null && !rightNode.IsWall;
        }
    }
}

