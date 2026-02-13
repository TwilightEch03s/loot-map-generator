using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class LegendUI : MonoBehaviour
{
    private PoiGenerator poiGenerator;
    private LootGenerator lootGenerator;
    private Vector2 scrollPosition = Vector2.zero;
    private const string SettingsFileName = "loot_generation.json";

    [Serializable]
    private class PoiSnapshot
    {
        public Vector2 position;
        public Vector2 size;
        public string difficulty;
    }

    [Serializable]
    private class LootSnapshot
    {
        public Vector2 position;
        public string rarity;
    }

    [Serializable]
    private class GenerationData
    {
        public string savedAtUtc;
        public List<PoiSnapshot> pois;
        public List<LootSnapshot> loot;
    }

    void Start()
    {
        poiGenerator = FindFirstObjectByType<PoiGenerator>();
        lootGenerator = FindFirstObjectByType<LootGenerator>();
    }

    void OnGUI()
    {
        int leftX = 10;
        int rightX = Screen.width - 390;
        int startY = 10;
        int boxWidth = 35;
        int boxHeight = 35;
        int spacing = 45;
        int textOffset = 45;
        
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.fontSize = 20;
        labelStyle.fontStyle = FontStyle.Bold;
        
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 16;
        titleStyle.fontStyle = FontStyle.Bold;
        
        GUIStyle smallLabelStyle = new GUIStyle(GUI.skin.label);
        smallLabelStyle.fontSize = 11;

        // Legend Panel
        int legendY = startY;
        GUI.Label(new Rect(leftX, legendY, 200, 30), "LOOT RARITY:", labelStyle);
        legendY += 30;
        
        DrawLegendItem(leftX, legendY, boxWidth, boxHeight, Color.white, "Common", labelStyle, textOffset);
        legendY += spacing;
        
        DrawLegendItem(leftX, legendY, boxWidth, boxHeight, Color.green, "Uncommon", labelStyle, textOffset);
        legendY += spacing;
        
        DrawLegendItem(leftX, legendY, boxWidth, boxHeight, Color.blue, "Rare", labelStyle, textOffset);
        legendY += spacing;
        
        DrawLegendItem(leftX, legendY, boxWidth, boxHeight, Color.magenta, "Epic", labelStyle, textOffset);
        legendY += spacing;
        
        DrawLegendItem(leftX, legendY, boxWidth, boxHeight, Color.yellow, "Legendary", labelStyle, textOffset);
        legendY += spacing + 15;
        
        GUI.Label(new Rect(leftX, legendY, 200, 30), "POI DIFFICULTY:", labelStyle);
        legendY += 30;
        
        DrawLegendItem(leftX, legendY, boxWidth, boxHeight, new Color(0f, 0.5f, 0f), "Easy", labelStyle, textOffset);
        legendY += spacing;
        
        DrawLegendItem(leftX, legendY, boxWidth, boxHeight, new Color(0.5f, 0.5f, 0f), "Medium", labelStyle, textOffset);
        legendY += spacing;
        
        DrawLegendItem(leftX, legendY, boxWidth, boxHeight, new Color(0.5f, 0f, 0f), "Hard", labelStyle, textOffset);

        // Control Panel
        GUI.Box(new Rect(rightX - 5, startY - 5, 395, Screen.height - 20), "");
        
        scrollPosition = GUI.BeginScrollView(new Rect(rightX, startY, 385, Screen.height - 30), scrollPosition, new Rect(rightX, startY, 365, Screen.height * 2));
        
        int controlY = startY;
        
        // Poi Controls Section
        if (poiGenerator != null)
        {
            GUI.Label(new Rect(rightX + 10, controlY, 350, 25), "POI SETTINGS", titleStyle);
            controlY += 30;
            
            GUI.Label(new Rect(rightX + 10, controlY, 300, 20), "POI Count:", smallLabelStyle);
            controlY += 20;
            poiGenerator.poiCount = (int)GUI.HorizontalSlider(new Rect(rightX + 10, controlY, 280, 20), poiGenerator.poiCount, 1, 20);
            GUI.Label(new Rect(rightX + 300, controlY - 2, 50, 20), poiGenerator.poiCount.ToString(), smallLabelStyle);
            controlY += 28;
            
            GUI.Label(new Rect(rightX + 10, controlY, 300, 20), "POI Size Min:", smallLabelStyle);
            controlY += 20;
            poiGenerator.poiSizeMin = GUI.HorizontalSlider(new Rect(rightX + 10, controlY, 280, 20), poiGenerator.poiSizeMin, 0.5f, 5f);
            GUI.Label(new Rect(rightX + 300, controlY - 2, 50, 20), poiGenerator.poiSizeMin.ToString("F2"), smallLabelStyle);
            controlY += 28;
            
            GUI.Label(new Rect(rightX + 10, controlY, 300, 20), "POI Size Max:", smallLabelStyle);
            controlY += 20;
            poiGenerator.poiSizeMax = GUI.HorizontalSlider(new Rect(rightX + 10, controlY, 280, 20), poiGenerator.poiSizeMax, 0.5f, 5f);
            GUI.Label(new Rect(rightX + 300, controlY - 2, 50, 20), poiGenerator.poiSizeMax.ToString("F2"), smallLabelStyle);
            controlY += 28;
            
            GUI.Label(new Rect(rightX + 10, controlY, 350, 25), "POI DIFFICULTY SETTINGS", titleStyle);
            controlY += 30;

            GUI.Label(new Rect(rightX + 10, controlY, 300, 20), "Easy POI Chance:", smallLabelStyle);
            controlY += 20;
            poiGenerator.easyPoiChance = GUI.HorizontalSlider(new Rect(rightX + 10, controlY, 280, 20), poiGenerator.easyPoiChance, 0f, 1f);
            GUI.Label(new Rect(rightX + 300, controlY - 2, 50, 20), (poiGenerator.easyPoiChance * 100).ToString("F0") + "%", smallLabelStyle);
            controlY += 28;
            
            GUI.Label(new Rect(rightX + 10, controlY, 300, 20), "Medium POI Chance:", smallLabelStyle);
            controlY += 20;
            poiGenerator.mediumPoiChance = GUI.HorizontalSlider(new Rect(rightX + 10, controlY, 280, 20), poiGenerator.mediumPoiChance, 0f, 1f);
            GUI.Label(new Rect(rightX + 300, controlY - 2, 50, 20), (poiGenerator.mediumPoiChance * 100).ToString("F0") + "%", smallLabelStyle);
            controlY += 28;
            
            GUI.Label(new Rect(rightX + 10, controlY, 300, 20), "Hard POI Chance:", smallLabelStyle);
            controlY += 20;
            poiGenerator.hardPoiChance = GUI.HorizontalSlider(new Rect(rightX + 10, controlY, 280, 20), poiGenerator.hardPoiChance, 0f, 1f);
            GUI.Label(new Rect(rightX + 300, controlY - 2, 50, 20), (poiGenerator.hardPoiChance * 100).ToString("F0") + "%", smallLabelStyle);
            controlY += 40;
        }
        
        // Loot Controls Section
        if (lootGenerator != null)
        {
            GUI.Label(new Rect(rightX + 10, controlY, 350, 25), "LOOT SETTINGS", titleStyle);
            controlY += 30;
            
            GUI.Label(new Rect(rightX + 10, controlY, 300, 20), "Loot Amount (per POI):", smallLabelStyle);
            controlY += 20;
            lootGenerator.lootPerPOI = (int)GUI.HorizontalSlider(new Rect(rightX + 10, controlY, 280, 20), lootGenerator.lootPerPOI, 1, 50);
            GUI.Label(new Rect(rightX + 300, controlY - 2, 50, 20), lootGenerator.lootPerPOI.ToString(), smallLabelStyle);
            controlY += 28;
            
            GUI.Label(new Rect(rightX + 10, controlY, 300, 20), "Random Loot Count:", smallLabelStyle);
            controlY += 20;
            lootGenerator.randomLootCount = (int)GUI.HorizontalSlider(new Rect(rightX + 10, controlY, 280, 20), lootGenerator.randomLootCount, 0, 500);
            GUI.Label(new Rect(rightX + 300, controlY - 2, 50, 20), lootGenerator.randomLootCount.ToString(), smallLabelStyle);
            controlY += 35;
            
            // Easy POI Rarity
            GUI.Label(new Rect(rightX + 10, controlY, 350, 20), "EASY POI RARITY", titleStyle);
            controlY += 25;
            
            GUI.Label(new Rect(rightX + 15, controlY, 280, 18), "Common: " + lootGenerator.easyCommonChance + "%", smallLabelStyle);
            controlY += 18;
            lootGenerator.easyCommonChance = (int)GUI.HorizontalSlider(new Rect(rightX + 15, controlY, 270, 18), lootGenerator.easyCommonChance, 0, 100);
            controlY += 23;
            
            GUI.Label(new Rect(rightX + 15, controlY, 280, 18), "Uncommon: " + lootGenerator.easyUncommonChance + "%", smallLabelStyle);
            controlY += 18;
            lootGenerator.easyUncommonChance = (int)GUI.HorizontalSlider(new Rect(rightX + 15, controlY, 270, 18), lootGenerator.easyUncommonChance, 0, 100);
            controlY += 23;
            
            GUI.Label(new Rect(rightX + 15, controlY, 280, 18), "Rare: " + lootGenerator.easyRareChance + "%", smallLabelStyle);
            controlY += 18;
            lootGenerator.easyRareChance = (int)GUI.HorizontalSlider(new Rect(rightX + 15, controlY, 270, 18), lootGenerator.easyRareChance, 0, 100);
            controlY += 23;
            
            GUI.Label(new Rect(rightX + 15, controlY, 280, 18), "Epic: " + lootGenerator.easyEpicChance + "%", smallLabelStyle);
            controlY += 18;
            lootGenerator.easyEpicChance = (int)GUI.HorizontalSlider(new Rect(rightX + 15, controlY, 270, 18), lootGenerator.easyEpicChance, 0, 100);
            controlY += 23;
            
            GUI.Label(new Rect(rightX + 15, controlY, 280, 18), "Legendary: " + lootGenerator.easyLegendaryChance + "%", smallLabelStyle);
            controlY += 18;
            lootGenerator.easyLegendaryChance = (int)GUI.HorizontalSlider(new Rect(rightX + 15, controlY, 270, 18), lootGenerator.easyLegendaryChance, 0, 100);
            controlY += 30;
            
            // Medium POI Rarity
            GUI.Label(new Rect(rightX + 10, controlY, 350, 20), "MEDIUM POI RARITY", titleStyle);
            controlY += 25;
            
            GUI.Label(new Rect(rightX + 15, controlY, 280, 18), "Common: " + lootGenerator.mediumCommonChance + "%", smallLabelStyle);
            controlY += 18;
            lootGenerator.mediumCommonChance = (int)GUI.HorizontalSlider(new Rect(rightX + 15, controlY, 270, 18), lootGenerator.mediumCommonChance, 0, 100);
            controlY += 23;
            
            GUI.Label(new Rect(rightX + 15, controlY, 280, 18), "Uncommon: " + lootGenerator.mediumUncommonChance + "%", smallLabelStyle);
            controlY += 18;
            lootGenerator.mediumUncommonChance = (int)GUI.HorizontalSlider(new Rect(rightX + 15, controlY, 270, 18), lootGenerator.mediumUncommonChance, 0, 100);
            controlY += 23;
            
            GUI.Label(new Rect(rightX + 15, controlY, 280, 18), "Rare: " + lootGenerator.mediumRareChance + "%", smallLabelStyle);
            controlY += 18;
            lootGenerator.mediumRareChance = (int)GUI.HorizontalSlider(new Rect(rightX + 15, controlY, 270, 18), lootGenerator.mediumRareChance, 0, 100);
            controlY += 23;
            
            GUI.Label(new Rect(rightX + 15, controlY, 280, 18), "Epic: " + lootGenerator.mediumEpicChance + "%", smallLabelStyle);
            controlY += 18;
            lootGenerator.mediumEpicChance = (int)GUI.HorizontalSlider(new Rect(rightX + 15, controlY, 270, 18), lootGenerator.mediumEpicChance, 0, 100);
            controlY += 23;
            
            GUI.Label(new Rect(rightX + 15, controlY, 280, 18), "Legendary: " + lootGenerator.mediumLegendaryChance + "%", smallLabelStyle);
            controlY += 18;
            lootGenerator.mediumLegendaryChance = (int)GUI.HorizontalSlider(new Rect(rightX + 15, controlY, 270, 18), lootGenerator.mediumLegendaryChance, 0, 100);
            controlY += 30;
            
            // Hard POI Rarity
            GUI.Label(new Rect(rightX + 10, controlY, 350, 20), "HARD POI RARITY", titleStyle);
            controlY += 25;
            
            GUI.Label(new Rect(rightX + 15, controlY, 280, 18), "Common: " + lootGenerator.hardCommonChance + "%", smallLabelStyle);
            controlY += 18;
            lootGenerator.hardCommonChance = (int)GUI.HorizontalSlider(new Rect(rightX + 15, controlY, 270, 18), lootGenerator.hardCommonChance, 0, 100);
            controlY += 23;
            
            GUI.Label(new Rect(rightX + 15, controlY, 280, 18), "Uncommon: " + lootGenerator.hardUncommonChance + "%", smallLabelStyle);
            controlY += 18;
            lootGenerator.hardUncommonChance = (int)GUI.HorizontalSlider(new Rect(rightX + 15, controlY, 270, 18), lootGenerator.hardUncommonChance, 0, 100);
            controlY += 23;
            
            GUI.Label(new Rect(rightX + 15, controlY, 280, 18), "Rare: " + lootGenerator.hardRareChance + "%", smallLabelStyle);
            controlY += 18;
            lootGenerator.hardRareChance = (int)GUI.HorizontalSlider(new Rect(rightX + 15, controlY, 270, 18), lootGenerator.hardRareChance, 0, 100);
            controlY += 23;
            
            GUI.Label(new Rect(rightX + 15, controlY, 280, 18), "Epic: " + lootGenerator.hardEpicChance + "%", smallLabelStyle);
            controlY += 18;
            lootGenerator.hardEpicChance = (int)GUI.HorizontalSlider(new Rect(rightX + 15, controlY, 270, 18), lootGenerator.hardEpicChance, 0, 100);
            controlY += 23;
            
            GUI.Label(new Rect(rightX + 15, controlY, 280, 18), "Legendary: " + lootGenerator.hardLegendaryChance + "%", smallLabelStyle);
            controlY += 18;
            lootGenerator.hardLegendaryChance = (int)GUI.HorizontalSlider(new Rect(rightX + 15, controlY, 270, 18), lootGenerator.hardLegendaryChance, 0, 100);
            controlY += 30;
            
            // Random Loot Rarity
            GUI.Label(new Rect(rightX + 10, controlY, 350, 20), "RANDOM LOOT RARITY", titleStyle);
            controlY += 25;
            
            GUI.Label(new Rect(rightX + 15, controlY, 280, 18), "Common: " + lootGenerator.randomCommonChance + "%", smallLabelStyle);
            controlY += 18;
            lootGenerator.randomCommonChance = (int)GUI.HorizontalSlider(new Rect(rightX + 15, controlY, 270, 18), lootGenerator.randomCommonChance, 0, 100);
            controlY += 23;
            
            GUI.Label(new Rect(rightX + 15, controlY, 280, 18), "Uncommon: " + lootGenerator.randomUncommonChance + "%", smallLabelStyle);
            controlY += 18;
            lootGenerator.randomUncommonChance = (int)GUI.HorizontalSlider(new Rect(rightX + 15, controlY, 270, 18), lootGenerator.randomUncommonChance, 0, 100);
            controlY += 23;
            
            GUI.Label(new Rect(rightX + 15, controlY, 280, 18), "Rare: " + lootGenerator.randomRareChance + "%", smallLabelStyle);
            controlY += 18;
            lootGenerator.randomRareChance = (int)GUI.HorizontalSlider(new Rect(rightX + 15, controlY, 270, 18), lootGenerator.randomRareChance, 0, 100);
            controlY += 23;
            
            GUI.Label(new Rect(rightX + 15, controlY, 280, 18), "Epic: " + lootGenerator.randomEpicChance + "%", smallLabelStyle);
            controlY += 18;
            lootGenerator.randomEpicChance = (int)GUI.HorizontalSlider(new Rect(rightX + 15, controlY, 270, 18), lootGenerator.randomEpicChance, 0, 100);
            controlY += 23;
            
            GUI.Label(new Rect(rightX + 15, controlY, 280, 18), "Legendary: " + lootGenerator.randomLegendaryChance + "%", smallLabelStyle);
            controlY += 18;
            lootGenerator.randomLegendaryChance = (int)GUI.HorizontalSlider(new Rect(rightX + 15, controlY, 270, 18), lootGenerator.randomLegendaryChance, 0, 100);
            controlY += 40;
        }
        
        GUI.EndScrollView();
        
        GUIStyle actionButtonStyle = new GUIStyle(GUI.skin.button) { fontSize = 16, fontStyle = FontStyle.Bold };
        int buttonWidth = 370;
        int buttonHeight = 40;
        int regenerateY = Screen.height - 45;
        int saveY = regenerateY - 45;

        if (GUI.Button(new Rect(rightX, saveY, buttonWidth, buttonHeight), "SAVE GENERATION", actionButtonStyle))
        {
            SaveGenerationToJson();
        }

        // Regenerate button at bottom
        if (GUI.Button(new Rect(rightX, regenerateY, buttonWidth, buttonHeight), "REGENERATE MAP", actionButtonStyle))
        {
            if (poiGenerator != null)
            {
                poiGenerator.Regenerate();
            }
            if (lootGenerator != null)
            {
                lootGenerator.Regenerate();
            }
        }
    }
    
    void DrawLegendItem(int x, int y, int width, int height, Color color, string label, GUIStyle style, int textOffset)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        GUI.DrawTexture(new Rect(x, y, width, height), texture);
        GUI.Box(new Rect(x, y, width, height), "");
        GUI.Label(new Rect(x + textOffset, y, 150, height), label, style);
    }

    private void SaveGenerationToJson()
    {
        if (poiGenerator == null && lootGenerator == null)
        {
            Debug.LogWarning("No generators found to save generation.");
            return;
        }

        List<PoiSnapshot> poiSnapshots = null;
        if (poiGenerator != null)
        {
            poiSnapshots = new List<PoiSnapshot>();
            foreach (GameObject poi in poiGenerator.GetGeneratedPOIs())
            {
                if (poi == null)
                {
                    continue;
                }

                PoiGenerator.POI poiScript = poi.GetComponent<PoiGenerator.POI>();
                if (poiScript == null)
                {
                    continue;
                }

                Vector3 pos = poi.transform.position;
                Vector3 scale = poi.transform.localScale;

                poiSnapshots.Add(new PoiSnapshot
                {
                    position = new Vector2(pos.x, pos.y),
                    size = new Vector2(scale.x, scale.y),
                    difficulty = poiScript.difficulty.ToString()
                });
            }
        }

        List<LootSnapshot> lootSnapshots = null;
        if (lootGenerator != null)
        {
            lootSnapshots = new List<LootSnapshot>();
            foreach (GameObject loot in lootGenerator.GetGeneratedLoot())
            {
                if (loot == null)
                {
                    continue;
                }

                LootGenerator.Loot lootScript = loot.GetComponent<LootGenerator.Loot>();
                if (lootScript == null)
                {
                    continue;
                }

                Vector3 pos = loot.transform.position;
                lootSnapshots.Add(new LootSnapshot
                {
                    position = new Vector2(pos.x, pos.y),
                    rarity = lootScript.rarity.ToString()
                });
            }
        }

        GenerationData data = new GenerationData
        {
            savedAtUtc = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            pois = poiSnapshots,
            loot = lootSnapshots
        };

        string filePath = GetSettingsFilePath();
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Saved generation to: " + filePath);
    }

    private string GetSettingsFilePath()
    {
        return Path.Combine(Application.dataPath, SettingsFileName);
    }
}
