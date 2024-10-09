using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeWalker : MonoBehaviour
{
    [SerializeField] public float Speed;

    private void Start()
    {

    }
    void Update()
    {
        RotateNow();
    }

    public void RotateNow()
    {
        transform.Rotate(0f, Speed, 0f);

    }
}
