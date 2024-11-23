using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VinesBlocker : MonoBehaviour
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
        foreach (Rigidbody rb in firstPartRbs)
        {
            rb.isKinematic = true;
        }

        foreach (Rigidbody rb in secPartRbs)
        {
            rb.isKinematic = true;
        }

        foreach (Rigidbody rb in thirdPartRbs)
        {
            rb.isKinematic = true;
        }
    }

    private void Update()
    {
       
    }
    private void MakeBreak(Rigidbody[] partRbs)
    {
        if (partRbs == null || partRbs.Length == 0) return; // Safeguard check


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
        MakeBreak(firstPartRbs);
        StartCoroutine(DisableFirstCol());
    }

    private void MakeSecondBreak()
    {
        MakeBreak(secPartRbs);
        StartCoroutine(DisableSecCol());
    }

    private void MakeThirdBreak()
    {
        MakeBreak(thirdPartRbs);
        StartCoroutine(DisableThirdCol());
    }


/*
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
        currentCell.isVines = false;
        StartCoroutine(DisableThirdCol());
    }*/

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
        currentCell.isVines = false;
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

    public bool MakeVinesBreak()
    {
        index++;
        if (index == 1)
        {
            MakeFirstBreak();
            return false;
        }
        else if (index == 2)
        {
            MakeSecondBreak();
            return false;
        }
        else if (index == 3)
        {
            MakeThirdBreak();
            return true;
        }
        return false;
    }

    public IEnumerator MakeVinesBreak_WithDelay()
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
