using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UIElements;

public class HomeScreen3D : MonoBehaviour
{
    [SerializeField] private FlowerStack[] mapTiles; 
    [SerializeField] private float jumpPower = 3f;
    [SerializeField] private int numJumps = 1;
    [SerializeField] private float duration = 1.5f;
    [SerializeField] private float rotationDuration = 1.5f;
    [SerializeField] private Ease ease = Ease.Linear;

    [SerializeField] private FlowerStack currentFlowerStack;
    [SerializeField] private FlowerStack choosednFlowerStack;


    private void Start()
    {
       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            for (int i = 0; i < mapTiles.Length; i++)
            {
                if (mapTiles[i].isOccupied)
                {
                    currentFlowerStack = mapTiles[i];
                }
                break;
            }

            choosednFlowerStack = ChooseRandomTile();

            if (choosednFlowerStack != null)
            {
                Debug.Log("Chosen tile: " + choosednFlowerStack.name);
            }
            else
            {
                Debug.Log("No available tiles to choose from.");
            }
            StartCoroutine(JumpHexaStack());
        }
    }


    private IEnumerator JumpHexaStack()
    {
        float yValue = 0.5f;

        for (int i = currentFlowerStack.currentStack.Count - 1; i >= 0 ; i--)
        {
            Transform objectToMove = currentFlowerStack.currentStack[i];

            Vector3 positionToMove = choosednFlowerStack.transform.position;
            positionToMove.y += yValue;

            GetDirection(positionToMove, objectToMove);

            if (objectToMove != null)
            {
                // Create a jump animation
                objectToMove.DOJump(positionToMove, jumpPower, numJumps, duration).SetEase(ease).OnComplete(() =>
                {
                    choosednFlowerStack.AddHexa(objectToMove);
                    objectToMove.transform.parent = choosednFlowerStack.transform;
                });

            }
            yield return new WaitForSeconds(0.02f);
            yValue += 0.2f;
        }

        currentFlowerStack.currentStack.Clear();
        choosednFlowerStack.isOccupied = true;
        currentFlowerStack.isOccupied = false;

        currentFlowerStack = choosednFlowerStack;

    }   

    private void GetDirection(Vector3 endPosition, Transform obj)
    {
        Quaternion oldRotation = obj.rotation;
        Vector3 movementDirection = (endPosition - obj.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
        obj.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);

        StartCoroutine(RotatePlayerSmoothly(obj, oldRotation));
    }

    IEnumerator RotatePlayerSmoothly(Transform obj, Quaternion oldRotation)
    {
        if (obj == null) yield break;

        float duration = rotationDuration;
        Quaternion startRotation = obj.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(-180f, 0f, 0f);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (obj == null) yield break;
            elapsed += Time.deltaTime;
            obj.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / duration);
            yield return null;
        }

        if (obj == null) yield break;

        obj.rotation = targetRotation;
        //yield return new WaitForSeconds(0.1f);
      /*  if (obj != null)
        {
            obj.rotation = Quaternion.Euler(targetRotation.x, oldRotation.y, targetRotation.z);
        }*/
    }

    private FlowerStack ChooseRandomTile()
    {
        FlowerStack randomTile = null;
        List<FlowerStack> availableTiles = new List<FlowerStack>();

        // Populate the list of available tiles, ignoring the currently chosen tile
        foreach (var tile in mapTiles)
        {
            if (tile != currentFlowerStack && !tile.isOccupied)
            {
                availableTiles.Add(tile);
            }
        }

        // Choose a random tile from the available ones if any exist
        if (availableTiles.Count > 0)
        {
            int randomIndex = Random.Range(0, availableTiles.Count);
            randomTile = availableTiles[randomIndex];
        }

        return randomTile;
    }


}
