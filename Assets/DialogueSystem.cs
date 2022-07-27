using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private DialogueGroup[] dialogueGroups;

    [System.Serializable]
    public class DialogueGroup
    {
        public EventSender trigger;
        public LineGroupSO group;

        public DialogueGroup(EventSender trigger, LineGroupSO group)
        {
            this.trigger = trigger;
            this.group = group;
        }
    }

    //[SerializeField] private LineGroupSO currentGroup;
    // Start is called before the first frame update
    void OnEnable()
    {
        //ShowLineGroup(currentGroup);
        foreach(DialogueGroup dGroup in dialogueGroups)
        {
            dGroup.trigger.OnActivate += ProcessDialogueEvent;
        }
    }
    void OnDisable()
    {
        //ShowLineGroup(currentGroup);
        foreach (DialogueGroup dGroup in dialogueGroups)
        {
            dGroup.trigger.OnActivate -= ProcessDialogueEvent;
        }
    }

    private void ProcessDialogueEvent(EventSender sender)
    {
        for(int i = 0; i < dialogueGroups.Length; i++)
        {
            if(dialogueGroups[i].trigger.Equals(sender))
            {
                ShowLineGroup(dialogueGroups[i].group);
            }
        }
    }

    private Coroutine lineGroupCorot = null;
    private void ShowLineGroup(LineGroupSO lineGroup)
    {
        if(lineGroupCorot != null)
        {
            StopCoroutine(lineGroupCorot);
            lineGroupCorot = null;
        }
        lineGroupCorot = StartCoroutine(ReadLineGroup(lineGroup));
    }

    private IEnumerator ReadLineGroup(LineGroupSO lineGroup)
    {
        dialogueText.enabled = true;
        for(int i = 0; i < lineGroup.lines.Length; i++)
        {
            dialogueText.text = lineGroup.lines[i].line;
            yield return new WaitForSeconds(lineGroup.lines[i].duration);
        }
        dialogueText.enabled = false;
    }
}
