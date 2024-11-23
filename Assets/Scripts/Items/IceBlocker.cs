using System.Collections;
using System.Collections.Generic;
using System.Security;
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
    [SerializeField] private GameObject firstObjCombine;
    [SerializeField] private GameObject secObj;
    [SerializeField] private GameObject secObjCombine;
    [SerializeField] private GameObject thirdObj;
    [SerializeField] private GameObject thirdObjCombine;
    [SerializeField] private float forceToBreak = 40f;
    [SerializeField] private float radiusToBreak = .5f;
    [SerializeField] private float thirdradius;
    [SerializeField] private BottomCell currentCell;

    [SerializeField] private ParticleSystem firstPartilces;
    [SerializeField] private ParticleSystem secondPartilces;
    [SerializeField] private ParticleSystem thirdPartilces;

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
        firstObjCombine.SetActive(true);
        firstObj.SetActive(false);
        secObjCombine.SetActive(true);
        secObj.SetActive(false);
        thirdObjCombine.SetActive(true);
        thirdObj.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            index++;
            if (index == 1)
            {
                MakeFirstBreak();
            }
            else if (index == 2)
            {
                MakeSecondBreak();
            }
            else if (index == 3)
            {
                MakeThirdBreak();
            }
        }
    }

    private void MakeBreak(Rigidbody[] partRbs, ParticleSystem particles)
    {
        if (partRbs == null || partRbs.Length == 0) return; // Safeguard check

        particles.Play();

        Vector3 explosionCenter = transform.position;

        foreach (Rigidbody rb in partRbs)
        {
            rb.isKinematic = false;

            // Randomized offset for varied directions
            Vector3 randomizedOffset = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(0.5f, 1.5f),
                Random.Range(-1f, 1f)
            );

            // Apply explosion force to push pieces in all directions
            rb.AddExplosionForce(forceToBreak, explosionCenter + randomizedOffset, radiusToBreak, upwardModifier, ForceMode.Impulse);
        }
    }

    // Call these methods as needed for first, second, and third breaks
    private void MakeFirstBreak()
    {
        firstObj.SetActive(true);
        firstObjCombine.SetActive(false);    
        MakeBreak(firstPartRbs, firstPartilces);
        StartCoroutine(DisableFirstCol());
    }

    private void MakeSecondBreak()
    {
        secObj.SetActive(true );
        secObjCombine.SetActive(false);
        MakeBreak(secPartRbs, secondPartilces);
        StartCoroutine(DisableSecCol());
    }

    private void MakeThirdBreak()
    {
        currentCell.isIce = false;
        currentCell.IceCellOpen();
        thirdObj.SetActive(true);
        thirdObjCombine.SetActive(false);
        MakeBreak(thirdPartRbs, thirdPartilces);
        StartCoroutine(DisableThirdCol());
    }

    private IEnumerator DisableFirstCol()
    {
        yield return new WaitForSeconds(0.6f);
        foreach (Rigidbody rb in firstPartRbs)
        {
            rb.gameObject.SetActive(false);
        }
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
        yield return new WaitForSeconds(0.6f);
        foreach (Rigidbody rb in secPartRbs)
        {
            rb.gameObject.SetActive(false);
        }
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
        currentCell.isIce = false;
        currentCell.IceCellOpen();
        yield return new WaitForSeconds(0.6f);
        foreach (Rigidbody rb in thirdPartRbs)
        {
            rb.gameObject.SetActive(false);
        }
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


    public IEnumerator MakeIceBreak_WithDelay()
    {
        yield return new WaitForSeconds(1.5f);
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
