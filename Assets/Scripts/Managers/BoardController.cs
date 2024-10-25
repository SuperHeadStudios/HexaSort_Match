using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lofelt.NiceVibrations;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public static BoardController instance;

    public BoardGenerator boardGenerator;

    public LayerMask columnMask;

    public LayerMask bottomMask;

    public HexaColumn currentHexaColumn;

    public BottomCell currentHitBottomCell;

    public List<HexaColumn> hexaColumnsInMap;

    private Vector3 mousePos;

    private Vector3 mouseWorldPos;

    public CellHolder cellHolder;

    public GameObject hmAttack;

    public bool IsNowHit = false;

    public ParticleSystem hammerEffect;

    public bool dontMove = true;

    public bool NowMakeIt = false;

    public enum BOARD_STATE
    {
        IDLE, PROCESSING
    };

    public BOARD_STATE currentState;

    private Vector3 lastMousePos;

    public ParticleSystem clearHexaColumVfx1;

    public ParticleSystem clearHexaColumVfx2;

    public FlyingStarRoot flyingStar;
    

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        boardGenerator = transform.GetComponent<BoardGenerator>();
    }

    public void InitBoardController()
    {
        currentHexaColumn = null;
        currentHitBottomCell = null;
        hexaColumnsInMap = new List<HexaColumn>();
        cellHolder.InitHolder();
        currentState = BOARD_STATE.IDLE;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.uiManager.gameView.currentState == GameView.BOOSTER_STATE.HAMMER)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //ClearColumn();
                
                ClearColumn();
                return;
            }
        }

        if (GameManager.instance.uiManager.gameView.currentState == GameView.BOOSTER_STATE.MOVE)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PickHexaColumnInMaps();
            }


            if (Input.GetMouseButton(0))
            {
                if (GameManager.instance.currentGameState != GameManager.GAME_STATE.PLAYING)
                    return;

                if (currentHexaColumn != null)
                {
                    DragCurrentColumn();
                }

            }

            if (Input.GetMouseButtonUp(0))
            {
                if (GameManager.instance.currentGameState != GameManager.GAME_STATE.PLAYING)
                    return;

                if (GameManager.instance.boardController.currentHitBottomCell == null)
                    ReleaseMovingCell();
                else
                {
                    if (GameManager.instance.boardController.currentHitBottomCell.hexaColumn.hexaCellList.Count == 0)
                        PutColumnInNewPlace();
                    else
                        ReleaseMovingCell();
                }
            }


            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.instance.currentGameState != GameManager.GAME_STATE.PLAYING)
                return;

            PickHexaColumn();
            PickBottomCell();
        }


        if (Input.GetMouseButton(0))
        {
            if (GameManager.instance.currentGameState != GameManager.GAME_STATE.PLAYING)
                return;

            if (currentHexaColumn == null)
                return;

            if (currentHexaColumn != null)
            {
                DragCurrentColumn();
            }

        }

        if (Input.GetMouseButtonUp(0))
        {

            if (GameManager.instance.currentGameState != GameManager.GAME_STATE.PLAYING)
                return;

            if (currentHexaColumn == null)
                return;

            if (GameManager.instance.boardController.currentHitBottomCell == null)
                ReleaseFocusCell();
            else
            {
                if (GameManager.instance.boardController.currentHitBottomCell.hexaColumn.hexaCellList.Count == 0)
                    PutColumnInHolder();
                else
                    ReleaseFocusCell();
            }
        }

    }

    public bool PickHexaColumn()
    {
        bool isHitColumn = false;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f, columnMask))
        {
            if (hit.transform.tag == "CellColumn")
            {
                // Debug.Log("You selected the " + hit.transform.name);

                if (hit.transform.GetComponent<HexaColumn>().cellHoder != null)
                {
                    currentHexaColumn = hit.transform.GetComponent<HexaColumn>();
                    currentHexaColumn.transform.SetParent(null);
                    currentHexaColumn.ExtendColliderHeight();
                    isHitColumn = true;
                    currentHexaColumn.isSelected = true;
                    MoveToDragPos();
                }
            }

        }

        return isHitColumn;
    }

    public bool PickHexaColumnInMaps()
    {
        bool isHitColumn = false;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f, columnMask))
        {
            if (hit.transform.tag == "CellColumn")
            {
                // Debug.Log("You selected the " + hit.transform.name);

                if (hit.transform.GetComponent<HexaColumn>().currentBottomCell != null)
                {
                    currentHexaColumn = hit.transform.GetComponent<HexaColumn>();
                    currentHexaColumn.transform.SetParent(null);
                    currentHexaColumn.ExtendColliderHeight();
                    isHitColumn = true;
                    currentHexaColumn.isSelected = true;
                    MoveToDragPos();
                }
            }

        }

        return isHitColumn;
    }

    public void PickBottomCell()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f, bottomMask))
        {
            if (hit.transform.tag == "BottomCell")
            {
                //Debug.Log("You selected the " + hit.transform.name);

                if (hit.transform.GetComponent<BottomCell>() != null)
                {
                    BottomCell bottomCell = hit.transform.GetComponent<BottomCell>();
                    //bottomCell.GetNearCells();
                    /*if (bottomCell.isLock)
                    {
                        bottomCell.UnLockCell();
                    }
                    else if (bottomCell.isWood)
                    {
                        bottomCell.UnLockCell();
                    }*/
                }
            }

        }

    }

    private void DragCurrentColumn()
    {
        mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(currentHexaColumn.transform.position).z);
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 offsetAnchor = new Vector3(mouseWorldPos.x, 2.5f, mouseWorldPos.z) - lastMousePos;
        currentHexaColumn.transform.localPosition += offsetAnchor;
        lastMousePos = new Vector3(mouseWorldPos.x, 2.5f, mouseWorldPos.z);
    }

    private void MoveToDragPos()
    {
        /*
        mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(currentHexaColumn.transform.position).z);
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        lastMousePos = new Vector3(mouseWorldPos.x, 2.5f, mouseWorldPos.z);
        currentHexaColumn.MoveToTarget(new Vector3(mouseWorldPos.x, 2.5f, mouseWorldPos.z));
        */
        mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(currentHexaColumn.transform.position).z);
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        lastMousePos = new Vector3(mouseWorldPos.x, 2.5f, mouseWorldPos.z);
        currentHexaColumn.transform.localPosition = lastMousePos;
    }

    private void ReleaseFocusCell()
    {
        if (currentHexaColumn != null)
        {
            currentHexaColumn.MoveBack();
            currentHexaColumn.isSelected = false;
            currentHexaColumn = null;
        }
        if (currentHitBottomCell != null)
        {
            currentHitBottomCell.UnSelectCell();
            currentHitBottomCell = null;
        }
    }

    private void ReleaseMovingCell()
    {
        if (currentHexaColumn != null)
        {
            currentHexaColumn.MoveToLastBottom();
            currentHexaColumn.isSelected = false;
            currentHexaColumn = null;
        }
        if (currentHitBottomCell != null)
        {
            currentHitBottomCell.UnSelectCell();
            currentHitBottomCell = null;
        }
    }

    public void PutColumnInHolder_2(HexaColumn hexaColumn, BottomCell bottomCell)
    {
        if (GameManager.instance.boardController.currentHitBottomCell.hexaColumn.hexaCellList.Count == 0)
            PutColumnInHolder(hexaColumn, bottomCell);
        else
            ReleaseFocusCell();
    }

    public void PutColumnInHolder(HexaColumn hexaColumn, BottomCell bottomCell )
    {
        AudioManager.instance.columnPlaceSfx.Play();

        if (GameManager.instance.levelIndex == 1)
            GameManager.instance.uiManager.gameView.DisableArrow();


        if (AudioManager.instance.hapticState == 1)
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);

        bottomCell.hexaColumn.AddCellColumn(hexaColumn);
        if (bottomCell.isPrefilled || bottomCell.isRandomPrefilled) return;
        bottomCell.UnSelectCell();
        hexaColumnsInMap.Add(GameManager.instance.boardController.currentHitBottomCell.hexaColumn);
        bottomCell.hexaColumn.cellHoder = null;
        cellHolder.CheckPiecesInHolder();

        hexaColumn.isSelected = false;
        cellHolder.hexaColumnList.Remove(hexaColumn);

        hexaColumn.cellHoder = null;
        hexaColumn.currentBottomCell = null;
        hexaColumn.EmptyColumnData();
        GameManager.instance.poolManager.RemoveHexaColumn(hexaColumn);
        hexaColumn = null;
        if (currentState == BOARD_STATE.IDLE)
        {
            currentState = BOARD_STATE.PROCESSING;
            currentHitBottomCell.GetNearCells();

            currentHitBottomCell = null;
            CleanHexaMap();
        }
        else
        {
            currentHitBottomCell = null;
        }

    }

    public void PutColumnInHolder()
    {
        AudioManager.instance.columnPlaceSfx.Play();

        if (GameManager.instance.levelIndex == 1)
            GameManager.instance.uiManager.gameView.DisableArrow();
       

        if (AudioManager.instance.hapticState == 1)
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);

        currentHitBottomCell.hexaColumn.AddCellColumn(currentHexaColumn);
        currentHitBottomCell.UnSelectCell();
        hexaColumnsInMap.Add(GameManager.instance.boardController.currentHitBottomCell.hexaColumn);
        currentHitBottomCell.hexaColumn.cellHoder = null;
        cellHolder.CheckPiecesInHolder();

        currentHexaColumn.isSelected = false;
        cellHolder.hexaColumnList.Remove(currentHexaColumn);

        currentHexaColumn.cellHoder = null;
        currentHexaColumn.currentBottomCell = null;
        currentHexaColumn.EmptyColumnData();
        GameManager.instance.poolManager.RemoveHexaColumn(currentHexaColumn);
        currentHexaColumn = null;
        if (currentState == BOARD_STATE.IDLE)
        {
            currentState = BOARD_STATE.PROCESSING;
            currentHitBottomCell.GetNearCells();

            currentHitBottomCell = null;
            CleanHexaMap();
        }
        else
        {
            currentHitBottomCell = null;
        }

    }

    public void PutColumnInNewPlace()
    {
        GameManager.instance.uiManager.gameView.SubMoveValue();
        GameManager.instance.uiManager.gameView.CloseMove();
        AudioManager.instance.columnPlaceSfx.Play();

        if (AudioManager.instance.hapticState == 1)
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);

        currentHitBottomCell.hexaColumn.AddMovingCells(currentHexaColumn);
        currentHexaColumn.EmptyColumnData();
        currentHitBottomCell.UnSelectCell();
        currentHexaColumn.transform.SetParent(currentHexaColumn.currentBottomCell.transform);
        currentHexaColumn.transform.localPosition = Vector3.zero;
        currentHexaColumn.isSelected = false;
        currentHexaColumn = null;
        if (currentState == BOARD_STATE.IDLE)
        {
            currentState = BOARD_STATE.PROCESSING;
            currentHitBottomCell.GetNearCells();
            currentHitBottomCell = null;
            CleanHexaMap();
        }
        else
        {
            currentHitBottomCell = null;
        }

    }



    #region AlllData

    public void RemoveEmptyElements()
    {
        List<HexaColumn> emptyElements = new List<HexaColumn>();

        for (int j = 0; j < GameManager.instance.boardController.hexaColumnsInMap.Count; j++)
        {
            if (GameManager.instance.boardController.hexaColumnsInMap[j].hexaCellList.Count == 0)
            {
                emptyElements.Add(GameManager.instance.boardController.hexaColumnsInMap[j]);
            }
        }

        for (int i = 0; i < emptyElements.Count; i++)
        {
            for (int j = 0; j < GameManager.instance.boardController.hexaColumnsInMap.Count; j++)
            {
                if (emptyElements[i] == GameManager.instance.boardController.hexaColumnsInMap[j])
                {
                    GameManager.instance.boardController.hexaColumnsInMap.RemoveAt(j);
                }
            }
        }
    }

    private void CleanHexaMap()
    {
        int nearCellMaxCount = 0;
        int cellIndexHasMaxNear = -1;

        for (int i = 0; i < GameManager.instance.boardController.hexaColumnsInMap.Count; i++)
        {
            if (GameManager.instance.boardController.hexaColumnsInMap[i].currentBottomCell.nearCellList.Count > nearCellMaxCount)
            {
                nearCellMaxCount = GameManager.instance.boardController.hexaColumnsInMap[i].currentBottomCell.nearCellList.Count;
                cellIndexHasMaxNear = i;
            }
        }

        //Debug.Log("MAX NEAR COUNT " + nearCellMaxCount);
        if (cellIndexHasMaxNear >= 0)
        {
            TransferDoubleCells(GameManager.instance.boardController.hexaColumnsInMap[cellIndexHasMaxNear],
                GameManager.instance.boardController.hexaColumnsInMap[cellIndexHasMaxNear].currentBottomCell.nearCellList[0].hexaColumn);
            return;
        }

        currentState = BOARD_STATE.IDLE;
        CheckSlotsInMap();
    }

    private void CheckSlotsInMap()
    {
        int slotsInMap = 0;

        for (int i = 0; i < GameManager.instance.boardGenerator.bottomCellList.Count; i++)
        {
            if (GameManager.instance.boardGenerator.bottomCellList[i].hexaColumn.hexaCellList.Count > 0)
            {
                slotsInMap++;
            }
        }
        
        if(GameManager.instance.boardGenerator.currentMapSlots == slotsInMap)
        {
            Debug.Log("Game Over");
            GameManager.instance.ShowGameLose();
        }
    }

    public void DestroyThreeColums()
    {
        int totalColumnDestroy = 0;

        for (int i = 0; i < GameManager.instance.boardGenerator.bottomCellList.Count; i++)
        {
            //DestroyColumnByHammer(GameManager.instance.boardGenerator.bottomCellList[i].hexaColumn);
            if (GameManager.instance.boardGenerator.bottomCellList[i].hexaColumn.hexaCellList.Count > 0)
            {
                totalColumnDestroy++;

                if (totalColumnDestroy >= 3)
                    break;
            }
        }
    }    

    IEnumerator DestroyColumnByHammer(HexaColumn column)
    {
        hmAttack.transform.position = column.hexaCellList[0].transform.position + new Vector3(0f, 1.0f, -2f);
        hammerEffect.transform.position = column.hexaCellList[0].transform.position + new Vector3(0f, 1.0f, 0f);
        //GameManager.instance.hammerExplosionVfx.Play();
        StartCoroutine(HammerBoosterAttack());

        yield return new WaitForSeconds(2f);   

        for (int j = 0; j < column.hexaCellList.Count; j++)
        {
            GameManager.instance.poolManager.RemoveHexaCell(column.hexaCellList[j]);
        }
        column.hexaCellList.Clear();
        column.currentHexaColumnData.columnDataList.Clear();
        column.cellColorList.Clear();
        column.topColorID = -1;
        GameManager.instance.uiManager.gameView.SubHammerValue();
        GameManager.instance.uiManager.gameView.CloseHammer();
        AudioManager.instance.hammerSound.Play();
        if (AudioManager.instance.hapticState == 1)
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
    }

    IEnumerator HammerBoosterAttack()
    {
        hmAttack.SetActive(true) ;
        yield return new WaitForSeconds(2);
        hmAttack.SetActive(false);
    }

    private void RefreshAllNearCells()
    {
        for (int i = 0; i < GameManager.instance.boardController.hexaColumnsInMap.Count; i++)
        {
            GameManager.instance.boardController.hexaColumnsInMap[i].currentBottomCell.GetNearCells();
        }
    }

    private void TransferDoubleCells(HexaColumn cell1, HexaColumn cell2)
    {
        int sizeOfColumn1 = cell1.cellColorList.Count;
        //cell2 to cell1
        int sizeOfLastPart2 = cell2.currentHexaColumnData.columnDataList[cell2.currentHexaColumnData.columnDataList.Count - 1].columnValue;

        int sizeOfColumn2 = cell2.cellColorList.Count;

        for (int i = 0; i < sizeOfLastPart2; i++)
        {
            //Debug.Log("Check Pcs" + name);
            cell2.hexaCellList[sizeOfColumn2 - 1 - i].transform.SetParent(cell1.transform);
            //cell2.hexaCellList[sizeOfColumn2 - sizeOfLastPart2 + i].transform.localPosition = new Vector3(0, 0.25f * (1 + i + sizeOfColumn1), 0);
            MoveCell(cell2.hexaCellList[sizeOfColumn2 - 1 - i].transform, new Vector3(0, 0.25f * (1 + i + sizeOfColumn1), 0), i, sizeOfLastPart2 - 1, cell1, cell2);
        }
    }

    private void MoveCell(Transform cell, Vector3 targetPos, int queue, int lastQueue, HexaColumn cell1, HexaColumn cell2)
    {
        //Debug.Log("Check Pcs" + name);
        List<Vector3> arcPoint = new List<Vector3>();

        for (int i = 0; i < 10; i++)
        {
            arcPoint.Add(SampleParabola(cell.localPosition, targetPos, 3.0f, (float)i / 9.0f));
        }

        cell.DOLocalPath(arcPoint.ToArray(), 0.5f, PathType.Linear).SetDelay((float)queue * 0.05f).SetLoops(1).SetEase(Ease.Linear).OnComplete(() =>
        {
            AudioManager.instance.columnJumpSfx.Play();
            if (queue == lastQueue)
            {
                //Debug.Log("End Transfer");
                cell1.Pop(cell2);
                cell2.Push();
                if (!CheckTopLayer(cell1))
                {
                    RefreshAllNearCells();
                    CleanHexaMap();
                }
                else
                {
                    dontMove = false;
                    //Debug.Log("FULL STACK");
                    ClearTopLayer(cell1);
                }
            }
        });

        if (cell1.currentBottomCell.column == cell2.currentBottomCell.column)
        {
            if (cell1.currentBottomCell.row > cell2.currentBottomCell.row)
                cell.DOLocalRotate(new Vector3(180, 0, 0), 0.5f).SetRelative().SetDelay((float)queue * 0.05f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    cell.transform.localRotation = Quaternion.Euler(0, 0, 0);

                });
            else
                cell.DOLocalRotate(new Vector3(-180, 0, 0), 0.5f).SetRelative().SetDelay((float)queue * 0.05f).SetEase(Ease.Linear).OnComplete(() =>
                {

                    cell.transform.localRotation = Quaternion.Euler(0, 0, 0);

                });
        }

        else
        {
            if (cell1.currentBottomCell.column > cell2.currentBottomCell.column)
                cell.DOLocalRotate(new Vector3(0, 0, -180), 0.5f).SetRelative().SetDelay((float)queue * 0.05f).SetEase(Ease.Linear).OnComplete(() =>
                {

                    cell.transform.localRotation = Quaternion.Euler(0, 0, 0);

                });
            else
                cell.DOLocalRotate(new Vector3(0, 0, 180), 0.5f).SetRelative().SetDelay((float)queue * 0.05f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    cell.transform.localRotation = Quaternion.Euler(0, 0, 0);
                });
        }
    }
#endregion

    private bool CheckTopLayer(HexaColumn cell)
    {
        bool isFull = false;
        int dataCount = cell.currentHexaColumnData.columnDataList.Count;
        int topSize = cell.currentHexaColumnData.columnDataList[dataCount - 1].columnValue;
        if (topSize >= 10)
        {

            isFull = true;
            cell.currentBottomCell.CheckNearByOnCompelteStake(topSize);
            cell.currentBottomCell.CheckCurrentCellCompleteStake();
        }

        return isFull;
    }



    private void ClearTopLayer(HexaColumn cell)
    {
        int dataCount = cell.currentHexaColumnData.columnDataList.Count;
        int topSize = cell.currentHexaColumnData.columnDataList[dataCount - 1].columnValue;

        for (int i = 0; i < topSize; i++)
        {
            //Debug.Log("CLEAR ELEMENT " + (dataCount - topSize + i) + " TOP SIZE " + topSize);
            RemoveElementInTop(cell.hexaCellList[cell.hexaCellList.Count - 1 - i].transform, i, topSize - 1, cell);
            //cell.hexaCellList[cell.hexaCellList.Count - topSize + i].gameObject.SetActive(false);s
        }
    }

    private void RemoveElementInTop(Transform element, int queue, int lastQueue, HexaColumn cell)
    {
        element.DOScale(Vector3.zero, 0.15f).SetDelay((float)queue * 0.15f).SetEase(Ease.Linear).OnComplete(() =>
        {
            AudioManager.instance.columnSellSfx.Play();
            int dataCount = cell.currentHexaColumnData.columnDataList.Count;
            int topSize = cell.currentHexaColumnData.columnDataList[dataCount - 1].columnValue;
            Color currentColor = cell.hexaCellList[dataCount+5].currentColor;
            if (queue == 0)
            {
                if (!clearHexaColumVfx1.gameObject.activeSelf)
                    clearHexaColumVfx1.gameObject.SetActive(true);
                clearHexaColumVfx1.transform.position = element.position;
                clearHexaColumVfx1.Play();

                flyingStar.SpawnStar(element.position,currentColor);
                GameManager.instance.boardGenerator.currentGoalNumber += topSize;
                GameManager.instance.uiManager.gameView.UpdateGoalBar();
            }
            if (queue == lastQueue)
            {
                if (cell.currentHexaColumnData.columnDataList.Count == 1)
                {
                    if (!clearHexaColumVfx2.gameObject.activeSelf)
                        clearHexaColumVfx2.gameObject.SetActive(true);
                    clearHexaColumVfx2.transform.position = element.position;
                    clearHexaColumVfx2.Play();
                }

                for (int i = 0; i < topSize; i++)
                {
                    //Destroy(cell.hexaCellList[cell.hexaCellList.Count - 1].gameObject);
                    GameManager.instance.poolManager.RemoveHexaCell(cell.hexaCellList[cell.hexaCellList.Count - 1]);
                    cell.hexaCellList.RemoveAt(cell.hexaCellList.Count - 1);
                    cell.cellColorList.RemoveAt(cell.cellColorList.Count - 1);
                }
                cell.currentHexaColumnData.columnDataList.RemoveAt(cell.currentHexaColumnData.columnDataList.Count - 1);
                if (cell.hexaCellList.Count == 0)
                {
                    //Debug.Log("CLEAR ALL ELEMENT");
                    cell.topColorID = -1;
                    RemoveEmptyElements();
                    
                }
                else
                {
                    cell.topColorID = cell.currentHexaColumnData.columnDataList[cell.currentHexaColumnData.columnDataList.Count - 1].colorID;
                }

                if (boardGenerator.currentGoalNumber >= boardGenerator.goalNumber &&
                    boardGenerator.currentWoodGoalNumber >= boardGenerator.woodGoalNumber &&
                    boardGenerator.currentHoneyGoalNumber >= boardGenerator.honeyGoalNumber &&
                    boardGenerator.currentGrassGoalNumber >= boardGenerator.grassGoalNumber)
                {
                    GameManager.instance.ShowGameWin();
                }
                else
                {
                    RefreshAllNearCells();
                    CleanHexaMap();
                }
            }
        });
    }

    public void ClearColumn()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f, bottomMask))
        {
            if (hit.transform.tag == "BottomCell")
            {
                //Debug.Log("You selected the " + hit.transform.name);

                if (hit.transform.GetComponent<BottomCell>() != null)
                {
                    BottomCell bottomCell = hit.transform.GetComponent<BottomCell>();
                    //bottomCell.GetNearCells();
                    if (bottomCell.hexaColumn.hexaCellList.Count > 0)
                    {
                        StartCoroutine(DestroyColumnByHammer(bottomCell.hexaColumn));
                    }
                }
            }
        }
    }

    public void ClearBoard()
    {
        for (int i = 0; i < hexaColumnsInMap.Count; i++)
            hexaColumnsInMap[i].ClearAllElements();
    }

    public void RotateAnim()
    {
        transform.DOLocalRotate(new Vector3(0, 360, 0), 2.0f).SetRelative().SetLoops(1).SetEase(Ease.Linear).OnComplete(() =>
        {

            transform.transform.localRotation = Quaternion.Euler(0, 0, 0);
            GameManager.instance.boardGenerator.ClearMap();

        });
    }

    #region PARABOLA
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
            Vector3 levelDirection = end - new Vector3(start.x, end.y, start.z);
            Vector3 right = Vector3.Cross(travelDirection, levelDirection);
            //Vector3 up = Vector3.Cross(right, travelDirection);
            Vector3 up = Vector3.Cross(right, levelDirection);
            if (end.y > start.y) up = -up;
            Vector3 result = start + t * travelDirection;
            result += (Mathf.Sin(t * Mathf.PI) * height) * up.normalized;
            return result;
        }
    }
    #endregion
}
