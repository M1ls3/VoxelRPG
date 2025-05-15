using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public TMP_Text buttonText;
    public AudioSource audioSource;
    private Button clickedButton;
    private float fontSizeIncrement = 0.05f;
    private int minFontSize = 140;
    private int maxFontSize = 170;
    private bool isMax = false;
    private bool isChangingColor = false;
    private float colorChangeDuration = 1.0f;
    Color startColor;
    Color endColor;

    void Start()
    {
        clickedButton = GetComponent<Button>();
        Button btn = clickedButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

        buttonText = GetComponentInChildren<TMP_Text>();
        buttonText.fontSize = minFontSize;
        startColor = buttonText.color;
        endColor = new Color(Random.Range(0.8f, 1f), Random.Range(0.8f, 1f), Random.Range(0.8f, 1f));
        StartCoroutine(ChangeColorOverTime());
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonText.fontSize < maxFontSize && !isMax)
        {
            buttonText.fontSize += fontSizeIncrement;
        }
        else if (buttonText.fontSize > minFontSize)
        {
            buttonText.fontSize -= fontSizeIncrement;
            isMax = true;
        }

        if (buttonText.fontSize <= minFontSize)
        {
            isMax = false;
        }
    }

    IEnumerator ChangeColorOverTime()
    {
        isChangingColor = true;
        float elapsedTime = 0f;

        while (elapsedTime < colorChangeDuration)
        {
            float t = elapsedTime / colorChangeDuration;
            buttonText.color = Color.Lerp(startColor, endColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        startColor = buttonText.color;
        endColor = new Color(Random.Range(0.1f, 1f), Random.Range(0.1f, 1f), Random.Range(0.1f, 1f));
        StartCoroutine(ChangeColorOverTime());
    }

    void TaskOnClick()
    {
        try
        {
            Debug.Log("Button to next Scene!");
            audioSource.Play();
            SceneManager.LoadScene("SampleScene");
            Time.timeScale = 1f;
        }
        catch { Debug.Log("Error"); }

        }
    }



