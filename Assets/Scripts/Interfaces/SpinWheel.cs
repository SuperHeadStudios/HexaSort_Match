using System.Collections;
using System.Data;
using DG.Tweening;
using GameSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SpinWheel : MonoBehaviour
{
    [Header("GameObjects")]
    [Space(5)]
    [SerializeField] private Image BlackImage;
    [SerializeField] private GameObject Frame;
    [SerializeField] private GameObject Upper;
    [SerializeField] private GameObject SpinObj;
    [SerializeField] private GameObject CoinsShow;
    [SerializeField] private GameObject Lock;
    [SerializeField] private Image FillImage;
    [SerializeField] private TextMeshProUGUI FillText;
    [SerializeField] private GameObject UnlockText;
    

    [Header("Buttons")]
    [Space(5)]
    [SerializeField] private Button adBtn;

    [SerializeField] private Button SpinButton;
    [SerializeField] private GameObject CloseButton;
    [SerializeField] private Sprite DisbleSprite;
    [SerializeField] private Sprite NormalSprite;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private GameObject spinAgainBtn;

    [Header("Animation Panel Settings")]
    [Space(5)]
    [SerializeField] private GameObject Animation_Panel;
    [SerializeField] private GameObject Reward_Image;
    [SerializeField] private GameObject Ray;
    [SerializeField] private TextMeshProUGUI Reward_Text;
    [SerializeField] private Sprite Reward_Coin;
    [SerializeField] private Sprite Reward_Direpowerup;
    [SerializeField] private Sprite Reward_RotationPowerup;

    private bool isStarted;
    
    private int rewardIndex;
    private int randomChange;
    private float startAngle = 0;
    private float finalAngle;
    private float currentLerpRotationTime;
    private float maxLerpRotationTime = 5f;

    private float[] sectorsAngles = new float[] { -90, -135, -180, -225, -270, -315, -360, -45 };
    private void Awake()
    {

    }

    private void Start()
    {
        if (PlayerPrefsManager.GetFixLevelnumber() < 12)
            Lock.SetActive(true);
        else
            Lock.SetActive(false);
    }


    public void SpinAvailableBtn()
    {
        SpinButton.enabled = true;
        SpinButton.image.sprite = NormalSprite;
    }

    private void CheckSpinButton()
    {
        if (PlayerPrefsManager.GetSpinCount() >= 7)
        {
            adBtn.gameObject.SetActive(false);
            SpinButton.enabled = true;
            SpinButton.gameObject.transform.GetComponent<Image>().sprite = NormalSprite;
            float A = (float)PlayerPrefsManager.GetSpinCount() / 7;
            FillImage.DOFillAmount(A, 0.35f).SetEase(Ease.Linear);
            FillText.text = "7/7";
        }
                
        else
        {
            adBtn.gameObject.SetActive(true);
            SpinButton.gameObject.SetActive(false);
            SpinButton.enabled = false;
            SpinButton.image.sprite = DisbleSprite;
            float A = (float)PlayerPrefsManager.GetSpinCount() / 7;
            FillImage.DOFillAmount(A, 0.35f).SetEase(Ease.Linear);
            FillText.text = PlayerPrefsManager.GetSpinCount().ToString() + "/7";
        }
    }

    #region ----- PUBLIC METHOD -----

    public void OnPointerDown()
    {
        if (SpinButton.transform.GetComponent<Image>().sprite == DisbleSprite)
            return;
        SpinButton.transform.GetComponent<Image>().sprite = pressedSprite;
    }
    public void OnPointerUp()
    {
        if (SpinButton.transform.GetComponent<Image>().sprite == DisbleSprite)
            return;
        SpinButton.transform.GetComponent<Image>().sprite = NormalSprite;
    }

    public void OPenPopup()
    {
        if (Lock.activeInHierarchy)
        {
            if (!UnlockText.activeInHierarchy)
                UnlockText.SetActive(true);
            return;
        }
        BlackImage.enabled = true;
        Frame.gameObject.SetActive(true);
        Upper.gameObject.SetActive(true);
        CheckSpinButton();
    }

    public void ClosePopup()
    {
        BlackImage.enabled = false;
        Frame.gameObject.SetActive(false);
        Upper.gameObject.SetActive(false);
    }
    public void TurnWheelWithAd()
    {
        //mobManager.instance.ShowRewardedAd();
        //dio_Manager.instance.ClickSound();
        CloseButton.gameObject.SetActive(false);
        randomChange = Random.Range(0, 3);
        currentLerpRotationTime = 0f;

        int fullCircles = Random.Range(5, 8);
        float randomFinalAngle;
        randomFinalAngle = sectorsAngles[UnityEngine.Random.Range(0, sectorsAngles.Length)];

        // Here we set up how many circles our wheel should rotate before stop
        finalAngle = -(fullCircles * 360 - randomFinalAngle);
        isStarted = true;
        StartCoroutine(TurnRoutine());

    }


    public void TurnWheel()
    {
        //dio_Manager.instance.ClickSound();
        CloseButton.gameObject.SetActive(false);
        SpinButton.interactable = false;
        randomChange = Random.Range(0, 3);
        currentLerpRotationTime = 0f;

        int fullCircles = Random.Range(5, 8);
        float randomFinalAngle;
        randomFinalAngle = sectorsAngles[UnityEngine.Random.Range(0, sectorsAngles.Length)];

        // Here we set up how many circles our wheel should rotate before stop
        finalAngle = -(fullCircles * 360 - randomFinalAngle);
        isStarted = true;
        StartCoroutine(TurnRoutine());
        
    }

    
    #endregion

    #region ----- PRIVATE METHOD -----
    private IEnumerator TurnRoutine()
    {
        while (isStarted)
        {
            float t = currentLerpRotationTime / maxLerpRotationTime;

            if (randomChange == 0)
                t = 1f - (1f - t) * (1f - t);
            else
                t = t * t * t * (t * (6f * t - 15f) + 10f);

            float angle = Mathf.Lerp(startAngle, finalAngle, t); //Linear Interpolation
            SpinObj.transform.eulerAngles = new Vector3(0, 0, angle);

            // Increment timer once per frame
            currentLerpRotationTime += Time.deltaTime;

            if (currentLerpRotationTime > maxLerpRotationTime || SpinObj.transform.eulerAngles.z == finalAngle)
            {
                currentLerpRotationTime = maxLerpRotationTime;
                isStarted = false;
                startAngle = finalAngle % 360;

                GiveAwardByAngle();
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void GiveAwardByAngle()
    {
        // Here you can set up rewards for every sector of wheel
        switch ((int)startAngle)
        {
            case 0:
                rewardIndex = 0;
                StartCoroutine(RewardPopup(rewardIndex));
                break;
            case -45:
                rewardIndex = 1;
                StartCoroutine(RewardPopup(rewardIndex));
                break;
            case -90:
                rewardIndex = 2;
                StartCoroutine(RewardPopup(rewardIndex));
                break;
            case -135:
                rewardIndex = 3;
                StartCoroutine(RewardPopup(rewardIndex));
                break;
            case -180:
                rewardIndex = 4;
                StartCoroutine(RewardPopup(rewardIndex));
                break;
            case -225:
                rewardIndex = 5;
                StartCoroutine(RewardPopup(rewardIndex));
                break;
            case -270:
                rewardIndex = 6;
                StartCoroutine(RewardPopup(rewardIndex));
                break;
            case -315:
                rewardIndex = 7;
                StartCoroutine(RewardPopup(rewardIndex));
                break;
            case -360:
                rewardIndex = 8;
                StartCoroutine(RewardPopup(rewardIndex));
                break;
            default:
                Debug.Log("There is no reward for this angle, please check angles");
                break;
        }

    }

    private IEnumerator RewardPopup(int rewardIndex)
    {
        yield return new WaitForSeconds(0.1f);
        SwitchReward(rewardIndex);

        StopCoroutine(TurnRoutine());
    }

    private void SwitchReward(int reward)
    {
        switch (reward)
        {
            case 0:
                GetReward(true, false, 25);
                break;
            case 1:
                GetReward(false, true, 3);
                break;
            case 2:
                GetReward(true, false, 25);
                break;
            case 3:
                GetReward(false, false, 2);
                break;
            case 4:
                GetReward(true, false, 50);
                break;
            case 5:
                GetReward(false, false, 3);
                break;
            case 6:
                GetReward(true, false, 100);
                break;
            case 7:
                GetReward(false, true, 1);
                break;
            case 8:
                GetReward(true, false, 25);
                break;
        }
    }

    private void GetReward(bool IsCoins = false, bool IsDirPowerup = false, int Ammount = 0)
    {
        if (IsCoins)
        {
            OpenAnimation(IsCoins, IsDirPowerup, Reward_Coin, Ammount);
        }
        else
        {
            if (IsDirPowerup)
            {
                OpenAnimation(IsCoins, IsDirPowerup, Reward_Direpowerup, Ammount);
            }
            else
            {
                OpenAnimation(IsCoins, IsDirPowerup, Reward_RotationPowerup, Ammount);
            }
        }
    }

    private void OpenAnimation(bool ISCoins = false, bool IsDirPowerup = false, Sprite spr = null, int Ammount = 0)
    {
        Animation_Panel.gameObject.SetActive(true);

        Reward_Text.text = Ammount.ToString();
        Reward_Image.transform.GetComponent<Image>().sprite = spr;
        Reward_Image.transform.DOLocalMove(new Vector3(0, 0, 0), 0.4f).SetEase(Ease.OutBack);
        Reward_Image.transform.DOScale(2.2f, 0.4f).SetEase(Ease.OutBack);

        Ray.transform.DOLocalMove(new Vector3(0, 0, 0), 0.4f).SetEase(Ease.OutBack);
        Ray.transform.DOScale(0.25f, 0.4f).SetEase(Ease.OutBack);
        Ray.transform.DORotate(new Vector3(0, 0, 360), 3, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        StartCoroutine(WaitandOffAnimation(Ammount, ISCoins, IsDirPowerup));
    }

    IEnumerator WaitandOffAnimation(int Ammount, bool ISCoins, bool Direpowerup)
    {
        yield return new WaitForSeconds(2.5f);

        Reward_Image.transform.DOScale(0, 0.02f).SetEase(Ease.OutBack);
        Reward_Image.transform.DOLocalMove(new Vector3(0, 200, 0), 0.05f).SetDelay(0.1f).SetEase(Ease.OutBack);

        Ray.transform.DOScale(0, 0.02f).SetEase(Ease.OutBack);
        Ray.transform.DOLocalMove(new Vector3(0, 200, 0), 0.05f).SetDelay(0.1f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            // DOTween.Kill(Ray);
        });

        PlayerPrefsManager.SaveSpinCount(0);
        CheckSpinButton();
        CloseButton.gameObject.SetActive(true);

        if (ISCoins)
        {
            //CoinsShow.transform.GetComponent<Getcoins>().AddCoin(Ammount, 0.6f);
        }
        else
        {
            if (Direpowerup)
            {
                int u = PlayerPrefsManager.GetPassPowerup() + Ammount;
                PlayerPrefsManager.SavePassPwerup(u);
            }
            else
            {
                int m = PlayerPrefsManager.GetRotatwPowerup() + Ammount;
                PlayerPrefsManager.SaveRotatePwerup(m);
            }
        }
        yield return new WaitForSeconds(0.5f);
        Animation_Panel.gameObject.SetActive(false);
    }

    #endregion
}
