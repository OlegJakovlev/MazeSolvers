using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Node : MonoBehaviour
{
    public enum NodeDirection
    {
        Up,
        Down,
        Left,
        Right
    };

    public bool IsChecked { get; private set; }
    public bool IsStart { get; private set; }
    public bool IsFinish { get; private set; }
    public bool IsWall { get; private set; }
    public bool IsDeadEnd { get; private set; }
    public int NodeID { get; private set; }
    public int RowID { get; private set; }
    public float Cost { get; set; } = Int32.MaxValue;
    public int MarkCount { get; private set; }
    
    public bool IsPartOfStartPath;
    public bool IsPartOfFinishPath;

    [SerializeField] private Material startNodeMaterial;
    [SerializeField] private Material finishNodeMaterial;
    [SerializeField] private Material currentNodeMaterial;
    [SerializeField] private Material checkedMaterial;
    [SerializeField] private Material deadEndMaterial;
    [SerializeField] private Material markedMaterial;
    [SerializeField] private Material solutionPartMaterial;
    
    private Renderer materialRenderer;

    private HashSet<Node> neighbours = new HashSet<Node>();
    private Dictionary<NodeDirection, Node> directionNeighbours = new Dictionary<NodeDirection, Node>();

    private void Awake()
    {
        materialRenderer = GetComponent<Renderer>();
    }

    public void Initialize(int ID, int rowID, bool isStart, bool isFinish, bool isWall)
    {
        NodeID = ID;
        RowID = rowID;
        IsStart = isStart;
        IsFinish = isFinish;
        IsWall = isWall;
    }

    public static void LinkNeighbours(Node one, Node two)
    {
        one.AddNeighbour(two);
        two.AddNeighbour(one);
    }

    public Node GetNeighbour(NodeDirection dir)
    {
        return directionNeighbours.TryGetValue(dir, out Node node) ? node : null;
    }

    public List<Node> GetAllValidNeighbours()
    {
        return neighbours.Where(node => !node.IsChecked && !node.IsWall).ToList();
    }

    public List<Node> GetNonWallNeighbours()
    {
        return neighbours.Where(node => !node.IsWall).ToList();
    }

    public List<Node> GetAllNeighbours()
    {
        return neighbours.ToList();
    }

    public List<Node> GetCheckedNeighbours()
    {
        return neighbours.Where(x => x.IsChecked).ToList();
    }

    public void ResetColor()
    {
        if (IsStart)
        {
            materialRenderer.material = startNodeMaterial;
        }
        else if (IsFinish)
        {
            materialRenderer.material = finishNodeMaterial;
        }
        else if (MarkCount == 2)
        {
            materialRenderer.material = markedMaterial;
        }
        else if (IsChecked)
        {
            materialRenderer.material = checkedMaterial;
        }
        else
        {
            materialRenderer.material = default;
        }
    }

    public void Check()
    {
        if (IsChecked)
        {
            materialRenderer.material = currentNodeMaterial;
            return;
        }

        if (!IsStart)
        {
            materialRenderer.material = checkedMaterial;
        }
        
        IsChecked = true;
    }

    public void Mark()
    {
        MarkCount++;

        if (MarkCount == 2)
        {
            materialRenderer.material = markedMaterial;
        }
    }

    public void SetAsDeadEnd()
    {
        if (IsDeadEnd)
        {
            return;
        }
        
        materialRenderer.material = deadEndMaterial;
        IsDeadEnd = true;
    }

    public void MarkAsPartOfSolution()
    {
        materialRenderer.material = solutionPartMaterial;
    }
    
    public void Print()
    {
        int right = -1;
        if (directionNeighbours.TryGetValue(NodeDirection.Right, out Node rightVal))
        {
            right = rightVal.NodeID;
        }
        
        int left = -1;
        if (directionNeighbours.TryGetValue(NodeDirection.Left, out Node leftVal))
        {
            left = leftVal.NodeID;
        }
        
        int top = -1;
        if (directionNeighbours.TryGetValue(NodeDirection.Up, out Node topVal))
        {
            top = topVal.NodeID;
        }
        
        int bottom = -1;
        if (directionNeighbours.TryGetValue(NodeDirection.Down, out Node bottomVal))
        {
            bottom = bottomVal.NodeID;
        }

        Debug.Log($"My ID: {NodeID}" +
                  $"\n My Cost: {Cost}" +
                  $"\n Right Neighbour: {right}" +
                  $"\n Left Neighbour: {left} " +
                  $"\n Top Neighbour: {top} " +
                  $"\n Bottom Neighbour: {bottom}");
    }

    private void AddNeighbour(Node neighbour)
    {
        if (NodeID == neighbour.NodeID - 1)
        {
            if (!directionNeighbours.ContainsKey(NodeDirection.Right))
            {
                directionNeighbours.Add(NodeDirection.Right, neighbour);
            }
        }

        if (NodeID == neighbour.NodeID + 1)
        {
            if (!directionNeighbours.ContainsKey(NodeDirection.Left))
            {
                directionNeighbours.Add(NodeDirection.Left, neighbour);
            }
        }

        if (RowID + 1 == neighbour.RowID)
        {
            if (!directionNeighbours.ContainsKey(NodeDirection.Down))
            {
                directionNeighbours.Add(NodeDirection.Down, neighbour);
            }
        }
        
        if (RowID - 1 == neighbour.RowID)
        {
            if (!directionNeighbours.ContainsKey(NodeDirection.Up))
            {
                directionNeighbours.Add(NodeDirection.Up, neighbour);
            }
        }

        neighbours.Add(neighbour);
    }
}
