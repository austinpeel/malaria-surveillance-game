using UnityEngine;
using UnityEngine.EventSystems;

public class SliderHandle : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        SendMessageUpwards("ToggleSlider", SendMessageOptions.DontRequireReceiver);
    }
}
