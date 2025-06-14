using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using GameSystem;

public class LoadingScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject adsObj;
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private float loadingDuration = 3f;
    [SerializeField] private GameObject AdPanel;
    [SerializeField] private GameObject TermsObj;
    [SerializeField] private TermsAndConditionsPopup TermsAndConditionsPopup;

    public AsyncOperation operation;
    public bool isInitialized = false;
    public bool isLoadHome = false;

    void Start()
    {
        AdPanel.SetActive(false);
        StartCoroutine(LoadSceneWithProgress());
    }

    IEnumerator LoadSceneWithProgress()
    {
        operation = SceneManager.LoadSceneAsync("Game");
        operation.allowSceneActivation = false;

        float elapsedTime = 0f;

        while (elapsedTime < loadingDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsedTime / loadingDuration) * 1f; // Max at 80%

            progressBar.value = progress;
            loadingText.text = (progress * 100f).ToString("F0") + "%";

            if (progress >= 0.25f && !TermsAndConditionsPopup.TermAcceptetd())
            {
                TermsObj.SetActive(true);
                yield return new WaitUntil(() => TermsAndConditionsPopup.TermAcceptetd());
            }

            if (!AppLovingAppOpenAdManager.instance.NoAds)
            {
                if (progress >= 0.8f && !AppLovingAppOpenAdManager.instance.IsAppOpenAdReady() && !PlayerPrefsManager.GetIsFirstPlay())
                {
                    while (!AppLovingAppOpenAdManager.instance.IsAppOpenAdReady())
                    {
                        AppLovingAppOpenAdManager.instance.LoadAppOpenAd();
                        yield return new WaitForSeconds(0.5f); // avoid hammering the load method
                    }
                }
            }

            yield return null;
        }

        progressBar.value = 1f;
        loadingText.text = "100%";


        if (PlayerPrefsManager.GetIsFirstPlay())
        {
            AppLovingAppOpenAdManager.instance.isInitialized = true;
            PlayerPrefsManager.SetIsFirstPlay(false);
        }
        else
        {
            StartCoroutine(ShowAppOpenWithDelay());
            /*float elapsedTime = 0f;

            while (elapsedTime < loadingDuration)
            {
                elapsedTime += Time.deltaTime;

                float progress = Mathf.Clamp01(elapsedTime / loadingDuration) * 0.8f; // Max at 80%

                progressBar.value = progress;
                loadingText.text = (progress * 100f).ToString("F0") + "%";
                yield return null;
            }

            elapsedTime = 0f;
            while (!isInitialized)
            {
                yield return null;
            }

            elapsedTime = 0;
            while (elapsedTime < 1)
            {
                elapsedTime += Time.deltaTime;
                float progress = 0.8f + (Mathf.Clamp01(elapsedTime / 1f) * 0.2f); // 80% to 100%

                progressBar.value = progress;
                loadingText.text = (progress * 100f).ToString("F0") + "%";
                yield return null;
            }*/
        }

        AdPanel.SetActive(true);

        while (!AppLovingAppOpenAdManager.instance.isInitialized && !AppLovingAppOpenAdManager.instance.NoAds)
        {
            yield return null;
        }
        operation.allowSceneActivation = AppLovingAppOpenAdManager.instance.isInitialized;
    }

    private IEnumerator ShowAppOpenWithDelay()
    {
        AdPanel.SetActive(false);
        yield return new WaitForSeconds(1f);
        AppLovingAppOpenAdManager.instance.ShowAdIfReady();
    }


}
