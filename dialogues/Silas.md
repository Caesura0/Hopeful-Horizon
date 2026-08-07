# Silas (The Carpenter)

**The Wound:** He is a master builder in a town that stopped growing. He feels useless because he has the skills to fix everything, but no materials and no one asking for his help. He spends his days whittling small trinkets instead of building homes.
**The Arc:** Moving from feeling obsolete to becoming the literal architect of the town's rebirth. He learns to ask for help instead of trying to do it all himself.

## Dialogue Flow

```mermaid
graph TD
    Phase1[Phase 1: Resigned] --> Phase2[Phase 2: Energized]
    Phase2 --> Phase3[Phase 3: Fulfilled]

    Q1>"""Careful on that porch step, it's rotted through. I'd fix it, but what's the point? Nobody walks down this street anymore anyway. I just sit here and whittle these little wooden birds. At least they don't complain about the roof leaking."""]
    Phase1 -.- Q1

    Q2>"""You actually got Elias to forge these nails? And you dragged that lumber all the way from the mill? Well... I suppose I can't let good materials go to waste. Grab the other end of this beam. Let's see if this old back still has a bridge left in it."""]
    Phase2 -.- Q2

    Q3>"""Did you see the new gazebo in the square? Todd helped me raise the main pillars. Boy's got the strength of an ox when he's not busy staring in a mirror. It's good to hear hammers ringing in this town again. Real good."""]
    Phase3 -.- Q3
```
