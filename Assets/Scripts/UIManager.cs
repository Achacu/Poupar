using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

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

    public void OnEnable()
    {
        CharMove.OnEscapePressed += TogglePauseMenu;
        if(nextSceneSender)nextSceneSender.OnActivate += NextScreen;
        Time.timeScale = 1;
    }
    public void OnDisable()
    {
        CharMove.OnEscapePressed -= TogglePauseMenu;
        if (nextSceneSender) nextSceneSender.OnActivate -= NextScreen;
    }

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject soundSettingsMenu;
    [SerializeField] private GameObject HUD;
    [SerializeField] private bool isPaused = false;
   // [SerializeField] private PlayerCamManager playerManager;
    private void TogglePauseMenu()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        soundSettingsMenu.SetActive(false);
        HUD.SetActive(!isPaused);
        //playerManager.SetPlayerInControl(!isPaused);
        Time.timeScale = isPaused ? 0 : 1;

    }
    public void DestroySelf() => Destroy(gameObject);
    public void LoadScene(int buildIndex) => SceneManager.LoadScene(buildIndex);

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(this);
        SetBusVolume(VolumeSlider.SliderType.Music, PlayerPrefs.HasKey("MusicVolume") ? PlayerPrefs.GetFloat("MusicVolume") : 0.5f);
        SetBusVolume(VolumeSlider.SliderType.SFX, PlayerPrefs.HasKey("SFXVolume") ? PlayerPrefs.GetFloat("SFXVolume") : 0.5f);

        if (screensData[screenIndex].waitTime > 0) NextScreen();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    [SerializeField, ContextMenuItem("NextScreen", "NextScreen")] private int screenIndex;
    [SerializeField] private EventSender nextSceneSender;
    private void NextScreen(EventSender sender) => NextScreen();
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
        if (screenIndex >= screensData.Length)
        {
            LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            yield break;
        }

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

    public static void SetBusVolume(VolumeSlider.SliderType type, float value)
    {        
        string busName = (type == VolumeSlider.SliderType.Music) ? "Musica" : "FX";
        FMOD.Studio.Bus bus = FMODUnity.RuntimeManager.GetBus("bus:/" + busName); //"bus:/Master/" +
        bus.setVolume(value);
    }

}
