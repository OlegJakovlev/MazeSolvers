using UnityEngine;

namespace Solvers
{
    public class PledgeMazeSolver : LeftHandMazeSolver
    {
        private bool followingWall;
        private int rotations;

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
            
            PerformPledge();
        }

        private void PerformPledge()
        {
            if (followingWall && rotations == 0)
            {
                followingWall = false;
            }

            if (!followingWall)
            {
                // "When the solver is facing the original direction again,
                // and the angular sum of the turns made is 0,
                // the solver leaves the obstacle and continues
                // moving in its original direction." (Wikipedia)
                if (CanMoveForward())
                {
                    MoveForward();
                }
                else
                {
                    // We can't go forward, so start following the wall.
                    followingWall = true;

                    // We want to have the wall on the left, so we turn right.
                    TurnRight();
                }

                return;
            }

            if (followingWall)
            {
                // Can we go left?
                TurnLeft();
                if (CanMoveForward())
                {
                    MoveForward();
                    return;
                }

                // Restore initial rotation
                TurnRight();

                int i = 0;

                Node forwardNode = currentNode.GetNeighbour(currentDirection);
                bool canMoveForward = CanMoveForward();
                
                // Try to right turn despite everything and go forward, ensure we are not marking it twice
                if (canMoveForward && forwardNode.IsChecked && forwardNode.MarkCount == 2)
                {
                    TurnRight();
                    
                    if (CanMoveForward())
                    {
                        MoveForward();
                        return;
                    }

                    TurnLeft();
                }
                
                while (!CanMoveForward())
                {
                    TurnRight();
                    i++;

                    if (i >= 4)
                    {
                        Debug.LogWarning("We cannot move in any way! I want to die!");
                        return;
                    }
                }

                MoveForward();
            }
        }

        protected override void TurnRight()
        {
            base.TurnRight();
            rotations++;
        }

        protected override void TurnLeft()
        {
            base.TurnLeft();
            rotations--;
        }

        public override void Solve()
        {
            rotations = 0;
            base.Solve();
        }
    }
}