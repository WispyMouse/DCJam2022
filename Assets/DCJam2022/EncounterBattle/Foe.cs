using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Foe : MonoBehaviour
{
    public FoeMember DataMember { get; protected set; }
    System.Action<CombatMember> actionOnSelection { get; set; }

    public GameObject HighlightArrow;

    public Slider HealthSlider;
    public TMP_Text HealthText;
    public SpriteRenderer Renderer;

    public GameObject Root;

    public void SetDataMember(FoeMember member)
    {
        DataMember = member;
        DataMember.CurProblemJuice = DataMember.MaxProblemJuice;
        UpdateFromMember();
    }

    public void UpdateFromMember()
    {
        HealthSlider.maxValue = DataMember.MaxProblemJuice;
        HealthSlider.value = DataMember.CurProblemJuice;
        HealthText.text = DataMember.CurProblemJuice.ToString();

        if (DataMember.BattleData != null)
        {
            Renderer.sprite = DataMember.BattleData.Appearance;

            if (DataMember.BattleData.AttackPhases[DataMember.CurPhase].AppearenceInPhase != null)
            {
                Renderer.sprite = DataMember.BattleData.AttackPhases[DataMember.CurPhase].AppearenceInPhase;
            }
        }
        

        if (DataMember.CurProblemJuice <= 0)
        {
            SetKO();
        }
    }

    public void SetTargetable(System.Action<CombatMember> chosen)
    {
        actionOnSelection = chosen;
    }

    public void ClearTargetable()
    {

    }

    public void SetHighlighted()
    {
        HighlightArrow.SetActive(true);
    }

    public void SetUnhighlighted()
    {
        HighlightArrow.SetActive(false);
    }

    public void SetKO()
    {
        DataMember.Standing = false;
        Root.gameObject.SetActive(false);
    }
}
