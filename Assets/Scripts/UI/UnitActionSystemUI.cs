using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainer;
    [SerializeField] private TextMeshProUGUI actionPointText;

    private List<ActionButtonUI> actionButtonUIList;

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChange;
        UnitActionSystem.Instance.OnSelectActionChange += UnitActionSystem_OnSelectedActionChange;
        UnitActionSystem.Instance.OnActionStarted += UnityActionSystem_OnActionStarted;
        TurnSystem.Instance.OnTurnChanged +=  TurnSystem_OnTurnChanged;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

        
        UpdateActionPoints();
        CreateUnitActionButtons();
        UpdateSelectVisual();
    }

    private void CreateUnitActionButtons()
    {
        foreach (Transform buttonTransform in actionButtonContainer)
        {
            Destroy(buttonTransform.gameObject);
        }
        actionButtonUIList.Clear(); //claring list
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach (BaseAction baseAction in selectedUnit.GetBaseAction())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainer);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);

            actionButtonUIList.Add(actionButtonUI);
        }
    }

    private void UpdateSelectVisual()
    {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectVisual();
        }
    }

    private void UnityActionSystem_OnActionStarted(object sender, EventArgs e) 
    { 
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnSelectedUnitChange(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectVisual();
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnSelectedActionChange(object sender, EventArgs e)
    {
        UpdateSelectVisual();
    }


    private void UpdateActionPoints()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        actionPointText.text = "Action Point: " + selectedUnit.GetActionPoint(); ;
    }
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e){
        UpdateActionPoints();
    }

    private void Unit_OnAnyActionPointsChanged(object sender,EventArgs e){
        UpdateActionPoints();
    }
}
