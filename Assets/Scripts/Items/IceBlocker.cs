using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class IceBlocker : MonoBehaviour
{
    
    [SerializeField] private Rigidbody[] firstPartRbs;
    [SerializeField] private Rigidbody secPartRb;
    [SerializeField] private Rigidbody thirdPartRb;
    [SerializeField] private Collider[] firstPartCol;
    [SerializeField] private Collider secPartCol;
    [SerializeField] private Collider thirdPartCol;
    [SerializeField] private GameObject firstObj;
    [SerializeField] private GameObject secObj;
    [SerializeField] private GameObject thirdObj;
    [SerializeField] private float forceToBreak = 40f;
    [SerializeField] private float radiusToBreak = .5f;
    [SerializeField] private float thirdradius;
    [SerializeField] private BottomCell currentCell;
    public float upwardModifier = 1.8f;
    public int index = 0;
    public bool isUsable;

    private void Awake()
    {
        
    }


    private void Start()
    {
        foreach(Rigidbody rb in firstPartRbs)
        {
            rb.isKinematic = true;
        }
        secPartRb.isKinematic = true;
        thirdPartRb.isKinematic = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(MakeIceBreak());
        }
    }

    private void MakeFirstBreak()
    {
        foreach (Rigidbody rb in firstPartRbs)
        {
            rb.isKinematic = false;
        }
        foreach (Rigidbody rb in firstPartRbs)
        {
            rb.AddExplosionForce(forceToBreak, transform.position, radiusToBreak, upwardModifier, ForceMode.Impulse);
        }
        StartCoroutine(DisableFirstCol());
    }
    private void MakeSecondBreak()
    {   secPartRb.isKinematic = false;
        secPartRb.AddExplosionForce(forceToBreak, transform.position, radiusToBreak, upwardModifier, ForceMode.Impulse);
        StartCoroutine(DisableSecCol());
    }
    private void MakeThirdBreak()
    {   thirdPartRb.isKinematic = false;
        thirdPartRb.AddExplosionForce(forceToBreak, transform.position, radiusToBreak, upwardModifier, ForceMode.Impulse);
        StartCoroutine(DisableThirdCol());
    }

    private IEnumerator DisableFirstCol()
    {
        yield return new WaitForSeconds(2F);
        foreach (Collider col in firstPartCol)
        {
            col.enabled = false;
        }
        foreach (Rigidbody rbs in firstPartRbs)
        {
            rbs.mass = 0.01f;
            rbs.isKinematic = false;
        }
        Destroy(firstObj, 4f);
    }
    private IEnumerator DisableSecCol()
    {
        yield return new WaitForSeconds(2F);
        secPartCol.enabled = false;
        secPartRb.mass = 0.01f;
        secPartRb.isKinematic = false;
        Destroy(secObj, 4f);
    }

    private IEnumerator DisableThirdCol()
    {
        yield return new WaitForSeconds(1.5F);
        thirdPartCol.enabled = false;
        thirdPartRb.mass = 0.01f;
        thirdPartRb.isKinematic = false;
        Destroy(thirdObj, 4f);
        currentCell.isIce = false;
    }

    public IEnumerator MakeIceBreak()
    {
        yield return new WaitForSeconds(2.4f);
        switch (index)
        {
            case 0:
                MakeFirstBreak();
                break;
            case 1:
                MakeSecondBreak();
                break;
            case 2:
                MakeThirdBreak();
                break;
        }
        index++;
    }
}
