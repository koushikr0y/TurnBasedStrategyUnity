using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private enum STATE
    {
        AIMING,
        SHOOTING,
        COOLOFF
    }
    private STATE state;

    private float stateTimer;
    private int maxShootDistance = 7;
    private int shootDamage = 40;
    private Unit targetUnit;

    private bool canShootBullet;

    private void Update()
    {
        if (!isActive) { return; }

        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case STATE.AIMING:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;

                float _rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * _rotateSpeed);
                break;
            case STATE.SHOOTING:
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case STATE.COOLOFF:
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case STATE.AIMING:
                state = STATE.SHOOTING;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                break;
            case STATE.SHOOTING:
                state = STATE.COOLOFF;
                float coolOffStateTime = 0.5f;
                stateTimer = coolOffStateTime;
                break;
            case STATE.COOLOFF:
                // isActive = false;
                // onActionComplete();
                ActionComplete();
                break;
        }
    }

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs { targetUnit = targetUnit,
        shootingUnit = unit});
        targetUnit.Damage(shootDamage);
    }
    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                // int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                // if (testDistance > maxShootDistance) { continue; }
                // validGridPositionList.Add(testGridPosition);

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //Grid Position is empty no unit
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtgridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    continue;       //both unit at same team
                }

                validGridPositionList.Add(testGridPosition);

            }
        }
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        // this.onActionComplete = onActionComplete;
        // isActive = true;
        ActionStart(onActionComplete);

        targetUnit = LevelGrid.Instance.GetUnitAtgridPosition(gridPosition);

        state = STATE.AIMING;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;

        canShootBullet = true;
    }
}
