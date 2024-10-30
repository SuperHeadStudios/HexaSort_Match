using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmoothUITrail : MonoBehaviour
{
    [Header("Trail Settings")]
    [SerializeField] private GameObject trailPointPrefab; // The prefab for each trail point
    [SerializeField] private int maxTrailPoints = 10; // Maximum number of trail points
    [SerializeField] private float trailSpawnDelay = 0.05f; // Delay between spawning trail points
    [SerializeField] private float followSpeed = 8f; // Speed at which each point follows

    private List<GameObject> trailPoints = new List<GameObject>(); // List to store trail points
    [SerializeField] private RectTransform uiObjectRect;
    private float trailSpawnTimer;

    private void Update()
    {
        // Spawn new trail point at intervals
        trailSpawnTimer += Time.deltaTime;
        if (trailSpawnTimer >= trailSpawnDelay)
        {
            SpawnTrailPoint();
            trailSpawnTimer = 0f;
        }

        // Move each trail point towards the previous point for smooth motion
        for (int i = 0; i < trailPoints.Count; i++)
        {
            RectTransform trailRect = trailPoints[i].GetComponent<RectTransform>();
            Vector3 targetPosition = (i == 0) ? uiObjectRect.position : trailPoints[i - 1].transform.position;
            trailRect.position = Vector3.Lerp(trailRect.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }

    private void SpawnTrailPoint()
    {
        // Instantiate a new trail point
        GameObject trailPoint = Instantiate(trailPointPrefab, uiObjectRect.position, Quaternion.identity, uiObjectRect.parent);
        trailPoints.Add(trailPoint);

        // Limit trail points to avoid excessive buildup
        if (trailPoints.Count > maxTrailPoints)
        {
            Destroy(trailPoints[0]);
            trailPoints.RemoveAt(0);
        }
    }
}
