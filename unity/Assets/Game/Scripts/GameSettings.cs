using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [Range(1, 9)] public int numTreatments = 5;
    [Range(0, 10)] public int numNeighborsHealed = 3;
    [Range(1, 10)] public int numNeighborsInfected = 5;
    [Range(0, 1)] public float infectionProbability = 0.4f;
    [Range(0, 1)] public float symptomaticProbability = 0.7f;
    public bool showTrueInfections = false;

    [Header("UI")]
    [SerializeField] private SettingsPanel settingsPanel;

    //private float prevSymptomaticProbability = 0.7f;
    private bool prevShowTrueInfections;

    public void ApplySettings()
    {
        if (settingsPanel)
        {
            settingsPanel.UpdateSettings(this);
        }

        // Initialize handles creating the people and updating the display panel
        GameManager gameManager = GetComponent<GameManager>();

        if (!Application.isPlaying)
        {
            gameManager.Initialize();
        }
        else
        {
            if (prevShowTrueInfections != showTrueInfections)
            {
                if (showTrueInfections)
                {
                    gameManager.ShowTrueInfections();
                }
                else
                {
                    gameManager.HideTrueInfections();
                }
                prevShowTrueInfections = showTrueInfections;
            }
        }
    }

    public void UpdateShowTrueInfections(float value)
    {
        showTrueInfections = value == 1;

        if (prevShowTrueInfections != showTrueInfections)
        {
            GameManager gameManager = GetComponent<GameManager>();
            if (showTrueInfections)
            {
                gameManager.ShowTrueInfections();
            }
            else
            {
                gameManager.HideTrueInfections();
            }
            prevShowTrueInfections = showTrueInfections;
        }
    }

    public void UpdateTreatmentsPerPhase(float value)
    {
        numTreatments = (int)value;
        GetComponent<GameManager>().UpdateDisplayPanel();
    }

    public void UpdateNumNeighborsHealed(float value)
    {
        numNeighborsHealed = (int)value;
    }

    public void UpdateNumNeighborsInfected(float value)
    {
        numNeighborsInfected = (int)value;
    }

    public void UpdateInfectionProbability(float value)
    {
        infectionProbability = value;
    }

    public void UpdateSymptomaticProbability(float value)
    {
        symptomaticProbability = value;
    }
}
