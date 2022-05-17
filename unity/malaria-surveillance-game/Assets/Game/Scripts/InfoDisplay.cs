using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class InfoDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI labelTMP;
    [SerializeField] private TextMeshProUGUI valueTMP;
    [SerializeField] private Color color;
    [SerializeField] private int value;

    private void OnValidate()
    {
        if (labelTMP)
        {
            labelTMP.color = color;
        }

        if (valueTMP)
        {
            valueTMP.color = color;
            valueTMP.text = value.ToString();
        }
    }

    public void UpdateValue(int value)
    {
        if (valueTMP)
        {
            this.value = value;
            valueTMP.text = value.ToString();
        }
    }

    public void UpdateLabel(string text)
    {
        if (labelTMP)
        {
            labelTMP.text = text;
        }
    }
}
