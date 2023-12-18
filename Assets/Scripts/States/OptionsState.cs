using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.Audio;

public class OptionsState : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if(GameManager.instance.currentState == GameManager.GameState.OptionsScreenState)
        {

        
        if (GameManager.instance.MapOfDay.isOn == true)
        {
            Debug.Log("MapOfDay is on");
            foreach (GameObject textAreaChild in GameManager.instance.TextAreaChildren)
            {
                if (textAreaChild.name == "Seed Text")
                {
                    textAreaChild.SetActive(false);
                    //SeedTwo.text = "0";
                    GameManager.instance.GetComponent<MapGenerator>().isMapOfTheDay = true;
                }
            }
        }
            if (GameManager.instance.MapOfDay.isOn == false)
            {
                GameManager.instance.GetComponent<MapGenerator>().isMapOfTheDay = false;
                foreach (GameObject textAreaChild in GameManager.instance.TextAreaChildren)
                {
                    if (textAreaChild.name == "Seed Text")
                    {
                        textAreaChild.SetActive(true);
                    }
                }
                foreach (GameObject toggleChild in GameManager.instance.MapOfDayToggleChildren)
                {
                    Debug.Log(toggleChild.name + "'s input field is " + toggleChild.GetComponent<TMP_InputField>());
                    if (toggleChild.GetComponent<TMP_InputField>() != null)
                    {
                        int.TryParse(toggleChild.GetComponent<TMP_InputField>().text, out GameManager.instance.GetComponent<MapGenerator>().mapSeed);
                        Debug.Log("mapSeed is... " + GameManager.instance.GetComponent<MapGenerator>().mapSeed);
                    }
                }
            }
        }
    }
    public void MapSeedChange()
    {

        Debug.Log("calling map seed change in options state.");
        foreach (GameObject textAreaChild in GameManager.instance.TextAreaChildren)
        {
            if (textAreaChild.GetComponent<Text>() != null)
            {
                Debug.Log("Text Area children that are Text: " + textAreaChild.GetComponentInChildren<Text>().name);
            }
            Debug.Log(textAreaChild.name);
            if (textAreaChild.name == "Seed Text")
            {
                textAreaChild.SetActive(true);
                //Debug.Log(textAreaChild.GetComponents<TextMeshProUGUI>());    - testing
                GameManager.instance.SeedTwo = textAreaChild.GetComponent<TextMeshProUGUI>();

                GameManager.instance.SeedTwo.text = "0";

                Debug.Log(GameManager.instance.SeedTwo.text);
                if (GameManager.instance.SeedTwo.text == textAreaChild.GetComponent<TextMeshProUGUI>().text)
                {
                    Debug.Log("if statement started");
                    GameManager.instance.SeedTwo.text = textAreaChild.GetComponent<TextMeshProUGUI>().text;
                    Debug.Log(" ''" + GameManager.instance.SeedTwo.text + "'' ");
                    if (GameManager.instance.SeedTwo.text == null)
                    {
                        Debug.Log("SeedTwo text is null");
                        GameManager.instance.GetComponent<MapGenerator>().isMapOfTheDay = true;
                        Debug.Log("is map of the day is on.");
                    }
                    if (GameManager.instance.SeedTwo.text == "00")
                    {
                        Debug.Log("Randomly generate.");
                        GameManager.instance.GetComponent<MapGenerator>().mapSeed = 0;
                    }
                    if (GameManager.instance.SeedTwo.text != null && GameManager.instance.SeedTwo.text != "00")
                    {
                        Debug.Log("Set mapSeed to their seed number for instantiation");
                        Debug.Log(" string length is.. " + GameManager.instance.SeedTwo.text.Length);
                        //int.TryParse()

                    }
                }
            }
        }
    }

    public void OnMultiplayercChange()
    {
        if (GameManager.instance.Multiplayer.isOn == false)
        {
            GameManager.instance.isMultiplayer = false;
            Debug.Log("Multiplayer is off");
        }
        if (GameManager.instance.Multiplayer.isOn == true)
        {
            GameManager.instance.isMultiplayer = true;
            Debug.Log("Multiplayer is on");
        }
    }
    public void OnSFXVolumeChange()
    {
        float sfx;
        GameManager.instance.audioMixer.SetFloat("sfxVolume", GameManager.instance.SFXVolume.value );
        Debug.Log("SFX Volume is " + GameManager.instance.SFXVolume.value + " .... and audio source volume is " + GameManager.instance.audioMixer.GetFloat("sfxVolume", out sfx) + sfx);
    }
    public void OnMusicVolumeChange()
    {
        float music;
        GameManager.instance.audioMixer.SetFloat("musicVolume", GameManager.instance.MusicVolume.value - 15);
        Debug.Log("Music Volume is " + GameManager.instance.MusicVolume.value + " .... and audio source volume is " + GameManager.instance.audioMixer.GetFloat("musicVolume", out music) + music);
    }



    /*   This is what I can use for toggle functions that will be called off of things like toggle buttons and input field changes
    public void OnMapOfDayChange()
    {
        if (GameManager.instance.MapOfDay.isOn == true)
        {
            GameManager.instance.GetComponent<MapGenerator>().isMapOfTheDay = true;
        }
        else if (GameManager.instance.MapOfDay.isOn == false)
        {
            GameManager.instance.GetComponent<MapGenerator>().isMapOfTheDay = false;
        }
    }
    */
}
