using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GameMenuButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private RectTransform menuDrawer = default;
    [SerializeField] private RectTransform clickZone = default;
    [SerializeField] private AnimationCurve slideCurve = default;
    //[SerializeField] private Color closedColor = Color.black;
    //[SerializeField] private Color openColor = Color.grey;

    //private Image icon;

    //TODO Clean this up. Now the button only seems to open the settings menu

    private bool canClick;

    //private void Awake()
    //{
    //    icon = GetComponent<Image>();
    //}

    private void Start()
    {
        //icon.color = closedColor;

        // Hide the menu drawer
        if (menuDrawer)
        {
            menuDrawer.anchoredPosition = menuDrawer.sizeDelta.x * Vector2.right;
        }

        canClick = true;

        OpenDrawer();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!canClick) { return; }

        OpenDrawer();
    }

    public void OpenDrawer()
    {
        if (!menuDrawer) { return; }

        if (clickZone)
        {
            clickZone.gameObject.SetActive(true);
        }

        StartCoroutine(SlideMenuDrawer(0.5f, Vector2.zero));
    }

    public void CloseDrawer()
    {
        if (!menuDrawer) { return; }

        if (clickZone)
        {
            clickZone.gameObject.SetActive(false);
        }

        StartCoroutine(SlideMenuDrawer(0.5f, menuDrawer.sizeDelta.x * Vector2.right));
    }

    private IEnumerator SlideMenuDrawer(float slideTime, Vector2 targetPosition)
    {
        // Disallow clicking while drawer is sliding in/out
        canClick = false;

        float time = 0;
        Vector2 startPosition = menuDrawer.anchoredPosition;

        while (time < slideTime)
        {
            time += Time.deltaTime;
            float t = slideCurve.Evaluate(time / slideTime);
            menuDrawer.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        menuDrawer.anchoredPosition = targetPosition;
        canClick = true;
    }
}
