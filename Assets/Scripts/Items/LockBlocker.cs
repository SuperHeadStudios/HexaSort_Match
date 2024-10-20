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
        yield return new WaitForSeconds(2f);
        currentCell.isLock = false;
        lockObj.SetActive(false);
        currentCell.isLock = false;
    }
}
