using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using GameSystem;
using Unity.VisualScripting;

public class LoadingScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject adsObj;
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private float loadingDuration = 3f;
    [SerializeField] private GameObject AdPanel;
    [SerializeField] private GameObject TermsObj;
    [SerializeField] private TermsAndConditionsPopup TermsAndConditionsPopup;

    [SerializeField] private bool isGameLoading = false;

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

            if (!isGameLoading)
            {
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
            }
            yield return null;
        }

        if (!isGameLoading)
        {

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

            }

            AdPanel.SetActive(true);

            while (!AppLovingAppOpenAdManager.instance.isInitialized && !AppLovingAppOpenAdManager.instance.NoAds)
            {
                yield return null;
            }
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
