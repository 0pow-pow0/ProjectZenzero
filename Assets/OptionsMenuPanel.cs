using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenuPanel : MonoBehaviour
{
    [SerializeField] public GameObject optionRect;
    [SerializeField] public GameObject buttonOption;
    [SerializeField] public GameObject buttonResume;
    [SerializeField] public GameObject buttonHome;


    public void Option_Button()
    {
        StartCoroutine(UIManagerGabri.GI().PressThen(buttonOption, () =>
        {
            // TODO
            //AudioManager.inst.AudioList[0].pitch = UnityEngine.Random.Range(0.8f, 1.0f);
            //AudioManager.inst.AudioList[0].Play();
            optionRect.SetActive(true);
            GameManager.GI().PauseGame();
            buttonOption.SetActive(false);
        }));
    }

    public void Resume_Button()
    {
        StartCoroutine(UIManagerGabri.GI().PressThen(buttonResume, () =>
        {
            //AudioManager.inst.AudioList[0].pitch = UnityEngine.Random.Range(0.8f, 1.0f);
            //AudioManager.inst.AudioList[0].Play();
            optionRect.SetActive(false);
            UIManagerGabri.GI().ExitUIElement();
            GameManager.GI().UnpauseGame();
            buttonOption.SetActive(true);
        }));
    }

    public void Home_Button()
    {
        //AudioManager.inst.AudioList[0].pitch = UnityEngine.Random.Range(0.8f, 1.0f);
        //AudioManager.inst.AudioList[0].Play();
        StartCoroutine(UIManagerGabri.GI().PressThen(buttonHome, () =>
        {
            SceneManager.LoadScene("HomeScreen");
        }));
    }

    public void SetVolumeSFX(float SliderValue)
    {
        //Mixer1.audioMixer.SetFloat("SFXVolume", MathF.Log10(SliderValue) * 20);
    }

    public void SetVolumeMusic(float SliderValue)
    {
        //Mixer1.audioMixer.SetFloat("MusicVolume", MathF.Log10(SliderValue) * 20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
