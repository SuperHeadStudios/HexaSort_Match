using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerStack : MonoBehaviour
{
    public bool isOccupied = false;
    public List<Transform> currentStack = new List<Transform>();

    public List<FlowerStack> nearByCells;

    public int moveCount = 2;

    public void AddHexa(Transform flower)
    {
        currentStack.Add(flower);
        isOccupied = true;
    }

    public void RemoveHexa() 
    { 
        currentStack.Clear();
        isOccupied = false; 
    }    


}
