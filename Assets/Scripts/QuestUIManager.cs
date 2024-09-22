using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestUIManager : MonoBehaviour
{
    public static QuestUIManager Instance;

    [SerializeField] GameObject questUIRowPrefab;


    [SerializeField] GameObject questContentWindow;
    [SerializeField] GameObject QuestUIHolder;

    List<Quest> questlist = new List<Quest>();

    [SerializeField] Transform questButton;


    

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        QuestUIHolder.SetActive(false);
        PlayerInput.Instance.OnQuestUIAction += PlayerInput_OnQuestUIAction;
        Quest.OnAnyQuestChanged += Quest_OnAnyQuestChanged;

    }

    private void Quest_OnAnyQuestChanged(object sender, Quest.QuestChangedEventArgs e)
    {
        //uipopup logic
    }

    private void PlayerInput_OnQuestUIAction(object sender, System.EventArgs e)
    {
        OpenCloseUI();
    }

    public void AddQuest(Quest quest)
    {
        questlist.Add(quest);
        RedrawQuestList();
    }
    

    public void RedrawQuestList()
    {
        DestroyAllChildren();
        //inventoryUISlotList.Clear();
        foreach (Quest quest in questlist)
        {
            Debug.Log(quest.GetQuestName());
            var questUIRow = Instantiate(questUIRowPrefab, questContentWindow.transform);
            Debug.Log(questUIRow);
            questUIRow.GetComponent<QuestRowUI>().SetUIText(quest.GetQuestName(), quest.GetQuestStatus().ToString());
        }
    }

    public void OpenCloseUI()
    {

        bool isActive = QuestUIHolder.activeSelf;
        var questRect = QuestUIHolder.GetComponent<RectTransform>();

        if (!isActive )
        {
            questRect.DOScale(0, 0);
            QuestUIHolder.SetActive(true);

            questRect.anchoredPosition = questButton.GetComponent<RectTransform>().anchoredPosition;
            questRect.DOAnchorPos(Vector2.zero, .5f, false);
            questRect.DOScale(1, .5f);
            
        }
        else
        {
            questRect.DOAnchorPos(questButton.GetComponent<RectTransform>().anchoredPosition, .5f, false).OnComplete(() => {
                QuestUIHolder.SetActive(false); 
            });
            questRect.DOScale(0, .5f);
        }

    }

    public void DestroyAllChildren()
    {
        // Loop through all child objects
        foreach (Transform child in questContentWindow.transform)
        {
            // Destroy each child GameObject
            Destroy(child.gameObject);
        }
    }


    public void DeselectButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
