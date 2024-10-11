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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(MakeLockOpen());
        }
    }

    public IEnumerator MakeLockOpen()
    {
        yield return new WaitForSeconds(2.4f);
        lockObj.SetActive(false);
        currentCell.isLock = false;
    }
}
