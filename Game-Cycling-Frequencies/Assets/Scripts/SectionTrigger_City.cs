using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SectionTrigger_City : MonoBehaviour
{
    public GameObject RoadSection;
    private float sectionLength = 1027f; // Distance between sections
    private int sectionCount = 1; // Tracks how many sections have been spawned

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
Vector3 spawnPosition = new Vector3(sectionLength * sectionCount, 0, -73 );
            Instantiate(RoadSection, spawnPosition, Quaternion.identity);
            sectionCount++;
        }
    }
}
