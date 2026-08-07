using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Caesura.Dialogue;

namespace Caesura.UI
{
    public class DialogueUI : MonoBehaviour
    {
        [Tooltip("Optional: Drag the player's PlayerConversant here. If null, it tries to find a 'Player' tagged object.")]
        [SerializeField] private PlayerConversant playerConversant;
        
        [Header("Dialogue Elements")]
        [SerializeField] private TextMeshProUGUI aiText;
        [SerializeField] private TextMeshProUGUI conversantName;
        [SerializeField] private GameObject aiResponsePanel;
        
        [Tooltip("Optional: Drag a UI Image component here to show the speaker portrait.")]
        [SerializeField] private Image portraitImage;
        
        [Tooltip("Optional: Drag an AudioSource here to play voice overs.")]
        [SerializeField] private AudioSource audioSource;
        
        [Header("Next/Quit Controls")]
        [SerializeField] private GameObject nextGUI;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button quitButton;
        
        [Header("Choice Elements")]
        [SerializeField] private Transform choiceRoot;
        [SerializeField] private GameObject choicePrefab;

        private void Start()
        {
            if (playerConversant == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    playerConversant = player.GetComponent<PlayerConversant>();
                }
            }

            if (playerConversant != null)
            {
                playerConversant.onConversationUpdated += UpdateUI;
                nextButton.onClick.AddListener(() => playerConversant.Next());
                quitButton.onClick.AddListener(() => playerConversant.Quit());
                UpdateUI();
            }
            else
            {
                Debug.LogError("DialogueUI could not find a PlayerConversant.");
                gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            if (playerConversant != null)
            {
                playerConversant.onConversationUpdated -= UpdateUI;
            }
        }

        private void UpdateUI()
        {
            gameObject.SetActive(playerConversant.IsActive());
            if (!playerConversant.IsActive())
            {
                return;
            }

            conversantName.text = playerConversant.GetCurrentConversantName();
            
            // Do not disable aiResponsePanel, because choiceRoot is a child of it!
            // aiResponsePanel.SetActive(!playerConversant.IsChoosing());
            
            nextGUI.SetActive(!playerConversant.IsChoosing());
            choiceRoot.gameObject.SetActive(playerConversant.IsChoosing());

            aiText.text = playerConversant.GetText();
            aiText.gameObject.SetActive(!playerConversant.IsChoosing());
            
            // Handle Portrait
            if (portraitImage != null)
            {
                Sprite portrait = playerConversant.GetSpeakerPortrait();
                if (portrait != null)
                {
                    portraitImage.sprite = portrait;
                    portraitImage.gameObject.SetActive(true);
                }
                else
                {
                    portraitImage.gameObject.SetActive(false);
                }
            }

            // Handle Audio
            if (audioSource != null)
            {
                audioSource.Stop();
                AudioClip clip = playerConversant.GetVoiceOver();
                if (clip != null)
                {
                    audioSource.clip = clip;
                    audioSource.Play();
                }
            }

            RectTransform textRect = aiText.GetComponent<RectTransform>();

            if (playerConversant.IsChoosing())
            {
                // textRect.anchorMax = new Vector2(0.55f, 0.75f); // Stop before the choices panel (which starts at 0.6)
                BuildChoiceList();
                nextButton.gameObject.SetActive(false);
                if (quitButton != null) quitButton.gameObject.SetActive(false);
            }
            else
            {
                // textRect.anchorMax = new Vector2(0.85f, 0.75f); // Stretch further when there are no choices
                
                bool hasNext = playerConversant.HasNext();
                nextButton.gameObject.SetActive(hasNext);
                
                if (quitButton != null) 
                {
                    quitButton.gameObject.SetActive(!hasNext);
                }
            }
        }

        private void BuildChoiceList()
        {
            // Clear existing choices
            foreach (Transform item in choiceRoot)
            {
                Destroy(item.gameObject);
            }

            foreach (DialogueNode choice in playerConversant.GetChoices())
            {
                GameObject choiceInstance = Instantiate(choicePrefab, choiceRoot);
                var textComp = choiceInstance.GetComponentInChildren<TextMeshProUGUI>();
                if (textComp != null)
                {
                    textComp.text = choice.GetNodeText();
                }
                
                var button = choiceInstance.GetComponentInChildren<Button>();
                if (button != null)
                {
                    button.onClick.AddListener(() => 
                    {
                        playerConversant.SelectChoice(choice);
                    });
                }
            }
        }
    }
}
