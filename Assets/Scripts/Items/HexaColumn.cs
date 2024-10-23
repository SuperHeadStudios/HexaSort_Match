using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class HexaColumn : MonoBehaviour
{
    //public HexaCell hexaCellPrefab;

    public List<HexaCell> hexaCellList;

    public List<int> cellColorList;

    public int topColorID;

    public HexaColumnData currentHexaColumnData;

    private const float localSpacingY = 0.25f;

    private const float colliderHeight = 0.26f;

    public BoxCollider boxCollider;

    public BottomCell currentBottomCell;

    public CellHolder cellHoder;

    public Vector3 positionInHoler;

    public bool isSelected;

    public float offsetRaycast;

    [SerializeField] private ColorConfig colorConfig;
    private int indexCount;

    public enum COLUMN_STATE
    {
        IDLE,
        MOVING
    };

    public COLUMN_STATE currentColumnState;

    private void Start()
    {
        if (currentBottomCell == null) return;
        colorConfig = Resources.Load("GameConfigs/ColorConfigSO") as ColorConfig;

        cellColorList.Clear();
        currentHexaColumnData.columnDataList.Clear();

        if (currentBottomCell.isRandomPrefilled)
        {
            StartCoroutine(SetRandomPrefilled());
        }

        if (currentBottomCell.isPrefilled)
        {
            StartCoroutine(SetPrefilled());
        }
    }    

    private IEnumerator SetRandomPrefilled()
    {
        yield return new WaitForSeconds(0.3f);

        int group1Count = Random.Range(2, 4); 
        int firstColorIndex = Random.Range(0, colorConfig.colorList.Count);
        int cellCount_1 = 0;

        for (int i = 0; i < group1Count && i < hexaCellList.Count; i++)
        {
            hexaCellList[i].meshRenderer.sharedMaterial = colorConfig.colorList[firstColorIndex].material;
            cellColorList.Add(colorConfig.colorList[firstColorIndex].colorID);
            cellCount_1++;
        }

        Debug.Log("Groupd 1 Count " + firstColorIndex);
        Debug.Log("cell 1 Count " + cellCount_1);

        ColumnData columnData_1 = new ColumnData(colorConfig.colorList[firstColorIndex].colorID, cellCount_1);
        currentHexaColumnData.columnDataList.Add(columnData_1);

        int secondColorIndex;
        do
        {
            secondColorIndex = Random.Range(0, colorConfig.colorList.Count);
        } while (secondColorIndex == firstColorIndex);
        int cellCount_2 = 0;
        
        for (int i = group1Count; i < hexaCellList.Count; i++)
        {
            hexaCellList[i].meshRenderer.sharedMaterial = colorConfig.colorList[secondColorIndex].material;
            cellColorList.Add(colorConfig.colorList[secondColorIndex].colorID);
            cellCount_2++;
        }

        Debug.Log("Groupd 1 Count " + secondColorIndex);
        Debug.Log("cell 1 Count " + cellCount_2);

        ColumnData columnData_2 = new ColumnData(colorConfig.colorList[secondColorIndex].colorID, cellCount_2);
        currentHexaColumnData.columnDataList.Add(columnData_2);

        topColorID = cellColorList[cellColorList.Count - 1];
    }

    private IEnumerator SetPrefilled()
    {
        yield return new WaitForSeconds(0.3f);

        int prefilledNum = Random.Range(1, colorConfig.colorList.Count);
        int cellCount_1 = 0;

        for (int i = 0; i < hexaCellList.Count; i++)
        {
            hexaCellList[i].meshRenderer.sharedMaterial = colorConfig.colorList[prefilledNum].material;
            cellColorList.Add(colorConfig.colorList[prefilledNum].colorID);
            cellCount_1++;
        }

        ColumnData columnData_1 = new ColumnData(colorConfig.colorList[prefilledNum].colorID, cellCount_1);
        currentHexaColumnData.columnDataList.Add(columnData_1);
        topColorID = cellColorList[cellColorList.Count - 1];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.U))
        {
            cellColorList.Clear();
            currentHexaColumnData.columnDataList.Clear();

            if (currentBottomCell == null) return;
            if (currentBottomCell.isPrefilled)
            {
                StartCoroutine(SetPrefilled());
            }

            if (currentBottomCell.isRandomPrefilled)
            {
                StartCoroutine(SetRandomPrefilled());
            }
        }

        if (isSelected)
            GetBottomCell();
    }

    public void InitColumn()
    {
        boxCollider = GetComponent<BoxCollider>();
        isSelected = false;
        topColorID = -1;
        currentColumnState = COLUMN_STATE.IDLE;
        currentBottomCell = null;
    }

    public void CreateColumn(HexaColumnData hexaColumnData)
    {
        currentHexaColumnData = hexaColumnData;

        for (int i = 0; i < hexaColumnData.columnDataList.Count; i++)
        {
            for (int j = 0; j < hexaColumnData.columnDataList[i].columnValue; j++)
            {
                HexaCell cell = GameManager.instance.poolManager.GetHexaCell();
                cell.transform.SetParent(transform);
                cell.transform.localPosition = new Vector3(0, localSpacingY * (1 + hexaCellList.Count), 0);
                cell.InitCell(hexaColumnData.columnDataList[i].colorID);
                cellColorList.Add(hexaColumnData.columnDataList[i].colorID);
                hexaCellList.Add(cell);
            }
        }

        if (currentHexaColumnData.columnDataList.Count > 0)
        {
            if (currentHexaColumnData.columnDataList[currentHexaColumnData.columnDataList.Count - 1].columnValue > 0)
                topColorID = currentHexaColumnData.columnDataList[currentHexaColumnData.columnDataList.Count - 1].colorID;
            else
                topColorID = -1;
        }
        else
            topColorID = -1;
        UpdateColliderHeight();
    }

    public void Push()
    {
        int dataCount = currentHexaColumnData.columnDataList.Count;
        int topSize = currentHexaColumnData.columnDataList[dataCount - 1].columnValue;
        for (int i = 0; i < topSize; i++)
        {
            cellColorList.RemoveAt(cellColorList.Count - 1);
            hexaCellList.RemoveAt(hexaCellList.Count - 1);
        }
        currentHexaColumnData.columnDataList.RemoveAt(currentHexaColumnData.columnDataList.Count - 1);

        if (hexaCellList.Count > 0)
        {
            topColorID = currentHexaColumnData.columnDataList[currentHexaColumnData.columnDataList.Count - 1].colorID;
        }
        else
        {
            topColorID = -1;
            GameManager.instance.boardController.hexaColumnsInMap.Remove(this);
        }

    }

    public void Pop(HexaColumn addColumn)
    {
        int dataCount = currentHexaColumnData.columnDataList.Count;
        int topSize = currentHexaColumnData.columnDataList[dataCount - 1].columnValue;

        int addDataCount = addColumn.currentHexaColumnData.columnDataList.Count;
        int addTopSize = addColumn.currentHexaColumnData.columnDataList[addDataCount - 1].columnValue;
        int addTopColor = addColumn.currentHexaColumnData.columnDataList[addDataCount - 1].colorID;

        for (int i = 0; i < addTopSize; i++)
        {
            cellColorList.Add(addTopColor);
            hexaCellList.Add(addColumn.hexaCellList[addColumn.hexaCellList.Count - 1 - i]);
        }

        currentHexaColumnData.columnDataList[dataCount - 1].columnValue += addTopSize;
    }

    public void AddCellColumn(HexaColumn addCellColumn)
    {
        //currentHexaColumnData = addCellColumn.currentHexaColumnData;
        currentHexaColumnData.columnDataList = new List<ColumnData>();
        for (int i = 0; i < addCellColumn.currentHexaColumnData.columnDataList.Count; i++)
        {
            currentHexaColumnData.columnDataList.Add(addCellColumn.currentHexaColumnData.columnDataList[i]);
        }
        for (int i = 0; i < addCellColumn.hexaCellList.Count; i++)
        {
            HexaCell cell = addCellColumn.hexaCellList[i];
            cell.transform.SetParent(transform);
            cell.transform.localPosition = new Vector3(0, localSpacingY * (1 + hexaCellList.Count), 0);
            hexaCellList.Add(cell);
        }

       // addCellColumn.EmptyColumnData();

        UpdateColliderHeight();
        UpdateColorList();
    }


    public void AddMovingCells(HexaColumn addCellColumn)
    {
        currentHexaColumnData.columnDataList = new List<ColumnData>();

        for (int i = 0; i < addCellColumn.currentHexaColumnData.columnDataList.Count; i++)
        {
            currentHexaColumnData.columnDataList.Add(addCellColumn.currentHexaColumnData.columnDataList[i]);
        }

        for (int i = 0; i < addCellColumn.hexaCellList.Count; i++)
        {
            HexaCell cell = addCellColumn.hexaCellList[i];
            cell.transform.SetParent(transform);
            cell.transform.localPosition = new Vector3(0, localSpacingY * (1 + hexaCellList.Count), 0);
            hexaCellList.Add(cell);
        }

        UpdateColliderHeight();
        UpdateColorList();
    }
/*

    public void AddNewHexaCell(int numberCell)
    {
        for (int i = 0; i < numberCell; i++)
        {
            HexaCell cell = GameManager.instance.poolManager.GetHexaCell();
            cell.transform.SetParent(transform);
            cell.transform.localPosition = new Vector3(0, localSpacingY * (1 + hexaCellList.Count), 0);
            hexaCellList.Add(cell);
        }

        UpdateColliderHeight();
    }*/

    public void UpdateColliderHeight()
    {
        if (boxCollider != null)
        {
            boxCollider.size = new Vector3(2.25f, colliderHeight * hexaCellList.Count, 2.25f);
            boxCollider.center = new Vector3(0, colliderHeight * hexaCellList.Count * 0.5f, 0);
        }
    }


    public void ExtendColliderHeight()
    {
        if (boxCollider != null)
        {
            boxCollider.size = new Vector3(1, colliderHeight * hexaCellList.Count + 3.0f, 1);

        }
    }

    public void UpdateColorList()
    {
        for (int i = 0; i < currentHexaColumnData.columnDataList.Count; i++)
        {
            for (int j = 0; j < currentHexaColumnData.columnDataList[i].columnValue; j++)
            {
                cellColorList.Add(currentHexaColumnData.columnDataList[i].colorID);
            }
        }
        topColorID = currentHexaColumnData.columnDataList[currentHexaColumnData.columnDataList.Count - 1].colorID;
    }

    public void MoveBack()
    {
        currentColumnState = COLUMN_STATE.MOVING;

        if (cellHoder != null)
        {
            transform.SetParent(cellHoder.transform);
        }
        transform.DOLocalMove(positionInHoler, 0.2f).SetEase(Ease.Linear).SetDelay(0.0f).OnComplete(() =>
        {
            currentColumnState = COLUMN_STATE.IDLE;
        });
    }

    public void MoveToLastBottom()
    {
        currentColumnState = COLUMN_STATE.MOVING;
        transform.SetParent(currentBottomCell.transform);
        transform.DOLocalMove(Vector3.zero, 0.2f).SetEase(Ease.Linear).SetDelay(0.0f).OnComplete(() =>
        {
            currentColumnState = COLUMN_STATE.IDLE;
        });

    }

    public void MoveToTarget(Vector3 targetPos)
    {
        currentColumnState = COLUMN_STATE.MOVING;
        Debug.Log("START MOVE");
        transform.DOLocalMove(targetPos, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            //highlightEffect.highlighted = false;
            Debug.Log("FINISH MOVE");
            currentColumnState = COLUMN_STATE.IDLE;
        });
    }

    public void ClearAllElements()
    {
        if (hexaCellList.Count <= 0)
            return;

        for (int i = 0; i < hexaCellList.Count; i++)
        {
            GameManager.instance.poolManager.RemoveHexaCell(hexaCellList[i]);
        }
        //GameManager.instance.poolManager.RemoveHexaColumn(this);
        EmptyColumnData();
        //currentBottomCell = null;
       // cellHoder = null;
    }

    public void EmptyColumnData()
    {
        hexaCellList.Clear();
        currentHexaColumnData.columnDataList.Clear();
        cellColorList.Clear();
        topColorID = -1;
        UpdateColliderHeight();
    }
    public LayerMask bottomCellMaksk;
    private RaycastHit hit;
    private BottomCell hitBottomCell;

    public void GetBottomCell()
    {
        if (Physics.Raycast(transform.position + new Vector3(0.0f, 0.0f, offsetRaycast), -transform.up, out hit, 10.0f, bottomCellMaksk))
        {
            if (hit.transform.tag == "BottomCell")
            {
                if (GameManager.instance.boardController.currentHitBottomCell == null)
                {
                    //Debug.Log("FIND BOTTOM");

                    hitBottomCell = hit.transform.GetComponent<BottomCell>();
                    if (hitBottomCell.isAd)
                        return;
                    else if(hitBottomCell.isWood)
                        return;
                    else if (hitBottomCell.isHoney)
                        return;
                    else if(hitBottomCell.isIce) 
                        return;
                    else if(hitBottomCell.isVines) 
                        return;
                    else if (hitBottomCell.isLock)
                        return;
                    if (hitBottomCell.hexaColumn.hexaCellList.Count > 0)
                        return;
                    GameManager.instance.boardController.currentHitBottomCell = hitBottomCell;
                    hitBottomCell.SelectCell();
                }
                else
                {
                    if (hit.transform.gameObject != GameManager.instance.boardController.currentHitBottomCell.gameObject)
                    {
                        // Debug.Log("FIND BOTTOM");
                        hitBottomCell = hit.transform.GetComponent<BottomCell>();
                        if (hitBottomCell.isAd)
                            return;
                        else if(hitBottomCell.isWood)
                            return;
                        else if( hitBottomCell.isHoney)
                            return;
                        else if (hitBottomCell.isIce) 
                            return;
                        else if (hitBottomCell.isVines)
                            return;
                        else if (hitBottomCell.isLock)
                            return;
                        if (hitBottomCell.hexaColumn.hexaCellList.Count > 0)
                            return;

                        GameManager.instance.boardController.currentHitBottomCell.UnSelectCell();
                        GameManager.instance.boardController.currentHitBottomCell = hitBottomCell;
                        hitBottomCell.SelectCell();
                        
                    }
                }
            }

            else
            {
                if (GameManager.instance.boardController.currentHitBottomCell != null)
                {
                    GameManager.instance.boardController.currentHitBottomCell.UnSelectCell();
                    GameManager.instance.boardController.currentHitBottomCell = null;
                }
            }
        }

        else
        {
            if (GameManager.instance.boardController.currentHitBottomCell != null)
            {
                GameManager.instance.boardController.currentHitBottomCell.UnSelectCell();
                GameManager.instance.boardController.currentHitBottomCell = null;
            }
        }

    }

}
