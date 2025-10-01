# IdleARPG Namespace Structure

**Last Updated**: October 1, 2025

## Overview

All scripts in the IdleARPG project now use proper C# namespaces for better organization and code clarity.

---

## Namespace Hierarchy

```
IdleARPG
├── IdleARPG.Data          // Data structures, models, serializable classes
├── IdleARPG.Managers      // Game system managers (singletons)
├── IdleARPG.Player        // Player-related code
├── IdleARPG.UI            // UI components and controllers
└── IdleARPG.Utilities     // Helper classes, formatters, extensions
```

---

## Current File Mappings

### **IdleARPG.Data**
- `SimpleResearchNode` - Research node data structure

### **IdleARPG.Managers**
- `ResearchManager` - Manages research points and unlocks
- `MenuManager` - Manages menu panels and navigation

### **IdleARPG.Player**
- `Player` - Player controller, movement, and progression

### **IdleARPG.UI**
- `ResearchUI` - Research tree UI controller

### **IdleARPG.Utilities**
- `NumberFormatter` - Number formatting utilities (K/M/B notation)

---

## Using Namespaces in Your Code

### Example: Using NumberFormatter in ResearchUI

```csharp
using TMPro;
using UnityEngine;
using IdleARPG.Utilities;  // Add this to use NumberFormatter

namespace IdleARPG.UI
{
    public class ResearchUI : MonoBehaviour
    {
        void UpdateDisplay(float points)
        {
            // Now you can use NumberFormatter
            string formatted = NumberFormatter.Format(points);
        }
    }
}
```

### Common Using Statements

For most scripts, you'll need:

```csharp
using UnityEngine;
using IdleARPG.Managers;    // For ResearchManager, MenuManager
using IdleARPG.Data;        // For SimpleResearchNode
using IdleARPG.Utilities;   // For NumberFormatter
```

---

## Future Namespaces (Planned)

As the project grows, these namespaces will be added:

- `IdleARPG.Combat` - Combat systems, damage, enemy AI
- `IdleARPG.Progression` - Leveling, XP, prestige systems
- `IdleARPG.Facilities` - Resource generation facilities
- `IdleARPG.Achievements` - Achievement tracking and rewards
- `IdleARPG.Save` - Save/load system (SaveManager)
- `IdleARPG.Audio` - Audio management
- `IdleARPG.Animation` - Animation controllers and helpers

---

## Best Practices

1. **Always use the full namespace** at the top of your file
2. **Add using statements** for namespaces you reference
3. **Keep related code together** in the same namespace
4. **Use descriptive names** - the namespace should explain what the code does
5. **Don't nest too deeply** - 2-3 levels maximum (IdleARPG.Category.Subcategory)

---

## Unity-Specific Notes

- MonoBehaviour scripts must be in a file with the **exact same name** as the class
- Namespace changes require **Unity to recompile** - this may take a moment
- If Unity shows errors after namespace changes, try **closing and reopening Unity**
- Inspector references are maintained even after adding namespaces

---

## Migration Checklist

When adding a new namespace:

- [ ] Add namespace declaration to file
- [ ] Add using statements for referenced namespaces
- [ ] Close the namespace with `}` at end of file
- [ ] Test in Unity to ensure no compilation errors
- [ ] Update this document with the new namespace

---

## Questions?

Refer to this document when:
- Adding new scripts
- Referencing existing scripts
- Organizing new features
- Planning system architecture
