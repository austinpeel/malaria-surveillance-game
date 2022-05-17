using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("Game Area")]
    [SerializeField] private RectTransform gameArea;

    [Header("Prefabs")]
    [SerializeField] private GameObject personPrefab;
    [SerializeField] private GameObject hubPrefab;

    [HideInInspector] public bool setRandomSeed;
    [HideInInspector] public int randomSeed = 42;

    public Person[] Initialize(int numPeoplePerHub, int numInfectedPerHub, float symptomaticProbability, bool showTrueInfections)
    {
        var people = new List<Person>();

        if (!personPrefab) { return null; }

        if (setRandomSeed)
        {
            Random.InitState(randomSeed);
        }

        RectTransform parent = gameArea ? gameArea : GetComponent<RectTransform>();
        Vector2 fieldExtent = new Vector2(parent.rect.width, parent.rect.height);

        var hubPositions = new List<Vector2>
        {
            0.25f * fieldExtent,
            new Vector2(0.75f * fieldExtent.x, 0.25f * fieldExtent.y),
            0.5f * fieldExtent,
            new Vector2(0.25f * fieldExtent.x, 0.75f * fieldExtent.y),
            0.75f * fieldExtent
        };

        //int numPeoplePerHub = Mathf.FloorToInt(100 / hubPositions.Count);
        int numPeople = 0;

        foreach (var hubPosition in hubPositions)
        {
            if (hubPrefab)
            {
                CreateHub(hubPosition, parent);
            }

            int numInfectedAtThisHub = 0;

            for (int i = 0; i < numPeoplePerHub; i++)
            {
                Vector2 newPosition = ProposePosition(hubPosition);

                // Avoid crowding
                while (PositionIsInvalid(newPosition, people))
                {
                    newPosition = ProposePosition(hubPosition);
                }

                Person newPerson = CreatePerson(newPosition, parent);
                //newPerson.index = numPeople;
                if (numInfectedAtThisHub < numInfectedPerHub)
                {
                    bool isSymptomatic = Random.Range(0f, 1f) <= symptomaticProbability;
                    newPerson.Infect(isSymptomatic);
                    if (showTrueInfections)
                    {
                        newPerson.RevealTrueInfection();
                    }
                    numInfectedAtThisHub++;
                }
                people.Add(newPerson);
                numPeople++;
            }
        }

        return people.ToArray();
    }

    private Vector2 ProposePosition(Vector2 center)
    {
        float magnitude = Mathf.Abs(Utils.RandomNormalValue(0f, 40f));
        Vector2 direction = Random.insideUnitCircle;
        direction /= direction.magnitude;

        Vector2 position = center + magnitude * direction;
        position += 5 * position.normalized;

        //Vector2 position = center + 100 * Random.insideUnitCircle;
        //position += 10 * position.normalized;

        return position;
    }

    private bool PositionIsInvalid(Vector2 position, List<Person> people)
    {
        bool invalid = false;

        foreach (Person person in people)
        {
            if (Vector2.Distance(position, person.Position) < 0.75f * person.Size)
            {
                invalid = true;
                break;
            }
        }

        return invalid;
    }

    private void CreateHub(Vector2 position, RectTransform parent)
    {
        var hub = Instantiate(hubPrefab, parent).GetComponent<RectTransform>();
        hub.anchorMin = Vector2.zero;
        hub.anchorMax = Vector2.zero;
        hub.pivot = 0.5f * Vector2.one;
        hub.anchoredPosition = position;
        hub.name = "Hub";
    }

    private Person CreatePerson(Vector2 position, RectTransform parent)
    {
        var person = Instantiate(personPrefab, parent).GetComponent<RectTransform>();
        person.anchorMin = Vector2.zero;
        person.anchorMax = Vector2.zero;
        person.pivot = 0.5f * Vector2.one;
        person.anchoredPosition = position;
        person.name = "Person";

        return person.GetComponent<Person>();
    }
}
