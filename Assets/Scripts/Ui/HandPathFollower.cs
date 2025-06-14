using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class HandPathFollower : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveDuration = 2f;
    public Transform handImage; // UI Image

    private int currentIndex = 0;

    void Start()
    {
        MoveToNextPoint();
    }

    void MoveToNextPoint()
    {
        if (waypoints.Length == 0) return;

        Vector3 target = waypoints[currentIndex].position;

        handImage.DOMove(target, moveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;
            MoveToNextPoint();
        });
    }

    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Gizmos.color = Color.cyan;

        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }

        // Optional: draw a line from last to first if looping
        if (waypoints.Length > 2 && waypoints[0] != null && waypoints[^1] != null)
        {
            Gizmos.DrawLine(waypoints[^1].position, waypoints[0].position);
        }
    }

}
