using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    [Header("Wheel Settings")]
    [SerializeField] private Transform wheel;
    [SerializeField] private float spinDuration = 4f;
    [SerializeField] private float initialSpeed = 1000f;
    [SerializeField] private Transform roots;

    [SerializeField] public ParticleSystem rewardGotParticle;
    [SerializeField] public ParticleSystem rewardPointParticle;
    [SerializeField] private ParticleSystem rightConfeti;
    [SerializeField] private ParticleSystem rightConfetiFall;
    [SerializeField] private ParticleSystem leftConfeti;
    [SerializeField] private ParticleSystem leftConfetiFall;

    [SerializeField] private Collider2D[] allColliders;

    [Header("------ Coins Move Settings ------"), Space(5)]
    [SerializeField] private float randPosi;
    [SerializeField] private GameObject cointPrefab;
    [SerializeField] private Transform cointTarget;
    [SerializeField] private Transform coinParent;
    public int rwValue;

    [SerializeField] private List<GameObject> cointList;

    [Header("UI and Assets")]
    [SerializeField] private GameObject resultObject;
    [SerializeField] private TextMeshProUGUI rewardValueTxt;
    [SerializeField] private TextMeshProUGUI dilogueText;
    [SerializeField] private Image rewardIcon;
    [SerializeField] private Sprite coinSpr, hammerSpr, moveSpr, shuffleSpr;
    [SerializeField] private Sprite spinBtn, spinOffBtn;
    [SerializeField] private GameObject freeBtn, adsBtn, closeBtn, closeTextBtn;

    private int spinCount = 3;
    private bool isSpinning;

    [SerializeField] private float minSpinDuration = 2.0f; // Minimum duration of the spin
    [SerializeField] private float maxSpinDuration = 5.0f; // Maximum duration of the spin
    [SerializeField] private float spinSpeed = 500.0f;     // Base spin speed
    [SerializeField] private AnimationCurve easingCurve;  // Easing curve for deceleration

    public bool isRewardComplete = false;
    public bool isCoinMakeTrue = false;
    private void OnEnable()
    {
       
    }

    public void SpinWheel()
    {
        if (isSpinning) return;

        isSpinning = true;
        float randomRotation = Random.Range(360f * 5, 360f * 10); 
        float spinDuration = Random.Range(minSpinDuration, maxSpinDuration);
        
        AudioManager.instance.spinWheel.Play();
        StartCoroutine(SpinAnimation(randomRotation, spinDuration));
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
        dilogueText.text = "Watch Ad To Spin Again";
        AudioManager.instance.spinWheel.Stop();
        EnableAllColliders();
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Spin Complete!");
        // Determine reward logic based on final rotation
        float finalAngle = wheel.eulerAngles.z;
        int rewardIndex = GetRewardIndex(finalAngle);
        Debug.Log("Reward Index: " + rewardIndex);
        ShowRewardWon();
    }

    private int GetRewardIndex(float angle)
    {
        // Assume the wheel has 8 sections, each 45 degrees
        int sectionCount = 8;
        float sectionAngle = 360f / sectionCount;

        // Calculate index
        int index = Mathf.FloorToInt(angle / sectionAngle);
        return index % sectionCount;
    }

    private void Start()
    {
        ResetRewardDisplay();
        DisableAllColliders();
        if(GameManager.instance.levelIndex == 5)
        {
            dilogueText.text = "You Have Completed Level - 5";
        }
        else if (GameManager.instance.levelIndex == 10)
        {
            dilogueText.text = "You Have Completed Level - 10";
        }
        else if (GameManager.instance.levelIndex == 15)
        {
            dilogueText.text = "You Have Completed Level - 15";
        }
        else if (GameManager.instance.levelIndex == 10)
        {
            dilogueText.text = "You Have Completed Level - 20";
        }
        else if (GameManager.instance.levelIndex == 10)
        {
            dilogueText.text = "You Have Completed Level - 25";
        }
        else if (GameManager.instance.levelIndex == 10)
        {
            dilogueText.text = "You Have Completed Level - 30";
        }
        else
        {
            dilogueText.text = "Tap Spin To Speen Wheel";
        }
    }

    private void DisableAllColliders()
    {
       GetComponent<Collider>().enabled = false;
    }
    private void EnableAllColliders()
    {
        GetComponent<Collider>().enabled = true;
    }

    private float EaseOutQuad(float t)
    {
        return t * (2 - t);
    }

    private void ShowRewardWon()
    {
        resultObject.GetComponent<Image>().enabled = true;
        roots.DOScale(Vector3.one, 0.5f).OnComplete(() =>
        {
            resultObject.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                rewardPointParticle.Play();
            });
            leftConfeti.Play();
            leftConfetiFall.Play();
            rightConfeti.Play(); 
            rightConfetiFall.Play();
            freeBtn.SetActive(false);
        });
    }
    private IEnumerator HideRewardWon()
    {
        yield return new WaitForSeconds(.5f);
        resultObject.GetComponent<Image>().enabled = false;
        resultObject.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.Linear);
        freeBtn.SetActive(true);
        freeBtn.GetComponent<Button>().interactable = false;
        freeBtn.GetComponent<Image>().sprite = spinOffBtn;
        closeBtn.SetActive(false);
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
                SetRewardUI(coinSpr, "5 coins", () => GameManager.instance.AddCoin(5));
                Debug.Log("Trigger1");
                break;
            case "X10":
                rwValue = 10;
                SetRewardUI(coinSpr, "10 Coins", () => GameManager.instance.AddCoin(10));
                break;
            case "X2":
                SetRewardUI(shuffleSpr, "X2", () => GameManager.instance.AddShuffleBooster(2));
                break;
            case "X3":
                SetRewardUI(moveSpr, "X3", () => GameManager.instance.AddMoveBooster(3));
                break;
            case "X100":
                rwValue = 10;
                SetRewardUI(coinSpr, "100 Coins", () => GameManager.instance.AddCoin(100));
                break;
            case "X1":
                rwValue = 10;
                SetRewardUI(hammerSpr, "X1", () => GameManager.instance.AddHammerBooster(1));
                break;
        }

        if (collision.gameObject.CompareTag("X5") || collision.gameObject.CompareTag("X10") || collision.gameObject.CompareTag("X100"))
        {
            isCoinMakeTrue = true;
            StartCoroutine(CoinAnimationMoving());
            StartCoroutine(RewardIconCoinAnimation());
        }
        else if (collision.gameObject.CompareTag("X2") || collision.gameObject.CompareTag("X3") || collision.gameObject.CompareTag("X1"))
        {
            isRewardComplete = true;
        }
        StartCoroutine(RewardIconAnimation());
        StartCoroutine(RewardMove());
    }


    public IEnumerator RewardIconAnimation()
    {
        if (isRewardComplete == true)
        {
            yield return new WaitForSeconds(2);

            rewardIcon.rectTransform.DOScale(Vector3.zero, .5f).SetEase(Ease.Linear);
        }
        else
        {
            Debug.Log("DoNothing");
        }
    }

    public IEnumerator RewardIconCoinAnimation()
    {
        if (isRewardComplete == true)
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
        if (isCoinMakeTrue)
        {
            StartCoroutine(SpawnCoins());
        }
    }

    public IEnumerator RewardMove()
    {
        if (isRewardComplete == true)
        {
            yield return new WaitForSeconds(2);
            AudioManager.instance.trailAudio.Play();
            rewardIcon.transform.DOLocalMoveY(-650, .3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                closeBtn.transform.DOScale(Vector3.one * 1.2f, 0.1f).SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    rewardGotParticle.Play();
                    AudioManager.instance.flowerCollectedSound.Play();
                    closeBtn.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBounce).OnComplete(() =>
                    {
                        StartCoroutine(HideRewardWon());
                    });
                });
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
            GameObject spwaCoin = Instantiate(cointPrefab, coinParent.position, Camera.main.transform.rotation, coinParent);
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
                }
            });

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ResetRewardDisplay()
    {
        DisableAllColliders();
        //rewardValueTxt.text = "Spin the Wheel!";
        resultObject.transform.DOScale(Vector3.zero, 0.05f);
        roots.DOScale(Vector3.one, 0.01f);
        //GameManager.instance.uiManager.luckyWheelView.HideView();
        closeBtn.SetActive(false);
        freeBtn.SetActive(true);
        rewardIcon.transform.DOMoveY(-156, 0.5f).OnComplete(() =>
        {
            rewardIcon.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
        });
    }
}
