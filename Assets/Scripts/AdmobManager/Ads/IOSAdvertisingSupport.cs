/*using System.Collections;
using System.Collections.Generic;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class IOSAdvertisingSupport : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
#if UNITY_IOS
        int mainVersion = 0;
        string[] versionPart = UnityEngine.iOS.Device.systemVersion.Split('.');
        int.TryParse(versionPart[0], out mainVersion);


        /// only run on iOS 14 devices

        if (mainVersion >= 14)
            ShowIOSTracking();
#endif

    }
    public void ShowIOSTracking()
    {
#if UNITY_IOS
        // check with iOS to see if the user has accepted or declined tracking
        var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

        if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
        }
#else
            Debug.Log("Unity iOS Support: App Tracking Transparency status not checked, because the platform is not iOS.");
#endif

     

    }
}
*/