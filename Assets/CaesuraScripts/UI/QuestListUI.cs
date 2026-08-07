using UnityEngine;
using Caesura.Quests;

namespace Caesura.UI
{
    public class QuestListUI : MonoBehaviour
    {
        [Tooltip("The prefab for a single quest entry in the list.")]
        [SerializeField] private QuestItemUI questPrefab;
        
        [Tooltip("Optional: Drag the player's QuestList here. If null, it tries to find a 'Player' tagged object.")]
        [SerializeField] private QuestList questList;

        [Tooltip("The root transform where quest items will be spawned. If null, uses this transform.")]
        [SerializeField] private Transform contentRoot;

        private void Start()
        {
            if (questList == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    questList = player.GetComponent<QuestList>();
                }
            }

            if (questList != null)
            {
                questList.onUpdate += Redraw;
                Redraw();
            }
            else
            {
                Debug.LogWarning("QuestListUI could not find a QuestList.");
            }
        }

        private void OnDestroy()
        {
            if (questList != null)
            {
                questList.onUpdate -= Redraw;
            }
        }

        private void Redraw()
        {
            DestroyChildren();
            Transform parent = contentRoot != null ? contentRoot : transform;
            foreach (QuestStatus status in questList.GetStatuses())
            {
                QuestItemUI uiInstance = Instantiate<QuestItemUI>(questPrefab, parent);
                uiInstance.Setup(status);
            }
        }

        private void DestroyChildren()
        {
            Transform parent = contentRoot != null ? contentRoot : transform;
            foreach (Transform child in parent)
            {  
                Destroy(child.gameObject);
            }
        }
    }
}
