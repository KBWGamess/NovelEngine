using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VN_Configuration
{
    public static VN_Configuration activeConfig;

    public static string filePath => $"{FilePaths.root}vnconfig.cfg";

    public const bool ENCRYPT = false;

    public bool display_fullscreen = true;
    public string display_resolution = "1920x1080";
    public bool continueSkippingAfterChoice = false;
    public float dialogueTextSpeed = 1f;
    public float dialogueAutoReadSpeed = 1f;

    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public float voicesVolume = 1f;
    public bool musicMute = false;
    public bool sfxMute = false;
    public bool voicesMute = false;

    public float historyLogScale = 1f;

    public void Load()
    {
        var ui = ConfigMenu.instance.ui;

        ConfigMenu.instance.SetDisplayToFullScreen(display_fullscreen);
        ui.SetButtonColors(ui.fullscreen, ui.windowed, display_fullscreen);

        int res_index = 0;
        for (int i = 0; i < ui.resolutions.options.Count; i++)
        {
            string resolution = ui.resolutions.options[i].text;
            if (resolution == display_resolution)
            {
                res_index = i;
                break;
            }
        }
        ui.resolutions.value = res_index;

        ui.SetButtonColors(ui.skippingContinue, ui.skippingStop, continueSkippingAfterChoice);

        ui.architectSpeed.value = dialogueTextSpeed;
        ui.autoReaderSpeed.value = dialogueAutoReadSpeed;

        ui.musicVolume.value = musicVolume;
        ui.sfxVolume.value = sfxVolume;
        ui.voicesVolume.value = voicesVolume;

        ui.musicMute.sprite = musicMute ? ui.mutedSymbol : ui.unmutedSymbol;
        ui.sfxMute.sprite = sfxMute ? ui.mutedSymbol : ui.unmutedSymbol;
        ui.voicesMute.sprite = voicesMute ? ui.mutedSymbol : ui.unmutedSymbol;
    }

    public void Save()
    {
        FileManager.Save(filePath, JsonUtility.ToJson(this), encrypt: ENCRYPT);
    }
}
