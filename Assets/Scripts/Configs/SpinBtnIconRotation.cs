using DG.Tweening;
using System.Collections;
using UnityEngine;

public class SpinBtnIconRotation : MonoBehaviour
{

    [SerializeField] private Transform wheel;
    [SerializeField] private float minSpinDuration = 2.0f;
    [SerializeField] private float maxSpinDuration = 50.0f;
    [SerializeField] private float spinSpeed = 1f;
    [SerializeField] private AnimationCurve easingCurve;

    [SerializeField] private ParticleSystem spinBtnParticle;

    private bool isSpinning;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnEnable()
    {
        StartCoroutine(WheelRotater());
    }

    private IEnumerator WheelRotater()
    {
        spinBtnParticle.Play();
        float defaultX = wheel.localEulerAngles.x;
        float defaultY = wheel.localEulerAngles.y;
        wheel.DOLocalRotate(new Vector3(defaultX, defaultY, 360f * 20), 5f, RotateMode.FastBeyond360)
             .SetEase(Ease.OutCirc);
        yield return null;
    }

}
