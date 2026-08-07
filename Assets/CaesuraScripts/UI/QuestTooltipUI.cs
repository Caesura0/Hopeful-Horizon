using UnityEngine;
using TMPro;
using Caesura.Quests;
using System.Text;

namespace Caesura.UI
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Transform objectiveContainer;
        [SerializeField] private GameObject objectivePrefab;
        [SerializeField] private GameObject objectiveIncompletePrefab;
        [SerializeField] private TextMeshProUGUI rewardText;

        public void Setup(QuestStatus status)
        {
            Quest quest = status.GetQuest();
            title.text = quest.GetTitle();
            
            // Clear existing objectives
            foreach (Transform item in objectiveContainer)
            {
                Destroy(item.gameObject);
            }

            foreach (var objective in quest.GetObjectives())
            {
                GameObject prefab = status.IsObjectiveComplete(objective.reference) ? objectivePrefab : objectiveIncompletePrefab;

                GameObject objectiveInstance = Instantiate(prefab, objectiveContainer);
                TextMeshProUGUI objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
                if (objectiveText != null)
                {
                    objectiveText.text = objective.description;
                }
            }
            
            rewardText.text = GetRewardText(quest);
        }

        private string GetRewardText(Quest quest)
        {
            StringBuilder sb = new StringBuilder();
            
            foreach (var reward in quest.GetRewards())
            {
                if (sb.Length > 0)
                {
                    sb.Append(", ");
                }
                if (reward.number > 1)
                {
                    sb.Append(reward.number).Append(" ");
                }
                sb.Append(reward.item.GetDisplayName());
            }
            
            if (sb.Length == 0)
            {
                return "No Reward.";
            }
            
            sb.Append(".");
            return sb.ToString();
        }
    }
}
