using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SectionTrigger_City : MonoBehaviour
{
    public GameObject RoadSection;
    private float sectionLength = 990f; // Distance between sections
    private int sectionCount = 1; // Tracks how many sections have been spawned

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            Vector3 spawnPosition = new Vector3(-152, 0, sectionLength * sectionCount);
            
            // Stel de gewenste rotatie in bij het spawnen
            GameObject spawnedObject = Instantiate(RoadSection, spawnPosition, Quaternion.Euler(0, -90f, 0));

            sectionCount++;
        }
    }
}
