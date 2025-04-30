using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class RandomSpawnerManager : MonoBehaviour
{
    [SerializeField] GameObject[] Fuses;
    [SerializeField] GameObject[] Keycards;
    [SerializeField] GameObject[] GasCans;

    //6 GasCan Locations
    //11 Fuse Locations
    //2 Keycard Locations

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnFuses();
        SpawnKeycards();
        SpawnGasCans();
    }

    void SpawnFuses()
    {
        int numberOfItemsToSpawn = 4;
        List<GameObject> selectedElements = new List<GameObject>();
        List<int> indices = new List<int>();

        // Create a list of indices for the array
        for (int i = 0; i < Fuses.Length; i++)
        {
            indices.Add(i);
        }

        // Randomly select elements from the array
        for (int i = 0; i < numberOfItemsToSpawn; i++)
        {
            int randomIndex = Random.Range(0, indices.Count);
            selectedElements.Add(Fuses[indices[randomIndex]]);

            // Set the selected element to active
            selectedElements[i].SetActive(true);
            indices.RemoveAt(randomIndex);
        }
    }
    void SpawnKeycards()
    {
        int numberOfItemsToSpawn = 1;
        List<GameObject> selectedElements = new List<GameObject>();
        List<int> indices = new List<int>();
        // Create a list of indices for the array
        for (int i = 0; i < Keycards.Length; i++)
        {
            indices.Add(i);
        }
        // Randomly select elements from the array
        for (int i = 0; i < numberOfItemsToSpawn; i++)
        {
            int randomIndex = Random.Range(0, indices.Count);
            selectedElements.Add(Keycards[indices[randomIndex]]);

            // Set the selected element to active
            selectedElements[i].SetActive(true);
            indices.RemoveAt(randomIndex);
        }
    }
    void SpawnGasCans()
    {
        int numberOfItemsToSpawn = 2;
        List<GameObject> selectedElements = new List<GameObject>();
        List<int> indices = new List<int>();
        // Create a list of indices for the array
        for (int i = 0; i < GasCans.Length; i++)
        {
            indices.Add(i);
        }
        // Randomly select elements from the array
        for (int i = 0; i < numberOfItemsToSpawn; i++)
        {
            int randomIndex = Random.Range(0, indices.Count);
            selectedElements.Add(GasCans[indices[randomIndex]]);

            // Set the selected element to active
            selectedElements[i].SetActive(true);
            indices.RemoveAt(randomIndex);
        }
    }
}
