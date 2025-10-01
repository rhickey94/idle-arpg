# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Unity 6000.2.6f1 IdleARPG game project using Universal Render Pipeline (URP) with C# as the primary language. **2D sprite-based** game with idle/incremental mechanics.

## Development Commands

**Unity Development:**
- Open project in Unity Editor version 6000.2.6f1
- Build: Unity Editor → File → Build Settings → Build
- Run Tests: Unity Editor → Window → General → Test Runner
- Play Mode: Unity Editor → Play button or Ctrl+P

**IDE:**
- Open `IdleARPG.sln` in Visual Studio with Unity workload
- Unity automatically regenerates .csproj files when scripts change

**Git Configuration:**
- Unity Editor Settings: Version Control Mode = "Visible Meta Files", Asset Serialization Mode = "Force Text"
- Use provided .gitignore for Unity projects
- Commit frequently with descriptive task-based messages

## Architecture

**Project Structure:**
- `Assets/Scripts/` - Core game logic organized by system:
  - `Managers/` - Singleton managers for game systems
  - `Player/` - Player controller and related components  
  - `Research/` - Research tree and progression system
  - `Resources/` - Resource generation and management
  - `Data/` - Data models and structures
  - `UI/` - Menu and interface systems
- `Assets/ScriptableObjects/` - Data-driven game configuration
- `Assets/Prefabs/` - Reusable game objects
- `Assets/Sprites/` - 2D art assets (characters, items, UI)
- `Assets/UI/` - UI components and layouts
- `Assets/Scenes/` - Game scenes (main: SampleScene.unity)

**Key Patterns:**
- Manager pattern for core systems (e.g., ResearchManager, MenuManager)
- ScriptableObjects for data configuration
- Unity's component-based architecture
- Unity Input System (configured for Unity 6.2)
- Event-driven communication between systems
- Menu panel system with show/hide functionality

**Technical Details:**
- C# 9.0 with .NET Standard 2.1
- Universal Render Pipeline with 2D renderer configuration
- Unity Input System package (resolved compatibility issues with Unity 6.2)
- TextMeshPro for all text rendering
- 2D sprite-based character and environment art

## Game Design: Idle-ARPG Hybrid

### Core Concept
A hybrid game combining Action RPG combat (like Diablo) with idle/incremental mechanics (like Cookie Clicker). The key innovation is separating power progression from efficiency progression:

- **Active Combat** = Power Gains (XP, levels, combat stats, equipment)
- **Idle Systems** = Efficiency Gains (automation, quality-of-life, progression multipliers)

### Design Philosophy
Players must actively fight to get stronger, but idle systems help them fight more effectively and progress faster when active. Neither mode replaces the other - they're symbiotic.

### Art Direction Decision
**2D sprite-based** for faster development, easier iteration, better genre fit for idle mechanics, and broader device compatibility. 3D was considered but rejected due to complexity and development time.

## Core Systems

### 1. Research & Development System (Idle Currency)
- Research Points generate while idle (1 per minute base rate)
- Spent on permanent upgrades across 4 tiers:
  - **Tier 1** (0-500 points): Basic automation (auto-loot, auto-target, auto-potion)
  - **Tier 2** (500-2,000): Combat enhancement (+XP, +crit, +drop rates)
  - **Tier 3** (2,000-10,000): Advanced systems (multi-target, boss tracking, perfect rotations)
  - **Tier 4** (10,000+): Reality manipulation (time dilation, omniscience, probability control)

### 2. Infrastructure & Resource Generation
Four idle facilities that generate consumable resources:
- **Combat Simulator** → Training Tokens (temporary XP/damage boosts)
- **Enchantment Workshop** → Enchantment Dust (gear enhancement)
- **Alchemy Laboratory** → Reagents (powerful temporary potions)
- **Intelligence Network** → Intel Points (boss locations, enemy analysis)

Each facility has exponential upgrade costs and generation rates.

### 3. Prestige System: "Ascension Chambers"
- Reset character level/gear but keep research and infrastructure
- Gain Bloodline Powers (permanent bonuses that stack)
- Unlock Legendary Blueprints and Memory Fragments
- Multiple ascension tiers with increasing requirements and rewards

### 4. Active Session Enhancement
- Session Planning Tools (efficiency calculators, goal tracking)
- Temporary Multipliers (powered by idle-generated resources)
- Automation Unlocks that enhance but don't replace manual play

### 5. Menu System Architecture
Professional menu system with:
- MainHUD (persistent UI elements)
- Panel-based menus (Research, Inventory, Character, Facilities, Settings)
- MenuManager script controlling panel visibility
- Keyboard shortcuts (R, I, F, C, ESC)
- Proper panel hierarchy and state management

### 6. Progression Feedback Systems
- Number formatting (K/M/B notation)
- Progress rate indicators (Research/sec, XP/sec)
- Time-to-next-unlock estimates
- Achievement system
- Floating damage numbers
- Celebration effects and notifications

## Implementation Progress

### Completed Systems ✅
- **ResearchManager.cs** - Research point generation (1pt/min), 3 nodes (auto-loot, xp_boost, hp_boost), save/load to PlayerPrefs
- **ResearchUI.cs** - Dynamic button generation, research unlock UI with cost display and state colors
- **Player.cs** - Unity Input System integration, WASD movement, XP/level system, research effect application

### Actually Built (Verified) ✅
- ✅ Research point generation system (1 point per minute base rate)
- ✅ Research tree with 3 basic nodes (auto-loot, xp_boost +10%, hp_boost)
- ✅ Event-driven architecture (OnResearchUnlocked, OnResearchPointsChanged events)
- ✅ Unity 6.2 Input System integration (programmatic input actions for movement, testing)
- ✅ Basic XP and leveling system on Player
- ✅ Save/load with PlayerPrefs for research data
- ✅ Research unlock effects applied to player

### Partially Implemented ⚠️
- ⚠️ **MenuManager.cs** - Core functionality complete, needs additional panels (Inventory, Character, Facilities, Settings)
- ⚠️ **Menu panel architecture** - MainHUD and ResearchTreePanel working, other panels pending

### NOT Yet Implemented ❌
- ❌ NumberFormatter.cs - Does not exist (UI uses basic :F1 formatting)
- ❌ K/M/B number notation - Not implemented
- ❌ Progress feedback displays - No visual progress bars or celebration effects
- ❌ Combat systems - No enemies, spawning, or loot
- ❌ Offline progression - No timestamp tracking implemented
- ❌ 2D sprite system - Player is 3D capsule mesh

### Technical Architecture
- Component-based design for modularity
- Event-driven communication between systems (ResearchManager events)
- Basic save/load with PlayerPrefs
- Ready for ScriptableObjects and menu system expansion

## Task Management and Progress Tracking

**IMPORTANT: GitHub Issues are the single source of truth for all tasks and progress.**

- All development tasks must be tracked as GitHub Issues
- The `Docs/task-list.md` file should be kept in sync with GitHub Issues for quick reference
- Do NOT track detailed task progress or task lists in this CLAUDE.md file
- This file should only contain high-level architectural information, design decisions, and implementation status

### Viewing Current Tasks
- Check GitHub Issues for current tasks and priorities
- See `Docs/task-list.md` for a local reference (synced with GitHub Issues)
- Use GitHub Project boards for milestone and phase tracking

### Task Workflow
1. Create GitHub Issue for each new task
2. Update `Docs/task-list.md` to match GitHub Issues
3. Reference issue numbers in commit messages (e.g., "Fixes #42")
4. Close issues when tasks are complete
5. Update CLAUDE.md only for architectural changes or major system completions

## Key Design Decisions Made

### Technical Decisions
- **2D over 3D**: Faster development, better for idle game mechanics
- **Unity 6.2**: Latest stable version with modern Input System
- **Menu-driven UI**: Professional game feel over debug-style always-visible UI
- **Component architecture**: Modular, testable, expandable systems

### Genre Analysis Decisions
- **Added missing systems**: Achievement framework, number formatting, damage feedback
- **Progression feedback priority**: Essential for player retention in idle games
- **Professional menu system**: Required for feature-complete game

### Art Direction Decisions
- **2D sprites**: Recommended starting resources include Kenney.nl, OpenGameArt.org
- **Placeholder art strategy**: Unity primitives → free 2D assets → polished art
- **Visual coherence**: 2D style supports UI-heavy idle game mechanics

## Current Implementation Status

### Recently Completed
- ✅ Basic research system (ResearchManager with 3 nodes)
- ✅ Unity 6.2 Input System programmatic setup working
- ✅ Simple research UI with dynamic button generation
- ✅ Player movement and XP/leveling foundation
- ✅ Event-driven architecture for research unlocks
- ✅ MenuManager.cs with panel-based architecture
- ✅ Keyboard shortcuts for menu navigation (R, I, F, C, ESC)
- ✅ Player input blocking when menus are open

### Current State
- MenuManager.cs implemented with panel-based menu system
- MainHUD with keyboard shortcuts (R, I, F, C, ESC)
- ResearchTreePanel integrated with menu system
- 3D capsule mesh for player (not 2D sprite yet)
- Basic research point generation (1/minute)
- No combat, enemies, or loot systems yet

### Next Steps
See GitHub Issues and `Docs/task-list.md` for current priorities and task breakdown.

## Development Notes

### Unity 6.2 Specific Issues Resolved
- Input System compatibility (switched from legacy Input class to programmatic InputAction setup)
- TextMeshPro integration for all text rendering
- Unity 6.2 requires programmatic input binding vs InputActionAsset for simple cases

### Key Learnings So Far
- Started with very basic prototype (3 scripts only)
- Event-driven architecture working well for research unlocks
- Need to implement menu system before adding more features
- 2D transition will require replacing 3D capsule with sprite renderer
- Idle game needs professional number formatting from the start

## Free Resource Strategy

### Current Art Sources
- Kenney.nl - UI elements, basic sprites
- OpenGameArt.org - 2D character and environment art
- Unity Asset Store - Free section for basic assets

### Audio Sources (Planned)
- Freesound.org - Sound effects
- YouTube Audio Library - Background music
- Incompetech - Royalty-free music

## Testing & Quality Assurance

### Current Testing Approach
- Manual testing of each completed task
- Save/load testing with each new system
- Menu navigation and state management testing
- Research point generation and spending verification

### Future Testing Plans
- Offline progression accuracy testing
- Cross-platform compatibility (desktop focus initially)
- Performance testing with 2D sprite systems
- User experience testing with menu navigation