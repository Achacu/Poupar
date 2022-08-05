using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThoughtManager : MonoBehaviour
{
    [SerializeField] private int thoughts1stScene = 4; 
    [SerializeField] private int thoughts2ndScene = 5;
    [SerializeField] private Sprite thoughtGotSprite;
    [SerializeField] private TMPro.TextMeshProUGUI thoughtCounter;
    //[SerializeField] private float alpha;
    // Start is called before the first frame update
    void Start()
    {
        Image[] thoughtImgs = GetComponentsInChildren<Image>();


        string thoughtName;
        int thoughtGotCount = 0;
        int thoughtIndex = 0;
        for(int i = 0; i < thoughts1stScene; i++)
        {
            thoughtName = "Pensamento0"+i;
            if(PlayerPrefs.HasKey(thoughtName) && (PlayerPrefs.GetInt(thoughtName) == 1))
            {
                thoughtGotCount++;
                thoughtImgs[thoughtIndex].sprite = thoughtGotSprite;
            }
            thoughtIndex++;
        }
        for (int i = 0; i < thoughts2ndScene; i++)
        {
            thoughtName = "Pensamento1" + i;
            if (PlayerPrefs.HasKey(thoughtName) && (PlayerPrefs.GetInt(thoughtName) == 1))
            {
                thoughtGotCount++;
                thoughtImgs[thoughtIndex].sprite = thoughtGotSprite;
            }
            thoughtIndex++;
        }
        thoughtCounter.text = thoughtGotCount + "/" + (thoughts1stScene+thoughts2ndScene);

        //foreach(Image img in thoughtImgs)
        //{
        //    img.color = new Color(0,0,0,alpha);
        //}
    }
}
