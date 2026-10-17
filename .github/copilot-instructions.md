# GitHub Copilot Instructions: Block Unwanted Minutiae (Continued)

## Mod Overview and Purpose

Block Unwanted Minutiae is a RimWorld mod designed to enhance player experience by providing control over the game's alerts, messages, and letters. This is a continuation and update of mr_chimick's original mod, revised for compatibility with new versions of RimWorld (1.3 and 1.4). The primary purpose is to allow players to block unwanted in-game minutiae that may be distracting, thus streamlining notifications to focus only on what's important.

## Key Features and Systems

- **Custom Filters**: Added possibility to customize filters for alerts, messages, and letters to block or allow specific notifications.
  
- **Messages Management**: 
  - Block non-essential messages such as tainted apparel deterioration while keeping important ones active.
  - Intuitive search and block functionality for standard vanilla messages.

- **Alerts Management**:
  - Customize alerts, such as idle colonists, with functionality that isolates colonist alerts without guest interruptions.
  - Comprehensive search and block tools for vanilla alerts.

- **Letters Management**:
  - Searchable and blockable vanilla letters list, enabling players to manage narrative elements selectively.

- **Miscellaneous Features**:
  - Disabling of the auto-select button for food and travel supplies in caravans, giving players manual control over logistics.

## Coding Patterns and Conventions

- The project follows a structured C# coding style with clear type and member declarations across Harmony patches.
- Class and method naming conventions are descriptive and follow PascalCase, ensuring readability.
- Extensive use of C# features like classes and methods to manage game logic, with code grouped logically by functionality (e.g., alerts, messages).

## XML Integration

- The mod includes XML files for defining update information, dependencies, and manifest entries.
- XML files are used to integrate mod content with different parts of RimWorld's game environment.
- Ensure XML entries are correctly formatted and validate against RimWorld's schema for smooth integration.

## Harmony Patching

- Harmony is used extensively to modify game behavior without altering original game files.
- Custom patches are implemented for specific game alerts and messages, utilizing `Prefix` and `Postfix` methods.
- **Important Patches Include**:
  - `Alert_ColonistsIdle_IdleColonists`: Adjustments to idle colonist notifications.
  - `BreakRiskAlertUtility_PawnsAtRiskExtreme` and `Major`: Tweaks to risk alert levels.
  - `Alert_Thought_GetReport`: Modifies the generation of thought reports for notifications.

## Suggestions for Copilot

- Automatically suggest namespace and dependency imports for C# files based on the mod's existing use of Harmony and RimWorld API.
- Provide common XML patterns, such as defentries and dependencies, that align with RimWorld's modding standards.
- Offer hint snippets for frequent coding tasks, like adding new filters in C# or understanding RimWorld's notification structure.
- Suggest code refactoring opportunities in C# to keep the project maintainable and efficient.
- When adding new Harmony patches, suggest possible `TargetMethods` and common patch types (`Prefix`, `Postfix`, `Transpiler`).

By following these guidelines and leveraging GitHub Copilot effectively, contributors can continue to enhance and maintain the Block Unwanted Minutiae mod to meet evolving community needs and updates in RimWorld.


This document should provide detailed guidance and assistance in using GitHub Copilot for maintaining and extending the "Block Unwanted Minutiae (Continued)" mod project.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

