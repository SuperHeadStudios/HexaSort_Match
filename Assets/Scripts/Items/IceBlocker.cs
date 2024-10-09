using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlocker : MonoBehaviour
{
    public static IceBlocker instance;
    [SerializeField] private Rigidbody firstPartRb;
    [SerializeField] private Rigidbody secondPartRb;
    [SerializeField] private Rigidbody thirdPartRb;
    [SerializeField] private Rigidbody fourthPartRb;
    [SerializeField] private float forceToBreak = 40f;
    [SerializeField] private float radiusToBreak = .5f;
    [SerializeField] private float thirdradius;
    public float upwardModifier = 1.8f;
    public int index = 0;
    public bool isUsable;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    private void Start()
    {
        firstPartRb.isKinematic = true;
        secondPartRb.isKinematic = true;
        thirdPartRb.isKinematic = true;
        fourthPartRb.isKinematic = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(MakeIceBreak());
        }
    }


    public void MakeFirstBreak()
    {   firstPartRb.isKinematic = false;
        firstPartRb.AddExplosionForce(forceToBreak, transform.position, radiusToBreak, upwardModifier, ForceMode.Impulse);
        Debug.Log("Broken 2");
    }
    public void MakeSecondBreak()
    {   firstPartRb.isKinematic = false;
        firstPartRb.AddExplosionForce(forceToBreak, transform.position, radiusToBreak, upwardModifier, ForceMode.Impulse);
        Debug.Log("Broken 2");
    }
    public void MakeThirdBreak()
    {   firstPartRb.isKinematic = false;
        firstPartRb.AddExplosionForce(forceToBreak, transform.position, radiusToBreak, upwardModifier, ForceMode.Impulse);
        Debug.Log("Broken 2");
    }

    public void MakeFourthBreak()
    {
        fourthPartRb.isKinematic = false;
        fourthPartRb.AddExplosionForce(forceToBreak, transform.position, thirdradius, upwardModifier, ForceMode.Impulse);
        BottomCell.instance.isIce = false;
        BottomCell.instance.meshRenderer.material = BottomCell.instance.cellMaterial;
        BottomCell.instance.iceObj.SetActive(false);
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
            case 3:
                MakeFourthBreak();
                break;

        }
        index++;
    }
}
