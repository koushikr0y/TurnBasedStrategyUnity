// using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MoveAction : BaseAction
{

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    private Vector3 targetPosition;
    // private Unit unit;
    // private bool isActive;

    private readonly float _moveSpeed = 4f;

    private float stopingDistance = .1f;

    [SerializeField] private int maxMoveDistance = 4;


    protected override void Awake()
    {
        // unit = GetComponent<Unit>();
        base.Awake();
        targetPosition = transform.position;
    }
    private void Update()
    {
        if (!isActive) { return; }

        Vector3 moveDir = (targetPosition - transform.position).normalized;

        if (Vector3.Distance(transform.position, targetPosition) > stopingDistance)
        {
            transform.position += _moveSpeed * Time.deltaTime * moveDir;
            // unitAnimator.SetBool("IsWalking", true);
        }
        else
        {
            // unitAnimator.SetBool("IsWalking", false);
            OnStopMoving?.Invoke(this, EventArgs.Empty);
            ActionComplete();
        }

    }

    // public void Move(GridPosition gridPosition, Action onActionComplete)
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        OnStartMoving?.Invoke(this, EventArgs.Empty);
        // this.onActionComplete = onActionComplete;
        // isActive = true;
    }

    // public bool IsValidActionGridPosition(GridPosition gridPosition)
    // {
    //     List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
    //     return validGridPositionList.Contains(gridPosition);
    // }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }
}
