using DG.Tweening;
using System.Collections;
using UnityEngine;

public class LockBlocker : MonoBehaviour
{
    [SerializeField] private GameObject lockObj;
    [SerializeField] private BottomCell currentCell;
    [SerializeField] private ParticleSystem unlockParticle;
    
    private void Start()
    {

    }

    private void Update()
    {
        
    }

    public IEnumerator MakeLockOpen()
    {
        yield return new WaitForSeconds(2.2f);
        currentCell.isLock = false;
        unlockParticle.Play();
        currentCell.OpenLockCell();
        yield return new WaitForSeconds(0.3f);
        currentCell.lockObj.SetActive(false);
        transform.DOScale(Vector3.zero, 0.4f).SetEase(Ease.InBack).OnComplete(() =>
        {
        });
        yield return new WaitForSeconds(0.5f);
        lockObj.SetActive(false);
    }
}
