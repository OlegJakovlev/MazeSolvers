using UnityEngine;

public abstract class MazeSolver : MonoBehaviour
{
    protected float floatFixedStepDeltaTime;
    protected float lastUpdateTime;

    protected Node currentNode;
    protected bool canStart;

    protected int solvingSteps;
    protected float startTime;
    private float finishTime;

    public void SetStepTime(float stepDeltaTime)
    {
        floatFixedStepDeltaTime = stepDeltaTime;
    }

    public abstract void Solve();

    protected virtual void Finish()
    {
        currentNode.Check();
        finishTime = Time.time;
        
        Debug.Log("Maze solved!");
        Debug.Log($"Steps: {solvingSteps} " +
                  $"\n Step delay: {floatFixedStepDeltaTime}" +
                  $"\n Absolute solving time: {finishTime - startTime}" +
                  $"\n Relative solving time: {finishTime - startTime - solvingSteps * floatFixedStepDeltaTime}");
        
        enabled = false;
    }
}
