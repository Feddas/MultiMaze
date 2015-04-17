using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;

public class TextFader : MonoBehaviour
{
    public Text[] UiTextFields;
    private int TextSpeed = 5;

    private List<string> textToShow;
    private Animator menuAnim;
    private bool isMenuOn = false;
    private int indexTextToShow;
    private int indexFullFadeSplits;
    private List<string> fullFadeSplits;
    private int numberOfShowTextThreads;

    void Start()
    {
        menuAnim = GetComponent<Animator>();
    }

    void Update()
    {
    }

    public void ShowText(string stuff)
    {
        if (isMenuOn == false)
        {
            menuAnim.SetTrigger("FadeIn");
            isMenuOn = true;
        }
        else
        {

        }

        numberOfShowTextThreads++; // TODO: handle race condition if time on first thread is longer than time on second thread. As this would cause the second thread to be cancelled when the first thread should have been canceled instead.

        indexFullFadeSplits = indexTextToShow = 0;
        fullFadeSplits = stuff.Split(':').ToList();
        textToShow = fullFadeSplits[indexFullFadeSplits].Split(';').ToList();

        UpdateUiText(textToShow[indexTextToShow]);
        float secondsToShow = (float)textToShow[indexTextToShow].Length / TextSpeed;
        //Debug.Log("showing " + textToShow[indexTextToShow] + " for " + secondsToShow + "s out of:" + stuff);
        this.Delay(secondsToShow, NextText);
    }

    void NextText()
    {
        if (numberOfShowTextThreads > 1) // abort due to interruption by new thread
        {
            numberOfShowTextThreads--;
            return;
        }

        indexTextToShow++;
        if (indexTextToShow >= textToShow.Count) // completed a single non-fullfade sequence
        {
            NextFullFade();
            return;
        }
        else
        {
            UpdateUiText(textToShow[indexTextToShow]);
            float secondsToShow = (float)textToShow[indexTextToShow].Length / TextSpeed;
            //Debug.Log("NextText: " + secondsToShow + " : " + currentTextIndex);
            this.Delay(secondsToShow, NextText);
        }
    }

    void NextFullFade()
    {
        indexTextToShow = 0;
        indexFullFadeSplits++;

        if (indexFullFadeSplits >= fullFadeSplits.Count) // completed entire sequence
        {
            menuAnim.SetTrigger("FadeOut");
            isMenuOn = false;
            numberOfShowTextThreads--;
        }
        else // fade to black
        {
            menuAnim.SetTrigger("FullBlack");

            this.Delay(2, FullFadeBackIn);
        }
    }

    void FullFadeBackIn()
    {
        menuAnim.SetTrigger("FullClear");
        textToShow = fullFadeSplits[indexFullFadeSplits].Split(';').ToList();
        UpdateUiText(textToShow[indexTextToShow]);
        float secondsToShow = (float)textToShow[indexTextToShow].Length / TextSpeed;
        this.Delay(secondsToShow, NextText);
    }

    void UpdateUiText(string newText)
    {
        foreach (var uiText in UiTextFields)
        {
            uiText.text = newText;
        }
    }
}