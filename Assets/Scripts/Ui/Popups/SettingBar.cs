using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingBar : MonoBehaviour
{
    private GameManager.GAME_STATE lastState;

    [SerializeField] private GameObject soundOnIcon;
    [SerializeField] private GameObject soundOffIcon;
    [SerializeField] private GameObject musicOnIcon;
    [SerializeField] private GameObject musicOffIcon;
    [SerializeField] private GameObject vibrateOnIcon;
    [SerializeField] private GameObject vibrateOffIcon;
    [SerializeField] private HomeView homeView;
    [SerializeField] private GameObject lifeBar;

    public bool isTogle = false;


    public void SoundBottonClick()
    {
        isTogle = !isTogle;
        if (isTogle == true)
        {
            AudioManager.instance.ToogleSound(true);
            soundOnIcon.SetActive(true);
            soundOffIcon.SetActive(false);
        }
        else
        {
            AudioManager.instance.ToogleSound(false);
            soundOffIcon.SetActive(true);
            soundOnIcon.SetActive(false) ;
        }
    }
    public void MusicBottonClick()
    {
        isTogle = !isTogle;
        if (isTogle == true)
        {
            AudioManager.instance.ToogleMusic(true);
            musicOnIcon.SetActive(true);
            musicOffIcon.SetActive(false);
        }
        else
        {
            AudioManager.instance.ToogleMusic(false);
            musicOffIcon.SetActive(true);
            musicOnIcon.SetActive(false) ;
        }
    }
    public void VibrateBottonClick()
    {
        isTogle = !isTogle;
        if (isTogle == true)
        {
            AudioManager.instance.ToogleHaptic(true);
            vibrateOnIcon.SetActive(true);
            vibrateOffIcon.SetActive(false);
        }   
        else
        {
            AudioManager.instance.ToogleHaptic(false);
            vibrateOffIcon.SetActive(true);
            vibrateOnIcon.SetActive(false) ;
        }
    }
    public void ReplayBottonClick()
    {
        GameManager.instance.BackToHome();
        homeView.PlayGame();
    }
    public void HomeBottonClick()
    {
        GameManager.instance.BackToHome();
        AudioManager.instance.clickSound.Play();
        lifeBar.SetActive(true);
    }
}
