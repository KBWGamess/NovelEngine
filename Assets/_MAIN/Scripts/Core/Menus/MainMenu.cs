using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VISUALNOVEL;

public class MainMenu : MonoBehaviour
{
    public const string MAIN_MENU_SCENE = "Main Menu";

    public static MainMenu instance {  get; private set; }

    public AudioClip menuMusic;
    public CanvasGroup mainPanel;
    private CanvasGroupController mainCG;

    private UIConfirmationMenu uiChoiceMenu => UIConfirmationMenu.instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        mainCG = new CanvasGroupController(this, mainPanel);
        AudioManager.instance.PlayTrack(menuMusic, channel: 0, startingVolume: 1);
    }

    public void Click_StartNewGame()
    {
        uiChoiceMenu.Show("Start a new game?", new UIConfirmationMenu.ConfirmationButton("Yes", StartNewGame), new UIConfirmationMenu.ConfirmationButton("No", null));
    }

    public void LoadGame(VNGameSave file)
    {
        VNGameSave.activeFile = file;
        StartCoroutine(StartingGame());
    }

    public void StartNewGame()
    {
        VNGameSave.activeFile = new VNGameSave();
        StartCoroutine(StartingGame());
    }

    private IEnumerator StartingGame()
    {
        mainCG.Hide(speed: 0.3f);
        AudioManager.instance.StopTrack(0);

        while (mainCG.isVisible)
            yield return null;

        UnityEngine.SceneManagement.SceneManager.LoadScene("VisualNovel");
    }
}