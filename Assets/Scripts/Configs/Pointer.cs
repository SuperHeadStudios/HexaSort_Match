using DG.Tweening;
using GameSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    [Header("Wheel Settings")]
    [SerializeField] private Transform wheel;
    [SerializeField] private Transform roots;
    [SerializeField] private List<int> angles;

    [Header("All Particles Of LuckyWheel"), Space (5)]
    [SerializeField] public ParticleSystem rewardGotParticle;
    [SerializeField] public ParticleSystem rewardPointParticle;
    [SerializeField] private ParticleSystem rightConfeti;
    [SerializeField] private ParticleSystem rightConfetiFall;
    [SerializeField] private ParticleSystem leftConfeti;
    [SerializeField] private ParticleSystem leftConfetiFall;

    [Header("------ Coins Move Settings ------"), Space(5)]
    [SerializeField] private GameObject cointPrefab;
    [SerializeField] private Transform cointTarget;
    [SerializeField] private Transform coinParent;
    public int rwValue;

    [SerializeField] private List<GameObject> cointList;

    [Header("UI and Assets")]
    [SerializeField] private GameObject resultObject;
    [SerializeField] private TextMeshProUGUI rewardValueTxt;
    [SerializeField] private TextMeshProUGUI wheelDialgueText;
    [SerializeField] private TextMeshProUGUI progressText;

    [SerializeField] private Image rewardIcon;
    [SerializeField] private Sprite coinSpr, hammerSpr, moveSpr, shuffleSpr;
    [SerializeField] private Sprite spinOffSpr;
    [SerializeField] private Button spnBtn, adsBtn, closeTextBtn;

    private bool isSpinning;
    private int spinIndex;

    public float maxValue;
    public float currentValue;
    [SerializeField] private Image fillImageSpin;

    [SerializeField] private float minSpinDuration = 2.0f; // Minimum duration of the spin
    [SerializeField] private float maxSpinDuration = 5.0f; // Maximum duration of the spin
    [SerializeField] private AnimationCurve easingCurve;  // Easing curve for deceleration

    
    public bool boosterReward;
    public bool coinReward;
    public bool spinAvail;
    

    private void Start()
    {
        angles = new List<int> { 45, 90, 135, 180, 225, 270, 315, 360 };
        spinIndex = PlayerPrefsManager.GetSpinIndexCount();

        if(spinIndex == 0)
        {
            maxValue = 5;
        }
        else if(spinIndex == 1)
        {
            maxValue = 10;
        }
        else
        {
            maxValue = 15;
        }
    }
    private void Update()
    {
        if(isSpinning)
        {
            closeTextBtn.gameObject.SetActive(false);
            adsBtn.gameObject.SetActive(false);
        }
        else
        {
            closeTextBtn.gameObject.SetActive(true);
        }
        
    }

    public void SpinWheel()
    {
        if (spinAvail)
        {
            spinAvail = false;
            spnBtn.GetComponent<Image>().sprite = spinOffSpr;
            if (isSpinning) return;
            isSpinning = true;
            int multipler = Random.Range(5, 10);
            float currentRotation = angles[Random.Range(0, angles.Count)];
            float randomRotation = (360 * multipler) + currentRotation;
            float spinDuration = Random.Range(minSpinDuration, maxSpinDuration);
            AudioManager.instance.spinWheel.Play();
            StartCoroutine(SpinAnimation(randomRotation, spinDuration));
            ResetWheelProgr();
            spnBtn.interactable = false;
        }
    }

    public void WatchAdsSpin()
    {
        AudioManager.instance.clickSound.Play();
        AdmobManager.instance.ShowRewardedAd(() =>
        {
            ResetRewardDisplay();
            GetComponent<Collider>().enabled = false;
            spinAvail = true;
            StartCoroutine(AdsSpin());
        });
    }


    public IEnumerator AdsSpin()
    {
        yield return new WaitForSeconds(1f);

        if (spinAvail)
        {
            spinAvail = false;
            spnBtn.GetComponent<Image>().sprite = spinOffSpr;
            if (isSpinning) yield break;
            isSpinning = true;
            int multipler = Random.Range(5, 10);
            float currentRotation = angles[Random.Range(0, angles.Count)];
            float randomRotation = (360 * multipler) + currentRotation;
            float spinDuration = Random.Range(minSpinDuration, maxSpinDuration);
            AudioManager.instance.spinWheel.Play();
            StartCoroutine(SpinAnimation(randomRotation, spinDuration));
            spnBtn.interactable = false;
        }
    }

    private IEnumerator SpinAnimation(float targetRotation, float duration)
    {
            float elapsedTime = 0f;
            float startRotation = wheel.localEulerAngles.z;
            float endRotation = startRotation + targetRotation;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;

                // Apply easing curve for smooth deceleration
                float easedT = easingCurve.Evaluate(t);

                float currentRotation = Mathf.Lerp(startRotation, endRotation, easedT);
                wheel.localEulerAngles = new Vector3(0, 0, currentRotation);

                yield return null;
            }
            // Snap to the final rotation
            wheel.localEulerAngles = new Vector3(0, 0, endRotation % 360);
            isSpinning = false;

            // Call reward logic
            StartCoroutine(OnSpinComplete());
    }

    private IEnumerator OnSpinComplete()
    {
        GetComponent<Collider>().enabled = true;
        UpdateUiText();
        AudioManager.instance.spinWheel.Stop();
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Spin Complete!");
        // Determine reward logic based on final rotation
        float finalAngle = wheel.eulerAngles.z;
        int rewardIndex = GetRewardIndex(finalAngle);
        Debug.Log("Reward Index: " + rewardIndex);
        ShowRewardWon();

        PlayerPrefsManager.SaveIsSpin(true);
       
        UpdateSpinWheel();
    }

    private int GetRewardIndex(float angle)
    {
        int sectionCount = 8;
        float sectionAngle = 360f / sectionCount;

        // Calculate index
        int index = Mathf.FloorToInt(angle / sectionAngle);
        return index % sectionCount;
    }

    private void Awake()
    {
        ResetRewardDisplay();
        GetComponent<Collider>().enabled = false;
    }

    public void UpdateSpinWheel()
    {
        UpdateUiText();
        currentValue = PlayerPrefsManager.GetSpineProgCount();

        float fillValue = currentValue / maxValue;
        fillImageSpin.fillAmount = fillValue;
        progressText.text = currentValue + "/"+ maxValue;

        if(currentValue >= maxValue)
        {
            spnBtn.gameObject.SetActive(true);
            adsBtn.gameObject.SetActive(false);
            spinAvail = true;
        }
        else
        {
            spnBtn.gameObject.SetActive(false);
            adsBtn.gameObject.SetActive(true);
        }
    }

    public void SpinFillBar()
    {
        currentValue = PlayerPrefsManager.GetSpineProgCount();
        if (currentValue >= maxValue) return;
        currentValue++;
        progressText.text = currentValue + "/"+ maxValue;
        PlayerPrefsManager.SaveSpineProgCount(currentValue);
        float fillValue = currentValue / maxValue;
        fillImageSpin.fillAmount = fillValue;

        if (currentValue >= maxValue)
        {
            spnBtn.gameObject.SetActive(false);
            adsBtn.gameObject.SetActive(true);
            spinAvail = true;
        }
        else
        {
            spnBtn.gameObject.SetActive(true);
            adsBtn.gameObject.SetActive(false);
        }
    }

    public void ResetWheelProgr()
    {
        if (currentValue < maxValue) return;

        currentValue = 0;
        PlayerPrefsManager.SaveSpineProgCount(currentValue);
        float fillValue = currentValue / maxValue;
        fillImageSpin.fillAmount = fillValue;
        spinIndex++;
        PlayerPrefsManager.SaveSpinIndexCount(spinIndex);

        UpdateUiText();
    }

    public void UpdateUiText()
    {
        if (spinIndex == 0)
        {
            maxValue = 5;
        }
        else if (spinIndex == 1)
        {
            maxValue = 10;
        }
        else
        {
            maxValue = 15;
        }

        float fillValue = currentValue / maxValue;
        fillImageSpin.fillAmount = fillValue;
        progressText.text = currentValue + "/ " + maxValue;

        if (spinIndex == 0) 
        {
            if (currentValue <= maxValue)
            {
                wheelDialgueText.text = "Win 5 levels to Spin the Wheel";
            }
            else
            {
                wheelDialgueText.text = "You Have Completed 5 levels";
            }
        }
        else if( spinIndex == 1)
        {
            if(currentValue <= maxValue)
            {
                wheelDialgueText.text = "Win 10 levels to Spin the Wheel";
            }
            else
            {
                wheelDialgueText.text = "You Have Completed 10 levels";
            }
        }
        else
        {
            if(currentValue <= maxValue)
            {
                wheelDialgueText.text = "Win 15 levels to Spin the Wheel";
            }
            else
            {
                wheelDialgueText.text = "You Have Completed 15 levels";
            }
        }

    }


    private void ShowRewardWon()
    {
        AudioManager.instance.boosterUnlockSound.Play();
        resultObject.GetComponent<Image>().enabled = true;
        roots.DOScale(Vector3.one, 0.5f).OnComplete(() =>
        {
            resultObject.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                rewardPointParticle.Play();
                if (coinReward)
                {
                    StartCoroutine(CoinAnimationMoving());
                }
                else if(boosterReward)
                {
                    StartCoroutine(RewardIconAnimation());
                    StartCoroutine(RewardMove());
                }
                else
                {
                    StartCoroutine(RewardIconAnimation());
                    StartCoroutine(RewardMove());
                }
            });
            leftConfeti.Play();
            leftConfetiFall.Play();
            rightConfeti.Play(); 
            rightConfetiFall.Play();
        });
    }
    private IEnumerator HideRewardWon()
    {
        yield return new WaitForSeconds(.5f);
        adsBtn.gameObject.SetActive(true);
        spnBtn.gameObject.SetActive(false);
        resultObject.GetComponent<Image>().enabled = false;
        resultObject.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.Linear);
    }

    private void SetRewardUI(Sprite icon, string value, System.Action rewardAction)
    {
        Debug.Log("SpritSwitching");
        rewardIcon.sprite = icon;
        rewardValueTxt.text = value;
        rewardAction.Invoke();
        /*roots.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
        {
            resultObject.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
        });*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "X5":
                rwValue = 10;
                SetRewardUI(coinSpr, "5 coins", () => GameManager.instance.AddCoin(0));
                Debug.Log("Trigger1");
                GetComponent<Collider>().enabled = false;
                break;
            case "X10":
                rwValue = 10;
                SetRewardUI(coinSpr, "10 Coins", () => GameManager.instance.AddCoin(0));
                GetComponent<Collider>().enabled = false;
                break;
            case "X2":
                SetRewardUI(shuffleSpr, "X2", () => GameManager.instance.AddShuffleBooster(2));
                GetComponent<Collider>().enabled = false;
                break;
            case "X3":
                SetRewardUI(moveSpr, "X3", () => GameManager.instance.AddMoveBooster(3));
                GetComponent<Collider>().enabled = false;
                break;
            case "X100":
                rwValue = 10;
                SetRewardUI(coinSpr, "100 Coins", () => GameManager.instance.AddCoin(0));
                GetComponent<Collider>().enabled = false;
                break;
            case "X1":
                rwValue = 10;
                SetRewardUI(hammerSpr, "X1", () => GameManager.instance.AddHammerBooster(1));
                GetComponent<Collider>().enabled = false;
                break;
                
        }

        if (collision.gameObject.CompareTag("X5") || collision.gameObject.CompareTag("X10") || collision.gameObject.CompareTag("X100"))
        {
            coinReward = true;
            
        }
        else if (collision.gameObject.CompareTag("X2") || collision.gameObject.CompareTag("X3") || collision.gameObject.CompareTag("X1"))
        {
            boosterReward = true;
        }
    }


    public IEnumerator RewardIconAnimation()
    {
        if (boosterReward)
        {
            yield return new WaitForSeconds(2);

            rewardIcon.rectTransform.DOScale(Vector3.zero, .5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                boosterReward = false;
                adsBtn.gameObject.SetActive(true);
            });
        }
        else
        {
            Debug.Log("DoNothing");
        }
    }

    public IEnumerator RewardIconCoinAnimation()
    {
        if (boosterReward)
        {
            yield return new WaitForSeconds(2);

            rewardIcon.rectTransform.DOScale(Vector3.zero, .5f).SetEase(Ease.Linear);
        }
        else
        {
            Debug.Log("DoNothing");
        }
    }

    public IEnumerator CoinAnimationMoving()
    {
        yield return new WaitForSeconds (1);
        if (coinReward)
        {
            StartCoroutine(SpawnCoins());
        }
    }

    public IEnumerator RewardMove()
    {
        if (boosterReward)
        {
            yield return new WaitForSeconds(2);
            AudioManager.instance.trailAudio.Play();
            rewardIcon.transform.DOLocalMoveY(-650, .3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                rewardGotParticle.Play();
                AudioManager.instance.flowerCollectedSound.Play();
                StartCoroutine(HideRewardWon());
            });
        }
        else
        {
            Debug.Log("DoNothing");
        }
    }


    /*
        private void UpdateButtons()
        {
            bool spinsLeft = --spinCount > 0;

            freeBtn.SetActive(spinsLeft);
            adsBtn.SetActive(!spinsLeft);
            closeBtn.SetActive(!spinsLeft);
            closeTextBtn.SetActive(spinsLeft);
        }*/
    public IEnumerator SpawnCoins()
    {
        int currentCoin = rwValue;

        for (int i = 0; i < rwValue; i++)
        {
            GameObject spwaCoin = Instantiate(cointPrefab, coinParent.position, Camera.main.transform.rotation, cointTarget);
            cointList.Add(spwaCoin);


            if (currentCoin == rwValue)
            {
                GameManager.instance.uiManager.coinView.CoinBarAnimationPlay();
            }
            currentCoin--;
            AudioManager.instance.coinCollectSound.Play();
            spwaCoin.transform.DOMove(cointTarget.position, 0.8f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                Destroy(spwaCoin, 0.01f);

                if (GameManager.instance.levelIndex > 0)
                {
                    GameManager.instance.AddCoin(1);
                }

                if (currentCoin == 0)
                {
                    GameManager.instance.uiManager.coinView.CoinAnimationStop();
                    StartCoroutine(HideRewardWon());
                    coinReward = false;
                    adsBtn.gameObject.SetActive(true);
                }
            });

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ResetRewardDisplay()
    {
        GetComponent<Collider>().enabled = false;
        //rewardValueTxt.text = "Spin the Wheel!";
        resultObject.transform.DOScale(Vector3.zero, 0.05f);
        roots.DOScale(Vector3.one, 0.01f);
        //GameManager.instance.uiManager.luckyWheelView.HideView();
        rewardIcon.transform.DOMoveY(-156, 0.5f).OnComplete(() =>
        {
            rewardIcon.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
        });
    }
}
