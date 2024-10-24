using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using static AdsControl;
using UnityEngine.Advertisements;
using Unity.VisualScripting;
using DG.Tweening;
using TMPro;

public class BottomCell : MonoBehaviour
{
    public static BottomCell instance;

    [HideInInspector] public int row;
    [HideInInspector] public int column;
    [HideInInspector] public int cost;
    public int currentLockCount;

    public HexaColumn hexaColumn;
    public HexaColumn hexaColumn_ice;
    public HexaColumn hexaColumn_Vines;

    public TextMeshPro currentLockText;

    public MeshRenderer meshRenderer;
    public Material cellMaterial;
    public Material cellSelectedMaterial;
    public Material lockMaterial;

    public GameObject AdObj;
    public GameObject lockObj;
    public GameObject woodObj;
    public GameObject grassObj;
    public GameObject honeyObj;
    public GameObject iceObj;
    public GameObject vinesObj;
    public GameObject iceHexa;
    public GameObject vinesHexa;
    public HexaColumn greenHexa;

    public bool isAd;
    public bool isLock;
    public bool isWood;
    public bool isGrass;
    public bool isHoney;
    public bool isIce;
    public bool isVines;
    public bool isRandomPrefilled;
    public bool isPrefilled;

    public HoneyBlocker honeyBlocker;
    public GrassBlocker grassBlocker;
    public WoodBlocker woodBlocker;
    public IceBlocker iceBlocker;
    public VinesBlocker vinesBlocker;
    public LockBlocker lockBlocker;

    public bool isBreakNow = false;
    public bool isUseNow;

    public BoardController boardController;
    public BoardGenerator boardGenerator;
    public int index;

    public float currentLeafPos;
    private float velocity;
    [SerializeField] private float smoothTime = 0.1f;

    public Transform centerLeaf;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        boardController = transform.GetComponentInParent<BoardController>(); 
        boardGenerator = transform.GetComponentInParent<BoardGenerator>(); 
        Invoke(nameof(CheckNearOnStart),0.5f);
    }
    private void Update()
    {
        UpdateLeafPosition();
        currentLockText.text = cost.ToString();
    }

    public void UpdateLeafPosition()
    {
        if (isGrass == true)
        {
            Vector3 leafPosition = centerLeaf.position;
            if (hexaColumn.hexaCellList.Count == 0)
            {
                centerLeaf.DOLocalMoveY(0.04F, smoothTime);
            }
            else
            {
                float moveOnY = (hexaColumn.hexaCellList.Count - 1) * currentLeafPos+0.04F;
                centerLeaf.DOLocalMoveY(moveOnY, smoothTime);
            }
            centerLeaf.position = leafPosition;
        }
    }

    public void CreateColumn()
    {
        hexaColumn = GameManager.instance.poolManager.GetHexaColumn();
        hexaColumn.InitColumn();
        hexaColumn.transform.SetParent(transform);
        hexaColumn.transform.localPosition = Vector3.zero;
        hexaColumn.currentBottomCell = this;
    }


    public void InitPrefilled(bool prefilled)
    {
        isPrefilled = prefilled;
        
        if (isPrefilled)
        {
            greenHexa.gameObject.SetActive(true);
        }
        else
        {
            greenHexa.gameObject.SetActive(false);
        }
    }


    public void InitRandomrefilled(bool prefilled)
    {
        isPrefilled = prefilled;

        if (isPrefilled)
        {
            greenHexa.gameObject.SetActive(true);
        }
        else
        {
            greenHexa.gameObject.SetActive(false);
        }
    }

    #region Blockers Init

    public void InitAdCell(bool isAdCell)
    {
        nearCellList = new List<BottomCell>();
        UnSelectCell();
        isAd = isAdCell;
        if (isAd)
            AdCell();
        else
            UnLockCell();
            OpenCell();
    }
    public void InitLockCell(bool isLockCell)
    {
        nearCellList = new List<BottomCell>();
        UnSelectCell();
        isLock = isLockCell;
        if (isLock)
            LockCell();
        else
            OpenLockCell();
    }
    public void InitWoodCell(bool isWoodCell)
    {   
        nearCellList = new List<BottomCell>();
        UnSelectCell();
        isWood = isWoodCell;
        if (isWood)
            WoodCell();
        else
            WoodCellOpen();
    }

    public void InitGrassCell(bool isGrassCell)
    {
        nearCellList = new List<BottomCell>();
        UnSelectCell();
        isGrass = isGrassCell;
        if (isGrass)
            GrassCell();
        else
            GrassCellOpen();
    }
    public void InitHoneyCell(bool isHoneyCell)
    {
        nearCellList = new List<BottomCell>();
        UnSelectCell();
        isHoney = isHoneyCell;
        if (isHoney)
            HoneyCell();
        else
            HoneyCellOpen();
    }
    public void InitIceCell(bool isIceCell)
    {
        nearCellList = new List<BottomCell>();
        UnSelectCell();
        isIce = isIceCell;
        if (isIce)
            IceCell();
        else
            IceCellOpen();
    }
    public void InitVinesCell(bool isVinesCell)
    {
        nearCellList = new List<BottomCell>();
        UnSelectCell();
        isVines = isVinesCell;
        if (isVines)
            VinesCell();
        else
            VinesCellOpen();
    }
    public void ClearBottomCell()
    {
        hexaColumn = null;
    }    

    public void SelectCell()
    {
        meshRenderer.material = cellSelectedMaterial;
        meshRenderer.transform.localPosition = new Vector3(0, 0.25f, 0);
    }

    public void UnSelectCell()
    {
        meshRenderer.material = cellMaterial;
        meshRenderer.transform.localPosition = new Vector3(0, 0.0f, 0);
    }

    private void AdCell()
    {
        meshRenderer.material = lockMaterial;
        AdObj.SetActive(true);
    }
    private void WoodCell()
    {
        woodObj.SetActive(true);
    }

    private void GrassCell()
    {
        grassObj.SetActive(true);   
    }

    private void HoneyCell()
    {
        honeyObj.SetActive(true);
    }

    private void IceCell()
    {
        iceHexa.SetActive(true);
        iceObj.SetActive(true);
    }
    private void VinesCell()
    {
        vinesHexa.SetActive(true);
        vinesObj.SetActive(true);
    }
    private void LockCell()
    {
        lockObj.SetActive(true);
    }
    private void OpenLockCell()
    {
        isLock = false;
        lockObj.SetActive(false);
    }

    private void OpenCell()
    {
        isAd = false;
        meshRenderer.material = cellMaterial;
        AdObj.SetActive(false);
    }

    private void WoodCellOpen()
    {
        isWood = false;
        woodObj.SetActive(false);
    }
    private void GrassCellOpen()
    {
        isGrass = false;
        grassObj.SetActive(false);
    }
    private void HoneyCellOpen()
    {
        isHoney = false;
        honeyObj.SetActive(false);
    }
    public void IceCellOpen()
    {
        isIce = false;
        iceObj.SetActive(false);
    }
    public void VinesCellOpen()
    {
        isVines = false;
        vinesObj.SetActive(false);
    }
    #endregion


    public void UnLockCell()
    {
        if (GameManager.instance.currentGameState != GameManager.GAME_STATE.PLAYING)
            return;
        else
        WatchAds();
    }

    public LayerMask bottomMask;

    public List<BottomCell> nearCellList;

    public void GetNearCells()
    {
        nearCellList.Clear();
        for(int i = 0; i < 6; i++)
        {
            Ray ray = new Ray(transform.position, Quaternion.Euler(0, 60.0f * i, 0) * transform.forward);
            RaycastHit hitData;

            if (Physics.Raycast(ray, out hitData, 1.5f, bottomMask))
            {
                BottomCell nearCell = hitData.transform.GetComponent<BottomCell>();
                if (nearCell == null)
                    Debug.Log("hexacellListNul");
                if (nearCell.hexaColumn.hexaCellList.Count > 0 && nearCell.hexaColumn.topColorID == hexaColumn.topColorID && nearCell.hexaColumn.topColorID != -1)
                {
                    nearCellList.Add(hitData.transform.GetComponent<BottomCell>());
                }
            }
        }
    }

    public void CheckNearByOnCompelteStake(int currentCount)
    {
        for (int i = 0; i < 6; i++)
        {
            Ray ray = new Ray(transform.position, Quaternion.Euler(0, 60.0f * i, 0) * transform.forward);
            RaycastHit hitData;

            if (Physics.Raycast(ray, out hitData, 1.5f, bottomMask))
            {
                BottomCell nearCell = hitData.transform.GetComponentInChildren<BottomCell>();
                if (nearCell.isWood == true)
                {
                    StartCoroutine(nearCell.woodBlocker.MakeWoodBreak());
                }
                else if (nearCell.isIce == true)
                {
                    if (nearCell.iceBlocker.MakeIceBreak())
                    {
                        nearCell.IceCellOpen();
                        BoardController.instance.currentHexaColumn = nearCell.hexaColumn_ice;
                        BoardController.instance.currentHitBottomCell = nearCell;
                        BoardController.instance.PutColumnInHolder_2(nearCell.hexaColumn_ice, nearCell);
                        nearCell.isIce = false;
                    }
                }
                else if (nearCell.isVines == true)
                {
                    if (nearCell.vinesBlocker.MakeVinesBreak())
                    {
                        nearCell.VinesCellOpen();
                        BoardController.instance.currentHexaColumn = nearCell.hexaColumn_Vines;
                        BoardController.instance.currentHitBottomCell = nearCell;
                        BoardController.instance.PutColumnInHolder_2(nearCell.hexaColumn_Vines, nearCell);
                        nearCell.isVines = false;
                    }
                }
                else if (nearCell.isHoney == true)
                {
                    StartCoroutine(nearCell.honeyBlocker.MakeHoneyBreak());
                }
                if (nearCell.isLock == true)
                {
                    nearCell.currentLockCount += currentCount;
                    nearCell.cost-= nearCell.currentLockCount;
                    if(nearCell.currentLockCount >= nearCell.cost)
                    {
                        StartCoroutine(nearCell.lockBlocker.MakeLockOpen());
                    }
                }
            }
        }
    }

    public void CheckNearOnStart()
    {
        if (isPrefilled)
        {
            PrefilledHexa(greenHexa);
        }
    }

    private void PrefilledHexa(HexaColumn hexaColumn)
    {
        BoardController.instance.currentHitBottomCell = this;
        BoardController.instance.currentHexaColumn = hexaColumn;
        BoardController.instance.PutColumnInHolder_2(hexaColumn, this);
    }

    public void CheckCurrentCellCompleteStake()
    {
        for (int i = 0; i < 6; i++)
        {
            Ray ray = new Ray(transform.position, Quaternion.Euler(0, 60.0f * i, 0) * transform.forward);
            if (Physics.Raycast(ray, 1.5f, bottomMask))
            {
                BottomCell currentCell = transform.GetComponent<BottomCell>();
                if(currentCell.isGrass == true)
                {
                    StartCoroutine(currentCell.grassBlocker.MakeGrassBreak());
                }
                /*if(currentCell.isLock == true)
                {
                    currentCell.currentLockCount++;
                    if (currentCell.isLock == true && currentCell.currentLockCount >= currentCell.lockCount)
                    {
                        currentCell.lockBlocker.MakeLockOpen();
                    }
                }*/
            }
        }
    }

    public void WatchAds()
    {
        AudioManager.instance.clickSound.Play();
        if (AdsControl.Instance.currentAdsType == ADS_TYPE.ADMOB)
        {
            if (AdsControl.Instance.rewardedAd != null)
            {
                if (AdsControl.Instance.rewardedAd.CanShowAd())
                {
                    AdsControl.Instance.ShowRewardAd(EarnReward);
                }
            }
        }
        else if (AdsControl.Instance.currentAdsType == ADS_TYPE.UNITY)
        {
            ShowRWUnityAds();
        }
        else if (AdsControl.Instance.currentAdsType == ADS_TYPE.MEDIATION)
        {
            if (AdsControl.Instance.rewardedAd.CanShowAd())

                AdsControl.Instance.ShowRewardAd(EarnReward);

            else
                ShowRWUnityAds();
        }
    }

    public void EarnReward(Reward reward)
    {
        AudioManager.instance.rewardDone.Play();
        isAd = false;
        meshRenderer.material = cellMaterial;
        AdObj.SetActive(false);
    }

    public void ShowRWUnityAds()
    {
        AdsControl.Instance.PlayUnityVideoAd((string ID, UnityAdsShowCompletionState callBackState) =>
        {

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                isAd = false;
                meshRenderer.material = cellMaterial;
                AdObj.SetActive(false);
            }

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                AdsControl.Instance.LoadUnityAd();
            }
        });
    }

}
