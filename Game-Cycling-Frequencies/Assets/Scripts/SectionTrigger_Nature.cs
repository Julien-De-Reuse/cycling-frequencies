using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SectionTrigger_Nature : MonoBehaviour
{
    public GameObject RoadSection;
    private float sectionLength = -730f; // Distance between sections
    private int sectionCount = 1; // Tracks how many sections have been spawned

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            Vector3 spawnPosition = new Vector3(0, 0, sectionLength * sectionCount);
            Instantiate(RoadSection, spawnPosition, Quaternion.identity);
            sectionCount++;
        }
    }
}
