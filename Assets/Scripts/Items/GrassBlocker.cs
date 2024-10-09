using System.Collections;
using UnityEngine;

public class GrassBlocker : MonoBehaviour
{
    public static GrassBlocker instance;
    [SerializeField] private Rigidbody[] rbs;
    [SerializeField] private float forceToBreak = 40f;
    [SerializeField] private float radiusToBreak = .5f;
    public float upwardModifier = 1.8f;
    [SerializeField] private BottomCell currentCell;

    [SerializeField] public Transform centerLeaf;

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
            StartCoroutine(MakeGrassBreak());
        }
    }

    public IEnumerator MakeGrassBreak()
    {
        yield return new WaitForSeconds(2.4f);
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = false;
            rb.AddExplosionForce(forceToBreak, transform.position, radiusToBreak, upwardModifier, ForceMode.Impulse);
        }
        currentCell.isGrass = false;
        GameManager.instance.IncreaseGrassCount();
        FlyingGrass.instance.SpawnGrass(transform.position);
    }
}
   