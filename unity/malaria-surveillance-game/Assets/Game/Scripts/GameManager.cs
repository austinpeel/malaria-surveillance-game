using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GameInitializer), typeof(GameSettings))]
public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private DisplayPanel displayPanel;
    [SerializeField] private Button infectButton;

    [Header("Starting Parameters")]
    [SerializeField] private int numPeoplePerHub = 20;
    [SerializeField, Range(1, 10)] private int numInfectedPerHub = 2;

    private GameSettings settings;
    private Person[] people;

    private int phase = 1;
    private int treatmentsRemaining;

    private int NumPeople => people.Length;
    private int NumInfected
    {
        get
        {
            int numInfected = 0;
            foreach (Person person in people)
            {
                if (person.isInfected)
                {
                    numInfected++;
                }
            }
            return numInfected;
        }
    }
    private int NumSymptomatic
    {
        get
        {
            int numSymptomatic = 0;
            foreach (Person person in people)
            {
                if (person.isSymptomatic)
                {
                    numSymptomatic++;
                }
            }
            return numSymptomatic;
        }
    }
    private int NumNotSymptomatic => NumPeople - NumSymptomatic;
    private int NumHealthy => NumPeople - NumInfected;

    private void Start()
    {
        // Get reference to game settings
        settings = GetComponent<GameSettings>();
        settings.ApplySettings();

        // Get all Person references that have been generated in Edit Mode
        people = FindObjectsOfType<Person>();

        // Synchronize info displays based on current settings
        treatmentsRemaining = settings.numTreatments;
        UpdateDisplayPanel();

        // Compute and store 10 nearest neighbors of each person
        foreach (Person person in people)
        {
            int[] allIndices = Utils.SortByDistance(person, people);
            List<int> neighborIndices = new List<int>();
            for (int i = 1; i < 11; i++)
            {
                neighborIndices.Add(allIndices[i]);
            }
            person.neighborIndices = neighborIndices;
        }
    }

    // Called by the Inspector button
    public void Initialize()
    {
        // Clear out all existing game objects
        Transform parent = transform.Find("Game Area");
        if (parent == null)
        {
            parent = transform;
        }
        for (int i = parent.childCount; i > 0; i--)
        {
            DestroyImmediate(parent.GetChild(0).gameObject);
        }

        // Get reference to settings component if necessary
        if (!settings)
        {
            settings = GetComponent<GameSettings>();
        }
        treatmentsRemaining = settings.numTreatments;

        // Distribute new people and hubs in the game area
        people = GetComponent<GameInitializer>().Initialize(numPeoplePerHub,
            numInfectedPerHub, settings.symptomaticProbability, settings.showTrueInfections);

        // Synchronize info display values and labels
        UpdateDisplayPanel();
    }

    public void UpdateDisplayPanel()
    {
        if (people == null)
        {
            people = FindObjectsOfType<Person>();
        }

        if (displayPanel)
        {
            displayPanel.SetPhase(phase);
            displayPanel.SetNumTreatments(treatmentsRemaining);

            if (settings.showTrueInfections)
            {
                displayPanel.SetStat1(NumHealthy, "healthy");
                displayPanel.SetStat2(NumInfected, "infected");
            }
            else
            {
                displayPanel.SetStat1(NumNotSymptomatic, "no symptoms");
                displayPanel.SetStat2(NumSymptomatic, "symptomatic");
            }
        }

        if (infectButton)
        {
            infectButton.interactable = treatmentsRemaining == 0 || NumSymptomatic == 0;
        }
    }

    private void OnDisable()
    {
        // Remove the reference when exiting play mode
        people = null;
    }

    public void HandlePersonClicked(Person person)
    {
        if (treatmentsRemaining == 0) { return; }

        // Only heal if a person is symptomatic (or infected if we can see the truth)
        bool treatPerson = settings.showTrueInfections ? person.isInfected : person.isSymptomatic;
        if (treatPerson)
        {
            person.Heal();
            person.TriggerAura();
            treatmentsRemaining--;

            // Potentially heal N infected neighbors (regardless of symptoms)
            int[] indices = Utils.SortByDistance(person, people);
            for (int i = 1; i < settings.numNeighborsHealed + 1; i++)
            {
                Person neighbor = people[indices[i]];
                if (neighbor.isInfected)
                {
                    neighbor.Heal();
                }
            }

            UpdateDisplayPanel();
        }

        HandlePersonMouseExit(person);
    }

    public void HandlePersonMouseEnter(Person person)
    {
        bool highlightNeighbors = settings.showTrueInfections ? person.isInfected : person.isSymptomatic;

        if (highlightNeighbors)
        {
            person.Highlight();
            for (int i = 0; i < settings.numNeighborsHealed; i++)
            {
                people[person.neighborIndices[i]].Highlight();
            }
        }
    }

    public void HandlePersonMouseExit(Person person)
    {
        person.Unhighlight();
        for (int i = 0; i < settings.numNeighborsHealed; i++)
        {
            people[person.neighborIndices[i]].Unhighlight();
        }
    }

    public void PropagateInfections()
    {
        // Infect N neighbors
        Person[] infectedPeople = Utils.GetInfectedPeople(people);
        foreach (Person infectedPerson in infectedPeople)
        {
            if (settings.showTrueInfections)
            {
                infectedPerson.TriggerAura();
            }
            else if (infectedPerson.isSymptomatic)
            {
                infectedPerson.TriggerAura();
            }

            int[] indices = Utils.SortByDistance(infectedPerson, people);
            for (int i = 1; i < settings.numNeighborsInfected + 1; i++)
            {
                Person neighbor = people[indices[i]];
                if (!neighbor.isInfected)
                {
                    if (Random.Range(0f, 1f) <= settings.infectionProbability)
                    {
                        bool symptomatic = Random.Range(0f, 1f) <= settings.symptomaticProbability;
                        neighbor.Infect(symptomatic);
                        if (settings.showTrueInfections)
                        {
                            neighbor.RevealTrueInfection();
                        }
                    }
                }
            }
        }

        phase++;
        treatmentsRemaining = settings.numTreatments;
        UpdateDisplayPanel();
    }

    public void ShowTrueInfections()
    {
        if (people == null)
        {
            people = FindObjectsOfType<Person>();
        }

        foreach (Person person in people)
        {
            person.RevealTrueInfection();
        }

        UpdateDisplayPanel();
    }

    public void HideTrueInfections()
    {
        if (people == null)
        {
            people = FindObjectsOfType<Person>();
        }

        foreach (Person person in people)
        {
            if (person.isInfected)
            {
                person.HideTrueInfection();
            }
        }

        UpdateDisplayPanel();
    }

    public void Restart()
    {
        phase = 1;
        Initialize();
        Start();
    }
}
