using UnityEngine;
using DG.Tweening;
using System.Collections;

public class SpawnFlowerTile : MonoBehaviour
{
    public static SpawnFlowerTile instance;

    [Header("Spawn Settings")]
    public GameObject objectToSpawn;           // 3D object prefab to spawn
    public Transform player;                   // Reference to the player's transform

    [Header("UI Settings")]
    public RectTransform targetUIPosition;     // UI target position for the object to move to
    public Canvas uiCanvas;                    // Reference to the canvas for World to Screen position conversion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown) // Trigger on any key press
        {
            //SpawnAndAnimate();
        }
    }

    public void SpawnAndAnimate(Transform player, int topSize, Color color, Material mat)
    {
        // Instantiate the object at the player's position
        GameObject spawnedObject = Instantiate(objectToSpawn, player.position, Quaternion.identity);
        spawnedObject.GetComponent<SetTileTrailColor>().SetColor(color, mat);

        spawnedObject.transform.DOLocalMoveY(3, 0.3f).SetEase(Ease.Linear);


        // Scale animation to half its original size
        spawnedObject.transform.DOScale(spawnedObject.transform.localScale * 0.35f, 0.2f).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                StartCoroutine(MoveObjectToUICurve(spawnedObject, player, topSize));
            });
    }

    private IEnumerator MoveObjectToUICurve(GameObject spawnedObject, Transform player, int topSize)
    {
        yield return new WaitForSeconds(0.02f);

        // Convert the UI position to world space
        Vector3 targetScreenPosition = targetUIPosition.position;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(uiCanvas.GetComponent<RectTransform>(), targetScreenPosition, Camera.main, out Vector3 targetWorldPosition);

        spawnedObject.transform.DOMove(targetWorldPosition, 0.6f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            GameManager.instance.boardGenerator.currentGoalNumber -= topSize;
            GameManager.instance.uiManager.gameView.UpdateGoalBar();
            Destroy(spawnedObject, 0.5f);
        });

    }
}
