using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectedDelverBottom : MonoBehaviour
{
    public DelverProfile Profile { get; set; }
    public Image SelectedImage;
    public TMP_Text DungeoneerName;
    public Transform MovesHolder;
    public PartyMoveSelectionLabel MoveName;

    public void SetDelver(DelverProfile profile)
    {
        Profile = profile;

        SelectedImage.sprite = profile.ChooseAPartyMemberPicture;

        for (int ii = 0; ii < MovesHolder.childCount; ii++)
        {
            Destroy(MovesHolder.GetChild(ii).gameObject);
        }

        foreach (PlayerMove move in Profile.AttackOptions)
        {
            PartyMoveSelectionLabel text = Instantiate(MoveName, MovesHolder);
            text.SetFromMove(move);
        }

        DungeoneerName.text = profile.ProfileName;
    }
}
