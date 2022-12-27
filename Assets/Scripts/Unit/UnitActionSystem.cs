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

    private void Awake()
    {
        if (Instance != null) 
        { 
            Debug.Log("Error");Destroy(gameObject);
            return; 
        } 
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(HandleUnitSelection()) return;

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseClick.GetPosition());

            if(selectUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition)){
                selectUnit.GetMoveAction().Move(mouseGridPosition);
            }

            // selectUnit.GetMoveAction().Move(MouseClick.GetPosition());
        }
    }

    private bool HandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
        {
            if(raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
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
