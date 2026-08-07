# Caesura Quest & Dialogue System

Welcome to the standalone Caesura system! This package is completely decoupled from the GameDevTV architecture and relies on standard C# events and native Unity UI.

## Setup Instructions

### 1. Creating Assets
Right-click in your Project window and go to **Create -> Caesura**. You can create:
* **Item**: A basic ScriptableObject for inventory items.
* **Quest**: A ScriptableObject containing Objectives and Rewards.
* **Dialogue**: A node-based conversation tree. You can edit it by opening **Window -> Caesura -> Dialogue Editor**.

### 2. Player Setup
1. Attach a **PlayerConversant** script to your player character.
2. Attach a **QuestList** script to your player character.
3. Attach a **SimpleInventory** script to your player character.

### 3. NPC Setup (World Interaction)
1. Attach an **NPCConversant** to an NPC. Assign a Dialogue asset to it.
2. Whenever the player interacts with the NPC (using a Raycast, Trigger Volume, or Button press), call `NPCConversant.Interact(playerConversant)`.
3. To trigger events from dialogue (like giving a quest), attach a **DialogueTrigger** to the NPC. Type in the action string (e.g. "give_quest") that matches the dialogue node, and hook up the UnityEvent in the inspector to `QuestGiver.GiveQuestToGameObject`.

### 4. Setting up the UI
1. Create a standard Canvas in your scene.
2. Attach **DialogueUI** to a panel and link up the text elements and choice prefab.
3. Attach **QuestListUI** to a scroll view to automatically populate active quests using your custom **QuestItemUI** prefab.

## Condition Evaluators
Quests and Dialogues use conditions (e.g., `HasItem`, `CompletedQuest`). 
To add custom logic (like checking the player's level), simply create a script on your player that implements `IConditionEvaluator` and returns a boolean when `Evaluate()` is called!
