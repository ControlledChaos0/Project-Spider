using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SmallHoldableState : BehaviorState {
    private SmallHoldable _smallHoldable;
    private SmallHoldableRoutine _smallHoldableRoutine;
    private GridCell[] _neighbors;
    private GridCell _selectedCell;
    private bool _startHeld;

    public override void EnterState()
    {
        Activate();
    }
    public override void UpdateState()
    {
        
    }
    public override void ExitState()
    {
        Deactivate();
    }

    public void Activate() {
        _smallHoldable = behaviorRoutine.Behavior as SmallHoldable;
        _smallHoldableRoutine = behaviorRoutine as SmallHoldableRoutine;
        _startHeld = _smallHoldableRoutine.TempIsHeld;
        if (_startHeld) {
            ActivatePutDown();
        } else {
            ActivatePickUp();
        }
    }
    private void ActivatePutDown() {
        GridManager.Instance.Activate();
        _neighbors = _smallHoldable.HeldEntity.OccupiedCell.Neighbors;
        foreach (GridCell gridCell in _neighbors) {
            gridCell.ShowCell();
        }
        _smallHoldableRoutine.TempIsHeld = true;
        _smallHoldableRoutine.TempHeldEntity = TurnSystem.Instance.ActiveEntity;
        GridManager.Instance.HoverAction += ShowCell;
        GridManager.Instance.ClickAction += ChooseCell;
    }
    private void ActivatePickUp() {
        _smallHoldableRoutine.TempIsHeld = false;
        _smallHoldableRoutine.TempHeldEntity = TurnSystem.Instance.ActiveEntity;
        _smallHoldable.ExecuteBehavior();
    }
    public void Deactivate() {
        if (_startHeld) {
            DeactivatePutDown();
        } else {
            DeactivatePickUp();
        }
    }
    private void DeactivatePutDown() {
        foreach (GridCell gridCell in _neighbors) {
            gridCell.HideCell();
        }
        _neighbors = null;
        GridManager.Instance.Deactivate();
        GridManager.Instance.HoverAction -= ShowCell;
        GridManager.Instance.ClickAction -= ChooseCell;
    }
    private void DeactivatePickUp() {

    }

    private void ShowCell(GridCell cell)
    {
        if (cell == null || !cell.IsShowing() || !_neighbors.Contains(cell))
        {
            _selectedCell?.RevertColor();
            _selectedCell = null;
            return;
        }

        if (_selectedCell != null && _selectedCell.Equals(cell))
        {
            return;
        }
            
        _selectedCell?.RevertColor();
        _selectedCell = cell;
        _selectedCell.TurnBlue();
    }

    private void ChooseCell(GridCell cell)
    {
        if (!_selectedCell || !_selectedCell.Equals(cell))
        {
            return;
        }

        _selectedCell.RevertColor();
        _smallHoldable.PlacedCell = _selectedCell;
        _selectedCell = null;
        _smallHoldable.ExecuteBehavior();
    }
}