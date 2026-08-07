# Elias (The Blacksmith)

**The Wound:** He feels like a sellout. He abandoned his family's craft for easy factory money, and now he has neither.
**The Arc:** Reclaiming his pride and realizing his artisanal skills are still needed in a modern world.

## Dialogue Flow

```mermaid
graph TD
    Phase1[Phase 1: Defeatist] --> Phase2[Phase 2: Spark of Hope]
    Phase2 --> Phase3[Phase 3: Pride Restored]

    Q1>"""Looking for a tool? Check the old factory scrap heap. That's where I threw my anvil ten years ago. Figured if a machine could stamp out a hundred horseshoes an hour, nobody needed me sweating over a forge. Guess we were all wrong, huh?"""]
    Phase1 -.- Q1

    Q2>"""You got Rowan to fire the kiln for this? And you dug this ore yourself? ... Put it on the bench. I'm out of practice, kid. I might ruin it. But... I suppose the hammer still fits my hand. Give me a day."""]
    Phase2 -.- Q2

    Q3>"""Silas came by earlier. Said the nails I forged held the bridge beams perfectly. You know, a factory nail is exactly the same as the next one. But a forged nail... it bites into the wood. It holds on. We're holding on, kid."""]
    Phase3 -.- Q3
```
