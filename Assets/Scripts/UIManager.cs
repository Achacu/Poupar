using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [System.Serializable]
    public class ScreenData
    {
        public Transform screen;
        public float waitTime;
        public EventSender sender;
        public float fadeOutToInDelay;

        public ScreenData(Transform screen, float waitTime, EventSender sender, float fadeOutToInDelay)
        {
            this.screen = screen;
            this.waitTime = waitTime;
            this.sender = sender;
            this.fadeOutToInDelay = fadeOutToInDelay;
        }
    }


    [SerializeField] private ScreenData[] screensData;
    [SerializeField] private GameObject blockingPanel;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    [SerializeField, ContextMenuItem("NextScreen", "NextScreen")] private int screenIndex;
    public void NextScreen()
    {
        StartCoroutine(NextScreenCorot());
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator NextScreenCorot()
    {
        StartCoroutine(FadeInOutScreen(screensData[screenIndex].screen, false));
        screensData[screenIndex].sender?.TriggerEvent(false);

        print("next screen");
        screenIndex++;
        if (screenIndex >= screensData.Length) yield break;

        blockingPanel.SetActive(true);
        yield return new WaitForSeconds(screensData[screenIndex-1].fadeOutToInDelay);
        blockingPanel.SetActive(false);

        StartCoroutine(FadeInOutScreen(screensData[screenIndex].screen, true));
        screensData[screenIndex].sender?.TriggerEvent(true);

        if (screensData[screenIndex].waitTime <= 0) yield break;

        yield return new WaitForSeconds(screensData[screenIndex].waitTime);
        NextScreen();
    }

    [SerializeField] private float fadeOutTime = 1f;
    [SerializeField] private float fadeInTime = 1f;
    private IEnumerator FadeInOutScreen(Transform screen, bool fadeIn)
    {
        float startTime = Time.time;
        TextMeshProUGUI[] texts = screen.GetComponentsInChildren<TextMeshProUGUI>();
        Image[] imgs = screen.GetComponentsInChildren<Image>();
        float alpha;
        float timeRatio = 0f;
        if (fadeIn) screen.gameObject.SetActive(true);
        while (timeRatio <= 1)
        {
            timeRatio = (Time.time - startTime) / (fadeIn? fadeInTime : fadeOutTime);
            alpha = fadeIn? timeRatio : 1-timeRatio;
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].alpha = alpha;
            }
            Color varColor;
            for (int i = 0; i < imgs.Length; i++)
            {
                varColor = imgs[i].color;
                varColor.a = alpha;
                imgs[i].color = varColor;
            }
            yield return null;
        }
        if(!fadeIn) screen.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
