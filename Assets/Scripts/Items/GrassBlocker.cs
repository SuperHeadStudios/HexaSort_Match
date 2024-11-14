using System.Collections;
using UnityEngine;

public class GrassBlocker : MonoBehaviour
{
    public static GrassBlocker instance;
    [SerializeField] private Rigidbody[] rbs;
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
       
    }
    private void LeafPositionUpdate()
    {
        if (currentCell.isGrass == true)
        {
            Vector3 leafPosition = centerLeaf.position;
            if (currentCell.hexaColumn.hexaCellList.Count == 0)
            {
                leafPosition.y = 0.017f;
            }
            else
            {
                leafPosition.y = currentCell.hexaColumn.hexaCellList.Count * 0.017f + 0.017f;
            }
            centerLeaf.position = leafPosition;
        }
    }

    public IEnumerator MakeGrassBreak()
    {
        yield return new WaitForSeconds(2.4f);
        SpawnFlowerTile.instance.GrassTileTrailSpawn(transform);
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = false;
            grassBreak.Play();
        }
        currentCell.isGrass = false;
    }
}
   