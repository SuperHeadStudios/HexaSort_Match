using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class HomeScreen3D : MonoBehaviour
{
    [SerializeField] private FlowerStack[] mapTiles; 
    [SerializeField] private float jumpPower = 3f;
    [SerializeField] private int numJumps = 1;
    [SerializeField] private float duration = 1.5f;

    private Queue<Transform> hexaStackQueue = new Queue<Transform>();


    private void Start()
    {
        for(int i = 0; i < mapTiles[0].currentStack.Count; i++)
        {

        }
    }


    private IEnumerator JumpHexaStack(List<Transform> flowerList)
    {

        yield return new WaitForSeconds(0f);
    }
}
