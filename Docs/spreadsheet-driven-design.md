# Spreadsheet-Driven Design for IdleARPG

**Status**: 📝 Planned for Task 65+

**Last Updated**: October 1, 2025

---

## Overview

This document outlines how to implement spreadsheet-driven game design for the IdleARPG project. This approach is recommended starting at **Task 65** when expanding the research tree.

---

## When to Implement

✅ **Implement When:**
- Adding 5+ additional research nodes (Task 65)
- Creating facility upgrade systems (Tasks 52-59)
- Designing prestige/bloodline powers (Tasks 113-120)
- Balancing enemy stats and loot tables (Phase 19+)

❌ **Don't Implement Yet (Tasks 1-64):**
- You're still prototyping
- Small number of data entries
- Hardcoded data is fine for now

---

## Phase 1: Research Tree (Task 65)

### Google Sheet Structure

Create a sheet: **"IdleARPG - Research Tree"**

**Columns:**
- `ID` (string) - Unique identifier (e.g., "auto_loot")
- `DisplayName` (string) - UI display name
- `Description` (string) - Tooltip text
- `Cost` (int) - Research point cost
- `Tier` (int) - 1-4 for progression
- `Prerequisites` (string) - Comma-separated IDs of required research
- `EffectType` (string) - automation, xp_multiplier, hp_multiplier, etc.
- `EffectValue` (float) - Numeric effect value

**Example Rows:**

```csv
ID,DisplayName,Description,Cost,Tier,Prerequisites,EffectType,EffectValue
auto_loot,Auto Loot,Automatically pick up items,50,1,,automation,1.0
xp_boost_1,XP Boost I,+10% XP gain,50,1,,xp_multiplier,1.1
xp_boost_2,XP Boost II,+25% XP gain,200,2,xp_boost_1,xp_multiplier,1.25
hp_boost_1,HP Boost I,+10% Max HP,50,1,,hp_multiplier,1.1
crit_chance_1,Crit Chance I,+5% critical hit chance,100,1,,crit_chance,0.05
```

---

## Implementation Steps

### Step 1: Create Data Class

```csharp
using System;
using System.Collections.Generic;

namespace IdleARPG.Data
{
    [Serializable]
    public class ResearchNodeData
    {
        public string Id;
        public string DisplayName;
        public string Description;
        public int Cost;
        public int Tier;
        public string Prerequisites;  // Comma-separated
        public string EffectType;
        public float EffectValue;

        // Helper property
        public List<string> GetPrerequisiteList()
        {
            if (string.IsNullOrEmpty(Prerequisites))
                return new List<string>();

            return new List<string>(Prerequisites.Split(','));
        }
    }
}
```

### Step 2: Create CSV Loader

```csharp
using System.Collections.Generic;
using UnityEngine;

namespace IdleARPG.Data
{
    public static class ResearchDataLoader
    {
        public static List<ResearchNodeData> LoadFromCSV()
        {
            TextAsset csvFile = Resources.Load<TextAsset>("Data/research");
            if (csvFile == null)
            {
                Debug.LogError("Could not find research.csv in Resources/Data/");
                return new List<ResearchNodeData>();
            }

            return ParseCSV(csvFile.text);
        }

        static List<ResearchNodeData> ParseCSV(string csvText)
        {
            var nodes = new List<ResearchNodeData>();
            string[] lines = csvText.Split('\n');

            // Skip header (line 0)
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;

                string[] values = line.Split(',');
                if (values.Length < 8) continue;

                var node = new ResearchNodeData
                {
                    Id = values[0].Trim(),
                    DisplayName = values[1].Trim(),
                    Description = values[2].Trim(),
                    Cost = int.Parse(values[3].Trim()),
                    Tier = int.Parse(values[4].Trim()),
                    Prerequisites = values[5].Trim(),
                    EffectType = values[6].Trim(),
                    EffectValue = float.Parse(values[7].Trim())
                };

                nodes.Add(node);
            }

            return nodes;
        }
    }
}
```

### Step 3: Update ResearchManager

```csharp
namespace IdleARPG.Managers
{
    public class ResearchManager : MonoBehaviour
    {
        [Header("Research Points")]
        public float CurrentResearchPoints = 0f;
        public float PointsPerSecond = 0.5f;

        [Header("Research Tree")]
        public List<ResearchNodeData> ResearchNodes;  // Changed from array

        void Awake()
        {
            // Load from CSV instead of hardcoding
            ResearchNodes = ResearchDataLoader.LoadFromCSV();
            Debug.Log($"Loaded {ResearchNodes.Count} research nodes from CSV");
        }

        // Rest of ResearchManager code stays the same...
    }
}
```

### Step 4: Export Sheet to CSV

1. In Google Sheets: **File → Download → Comma Separated Values (.csv)**
2. Save to: `Assets/Resources/Data/research.csv`
3. Unity will auto-import it

---

## Phase 2: Advanced Features (Later Tasks)

### XP Curve (Task 28+)

**Sheet: "XP Curve"**

```csv
Level,XP_Required,Cumulative_XP,HP_Gain,Damage_Gain
1,100,0,10,1.0
2,120,100,12,1.1
3,144,220,14,1.2
...
```

### Enemy Stats (Task 31+)

**Sheet: "Enemy Database"**

```csv
ID,DisplayName,HP,Damage,XP_Drop,Gold_Drop,Spawn_Weight
goblin_weak,Weak Goblin,50,5,10,5,10
goblin_strong,Strong Goblin,150,15,30,15,5
orc_warrior,Orc Warrior,500,40,100,50,3
```

### Facility Upgrades (Task 52+)

**Sheet: "Training Simulator Costs"**

```csv
Level,Cost,Generation_Rate,Storage_Cap
1,100,5,50
2,250,7,75
3,500,10,100
...
```

---

## Benefits of This Approach

### 1. Fast Iteration
- Change costs without recompiling
- Test different balance scenarios
- Designer can tweak values directly

### 2. Visualization
- Plot progression curves in sheets
- See cost scaling visually
- Spot balance issues early

### 3. Team Collaboration
- Share sheet with team
- Non-programmers can contribute
- Version history in Google Sheets

### 4. Formula Testing
- Test formulas in sheets first
- Calculate time-to-max-level
- Verify cost scaling

---

## Advanced: Loading from Google Sheets API (Optional)

For live updates without rebuild:

```csharp
using System.Collections;
using UnityEngine.Networking;

public static IEnumerator LoadFromGoogleSheetsAPI(System.Action<List<ResearchNodeData>> callback)
{
    const string SHEET_URL = "https://docs.google.com/spreadsheets/d/YOUR_ID/export?format=csv&gid=0";

    using (UnityWebRequest www = UnityWebRequest.Get(SHEET_URL))
    {
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            var nodes = ParseCSV(www.downloadHandler.text);
            callback?.Invoke(nodes);
        }
        else
        {
            Debug.LogError($"Failed to load from Google Sheets: {www.error}");
            // Fallback to local CSV
            callback?.Invoke(LoadFromCSV());
        }
    }
}
```

**To enable:**
1. File → Share → Publish to web
2. Select "Comma-separated values (.csv)"
3. Copy the URL
4. Use in UnityWebRequest

---

## Tools & Resources

### CSV Libraries
- Built-in `string.Split()` (simple, works for basic CSVs)
- [CsvHelper](https://github.com/JoshClose/CsvHelper) (robust, handles edge cases)
- Custom parser (current approach)

### Google Sheets Tips
- Use **Data Validation** to prevent errors
- Add **Conditional Formatting** for warnings (e.g., cost > 1M)
- Keep a **Changelog** tab for tracking balance changes
- Use **Named Ranges** for easier API access

### Example Formulas

**Cost Scaling:**
```
=Base_Cost * (Multiplier ^ (Tier - 1))
```

**Time to Unlock:**
```
=Cost / (Points_Per_Second * 60)
```

**Balance Warning:**
```
=IF(Cost > 100000, "⚠️ Very Expensive", "OK")
```

---

## Migration Checklist

When implementing spreadsheet-driven design:

- [ ] Create Google Sheet with proper structure
- [ ] Export initial CSV with existing data
- [ ] Create `ResearchDataLoader` class
- [ ] Update `ResearchManager` to load from CSV
- [ ] Test that all existing research nodes work
- [ ] Add 5 new Tier 1 nodes to sheet
- [ ] Verify costs and effects
- [ ] Update save/load system if needed
- [ ] Document sheet structure for team

---

## Future Sheets to Create

As the project grows, create sheets for:

- ✅ Research Tree (Task 65)
- ⬜ XP Curve (Task 28+)
- ⬜ Enemy Stats (Task 31+)
- ⬜ Loot Tables (Task 31+)
- ⬜ Facility Costs (Task 52+)
- ⬜ Prestige Bonuses (Task 113+)
- ⬜ Achievement List (Task 105+)
- ⬜ Item Database (Task 97+)

---

## Questions?

Refer to this document when you reach Task 65 and are ready to expand the research tree beyond the initial 3 nodes.

**Reminder set**: User requested notification about spreadsheet-driven design at Task 65 ✅
