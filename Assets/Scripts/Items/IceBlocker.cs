using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class IceBlocker : MonoBehaviour
{
    [SerializeField] private Rigidbody[] firstPartRbs;
    [SerializeField] private Rigidbody[] secPartRbs;
    [SerializeField] private Rigidbody[] thirdPartRbs;
    [SerializeField] private Collider[] firstPartCol;
    [SerializeField] private Collider[] secPartCol;
    [SerializeField] private Collider[] thirdPartCol;
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

    public bool nowCall = false;

    private void Awake()
    {
        
    }


    private void Start()
    {
        foreach(Rigidbody rb in firstPartRbs)
        {
            rb.isKinematic = true;
        }
        
        foreach(Rigidbody rb in secPartRbs)
        {
            rb.isKinematic = true;
        }
        
        foreach(Rigidbody rb in thirdPartRbs)
        {
            rb.isKinematic = true;
        }
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(MakeIceBreak());
        }*/
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
    {
        foreach (Rigidbody rb in secPartRbs)
        {
            rb.isKinematic = false;
        }
        foreach (Rigidbody rb in secPartRbs)
        {
            rb.AddExplosionForce(forceToBreak, transform.position, radiusToBreak, upwardModifier, ForceMode.Impulse);
        }
        StartCoroutine(DisableSecCol());

    }
    private void MakeThirdBreak()
    {
        foreach (Rigidbody rb in thirdPartRbs)
        {
            rb.isKinematic = false;
        }
        foreach (Rigidbody rb in thirdPartRbs)
        {
            rb.AddExplosionForce(forceToBreak, transform.position, radiusToBreak, upwardModifier, ForceMode.Impulse);
        }
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
        firstObj.SetActive(false);  
    }
    private IEnumerator DisableSecCol()
    {
        yield return new WaitForSeconds(2F); 
        foreach (Collider col in secPartCol)
        {
            col.enabled = false;
        }
        foreach (Rigidbody rbs in secPartRbs)
        {
            rbs.mass = 0.01f;
            rbs.isKinematic = false;
        }
        secObj.SetActive(false);
    }

    private IEnumerator DisableThirdCol()
    {
        yield return new WaitForSeconds(1.5F);
        foreach (Collider col in thirdPartCol)
        {
            col.enabled = false;
        }
        foreach (Rigidbody rbs in thirdPartRbs)
        {
            rbs.mass = 0.01f;
            rbs.isKinematic = false;
        }
        thirdObj.SetActive(false);
    }

    public bool MakeIceBreak()
    {
        index++;
        if (index == 1)
        {
            MakeFirstBreak();
            return false;
        }
        else if(index == 2)
        {
            MakeSecondBreak();
            return false;
        }
        else if(index == 3)
        {
            MakeThirdBreak();
            return true;
        }
        return false;
    }
}
