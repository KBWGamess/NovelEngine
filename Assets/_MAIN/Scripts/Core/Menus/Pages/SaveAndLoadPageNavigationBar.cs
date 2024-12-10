using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveAndLoadPageNavigationBar : MonoBehaviour
{
    [SerializeField] private SaveAndLoadMenu menu;

    private bool initialized = false;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject previousButton;
    [SerializeField] private GameObject nextButton;

    private const int MAX_BUTTONS = 5;

    public int selectedPage { get; private set; } = 1;
    private int maxPages = 0;

    private void Start()
    {
        InitilizeMenu();
    }

    private void InitilizeMenu()
    {
        if (initialized)
            return;

        initialized = true;

        maxPages = Mathf.CeilToInt((float)SaveAndLoadMenu.MAX_FILES / menu.slotsPerPage);
        int pageButtonLimit = MAX_BUTTONS < maxPages ? MAX_BUTTONS : maxPages;

        for (int i = 1; i <= pageButtonLimit; i++)
        {
            GameObject ob = Instantiate(buttonPrefab.gameObject, buttonPrefab.transform.parent);
            ob.SetActive(true);

            Button button = ob.GetComponent<Button>();

            ob.name = i.ToString();
            TextMeshProUGUI txt = button.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = i.ToString();
            int closureIndex = i;
            button.onClick.AddListener(() => SelectedSaveFilePage(closureIndex));
        }

        previousButton.SetActive(pageButtonLimit < maxPages);
        nextButton.SetActive(pageButtonLimit < maxPages);

        nextButton.transform.SetAsLastSibling();
    }

    private void SelectedSaveFilePage(int pageNumber)
    {
        selectedPage = pageNumber;
        menu.PopulateSaveSlotsForPage(pageNumber);
    }

    public void ToNextPage()
    {
        if (selectedPage < maxPages)
            SelectedSaveFilePage(selectedPage + 1);
    }

    public void ToPreviousPage()
    {
        if (selectedPage > 1)
            SelectedSaveFilePage(selectedPage - 1);
    }
}
