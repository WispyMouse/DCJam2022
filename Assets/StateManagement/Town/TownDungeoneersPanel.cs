using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownDungeoneersPanel : MonoBehaviour
{
    public TownSceneHelperTools Tools;

    public List<Image> DungeoneerImages;
    public List<GameObject> DungeonImageRoot;

    private void OnEnable()
    {
        StartCoroutine(UpdatePanel());
    }

    public IEnumerator UpdatePanel()
    {
        while (Tools?.SceneHelperInstance?.PlayerParty == null)
        {
            yield return new WaitForEndOfFrame();
        }

        for (int ii = 0; ii < DungeoneerImages.Count; ii++)
        {
            if (Tools.SceneHelperInstance.PlayerParty.PartyMembers.Count > ii)
            {
                DungeoneerImages[ii].sprite = Tools.SceneHelperInstance.PlayerParty.PartyMembers[ii].FromProfile.ChooseAPartyMemberPicture;
                DungeonImageRoot[ii].gameObject.SetActive(true);
            }
            else
            {
                DungeonImageRoot[ii].gameObject.SetActive(false);
            }
        }
    }
}
