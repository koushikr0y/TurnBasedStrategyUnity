using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float totalSpinAmount;
    // private bool isActive;

    // public delegate void SpinCompleteDelegate();
    // private SpinCompleteDelegate onSpinComplete;

    // private Action onSpinComplete;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        float spinAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAmount, 0);

        totalSpinAmount += spinAmount;
        if (totalSpinAmount >= 360f)
        {
            isActive = false;
            onActionComplete();
        }
    }
    // public void Spin(Action onSpinComplete)
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        // this.onActionComplete = onSpinComplete;
        this.onActionComplete = onActionComplete;
        isActive = true;
        totalSpinAmount = 0f;
    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {

       // List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition>{
            unitGridPosition
        };
    }

    public override int GetActionPointCost()
    {
        return 2;
    }

}
