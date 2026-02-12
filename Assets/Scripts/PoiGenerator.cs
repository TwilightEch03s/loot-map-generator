using UnityEngine;
using System.Collections.Generic;

public class PoiGenerator : MonoBehaviour
{
    public GameObject poiPrefab;

    // Adjustable parameters
    [Header("POI Generation Settings")]
    public int poiCount = 7;
    public float poiSizeMin = 1f;
    public float poiSizeMax = 3f;
    [Header("POI Difficulty Chances")]
    [Range(0,1f)] public float easyPoiChance = 0.5f;
    [Range(0,1f)] public float mediumPoiChance = 0.40f;
    [Range(0,1f)] public float hardPoiChance = 0.35f;

    [Header("Island Settings")]
    public Vector2 islandSize = new Vector2(10, 10);
    private List<Rect> placedPOIs = new List<Rect>();
    private List<GameObject> generatedPOIs = new List<GameObject>();

    public class POI : MonoBehaviour
    {
        public enum Difficulty { Easy, Medium, Hard }
        public Difficulty difficulty;
    }

    void Start()
    {
        GeneratePoi();
    }

    public void Regenerate()
    {
        // Clear existing POIs
        foreach (GameObject poi in generatedPOIs)
        {
            Destroy(poi);
        }
        generatedPOIs.Clear();
        placedPOIs.Clear();
        
        // Regenerate
        GeneratePoi();
    }

    public List<GameObject> GetGeneratedPOIs()
    {
        return generatedPOIs;
    }

    void GeneratePoi()
    {
        for (int i = 0; i < poiCount; i++)
        {
            bool placed = false;
            int attempts = 0;

            // Try to place the POI without overlapping existing ones
            while (!placed && attempts < 50)
            {
                attempts++;

                // Random size
                float width = Random.Range(poiSizeMin, poiSizeMax);
                float height = Random.Range(poiSizeMin, poiSizeMax);

                // Random position within island bounds
                Vector2 pos = new Vector2(
                    Random.Range(-islandSize.x / 2f + width / 2f, islandSize.x / 2f - width / 2f),
                    Random.Range(-islandSize.y / 2f + height / 2f, islandSize.y / 2f - height / 2f)
                );

                Rect newRect = new Rect(pos.x - width / 2f, pos.y - height / 2f, width, height);

                // Check for overlaps
                bool overlaps = false;
                foreach (Rect r in placedPOIs)
                {
                    if (r.Overlaps(newRect))
                    {
                        overlaps = true;
                        break;
                    }
                }
                
                // If no overlap, place the POI
                if (!overlaps)
                {
                    GameObject poi = Instantiate(poiPrefab, pos, Quaternion.identity);
                    poi.transform.localScale = new Vector3(width, height, 1f);
                    SpriteRenderer sr = poi.GetComponent<SpriteRenderer>();
                    POI poiScript = poi.AddComponent<POI>();
                    generatedPOIs.Add(poi);

                    // Assign difficulty randomly
                    float roll = Random.value;
                    if (roll < easyPoiChance) {
                        poiScript.difficulty = POI.Difficulty.Easy;
                    } else if (roll < easyPoiChance + mediumPoiChance) {
                        poiScript.difficulty = POI.Difficulty.Medium;
                    } else {
                        poiScript.difficulty = POI.Difficulty.Hard;
                    }

                    // Color by difficulty
                    if (sr != null)
                    {
                        switch (poiScript.difficulty)
                        {
                            case POI.Difficulty.Easy: sr.color = new Color(0f, 0.5f, 0f); break;
                            case POI.Difficulty.Medium: sr.color = new Color(0.5f, 0.5f, 0f); break;
                            case POI.Difficulty.Hard: sr.color = new Color(0.5f, 0f, 0f); break;
                        }
                    }

                    // Record the placed POI
                    placedPOIs.Add(newRect);
                    placed = true;
                }
            }
        }
    }
}
