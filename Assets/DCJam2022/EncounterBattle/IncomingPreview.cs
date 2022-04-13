using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomingPreview : MonoBehaviour
{
    public Transform PreviewRowParent;
    public IncomingPreviewRow RowPF;

    public void SetFromRemaining(List<EncounterWave> WavesRemaining)
    {
        for (int ii = 0; ii < PreviewRowParent.childCount; ii++)
        {
            Destroy(PreviewRowParent.GetChild(ii).gameObject);
        }

        foreach (EncounterWave wave in WavesRemaining)
        {
            IncomingPreviewRow piece = Instantiate(RowPF, PreviewRowParent);
            piece.SetFromRow(wave);
        }
    }
}
