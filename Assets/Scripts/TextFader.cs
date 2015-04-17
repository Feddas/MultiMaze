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
            // TODO: interrupt previous menu (or play after current menu is done)
        }

        indexFullFadeSplits = indexTextToShow = 0;
        fullFadeSplits = stuff.Split(':').ToList();
        textToShow = fullFadeSplits[indexFullFadeSplits].Split(';').ToList();

        Debug.Log("showing " + textToShow[indexTextToShow] + " out of:" + stuff);
        UpdateUiText(textToShow[indexTextToShow]);
        float secondsToShow = (float)textToShow[indexTextToShow].Length / TextSpeed;
        //Debug.Log("Entered: " + secondsToShow);
        this.Delay(secondsToShow, NextText);
    }

    void NextText()
    {
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