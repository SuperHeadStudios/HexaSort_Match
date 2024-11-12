using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyBlocker : MonoBehaviour
{
    public static HoneyBlocker instance;
    [SerializeField] private Rigidbody[] rbs;
    [SerializeField] private Collider[] cols;
    [SerializeField] private GameObject honeyObj;
    [SerializeField] private float forceToBreak = 40f;
    [SerializeField] private float radiusToBreak = .5f;
    public float upwardModifier = 1.8f;

    [SerializeField] private BeeFly beeFly;
    [SerializeField] private BeeWalker beeWalk;
    [SerializeField] private BottomCell currentCell;

    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(MakeHoneyBreak());
        }
    }

    public IEnumerator MakeHoneyBreak()
    {
        yield return new WaitForSeconds(2.4f);
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = false;
            rb.AddExplosionForce(forceToBreak, transform.position, radiusToBreak, upwardModifier, ForceMode.Impulse);
            StartCoroutine(DisableCol());
            StartCoroutine(Delay());
            beeFly.FlyNow = true;
        }
        SpawnFlowerTile.instance.HoneyTileTrailSpawn(transform);
    }

    public IEnumerator DisableCol()
    {
        yield return new WaitForSeconds(2F);
        currentCell.isHoney = false;
        foreach(Collider col in cols)
        {
            col.enabled = false;
        }
       
        foreach (Rigidbody rb in rbs )
        {
            rb.mass = 0.01F;
            rb.isKinematic = false;
        }
        
        
    }


    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.000001f);
        beeWalk.Speed = 0f;
    }
}
