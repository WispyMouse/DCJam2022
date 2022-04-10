using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedDelverBottom : MonoBehaviour
{
    public DelverProfile Profile { get; set; }
    public Image SelectedImage;

    public void SetDelver(DelverProfile profile)
    {
        Profile = profile;

        SelectedImage.sprite = profile.ChooseAPartyMemberPicture;
    }
}
