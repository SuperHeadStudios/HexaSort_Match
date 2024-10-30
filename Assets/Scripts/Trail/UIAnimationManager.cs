using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIAnimationManager : MonoBehaviour
{
    public static UIAnimationManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void TriggerUIAnimation(RectTransform targetUI)
    {
        // Start a scale animation on the UI element for a bounce effect
        targetUI.DOPunchScale(Vector3.one * 0.2f, 0.5f, 5, 0.8f)
            .SetEase(Ease.OutBounce);
    }
}
