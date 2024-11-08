using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WoodBlocker : MonoBehaviour
{
    //public static WoodBlocker instance;
    [SerializeField] private Rigidbody[] firstPartRbs;
    [SerializeField] private Rigidbody[] secPartRbs;
    [SerializeField] private Rigidbody[] thirdPartRbs;
    [SerializeField] private GameObject firstObj;
    [SerializeField] private GameObject secObj;
    [SerializeField] private GameObject thirdObj;
    [SerializeField] private float forceToBreak = 40f;
    [SerializeField] private float radiusToBreak = .5f;
    [SerializeField] private float thirdradius;
    [SerializeField] private Collider[] firstColliders;
    [SerializeField] private Collider[] secColliders;
    [SerializeField] private Collider[] thirdColliders;
    [SerializeField] private BottomCell currentCell;
    public float upwardModifier = 1.8f;
    public int index = 0;
    public bool finalBroken = false;
    

    
    private void Awake()
    {
        /*if (instance == null)
        {
            instance = this;
        }*/
    }


    private void Start()
    {
        foreach (Rigidbody rb in firstPartRbs)
        {
            rb.isKinematic = true;
        }
        foreach(Rigidbody rb in secPartRbs)
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MakeThirdBreak();
        }
    }

    public void MakeFirstBreak()
    {
        foreach (Rigidbody rb in firstPartRbs)
        {
            rb.isKinematic = false;
            rb.AddExplosionForce(forceToBreak, transform.position, radiusToBreak, upwardModifier, ForceMode.Impulse);
            Debug.Log("Broken 2");
            StartCoroutine(DisableFirstCol());
            
        }
    }
    public void MakeSecondBreak()
    {
        foreach (Rigidbody rb in secPartRbs)
        {
            Debug.Log("Brokennnnnnnnnnnn");
            rb.isKinematic = false;
            rb.AddExplosionForce(forceToBreak, transform.position, radiusToBreak, upwardModifier, ForceMode.Impulse);
            StartCoroutine(DisableSecCol());
        }
    }
    public void MakeThirdBreak()
    {
        foreach (Rigidbody rb in thirdPartRbs)
        {
            finalBroken = true;
            rb.isKinematic = false;
            rb.AddExplosionForce(forceToBreak, transform.position, thirdradius, upwardModifier, ForceMode.Impulse);
            StartCoroutine(DisableThirdCol());
        }
       // FlyingWood.instance.SpawnWood(transform.position);

        SpawnFlowerTile.instance.WoodTileTrailSpwan(transform);
    }

    public IEnumerator DisableFirstCol()
    {
        yield return new WaitForSeconds(2F);
        foreach (Collider col in firstColliders)
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
    public IEnumerator DisableSecCol()
    {
        yield return new WaitForSeconds(2F);
        foreach (Collider col in secColliders)
        {
            col.enabled = false;
        }
        foreach (Rigidbody rbs in secPartRbs)
        {
            rbs.mass = 0.01f;
            rbs.isKinematic = false;
        }
        Destroy(secObj, 4f);
    }

    public IEnumerator DisableThirdCol()
    {
        yield return new WaitForSeconds(1.5F);
        foreach (Collider col in thirdColliders)
        {
            col.enabled = false;
        }
        foreach (Rigidbody rbs in thirdPartRbs)
        {
            rbs.mass = 0.01f;
            rbs.isKinematic = false;
        }
        Destroy(thirdObj, 4f);
        currentCell.isWood = false;
    }

    public IEnumerator MakeWoodBreak()
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
