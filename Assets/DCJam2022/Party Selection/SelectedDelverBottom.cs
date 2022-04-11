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
    public TMP_Text MoveName;

    public void SetDelver(DelverProfile profile)
    {
        Profile = profile;

        SelectedImage.sprite = profile.ChooseAPartyMemberPicture;

        for (int ii = 0; ii < MovesHolder.childCount; ii++)
        {
            Destroy(MovesHolder.GetChild(ii).gameObject);
        }

        foreach (string move in Profile.AttackOptions)
        {
            TMP_Text text = Instantiate(MoveName, MovesHolder);
            text.text = move;
        }
        DungeoneerName.text = profile.ProfileName;
    }
}
