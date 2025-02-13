using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerAd : MonoBehaviour
{
    private void Start()
    {
        AdmobManager.instance.ShowBannerAd();
    }
}
