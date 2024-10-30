using UnityEngine;
using DG.Tweening;

public class ObjectToUIAnimation : MonoBehaviour
{
    [Header("UI Target & Animation Settings")]
    [SerializeField] private RectTransform uiTarget; // UI Target element
    [SerializeField] private float moveDuration = 1.5f; // Duration of the movement
    [SerializeField] private Transform curvePoint; // A midpoint for the curve (to define the curved path)

    private void Start()
    {
        MoveObjectToUI();
    }

    private void MoveObjectToUI()
    {
        // Convert UI position to World space for 3D object
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(uiTarget.position);
        targetPosition.z = 0f; // Set z to 0 to keep it on the same plane

        // Use DOTween to create a curved movement
        transform.DOPath(new Vector3[] { transform.position, curvePoint.position, targetPosition }, moveDuration, PathType.CatmullRom)
            .SetEase(Ease.InOutSine)
            .OnComplete(OnObjectReachedUI); // Trigger UI animation when complete
    }

    private void OnObjectReachedUI()
    {
        // Trigger UI animation and destroy the 3D object
        AnimateUI();
        Destroy(gameObject);
    }

    private void AnimateUI()
    {
        // Code to animate the UI element when the 3D object reaches it
        UIAnimationManager.Instance.TriggerUIAnimation(uiTarget);
    }
}
