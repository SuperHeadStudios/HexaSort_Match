using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockBlocker : MonoBehaviour
{
    [SerializeField] private GameObject lockObj;
    [SerializeField] private BottomCell currentCell;
    
    private void Start()
    {

    }

    private void Update()
    {
        
    }

    public IEnumerator MakeLockOpen()
    {
        yield return new WaitForSeconds(1.5f);
        transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InBack).OnComplete(() =>
        {
            lockObj.SetActive(false);
            currentCell.isLock = false;
            currentCell.isLock = false;
        });
    }
}
