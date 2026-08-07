# Old Ben (The Stubborn Gatekeeper)

**The Wound:** He feels abandoned by the younger generation who all went to work at the factory. He uses gruffness as a shield against loneliness.
**The Arc:** Admitting he is lonely and accepting that the new generation cares.

## Dialogue Flow

```mermaid
graph TD
    Phase1[Phase 1: Hostile] --> Phase2[Phase 2: Softening]
    Phase2 --> Phase3[Phase 3: Acceptance]

    Q1>"""What do you want? Sightseeing? Town's closed. Go take pictures of the rusted gears at the factory. Oh, you want to help? People only help when they want something. What is it? The pickaxe? Figures. Fine. Milk the cows. I don't give handouts."""]
    Phase1 -.- Q1

    Q2>"""You brought it back in one piece. And you wiped the mud off the handle. ...Hmph. Most folks these days would've just left it in the rain. I suppose you're not completely useless. If you're heading past the bakery, tell Clementine I... well, just tell her I said hello."""]
    Phase2 -.- Q2

    Q3>"""I complained for five years that this town was dead. But I never lifted a finger to revive it. I just sat on my porch and watched it rot. I was waiting for someone like you to come along and prove me wrong. Here. Take this cheese to the festival table. And don't you dare drop it."""]
    Phase3 -.- Q3
```
