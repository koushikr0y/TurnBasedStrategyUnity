//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionsArray;
    private int actionPoints = 2;        //action points


    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionsArray = GetComponents<BaseAction>();
    }
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMoveGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    public BaseAction[] GetBaseAction()
    {
        return baseActionsArray;
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public bool TrySpendActionPointToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoint(baseAction.GetActionPointCost());
            return true;
        }
        else { return false; }
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {

        return actionPoints >= baseAction.GetActionPointCost();

        // if (actionPoints >= baseAction.GetActionPointCost())
        // {
        //     return true;
        // }
        // else
        // {
        //     return false;
        // }
    }

    private void SpendActionPoint(int amount)
    {
        actionPoints -= amount;
    }

    public int GetActionPoint()
    {
        return actionPoints;
    }

}
