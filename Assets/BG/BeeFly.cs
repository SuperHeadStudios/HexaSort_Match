using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BeeFly : MonoBehaviour
{
    [SerializeField] private float flyingSpeed;
    [SerializeField] private float FlyTime;

    public bool FlyNow = false;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (FlyNow == true)
        {
            StartCoroutine(FlyBee());
        }
    }


    public IEnumerator FlyBee()
    {
        yield return new WaitForSeconds(FlyTime);
        transform.Translate(flyingSpeed,.1f,0f);

    }
}
