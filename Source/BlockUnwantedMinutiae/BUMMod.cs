using System.Collections.Generic;
using BlockUnwantedMinutiae.Patches;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace BlockUnwantedMinutiae;

public class BUMMod : Mod
{
    private const int LINE_MAX = 100;
    private static readonly List<TabRecord> tabsList = [];
    private static string searchText = "";
    public static BUMMod Instance;
    public readonly BUMSettings settings;
    private Vector2 scrollPosition;
    private Tab tab = Tab.Messages;

    public BUMMod(ModContentPack content) : base(content)
    {
        Instance = this;
        settings = GetSettings<BUMSettings>();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        // Create tab menu area
        var tabRect = inRect;
        tabRect.yMin += 30f;

        tabsList.Clear();
        tabsList.Add(new TabRecord("Messages", () => tab = Tab.Messages, tab == Tab.Messages));
        tabsList.Add(new TabRecord("Alerts", () => tab = Tab.Alerts, tab == Tab.Alerts));
        tabsList.Add(new TabRecord("Letters", () => tab = Tab.Letters, tab == Tab.Letters));
        tabsList.Add(new TabRecord("Misc", () => tab = Tab.Misc, tab == Tab.Misc));

        Widgets.DrawMenuSection(tabRect);
        TabDrawer.DrawTabs(tabRect, tabsList);
        tabsList.Clear();

        // Create Custom and Generic menu areas

        var contentRect = tabRect;
        contentRect = contentRect.ContractedBy(20f);

        var customRect = contentRect;
        var genericRect = contentRect;
        genericRect.yMin += 85f;
        var genericTitleRect = genericRect;
        genericTitleRect.width -= 200f;
        var genericBtnRect = genericRect;
        genericBtnRect.width = 95f;
        genericBtnRect.height = 25f;
        genericBtnRect.x = genericTitleRect.x + genericTitleRect.width + 10f;
        genericBtnRect.y += 29f;
        var genericViewRect = genericRect;
        genericViewRect.yMin += 60f;

        var customList = new Listing_Standard();
        customList.Begin(customRect);
        Text.Font = GameFont.Medium;

        switch (tab)
        {
            case Tab.Messages:
                customList.Label("Custom Message Blockers");
                Text.Font = GameFont.Small;
                customList.CheckboxLabeled("MessageDeterioratedAway - But only tainted apparel",
                    ref settings.taintedMessagePatch);
                break;
            case Tab.Alerts:
                customList.Label("Custom Alert Blockers");
                Text.Font = GameFont.Small;
                customList.CheckboxLabeled("ColonistsIdle - But only for your own colonists, not guests",
                    ref settings.idleColonistsPatch);
                break;
            case Tab.Letters:
                customList.Label("Custom Letter Blockers");
                Text.Font = GameFont.Small;
                customList.Label("No custom letter blockers currently; requests open on the workshop.");
                break;
            case Tab.Misc:
                customList.Label("Miscellaneous Blockers");
                Text.Font = GameFont.Small;
                customList.CheckboxLabeled("Remove and disable the automatically add food to caravan checkbox",
                    ref settings.drawAutoSelectCheckboxPatch);
                break;
        }

        customList.Gap();
        customList.GapLine();
        customList.End();

        if (tab is Tab.Messages or Tab.Alerts or Tab.Letters)
        {
            var genericTitle = new Listing_Standard();
            genericTitle.Begin(genericTitleRect);
            Text.Font = GameFont.Medium;
            switch (tab)
            {
                case Tab.Messages:
                    genericTitle.Label("Generic Message Blockers");
                    break;
                case Tab.Alerts:
                    genericTitle.Label("Generic Alert Blockers");
                    break;
                case Tab.Letters:
                    genericTitle.Label("Generic Letter Blockers");
                    break;
            }

            Text.Font = GameFont.Small;
            searchText = genericTitle.TextEntry(searchText);
            genericTitle.End();

            if (Widgets.ButtonText(genericBtnRect, "Select All"))
            {
                ChangeTabPatches(true);
            }

            genericBtnRect.x += 105f;
            if (Widgets.ButtonText(genericBtnRect, "Deselect All"))
            {
                ChangeTabPatches(false);
            }

            var scrollRect = genericViewRect;
            switch (tab)
            {
                case Tab.Messages:
                    scrollRect.height = 26.1f * BUMSettings.genericMessage_labels.Count;
                    break;
                case Tab.Alerts:
                    scrollRect.height = 26.1f * BUMSettings.genericAlert_labels.Count;
                    break;
                case Tab.Letters:
                    scrollRect.height = 26.1f * BUMSettings.genericLetter_labels.Count;
                    break;
            }

            scrollRect.width = genericViewRect.width - 20f;
            var searchOn = searchText.Length > 0;
            if (searchOn)
            {
                scrollPosition = Vector2.zero;
            }

            Widgets.BeginScrollView(genericViewRect, ref scrollPosition, scrollRect);
            var listRect = scrollRect;
            var listingStandard = new Listing_Standard
            {
                verticalSpacing = 4f
            };
            listingStandard.Begin(listRect);

            switch (tab)
            {
                case Tab.Messages:
                    for (var i = 0; i < BUMSettings.genericMessage_labels.Count; i++)
                    {
                        var label = BUMSettings.genericMessage_labels[i];
                        string message = $"{label} - " + label.Translate();

                        if (searchOn && !message.ToLower().Contains(searchText.ToLower()))
                        {
                            continue;
                        }

                        if (message.Length > LINE_MAX)
                        {
                            message = $"{message.Substring(0, LINE_MAX)}...";
                        }

                        listingStandard.CheckboxLabeled(message, ref settings.genericMessage_values[i]);
                    }

                    break;
                case Tab.Alerts:
                    for (var i = 0; i < BUMSettings.genericAlert_labels.Count; i++)
                    {
                        var label = BUMSettings.genericAlert_labels[i];
                        string message = $"{label} - " + label.Translate();

                        if (searchOn && !message.ToLower().Contains(searchText.ToLower()))
                        {
                            continue;
                        }

                        if (message.Length > LINE_MAX)
                        {
                            message = $"{message.Substring(0, LINE_MAX)}...";
                        }

                        listingStandard.CheckboxLabeled(message, ref settings.genericAlert_values[i]);
                    }

                    break;
                case Tab.Letters:
                    for (var i = 0; i < BUMSettings.genericLetter_labels.Count; i++)
                    {
                        var label = BUMSettings.genericLetter_labels[i];
                        string message = $"{label} - " + label.Translate();

                        if (searchOn && !message.ToLower().Contains(searchText.ToLower()))
                        {
                            continue;
                        }

                        if (message.Length > LINE_MAX)
                        {
                            message = $"{message.Substring(0, LINE_MAX)}...";
                        }

                        listingStandard.CheckboxLabeled(message, ref settings.genericLetter_values[i]);
                    }

                    break;
            }

            listingStandard.End();
            Widgets.EndScrollView();
        }

        base.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "BUM: Block Unwanted Minutiae";
    }

    public void ChangeTabPatches(bool newState)
    {
        if (newState)
        {
            SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera();
        }
        else
        {
            SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera();
        }

        switch (tab)
        {
            case Tab.Messages:
                for (var i = 0; i < settings.genericMessage_values.Length; i++)
                {
                    settings.genericMessage_values[i] = newState;
                }

                break;
            case Tab.Alerts:
                for (var i = 0; i < settings.genericAlert_values.Length; i++)
                {
                    settings.genericAlert_values[i] = newState;
                }

                break;
            case Tab.Letters:
                for (var i = 0; i < settings.genericLetter_values.Length; i++)
                {
                    settings.genericLetter_values[i] = newState;
                }

                break;
        }
    }

    public override void WriteSettings()
    {
        base.WriteSettings();
        settings.ResetPatches();
        GenericMessagePatchHelper.ResetPatches();
    }

    private enum Tab
    {
        Messages,
        Alerts,
        Letters,
        Misc
    }
}