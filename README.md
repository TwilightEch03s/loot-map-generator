# LootMapGenerator

## Overview

**LootMapGenerator** is a Unity tool that procedurally generates **Points of Interest (POIs)** and **Loot Locations** across a map.

---

## Tutorial

1. Open the project in Unity.
2. Press the **Play** button.
3. Adjust generation parameters:
   - In the in game **settings panel**, or  
   - By selecting either **LootManager** or **PoiGenerator** in the **Hierarchy** and modifying their settings.

Parameter changes will immediately affect how POIs and loot are generated across the map.

---

## Parameter Explanations

### POI Parameters

- <u>**Poi Count**</u> – Number of POIs on the map
- <u>**Poi Size Min**</u> – Minimum size of the POI
- <u>**Poi Size Max**</u> – Maximum size of the POI

- <u>**Easy Poi Chance**</u> - Chance of an easy POI spawning (Ranges from 0 -1 )
- <u>**Medium Poi Chance**</u> - Chance of a medium POI spawning (Ranges from 0 - 1)
- <u>**Hard Poi Chance**</u> - Chance of a hard POI spawning (Ranges from 0 - 1)
- <u>**Island Size**</u> - Dimensions of the island (x, y)

### Loot Parameters

- <u>**Loot Per POI**</u> - Amount of loot generated in each POI
- <u>**Random Loot Count**</u> - Amount of loot generated outside the POI
- <u>**Island Size**</u> - Dimensions of the island (x, y)

#### Easy POI Rarity Chances

- <u>**Easy Common Chance**</u> - Percentage of common loot spawning in easy POI (Ranges from %0 - %100)
- <u>**Easy Uncommon Chance**</u> - Percentage of uncommon loot spawning in easy POI (Ranges from %0 -% 100)
- <u>**Easy Rare Chance**</u> - Percentage of rare loot spawning in easy POI (Ranges from %0 - %100)
- <u>**Easy Epic Chance**</u> - Percentage of epic loot spawning in easy POI (Ranges from %0 - %100)
- <u>**Easy Legendary Chance**</u> - Percentage of legendary loot spawning in easy POI (Ranges from %0 - %100)

#### Medium POI Rarity Chances

- <u>**Medium Common Chance**</u> - Percentage of common loot spawning in medium POI (Ranges from %0 - %100)
- <u>**Medium Uncommon Chance**</u> - Percentage of uncommon loot spawning in medium POI (Ranges from %0 -% 100)
- <u>**Medium Rare Chance**</u> - Percentage of rare loot spawning in medium POI (Ranges from %0 - %100)
- <u>**Medium Epic Chance**</u> - Percentage of epic loot spawning in medium POI (Ranges from %0 - %100)
- <u>**Medium Legendary Chance**</u> - Percentage of legendary loot spawning in medium POI (Ranges from %0 - %100)

#### Hard POI Rarity Chances

- <u>**Hard Common Chance**</u> - Percentage of common loot spawning in hard POI (Ranges from %0 - %100)
- <u>**Hard Uncommon Chance**</u> - Percentage of uncommon loot spawning in hard POI (Ranges from %0 -% 100)
- <u>**Hard Rare Chance**</u> - Percentage of rare loot spawning in hard POI (Ranges from %0 - %100)
- <u>**Hard Epic Chance**</u> - Percentage of epic loot spawning in hard POI (Ranges from %0 - %100)
- <u>**Hard Legendary Chance**</u> - Percentage of legendary loot spawning in hard POI (Ranges from %0 - %100)

#### Random Loot Rarity Chances

- <u>**Random Common Chance**</u> - Percentage of common loot spawning outside of a POI (Ranges from %0 - %100)
- <u>**Random Uncommon Chance**</u> - Percentage of uncommon loot spawning outside of a POI (Ranges from %0 -% 100)
- <u>**Random Rare Chance**</u> - Percentage of rare loot spawning outside of a POI (Ranges from %0 - %100)
- <u>**Random Epic Chance**</u> - Percentage of epic loot spawning outside of a POI (Ranges from %0 - %100)
- <u>**Random Legendary Chance**</u> - Percentage of legendary loot spawning outside of a POI (Ranges from %0 - %100)

---

## Limitations

- The system currently supports five loot rarities:
  - **Common**
  - **Uncommon**
  - **Rare**
  - **Epic**
  - **Legendary**
- Rarity tiers are fixed and require code modification to expand/adjust.
- Loot/POI Generation only generates in a square area
- POI shapes only support random sizes of a rectangle and would need code modification to have specific POIs to be fixed. (Note: All POIs can be fixed to a specific size by making **Poi Size Min** and **Poi Size Max** values the same number)
