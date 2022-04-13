using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncomingPreviewPiece : MonoBehaviour
{
    public Image Graphic;
    public void SetFromEncounterPhase(FoeEncounterPhase phase)
    {
        Graphic.sprite = phase.EncounteredFoe.AttackPhases[phase.FoeStartingPhase].AppearenceInPhase;
    }
}
