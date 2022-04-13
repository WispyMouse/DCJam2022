using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveTooltipShower : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MoveTooltip Tooltip;
    public float HoverTimeToShow = .6f;
    float hoverTime { get; set; } = 0;
    bool beingHovered { get; set; } = false;

    void Start()
    {
        Tooltip.Hide();
    }

    void Update()
    {
        if (beingHovered)
        {
            hoverTime += Time.deltaTime;

            if (hoverTime > HoverTimeToShow)
            {
                Tooltip.Show();
            }
        }
        else
        {
            Tooltip.Hide();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        beingHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverTime = 0;
        beingHovered = false;
    }

    private void OnDisable()
    {
        beingHovered = false;
        hoverTime = 0;
        Tooltip.Hide();
    }
}
