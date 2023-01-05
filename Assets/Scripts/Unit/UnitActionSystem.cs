//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChange;

    [SerializeField] private Unit selectUnit;
    [SerializeField] private LayerMask unitLayerMask;


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

    private void Update()
    {
        if (isBusy) { return; }
        if (Input.GetMouseButtonDown(0))
        {
            if (HandleUnitSelection()) return;

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseClick.GetPosition());

            if (selectUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
            {
                SetBusy();
                selectUnit.GetMoveAction().Move(mouseGridPosition,ClearBusy);
            }

            // selectUnit.GetMoveAction().Move(MouseClick.GetPosition());
        }

        if (Input.GetMouseButtonDown(1))
        {
            SetBusy();
            selectUnit.GetSpinAction().Spin(ClearBusy);
        }
    }

    private void SetBusy()
    {
        isBusy = true;
    }

    private void ClearBusy()
    {
        isBusy = false;
    }

    private bool HandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
        {
            if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectedUnit(unit);
                return true;
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectUnit = unit;

        OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);
        //if(OnSelectedUnitChange != null)
        //{
        //    OnSelectedUnitChange(this, EventArgs.Empty);
        //}
    }

    public Unit GetSelectedUnit()
    {
        return selectUnit;
    }
}
