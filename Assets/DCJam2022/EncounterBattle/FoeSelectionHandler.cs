using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FoeSelectionHandler : MonoBehaviour
{
    public Camera BattleCamera;
    public Action<CombatMember> ChosenAction { get; set; }
    Foe lastHighlighted { get; set; } = null;

    void Update()
    {
        bool buttonHit = false;
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            buttonHit = true;
        }

        Ray castRay = BattleCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        Foe foundFoe = null;

        if (Physics.Raycast(castRay, out hit))
        {
            // can be null
            foundFoe = hit.collider.gameObject.GetComponent<Foe>();
        }

        if (foundFoe == null)
        {
            lastHighlighted?.SetUnhighlighted();

            lastHighlighted = null;
            return;
        }

        lastHighlighted?.NMEPreviewInstance.Hide();
        foundFoe.NMEPreviewInstance.Show();

        if (ChosenAction == null)
        {
            lastHighlighted = foundFoe;
            return;
        }

        if (buttonHit)
        {
            ChosenAction(foundFoe.DataMember);
        }
        else
        {
            if (foundFoe != lastHighlighted)
            {
                foundFoe.SetHighlighted();

                if (lastHighlighted != null && lastHighlighted.gameObject != null)
                {
                    lastHighlighted?.SetUnhighlighted();
                }
                lastHighlighted = foundFoe;
            }
        }
    }
}
