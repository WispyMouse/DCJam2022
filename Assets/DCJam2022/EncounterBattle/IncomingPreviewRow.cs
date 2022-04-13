using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomingPreviewRow : MonoBehaviour
{
    public IncomingPreviewPiece PreviewPiecePF;

    public Transform PieceParent;

    public void SetFromRow(EncounterWave fromWave)
    {
        for (int ii = 0; ii < PieceParent.childCount; ii++)
        {
            Destroy(PieceParent.GetChild(ii).gameObject);
        }

        foreach (FoeEncounterPhase encounterPhase in fromWave.FoesInWave)
        {
            IncomingPreviewPiece piece = Instantiate(PreviewPiecePF, PieceParent);
            piece.SetFromEncounterPhase(encounterPhase);
        }
    }
}
