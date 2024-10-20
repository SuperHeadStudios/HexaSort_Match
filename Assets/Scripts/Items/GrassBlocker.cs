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
    [SerializeField] private ParticleSystem grassBreak;

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
    private void LeafPositionUpdate()
    {
        if (currentCell.isGrass == true)
        {
            Vector3 leafPosition = centerLeaf.position;
            if (currentCell.hexaColumn.hexaCellList.Count == 0)
            {
                leafPosition.y = 0.028F;
            }
            else
            {
                leafPosition.y = currentCell.hexaColumn.hexaCellList.Count * 0.014f + 0.028F;
            }
            centerLeaf.position = leafPosition;
        }
    }

    public IEnumerator MakeGrassBreak()
    {
        yield return new WaitForSeconds(2.4f);
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = false;
            grassBreak.Play();
            //rb.AddExplosionForce(forceToBreak, transform.position, radiusToBreak, upwardModifier, ForceMode.Impulse);
        }
        currentCell.isGrass = false;
        FlyingGrass.instance.SpawnGrass(transform.position);
        //GameManager.instance.IncreaseGrassCount();
    }
}
   