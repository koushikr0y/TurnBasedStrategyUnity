// using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MoveAction : BaseAction
{
    private Vector3 targetPosition;
    // private Unit unit;
    // private bool isActive;
    [SerializeField] private Animator unitAnimator;

    private readonly float _moveSpeed = 4f;
    private readonly float _rotateSpeed = 10f;
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
            unitAnimator.SetBool("IsWalking", true);
        }
        else
        {
            unitAnimator.SetBool("IsWalking", false);
            isActive = false;
            onActionComplete();
        }

        transform.position += _moveSpeed * Time.deltaTime * moveDir;
        transform.forward = Vector3.Lerp(transform.forward, moveDir, Time.deltaTime * _rotateSpeed);
    }

    public void Move(GridPosition gridPosition,Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        isActive = true;
    }

    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList()
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


}
