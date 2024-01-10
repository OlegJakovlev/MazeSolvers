using Solvers;
using UnityEngine;

public class SolverSelector : MonoBehaviour
{
    private enum SolverType
    {
        AStar,
        BFS,
        DFS,
        BiDFS,
        DeadEnd,
        LeftHand,
        RightHand,
        Pledge,
        Random,
        Soukup,
        Tremaux,
        TremauxCustom,
    }

    [SerializeField] private SolverType selectedSolver;
    [SerializeField] private float algorithmStepTime = 0.2f;
    
    private MazeSolver implementation;

    public void Initialize()
    {
        switch (selectedSolver)
        {
            case SolverType.AStar:
                implementation = gameObject.AddComponent<AStarMazeSolver>();
                break;
            
            case SolverType.BFS:
                implementation = gameObject.AddComponent<BFSMazeSolver>();
                break;
            
            case SolverType.DFS:
                implementation = gameObject.AddComponent<DFSMazeSolver>();
                break;
            
            case SolverType.BiDFS:
                implementation = gameObject.AddComponent<SimultaneousDFSMazeSolver>();
                break;
            
            case SolverType.DeadEnd:
                implementation = gameObject.AddComponent<DeadEndFillerMazeSolver>();
                break;
            
            case SolverType.LeftHand:
                implementation = gameObject.AddComponent<LeftHandMazeSolver>();
                break;
            
            case SolverType.RightHand:
                implementation = gameObject.AddComponent<RightHandMazeSolver>();
                break;
            
            case SolverType.Pledge:
                implementation = gameObject.AddComponent<PledgeMazeSolver>();
                break;
            
            case SolverType.Random:
                implementation = gameObject.AddComponent<RandomMazeSolver>();
                break;
            
            case SolverType.Soukup:
                implementation = gameObject.AddComponent<SoukupMazeSolver>();
                break;
            
            case SolverType.Tremaux:
                implementation = gameObject.AddComponent<TremauxMazeSolver>();
                break;
            
            case SolverType.TremauxCustom:
                implementation = gameObject.AddComponent<TremauxCustomMazeSolver>();
                break;
        }

        implementation.SetStepTime(algorithmStepTime);
    }

    public void Solve()
    {
        implementation.Solve();
    }
}