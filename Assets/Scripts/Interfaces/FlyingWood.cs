using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingWood : MonoBehaviour
{
    public static FlyingWood instance;

    public GameObject starPrefab;

    public RectTransform targetRoot;

    public Canvas uiCanvas;

    private RectTransform starObj1;
    /* starObj2, starObj3;*/

    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        GameObject obj1 = Instantiate(starPrefab) as GameObject;
        obj1.SetActive(true);
        starObj1 = obj1.GetComponent<RectTransform>();
        starObj1.SetParent(transform);
        starObj1.localScale = Vector3.one;

        starObj1.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }


    List<Vector3> arcPoint = new List<Vector3>();

    Vector3 midlePoint;

    public void SpawnWood(Vector3 spawnPos)
    {
        if (!starObj1.gameObject.activeInHierarchy)
        {
            starObj1.gameObject.SetActive(true);
            spawnPos = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z) - Camera.main.transform.forward;
            starObj1.position = WorldToCanvasPosition(uiCanvas, starObj1, Camera.main, spawnPos);

            arcPoint.Clear();
            midlePoint = BetweenP(starObj1.localPosition, targetRoot.localPosition, 0.5f);

            for (int i = 0; i < 10; i++)
            {
                arcPoint.Add(SampleParabola(starObj1.localPosition, midlePoint, -100.0f, (float)i / 9.0f));
            }

            for (int i = 0; i < 10; i++)
            {
                arcPoint.Add(SampleParabola(midlePoint, targetRoot.localPosition, 100.0f, (float)i / 9.0f));
            }

            starObj1.DOLocalPath(arcPoint.ToArray(), 1.0f, PathType.Linear).SetLoops(1).SetEase(Ease.Linear).OnComplete(() =>
            {
                starObj1.gameObject.SetActive(false);
                GameManager.instance.IncreaseWoodCount();       
            });
        }
        else
        {
            // GameManager.Instance.uiManager.gameView.GetStarCombo(combo);
        }
    }

    private Vector2 WorldToCanvasPosition(Canvas canvas, RectTransform canvasRect, Camera camera, Vector3 position)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camera, position);
        return screenPoint;
    }

    Vector3 BetweenP(Vector3 start, Vector3 end, float percent)
    {
        return (start + percent * (end - start));
    }

    Vector3 SampleParabola(Vector3 start, Vector3 end, float height, float t)
    {
        if (Mathf.Abs(start.y - end.y) < 0.1f)
        {
            //start and end are roughly level, pretend they are - simpler solution with less steps
            Vector3 travelDirection = end - start;
            Vector3 result = start + t * travelDirection;
            result.y += Mathf.Sin(t * Mathf.PI) * height;
            return result;
        }
        else
        {
            //start and end are not level, gets more complicated
            Vector3 travelDirection = end - start;
            Vector3 levelDirecteion = end - new Vector3(start.x, end.y, start.z);
            Vector3 right = Vector3.Cross(travelDirection, levelDirecteion);
            Vector3 up = Vector3.Cross(right, travelDirection);
            if (end.y > start.y) up = -up;
            Vector3 result = start + t * travelDirection;
            result += (Mathf.Sin(t * Mathf.PI) * height) * up.normalized;
            return result;
        }
    }


}
