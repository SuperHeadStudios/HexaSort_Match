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

    public void MakeLockOpen()
    {
        currentCell.isLock = false;
        lockObj.SetActive(false);
        currentCell.isLock = false;
    }
}
