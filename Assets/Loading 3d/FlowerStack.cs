using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerStack : MonoBehaviour
{
    public bool isOccupied = false;
    public List<Transform> currentStack = new List<Transform>();

    public void AddHexa(List<Transform> flowerList)
    {
        currentStack = flowerList;
        isOccupied = true;
    }

    public void RemoveHexa() 
    { 
        currentStack.Clear();
        isOccupied = false; 
    }    
}
