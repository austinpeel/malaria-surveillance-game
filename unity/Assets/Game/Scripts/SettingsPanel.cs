using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider numTreatmentsSlider;
    [SerializeField] private Slider numNeighborsHealedSlider;
    [SerializeField] private Slider numNeighborsInfectedSlider;
    [SerializeField] private Slider infectionProbabilitySlider;
    [SerializeField] private Slider symptomaticProbabilitySlider;
    [SerializeField] private Slider showTrueInfectionsSlider;

    public void UpdateSettings(GameSettings settings)
    {
        if (numTreatmentsSlider)
        {
            numTreatmentsSlider.value = settings.numTreatments;
        }
        if (numNeighborsHealedSlider)
        {
            numNeighborsHealedSlider.value = settings.numNeighborsHealed;
        }
        if (numNeighborsInfectedSlider)
        {
            numNeighborsInfectedSlider.value = settings.numNeighborsInfected;
        }
        if (infectionProbabilitySlider)
        {
            infectionProbabilitySlider.value = settings.infectionProbability;
        }
        if (symptomaticProbabilitySlider)
        {
            symptomaticProbabilitySlider.value = settings.symptomaticProbability;
        }
        if (showTrueInfectionsSlider)
        {
            showTrueInfectionsSlider.value = settings.showTrueInfections ? 1 : 0;
        }
    }
}
