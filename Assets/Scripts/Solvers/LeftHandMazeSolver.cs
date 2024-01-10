using UnityEngine;

namespace Solvers
{
    public class LeftHandMazeSolver : MazeSolver
    {
        protected Node.NodeDirection currentDirection;

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
            
            PerformLeftHand();
        }

        private void PerformLeftHand()
        {
            if (CanTurnLeft())
            {
                TurnLeft();
                MoveForward();
            }
            else if (CanMoveForward())
            {
                MoveForward();
            }
            else
            {
                TurnRight();
            }
        }

        private bool CanTurnLeft()
        {
            TurnLeft();
            Node leftNode = currentNode.GetNeighbour(currentDirection);
            TurnRight();
            
            return leftNode != null && !leftNode.IsWall;
        }

        protected bool CanMoveForward()
        {
            Node forwardNode = currentNode.GetNeighbour(currentDirection);
            return forwardNode != null && !forwardNode.IsWall;
        }

        protected virtual void TurnLeft()
        {
            switch (currentDirection)
            {
                case Node.NodeDirection.Up:
                    currentDirection = Node.NodeDirection.Left;
                    break;
                
                case Node.NodeDirection.Left:
                    currentDirection = Node.NodeDirection.Down;
                    break;
                
                case Node.NodeDirection.Down:
                    currentDirection = Node.NodeDirection.Right;
                    break;
                
                case Node.NodeDirection.Right:
                    currentDirection = Node.NodeDirection.Up;
                    break;
            }
        }

        protected virtual void TurnRight()
        {
            switch (currentDirection)
            {
                case Node.NodeDirection.Up:
                    currentDirection = Node.NodeDirection.Right;
                    break;
                
                case Node.NodeDirection.Right:
                    currentDirection = Node.NodeDirection.Down;
                    break;
                
                case Node.NodeDirection.Down:
                    currentDirection = Node.NodeDirection.Left;
                    break;
                
                case Node.NodeDirection.Left:
                    currentDirection = Node.NodeDirection.Up;
                    break;
            }
        }

        protected void MoveForward()
        {
            Node nextNode = currentNode.GetNeighbour(currentDirection);

            if (nextNode != null)
            {
                currentNode.ResetColor();
                currentNode = nextNode;
                currentNode.Check();
            }
        }

        public override void Solve()
        {
            startTime = Time.time;
            
            currentNode = MazeGenerator.GetStartNode();
            currentNode.Check();
            
            currentDirection = Node.NodeDirection.Up;
            canStart = true;
        }
    }
}

