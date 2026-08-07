using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Caesura.Quests;

namespace Caesura.UI
{
    public class QuestItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI progress;
        
        [Header("Tooltip Settings (Optional)")]
        [Tooltip("The prefab for the tooltip. If assigned, a tooltip will spawn on hover.")]
        [SerializeField] private GameObject tooltipPrefab;
        [Tooltip("Where to spawn the tooltip. Usually a canvas or parent panel.")]
        [SerializeField] private Transform tooltipParent;

        private QuestStatus status;
        private GameObject activeTooltip;

        public void Setup(QuestStatus status)
        {
            this.status = status;
            if (title != null) title.text = status.GetQuest().GetTitle();
            if (progress != null) progress.text = status.GetCompletedCount() + "/" + status.GetQuest().GetObjectiveCount();
        }

        public QuestStatus GetQuestStatus()
        {
            return status;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (tooltipPrefab != null && activeTooltip == null)
            {
                Transform parent = tooltipParent != null ? tooltipParent : transform.root;
                activeTooltip = Instantiate(tooltipPrefab, parent);
                
                // Position it near the mouse
                activeTooltip.transform.position = Input.mousePosition;
                
                QuestTooltipUI tooltipUI = activeTooltip.GetComponent<QuestTooltipUI>();
                if (tooltipUI != null)
                {
                    tooltipUI.Setup(status);
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (activeTooltip != null)
            {
                Destroy(activeTooltip);
            }
        }
        
        private void OnDisable()
        {
            if (activeTooltip != null)
            {
                Destroy(activeTooltip);
            }
        }
    }
}
