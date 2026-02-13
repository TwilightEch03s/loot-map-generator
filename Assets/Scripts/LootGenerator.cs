using UnityEngine;
using System.Collections.Generic;

public class LootGenerator : MonoBehaviour
{
    public GameObject lootPrefab;
    
    // Adjustable parameters
    [Header("Loot Generation Settings")]
    public int lootPerPOI = 10;
    public int randomLootCount = 150;
    public Vector2 islandSize = new Vector2(10, 10);

    // Easy POI Rarity Chances
    [Header("Easy POI Rarity Chances")]
    [Range(0,100)] public int easyCommonChance = 60;
    [Range(0,100)] public int easyUncommonChance = 35;
    [Range(0,100)] public int easyRareChance = 5;
    [Range(0,100)] public int easyEpicChance = 0;
    [Range(0,100)] public int easyLegendaryChance = 0;

    // Medium POI Rarity Chances
    [Header("Medium POI Rarity Chances")]
    [Range(0,100)] public int mediumCommonChance = 0;
    [Range(0,100)] public int mediumUncommonChance = 60;
    [Range(0,100)] public int mediumRareChance = 35;
    [Range(0,100)] public int mediumEpicChance = 5;
    [Range(0,100)] public int mediumLegendaryChance = 0;

    // Hard POI Rarity Chances
    [Header("Hard POI Rarity Chances")]
    [Range(0,100)] public int hardCommonChance = 0;
    [Range(0,100)] public int hardUncommonChance = 0;
    [Range(0,100)] public int hardRareChance = 60;
    [Range(0,100)] public int hardEpicChance = 35;
    [Range(0,100)] public int hardLegendaryChance = 5;

    // Rarity chances for random loot outside POIs
    [Header("Random Loot Rarity Chances")]
    [Range(0,100)] public int randomCommonChance = 50;
    [Range(0,100)] public int randomUncommonChance = 30;
    [Range(0,100)] public int randomRareChance = 15;
    [Range(0,100)] public int randomEpicChance = 4;
    [Range(0,100)] public int randomLegendaryChance = 1;

    public enum LootRarity { Common, Uncommon, Rare, Epic, Legendary }

    public class Loot : MonoBehaviour
    {
        public LootRarity rarity;
    }

    private List<GameObject> generatedLoot = new List<GameObject>();

    void Start()
    {
        GenerateLoot();
    }

    public void Regenerate()
    {
        // Clear existing loot
        foreach (GameObject loot in generatedLoot)
        {
            Destroy(loot);
        }
        generatedLoot.Clear();
        
        // Regenerate
        GenerateLoot();
    }

    public List<GameObject> GetGeneratedLoot()
    {
        return generatedLoot;
    }

    // Generate loot items within POIs based on their difficulty
    void GenerateLoot()
    {
        // Get POIs from PoiGenerator to ensure we're using fresh references
        PoiGenerator poiGen = FindFirstObjectByType<PoiGenerator>();
        if (poiGen == null) 
        {
            return;
        }

        List<GameObject> poiGameObjects = poiGen.GetGeneratedPOIs();
        List<Rect> poiBounds = new List<Rect>();

        // Generate loot within POIs
        foreach (GameObject poiGO in poiGameObjects)
        {
            PoiGenerator.POI poi = poiGO.GetComponent<PoiGenerator.POI>();
            if (poi == null) 
            {
                continue;
            }

            // Get POI bounds
            Transform poiTransform = poi.transform;
            Vector3 poiPos = poiTransform.position;
            Vector3 poiScale = poiTransform.localScale;
            
            // Store POI bounds for later collision check
            Rect poiRect = new Rect(
                poiPos.x - poiScale.x / 2f,
                poiPos.y - poiScale.y / 2f,
                poiScale.x,
                poiScale.y
            );
            poiBounds.Add(poiRect);
            
            // Generate loot within this POI
            for (int i = 0; i < lootPerPOI; i++)
            {
                Vector2 pos = new Vector2(
                    poiPos.x + Random.Range(-poiScale.x / 2f, poiScale.x / 2f),
                    poiPos.y + Random.Range(-poiScale.y / 2f, poiScale.y / 2f)
                );
                GameObject loot = Instantiate(lootPrefab, pos, Quaternion.identity);
                SpriteRenderer sr = loot.GetComponent<SpriteRenderer>();
                LootRarity rarity = GetRarityForDifficulty(poi.difficulty);
                Loot lootScript = loot.GetComponent<Loot>();
                if (lootScript == null)
                {
                    lootScript = loot.AddComponent<Loot>();
                }
                lootScript.rarity = rarity;
                sr.color = GetColorForRarity(rarity);
                generatedLoot.Add(loot);
            }
        }

        // Generate random loot outside POIs
        for (int i = 0; i < randomLootCount; i++)
        {
            bool validPosition = false;
            Vector2 pos = Vector2.zero;
            int attempts = 0;
            
            // Try to find a position outside all POIs
            while (!validPosition && attempts < 50)
            {
                attempts++;
                pos = new Vector2(
                    Random.Range(-islandSize.x / 2f, islandSize.x / 2f),
                    Random.Range(-islandSize.y / 2f, islandSize.y / 2f)
                );
                
                // Check if position is inside any POI
                bool insidePOI = false;
                foreach (Rect rect in poiBounds)
                {
                    if (rect.Contains(pos))
                    {
                        insidePOI = true;
                        break;
                    }
                }
                
                if (!insidePOI)
                {
                    validPosition = true;
                }
            }
            
            if (validPosition || attempts >= 50)
            {
                GameObject loot = Instantiate(lootPrefab, pos, Quaternion.identity);
                SpriteRenderer sr = loot.GetComponent<SpriteRenderer>();
                LootRarity rarity = GetRandomRarity();
                Loot lootScript = loot.GetComponent<Loot>();
                if (lootScript == null)
                {
                    lootScript = loot.AddComponent<Loot>();
                }
                lootScript.rarity = rarity;
                sr.color = GetColorForRarity(rarity);
                generatedLoot.Add(loot);
            }
        }
    }

    // Determine loot rarity based on POI difficulty
    LootRarity GetRarityForDifficulty(PoiGenerator.POI.Difficulty difficulty)
    {
        if (difficulty == PoiGenerator.POI.Difficulty.Easy) {
            float easyTotal = easyCommonChance + easyUncommonChance + easyRareChance + easyEpicChance + easyLegendaryChance;
            if (easyTotal <= 0) {
                return LootRarity.Common;
            }
            float easyRoll = Random.Range(0f, easyTotal);
            if (easyRoll < easyCommonChance) {
                return LootRarity.Common;
            }
            easyRoll -= easyCommonChance;
            if (easyRoll < easyUncommonChance) {
                return LootRarity.Uncommon;
            }
            easyRoll -= easyUncommonChance;
            if (easyRoll < easyRareChance) {
                return LootRarity.Rare;
            }
            easyRoll -= easyRareChance;
            if (easyRoll < easyEpicChance) {
                return LootRarity.Epic;
            }
            return LootRarity.Legendary;
        }
        if (difficulty == PoiGenerator.POI.Difficulty.Medium) {
            float mediumTotal = mediumCommonChance + mediumUncommonChance + mediumRareChance + mediumEpicChance + mediumLegendaryChance;
            if (mediumTotal <= 0) {
                return LootRarity.Uncommon; // Fallback
            }
            float mediumRoll = Random.Range(0f, mediumTotal);
            if (mediumRoll < mediumCommonChance) {
                return LootRarity.Common;
            }
            mediumRoll -= mediumCommonChance;
            if (mediumRoll < mediumUncommonChance) {
                return LootRarity.Uncommon;
            }
            mediumRoll -= mediumUncommonChance;
            if (mediumRoll < mediumRareChance) {
                return LootRarity.Rare;
            }
            mediumRoll -= mediumRareChance;
            if (mediumRoll < mediumEpicChance) {
                return LootRarity.Epic;
            }
            return LootRarity.Legendary;
        }

        if (difficulty == PoiGenerator.POI.Difficulty.Hard) {
            float hardTotal = hardCommonChance + hardUncommonChance + hardRareChance + hardEpicChance + hardLegendaryChance;
            if (hardTotal <= 0) {
                return LootRarity.Rare; // Fallback
            }
            float hardRoll = Random.Range(0f, hardTotal);
            if (hardRoll < hardCommonChance) {
                return LootRarity.Common;
            }
            hardRoll -= hardCommonChance;
            if (hardRoll < hardUncommonChance) {
                return LootRarity.Uncommon;
            }
            hardRoll -= hardUncommonChance;
            if (hardRoll < hardRareChance) {
                return LootRarity.Rare;
            }
            hardRoll -= hardRareChance;
            if (hardRoll < hardEpicChance) {
                return LootRarity.Epic;
            }
            return LootRarity.Legendary;
        }
        
        return LootRarity.Common;
    }

    // Determine rarity for random loot outside POIs
    LootRarity GetRandomRarity()
    {
        float total = randomCommonChance + randomUncommonChance + randomRareChance + randomEpicChance + randomLegendaryChance;
        float roll = Random.Range(0f, total);

        if (roll < randomCommonChance) {
            return LootRarity.Common;
        }
        roll -= randomCommonChance;

        if (roll < randomUncommonChance) {
            return LootRarity.Uncommon;
        }
        roll -= randomUncommonChance;

        if (roll < randomRareChance) {
            return LootRarity.Rare;
        }
        roll -= randomRareChance;

        if (roll < randomEpicChance) {
            return LootRarity.Epic;
        }
        roll -= randomEpicChance;
        return LootRarity.Legendary;
    }

    // Get color based on rarity
    Color GetColorForRarity(LootRarity rarity)
    {
        if (rarity == LootRarity.Common) {
            return Color.white;
        }
        if (rarity == LootRarity.Uncommon) {
            return Color.green;
        }
        if (rarity == LootRarity.Rare) {
            return Color.blue;
        }
        if (rarity == LootRarity.Epic) {
            return Color.magenta;
        }
        if (rarity == LootRarity.Legendary) {
            return Color.yellow;
        }
        return Color.white;
    }
}
