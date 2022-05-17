using UnityEngine;

public class DisplayPanel : MonoBehaviour
{
    [Header("Info Displays")]
    [SerializeField] private InfoDisplay phaseInfo;
    [SerializeField] private InfoDisplay statDisplay1;
    [SerializeField] private InfoDisplay statDisplay2;
    [SerializeField] private InfoDisplay treatmentInfo;

    public void SetPhase(int phase)
    {
        if (phaseInfo)
        {
            phaseInfo.UpdateValue(phase);
        }
    }

    public void SetStat1(int value)
    {
        if (statDisplay1)
        {
            statDisplay1.UpdateValue(value);
        }
    }

    public void SetStat1(int value, string label)
    {
        if (statDisplay1)
        {
            statDisplay1.UpdateValue(value);
            statDisplay1.UpdateLabel(label);
        }
    }

    public void SetStat2(int value)
    {
        if (statDisplay2)
        {
            statDisplay2.UpdateValue(value);
        }
    }

    public void SetStat2(int value, string label)
    {
        if (statDisplay2)
        {
            statDisplay2.UpdateValue(value);
            statDisplay2.UpdateLabel(label);
        }
    }

    public void SetNumTreatments(int numTreatments)
    {
        if (treatmentInfo)
        {
            treatmentInfo.UpdateValue(numTreatments);
        }
    }
}
