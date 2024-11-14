using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UIElements;

public class HomeScreen3D : MonoBehaviour
{
    [SerializeField] private FlowerStack[] mapTiles; 
    [SerializeField] private List<FlowerStack> mainFilledTiles;
    [SerializeField] private float jumpPower = 3f;
    [SerializeField] private int numJumps = 1;
    [SerializeField] private float duration = 1.5f;
    [SerializeField] private float rotationDuration = 1.5f;
    [SerializeField] private Ease ease = Ease.Linear;
    [SerializeField] private FlowerStack choosenFlowerStack;
    [SerializeField] private Vector3 rotateFactor;
    [SerializeField] private int firstStackMoveCount = 0;

    [SerializeField] private Camera Camera;
    [SerializeField] private Transform cameraPosition;

    [SerializeField] private bool isLoading = false;

    private void Start()
    {
        StartCoroutine(StartLoopAnimation());
    }

    private void OnEnable()
    {
        if (isLoading) return;
        Camera.transform.position = cameraPosition.position;
        Camera.transform.rotation = cameraPosition.rotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            //Camera.transform.parent = cameraPosition;
            Camera.transform.position = cameraPosition.position;
            Camera.transform.rotation = cameraPosition.rotation;
        }
    }

    private IEnumerator StartLoopAnimation()
    {
        while (true)
        {
            if (firstStackMoveCount == 0)
            {
                mainFilledTiles.Clear();

                for (int i = 0; i < mapTiles.Length; i++)
                {
                    if (mapTiles[i].isOccupied)
                    {
                        mainFilledTiles.Add(mapTiles[i]);
                    }
                }
            }

            StartCoroutine(JumpHexaStack(mainFilledTiles[firstStackMoveCount]));

            yield return new WaitForSeconds(1.5f);

        }
    }


    private IEnumerator JumpHexaStack(FlowerStack currentFlowerStack)
    {
        float yValue = 0.5f;

        choosenFlowerStack = ChooseRandomTile(currentFlowerStack);
        for (int i = currentFlowerStack.currentStack.Count - 1; i >= 0; i--)
        {
            Transform objectToMove = currentFlowerStack.currentStack[i];

            Vector3 positionToMove = choosenFlowerStack.transform.position;
            positionToMove.y += yValue;

            GetDirection(positionToMove, objectToMove);

            if (objectToMove != null)
            {
                // Create a jump animation
                objectToMove.DOJump(positionToMove, jumpPower, numJumps, duration).SetEase(ease).OnComplete(() =>
                {
                    choosenFlowerStack.AddHexa(objectToMove);
                    objectToMove.transform.parent = choosenFlowerStack.transform;
                });

            }
            yield return new WaitForSeconds(0.02f);
            yValue += 0.2f;
        }


        currentFlowerStack.currentStack.Clear();
        choosenFlowerStack.isOccupied = true;
        currentFlowerStack.isOccupied = false;

        firstStackMoveCount++;

        if(firstStackMoveCount > 2)
        {
            firstStackMoveCount = 0;
        }
        yield return new WaitForSeconds(1f);
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

    private FlowerStack ChooseRandomTile(FlowerStack currentFlowerStack)
    {
        FlowerStack randomTile = null;
        List<FlowerStack> availableTiles = new List<FlowerStack>();

        // Populate the list of available tiles, ignoring the currently chosen tile
        foreach (var tile in currentFlowerStack.nearByCells)
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
