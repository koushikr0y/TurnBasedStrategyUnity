//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitVisualSelect : MonoBehaviour
{
    [SerializeField] Unit unit;
    private MeshRenderer meshRenderer;
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectUnitChange;
        UpdateVisual();
    }

    private void UnitActionSystem_OnSelectUnitChange(object sender,EventArgs empty)
    {
        UpdateVisual();
    }

    private void UpdateVisual() 
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() == unit)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }
}
