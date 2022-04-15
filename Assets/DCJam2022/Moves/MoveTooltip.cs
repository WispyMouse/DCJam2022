using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoveTooltip : MonoBehaviour
{
    public TMP_Text MoveName;
    public TMP_Text AbilityDescription;
    public TMP_Text Speed;
    public GameObject Root;

    bool nudged { get; set; } = false;

    public virtual void SetFromMove(MoveBase move)
    {
        this.MoveName.text = move.MoveName;
        this.AbilityDescription.text = string.Format(move.Description, move.DamageFloor, move.DamageCeiling);
        switch (move.Speed)
        {
            case SpeedTier.Slow:
                Speed.text = "Slow";
                break;
            case SpeedTier.Normal:
                Speed.text = "Normal";
                break;
            case SpeedTier.Fast:
                Speed.text = "Fast";
                break;
            default:
                Speed.text = "???";
                break;
        }

        TooltipHolder tooltipHolder = GameObject.FindObjectOfType<TooltipHolder>();

        Transform parentToUpdate = Root.transform?.parent;

        if (parentToUpdate.parent != null && parentToUpdate.parent.GetComponent<RectTransform>() != null)
        {
            parentToUpdate = parentToUpdate.parent;
        }

        RectTransform parentToRefresh = parentToUpdate?.GetComponent<RectTransform>();

        if (parentToRefresh != null && parentToRefresh.gameObject != null && parentToRefresh.gameObject.activeInHierarchy)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentToRefresh);
        }
        
        transform.SetParent(tooltipHolder.transform, true);
        Hide();
    }

    public void Show()
    {
        RectTransform pos = Root.GetComponent<RectTransform>();
        if (!nudged && pos.position.x > Screen.width / 2f)
        {
            pos.anchoredPosition = pos.anchoredPosition + Vector2.left * 300f;
            nudged = true;
        }

        Root.SetActive(true);
    }

    public void Hide()
    {
        Root.SetActive(false);
    }
}
