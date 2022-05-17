using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Utils
{
    // Marsaglia polar method for sampling from a Gaussian distribution
    public static float RandomNormalValue(float mu, float sigma)
    {
        float x1, x2, s;
        do
        {
            x1 = 2f * Random.value - 1f;
            x2 = 2f * Random.value - 1f;
            s = x1 * x1 + x2 * x2;
        } while (s == 0f || s >= 1f);

        s = Mathf.Sqrt(-2f * Mathf.Log(s) / s);

        return mu + x1 * s * sigma;

        //float y = Mathf.Sqrt(-2f * Mathf.Log(x1)) * Mathf.Sin(2f * Mathf.PI * x2);
        //return y * sigma + mu;
    }

    // Return an index array of people sorted by proximity to target
    public static int[] SortByDistance(Person target, Person[] people)
    {
        int[] indices = new int[people.Length];
        float[] distances = new float[people.Length];

        for (int i = 0; i < people.Length; i++)
        {
            indices[i] = i;
            distances[i] = Vector2.Distance(target.Position, people[i].Position);
        }
        System.Array.Sort(distances, indices);

        return indices;
    }

    // Return an array of only the infected people from a Person array
    public static Person[] GetInfectedPeople(Person[] people)
    {
        var infectedPeople = new List<Person>();

        foreach (Person person in people)
        {
            if (person.isInfected)
            {
                infectedPeople.Add(person);
            }
        }

        return infectedPeople.ToArray();
    }

    public static IEnumerator ScaleAndFadeUI(RectTransform aura, float emitTime, float targetScale, bool deactivateAfter)
    {
        Image image = aura.GetComponent<Image>();
        Color startColor = image.color;
        Color targetColor = startColor - new Color(0, 0, 0, startColor.a);
        Vector3 startScale = aura.localScale;

        float time = 0;
        while (time < emitTime)
        {
            time += Time.deltaTime;
            aura.localScale = Mathf.Lerp(startScale.x, targetScale, time / emitTime) * Vector3.one;
            image.color = Color.Lerp(startColor, targetColor, time / emitTime);
            yield return null;
        }

        aura.localScale = startScale;
        aura.gameObject.SetActive(!deactivateAfter);
        image.color = startColor;
    }
}
