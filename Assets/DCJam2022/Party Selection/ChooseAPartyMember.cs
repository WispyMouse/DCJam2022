using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChooseAPartyMember : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public DelverProfile Profile;
    public Image Renderer;
    public PartySelectionSceneHelperTools HelperTools;

    void Awake()
    {
        Renderer.sprite = Profile.ChooseAPartyMemberPicture;
    }

    public void Clicked()
    {
        GameObject.FindObjectOfType<PartySelectionSceneHelperTools>().SelectDelver(Profile);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HelperTools.HoveredDungeoneer(Profile);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HelperTools.ClearHoveredDungeoneer(Profile);
    }
}
