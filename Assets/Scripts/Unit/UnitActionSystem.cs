//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChange;
    public event EventHandler OnSelectActionChange;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;


    [SerializeField] private Unit selectUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private BaseAction selectedAction;
    private bool isBusy;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Error"); Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SetSelectedUnit(selectUnit);
    }

    private void Update()
    {
        if (isBusy) { return; }
        // if (Input.GetMouseButtonDown(0))
        // {
        if (!TurnSystem.Instance.IsPlayerTurn()) { return; }
        if (EventSystem.current.IsPointerOverGameObject()) { return; }
        if (HandleUnitSelection()) return;
        HandleSelectedAction();
        // GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseClick.GetPosition());

        // if (selectUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
        // {
        //     SetBusy();
        //     selectUnit.GetMoveAction().Move(mouseGridPosition, ClearBusy);
        // }
        // selectUnit.GetMoveAction().Move(MouseClick.GetPosition());
        // }

        // if (Input.GetMouseButtonDown(1))
        // {
        //     SetBusy();
        //     selectUnit.GetSpinAction().Spin(ClearBusy);
        // }
    }

    private void HandleSelectedAction()
    {
        if (Input.GetMouseButton(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseClick.GetPosition());

            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition)) { return; }

            if (!selectUnit.TrySpendActionPointToTakeAction(selectedAction)) { return; }

            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);

            OnActionStarted?.Invoke(this, EventArgs.Empty);

            // if (selectedAction.IsValidActionGridPosition(mouseGridPosition))
            // {
            //     if (selectUnit.CanSpendActionPointsToTakeAction(selectedAction))
            //     {
            //             SetBusy();
            //             selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            //     }
            // }

            // switch (selectedAction)
            // {
            //     case MoveAction moveAction:
            //         if (moveAction.IsValidActionGridPosition(mouseGridPosition))
            //         {
            //             SetBusy();
            //             moveAction.Move(mouseGridPosition, ClearBusy);
            //         }
            //         break;
            //     case SpinAction spinAction:
            //         SetBusy();
            //         spinAction.Spin(ClearBusy);
            //         break;
            // }
        }
    }

    private void SetBusy()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    private void ClearBusy()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(this, isBusy);

    }

    private bool HandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit == selectUnit)
                    {
                        return false;
                    }

                    if (unit.IsEnemy()) { return false; }
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectUnit = unit;
        SetSelectionAction(unit.GetMoveAction());
        OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectionAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
        OnSelectActionChange?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }
}
