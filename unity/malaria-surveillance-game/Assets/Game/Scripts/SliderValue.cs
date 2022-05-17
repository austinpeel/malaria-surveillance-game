using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderValue : MonoBehaviour
{
    public TextMeshProUGUI valueTMP;
    public bool onOff;
    public int decimalDigits = 1;

    public void SetSliderValue(float value)
    {
        if (valueTMP)
        {
            if (onOff)
            {
                valueTMP.text = value == 0 ? "off" : "on";
            }
            else
            {
                valueTMP.text = value.ToString("F" + decimalDigits.ToString());
            }
        }
    }

    public void ToggleSlider()
    {
        if (!onOff) { return; }

        Slider slider = GetComponent<Slider>();
        if (slider.value == 0)
        {
            slider.value = 1;
            SetSliderValue(1);
        }
        else
        {
            slider.value = 0;
            SetSliderValue(0);
        }
    }
}
