using UnityEngine;
using DG.Tweening;
using System.Collections;

public class SpawnFlowerTile : MonoBehaviour
{
    public static SpawnFlowerTile instance;

    [Header("----- Spawn Star Settings -----"), Space(5)]
    public GameObject objectToSpawn;           // 3D object prefab to spawn
    public Transform player;                   // Reference to the player's transform

    [Header("UI Settings")]
    public RectTransform targetUIPosition;     // UI target position for the object to move to
    public RectTransform targetUIPosition_2;     // UI target position for the object to move to
    public Canvas uiCanvas;                    // Reference to the canvas for World to Screen position conversion

    [Header("----- Spawn Wood Settings -----"), Space(5)]
    [SerializeField] private GameObject woodTile;
    [SerializeField] private RectTransform woodTarget;
    [SerializeField] private Transform woodIcon;
    [SerializeField] private ParticleSystem woodParticle;

    [Header("----- Spawn Grass Settings -----"), Space(5)]
    [SerializeField] private GameObject grassTile;
    [SerializeField] private RectTransform grassTarget;
    [SerializeField] private Transform grassIcon;
    [SerializeField] private ParticleSystem grassParticle;

    [Header("----- Spawn Honey Settings -----"), Space(5)]
    [SerializeField] private GameObject honeyTile;
    [SerializeField] private RectTransform honeyTarget;
    [SerializeField] private Transform honeyIcon;
    [SerializeField] private ParticleSystem honyParticles;
    [SerializeField] private Camera uiCamera;


    private TileType currentTileType = TileType.None;

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

    #region Flower Tile

    public void SpawnAndAnimate(Transform player, int topSize, Color color, Material mat)
    {
        // Instantiate the object at the player's position
        GameObject spawnedObject = Instantiate(objectToSpawn, player.position, Quaternion.identity);
        spawnedObject.GetComponent<SetTileTrailColor>().SetColor(color, mat);

        spawnedObject.transform.DOLocalMoveY(3, 0.3f).SetEase(Ease.Linear);

        // Scale animation to half its original crown
        spawnedObject.transform.DOScale(spawnedObject.transform.localScale * 0.35f, 0.2f).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                StartCoroutine(MoveObjectToUICurve(spawnedObject, player, topSize));
            });
    }

    private IEnumerator MoveObjectToUICurve(GameObject spawnedObject, Transform player, int topSize)
    {
        yield return new WaitForSeconds(0.02f);

        // Calculate target UI position relative to the Camera Space Canvas
        Vector3 targetViewportPosition = Vector3.zero;

        if (BoardController.instance.boardGenerator.isBlockers)
        {
            targetViewportPosition = uiCamera.WorldToViewportPoint(targetUIPosition_2.position);
        }
        else
        {
            targetViewportPosition = uiCamera.WorldToViewportPoint(targetUIPosition.position);
        }

        // Ensure the z-component matches the canvas plane distance
        Vector3 targetWorldPosition = uiCamera.ViewportToWorldPoint(new Vector3(
            targetViewportPosition.x,
            targetViewportPosition.y,
            uiCanvas.planeDistance // Ensures the object moves to the correct depth relative to the camera
        ));

        // Animate the object to move to the target position with a smooth curve
        spawnedObject.transform.DOMove(targetWorldPosition, 0.6f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            // Update UI and clean up the spawned object
            GameManager.instance.uiManager.gameView.UpdateGoalBar();
            spawnedObject.transform.GetChild(0).gameObject.SetActive(false);
            spawnedObject.transform.GetChild(1).gameObject.SetActive(false);

            // Destroy the object after some time
            Destroy(spawnedObject, 3f);

            // Check and update progress status
            if (GameManager.instance.boardGenerator.currentGoalNumber <= 0 &&
                GameManager.instance.boardGenerator.currentWoodGoalNumber <= 0 &&
                GameManager.instance.boardGenerator.currentHoneyGoalNumber <= 0 &&
                GameManager.instance.boardGenerator.currentGrassGoalNumber <= 0)
            {
                GameManager.instance.isProgressFinished = true;
            }
        });
    }


    #endregion

    #region All Tile

    public void WoodTileTrailSpawn(Transform player)
    {
        currentTileType =  TileType.Wood;
        TileSpawnAndAnimate(woodTile, woodTarget, player);
    }

    public void HoneyTileTrailSpawn(Transform player)
    {
        currentTileType = TileType.Honey;
        TileSpawnAndAnimate(honeyTile, honeyTarget, player);
    }

    public void GrassTileTrailSpawn(Transform player)
    {
        currentTileType = TileType.Grass;
        TileSpawnAndAnimate(grassTile, grassTarget, player);
    }

    private void WoodTileDataeUpdate()
    {
        GameManager.instance.IncreaseWoodCount();
        PlayCollectEffectBlocker(woodParticle, woodIcon);
    }

    private void HoneyTileDataeUpdate()
    {
        GameManager.instance.IncreaseHoneyCount();
        PlayCollectEffectBlocker(honyParticles, honeyIcon);
    }

    private void GrassTileDataeUpdate()
    {
        GameManager.instance.IncreaseGrassCount();
        PlayCollectEffectBlocker(grassParticle, grassIcon);
    }

    public void PlayCollectEffectBlocker(ParticleSystem ps, Transform icon)
    {
        ps.Play();

        AudioManager.instance.flowerCollectedSound.Play();

        icon.DOScale(Vector3.one * 1f, 0.3f / 2).SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                icon.DOScale(Vector3.one * 0.7f, 0.3f / 2).SetEase(Ease.InOutSine);
            });
    }

    public void UpdateBlockerProgress()
    {
        switch (currentTileType)
        {
            case TileType.Wood:
                WoodTileDataeUpdate();
                break;
            case TileType.Honey:
                HoneyTileDataeUpdate();
                break;
            case TileType.Grass:
                GrassTileDataeUpdate();
                break;
            default:
                Debug.LogWarning("Unknown tile type");
                break;
        }
    }

    #endregion

    #region Tiles Trails Blockers

    public void TileSpawnAndAnimate(GameObject spawnTile, RectTransform target, Transform player)
    {
        // Instantiate the object at the player's position
        GameObject spawnedObject = Instantiate(spawnTile, player.position, Quaternion.identity);

        // Initial upward movement animation
        spawnedObject.transform.DOLocalMoveY(3, 0.3f).SetEase(Ease.Linear);

        // Scale animation to shrink the object smoothly
        spawnedObject.transform.DOScale(spawnedObject.transform.localScale * 0.35f, 0.2f).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                StartCoroutine(TileMoveObjectToUICurve(spawnedObject, target, player));
            });
    }

    private IEnumerator TileMoveObjectToUICurve(GameObject spawnedObject, RectTransform target, Transform player)
    {
        // Ensure there's a slight delay before starting the movement
        yield return new WaitForSeconds(0.02f);

        // Calculate target UI position relative to the Camera Space Canvas
        Vector3 targetScreenPosition = RectTransformUtility.WorldToScreenPoint(uiCamera, target.position);
        Vector3 targetWorldPosition = uiCamera.ScreenToWorldPoint(new Vector3(
            targetScreenPosition.x,
            targetScreenPosition.y,
            uiCanvas.planeDistance // Ensures the correct depth based on canvas plane distance
        ));



        // Animate the object to move to the target position with a smooth curve
        spawnedObject.transform.DOMove(targetWorldPosition, 0.6f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            // Update progress status and clean up the object
            UpdateBlockerProgress();
            Destroy(spawnedObject, 0.5f);

            // Check if all goals are completed and set the progress flag
            if (GameManager.instance.boardGenerator.currentGoalNumber <= 0 &&
                GameManager.instance.boardGenerator.currentWoodGoalNumber <= 0 &&
                GameManager.instance.boardGenerator.currentHoneyGoalNumber <= 0 &&
                GameManager.instance.boardGenerator.currentGrassGoalNumber <= 0)
            {
                GameManager.instance.isProgressFinished = true;
            }
        });
    }


    #endregion
}
public enum TileType
{
    None,
    Wood,
    Grass,
    Honey
}