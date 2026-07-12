# GitHub Copilot Instructions for Block Unwanted Minutiae (Continued)

## Mod Overview and Purpose

The "Block Unwanted Minutiae (Continued)" mod aims to enhance the quality of life for RimWorld players by providing a customizable experience to block certain unwanted messages, alerts, and letters that occur during gameplay. The mod offers settings to enable or disable specific notifications so players can focus on more important aspects of their game without unnecessary interruptions.

## Key Features and Systems

- **Messages Filtering**: Filter out unwelcome messages like detailed information about apparel deterioration, while keeping essential notifications active.
- **Alerts Management**: Customize which alerts are shown, such as only counting colonists for idle alerts and not guests.
- **Letters Management**: Choose which letters and notifications you want to see, enabling a clutter-free notification system.
- **Miscellaneous Features**: Disable automatic selections in the form caravan menu for a more customizable caravan planning experience.
- **Custom Filters**: Recently added feature allowing players to set up custom filters as per their preference.

## Coding Patterns and Conventions

- **C# Naming Conventions**:
  - Use PascalCase for class and method names (e.g., `BreakRiskAlertUtility_PawnsAtRiskExtreme`).
  - Method members follow a clear naming convention indicating their purpose (`Postfix`, `Prefix`, `TargetMethods`).

- **Code Structure**:
  - Separate different types of functionalities using distinct files for better organization. Patch-specific logic is housed under the `HarmonyPatches` directory.
  - Helper methods are located in a centralized file like `GenericMessagePatchHelper.cs` to be reused across various patches.

## XML Integration

- XML files are used to define mod metadata and dependencies, such as in `About.xml` and `Manifest.xml`.
- Custom XML definitions can be added in files like `UpdateInfo.xml` to manage specific game elements influenced by the mod.
- XML integration ensures that mod data is structured and easily extensible for future updates.

## Harmony Patching

- **Harmony**: The mod uses Harmony, a library for patching, to modify existing game behavior without directly altering the game's source code.
- **Patch Types**:
  - Use `Prefix` and `Postfix` methods within patch classes to hook into specific game functions.
  - `TargetMethods` lists which original game methods are affected by the patches, allowing precise control over game logic modifications.

## Suggestions for Copilot

1. **Automation of Routine Code**:
   - Allow Copilot to auto-generate boilerplate Harmony patch setups, providing stubs for `Prefix` and `Postfix` methods along with logging to verify patch application.

2. **XML Editing**:
   - Provide Copilot with context about XML structure so it can intelligently autofill fields related to dependencies and versioning.

3. **Comprehensive Logging**:
   - Encourage generating detailed logging within patches to track what messages or alerts are being blocked.

4. **Custom Filter Tools**:
   - Use Copilot to support more complex algorithms for filter creation, especially handling dynamically altering notification lists.

5. **Settings UI**:
   - Leverage Copilot to develop intuitive UI elements for mod settings that allow players to toggle various notifications on and off seamlessly.

These guidelines and suggestions will aid in maintaining the mod's quality, fostering clean code practices, and enriching the development experience through tools like GitHub Copilot.

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

