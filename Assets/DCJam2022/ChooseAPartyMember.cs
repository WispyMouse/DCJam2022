using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseAPartyMember : MonoBehaviour
{
    public DelverProfile Profile;
    public Image Renderer;

    void Awake()
    {
        Renderer.sprite = Profile.ChooseAPartyMemberPicture;
    }

    public void Clicked()
    {
        GameObject.FindObjectOfType<PartySelectionSceneHelperTools>().SelectDelver(Profile);
    }
}
