using System;
using System.Linq;
using UnityEngine;

namespace IdleARPG.Managers
{
    [System.Serializable]
    public class SimpleResearchNode
    {
        public string Id;
        public string DisplayName;
        public string Description;
        public int Cost;
        public bool IsUnlocked = false;
    }


    public class ResearchManager : MonoBehaviour
    {
        [Header("Research Points")]
        public float CurrentResearchPoints = 0f;
        public float PointsPerSecond = 0.5f; // 30 point per minute as base

        [Header("Basic Research Tree")]
        public SimpleResearchNode[] ResearchNodes =
        {
        new() {
            Id = "auto_loot",
            DisplayName = "Auto Loot",
            Description = "Automatically pick up items",
            Cost = 50
        },
        new() {
            Id = "xp_boost",
            DisplayName = "XP Boost I",
            Description = "+10% XP gain",
            Cost = 100
        },
        new() {
            Id = "hp_boost",
            DisplayName = "HP Boost I",
            Description = "+10% Max HP gain",
            Cost = 50
        },
    };

        public static event Action<float> OnResearchPointsChanged;
        public static event Action<string> OnResearchUnlocked;


        void Start()
        {
            InvokeRepeating(nameof(GenerateResearchPoints), 1f, 1f);

            LoadResearchData();

            Debug.Log("Research Manager started!");
        }

        void GenerateResearchPoints()
        {
            CurrentResearchPoints += PointsPerSecond;
            OnResearchPointsChanged?.Invoke(CurrentResearchPoints);
        }

        public bool CanUnlockResearch(string researchId)
        {
            var node = GetResearchNode(researchId);
            if (node == null || node.IsUnlocked) return false;

            return CurrentResearchPoints >= node.Cost;
        }

        public bool UnlockResearch(string researchId)
        {
            if (!CanUnlockResearch(researchId)) return false;

            var node = GetResearchNode(researchId);
            CurrentResearchPoints -= node.Cost;
            node.IsUnlocked = true;

            Debug.Log($"Unlocked research: {node.DisplayName}");
            OnResearchUnlocked?.Invoke(researchId);

            SaveResearchData();
            return true;
        }

        public bool IsResearchUnlocked(string researchId)
        {
            var node = GetResearchNode(researchId);
            return node != null && node.IsUnlocked;
        }

        SimpleResearchNode GetResearchNode(string researchId)
        {
            return ResearchNodes.FirstOrDefault(node => node.Id == researchId);
        }

        void SaveResearchData()
        {
            PlayerPrefs.SetFloat("ResearchPoints", CurrentResearchPoints);
            foreach (var node in ResearchNodes)
            {
                PlayerPrefs.SetInt($"Research_{node.Id}", node.IsUnlocked ? 1 : 0);
            }

            PlayerPrefs.Save();
        }

        void LoadResearchData()
        {
            CurrentResearchPoints = PlayerPrefs.GetFloat("ResearchPoints", 0f);
            foreach (var node in ResearchNodes)
            {
                node.IsUnlocked = PlayerPrefs.GetInt($"Research_{node.Id}", 0) == 1;
            }

            OnResearchPointsChanged?.Invoke(CurrentResearchPoints);
        }

        [ContextMenu("Add 100 Research Points")]
        public void AddTestPoints()
        {
            CurrentResearchPoints += 100;
            OnResearchPointsChanged?.Invoke(CurrentResearchPoints);
            Debug.Log($"Added test points. Total: {CurrentResearchPoints}");
        }
    }
}
