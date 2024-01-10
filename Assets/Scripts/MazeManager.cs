using UnityEngine;

public class MazeManager : MonoBehaviour
{
    [SerializeField] private MazeGenerator mazeGenerator;
    [SerializeField] private SolverSelector solveSelector;

    private void Awake()
    {
        mazeGenerator.Initialize();
        solveSelector.Initialize();
    }

    private void Start()
    {
        solveSelector.Solve();
    }
}
