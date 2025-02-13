using UnityEngine;
using UnityEngine.UI;

public class AdsButton : MonoBehaviour
{
    public bool IsNumberButton;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsNumberButton)
        {
            if (transform.GetChild(0).transform.GetChild(0).transform.GetComponent<Text>().text == "Ad")
            {
                //if (AdsControl.Instance.rewardedAd.CanShowAd())
                //{
                // transform.GetComponent<Button>().interactable = true;
                //}
                // else
                //  {
                //  transform.GetComponent<Button>().interactable = false;
                // }
            }
            else
                transform.GetComponent<Button>().interactable = true;
        }
        else
        {
            //if (AdsControl.Instance.rewardedAd.CanShowAd())
            //{
            //  transform.GetComponent<Button>().interactable = true;
            //}
            // else
            //{
            // transform.GetComponent<Button>().interactable = false;
            // }
        }
    }
}
