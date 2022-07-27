using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractCursor : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite canInteractSprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnEnable()
    {
        Interact.OnCanInteractChange += ChangeSprite;
    }

    private void ChangeSprite(bool canInteract)
    {
        img.sprite = canInteract ? canInteractSprite : normalSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
