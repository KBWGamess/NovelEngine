using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class DialogueLine
    {
        public string rawData {  get; private set; } = string.Empty;

        public DL_SPEAKER_DATA speakerData;
        public DL_DIALOGUE_DATA dialogueData;
        public DL_COMMAND_DATA commandData;

        public bool hasSpeaker => speakerData != null; //speaker != string.Empty;
        public bool hasDialogue => dialogueData != null;
        public bool hasCommanhs => commandData != null;

        public DialogueLine(string rawLine, string speaker, string dialogue, string commands)
        {
            rawData = rawLine;
            this.speakerData = (string.IsNullOrWhiteSpace(speaker) ? null : new DL_SPEAKER_DATA(speaker));
            this.dialogueData = (string.IsNullOrWhiteSpace(dialogue) ? null : new DL_DIALOGUE_DATA(dialogue));
            this.commandData = (string.IsNullOrWhiteSpace(commands) ? null : new DL_COMMAND_DATA(commands));
        }
    }
}
