using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode, RequireComponent(typeof(RectTransform))]
public class Person : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Sprites")]
    public Image icon;
    public Image aura;
    public Image glow;

    // TODO move sprites to GameManager for better memory efficiency?
    [Header("Icons")]
    public Sprite notPregnantIcon;
    public Sprite pregnantIcon;

    [Header("Properties")]
    public bool isInfected;
    public bool isSymptomatic;
    public bool isPregnant;

    private RectTransform rectTransform;

    public Vector2 Position => rectTransform.anchoredPosition;
    public float Size => (rectTransform.sizeDelta * rectTransform.localScale).magnitude;

    //[HideInInspector] public int index;
    [HideInInspector] public List<int> neighborIndices;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (icon)
        {
            icon.gameObject.SetActive(true);
        }

        if (aura)
        {
            aura.gameObject.SetActive(false);
        }

        if (glow)
        {
            glow.gameObject.SetActive(false);
        }
    }

    public void Infect(bool isSymptomatic)
    {
        isInfected = true;
        this.isSymptomatic = isSymptomatic;

        if (!icon) { return; }
        icon.color = this.isSymptomatic ? ColorPalette.symptomatic : ColorPalette.healthy;
        SynchronizeColors(icon.color);
    }

    public void Heal()
    {
        isInfected = false;
        isSymptomatic = false;

        if (!icon) { return; }
        icon.color = ColorPalette.healthy;
        SynchronizeColors(icon.color);
    }

    public void RevealTrueInfection()
    {
        if (!icon) { return; }

        icon.color = isInfected ? ColorPalette.symptomatic : ColorPalette.healthy;
        SynchronizeColors(icon.color);
    }

    public void HideTrueInfection()
    {
        if (!icon) { return; }

        if (isInfected && !isSymptomatic)
        {
            icon.color = ColorPalette.healthy;
            SynchronizeColors(icon.color);
        }
    }

    private void SynchronizeColors(Color color)
    {
        if (aura)
        {
            color.a = 0.2f;
            aura.color = color;
        }

        if (glow)
        {
            glow.color = color;
        }
    }

    public void TriggerAura()
    {
        if (aura)
        {
            aura.gameObject.SetActive(true);
            StartCoroutine(Utils.ScaleAndFadeUI(aura.GetComponent<RectTransform>(), 0.6f, 5, true));
        }
    }

    public void Highlight()
    {
        if (glow)
        {
            glow.gameObject.SetActive(true);
        }
    }

    public void Unhighlight()
    {
        if (glow)
        {
            glow.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SendMessageUpwards("HandlePersonClicked", this, SendMessageOptions.DontRequireReceiver);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SendMessageUpwards("HandlePersonMouseEnter", this, SendMessageOptions.DontRequireReceiver);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SendMessageUpwards("HandlePersonMouseExit", this, SendMessageOptions.DontRequireReceiver);
    }
}
