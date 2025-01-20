using System;
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
    private static readonly Color alternateBackground = new Color(0.1f, 0.1f, 0.1f, 0.5f);
    public readonly BUMSettings settings;
    private string newRule;
    private Vector2 scrollPosition;
    private Vector2 scrollPosition2;
    private Vector2 scrollPosition3;
    private Tab tab = Tab.Custom;

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
        tabsList.Add(new TabRecord("Custom", () => tab = Tab.Custom, tab == Tab.Custom));
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
            case Tab.Custom:
                Text.Font = GameFont.Small;
                var addRect = customList.GetRect(30f);
                newRule = Widgets.TextField(addRect.LeftPart(0.9f), newRule);
                if (!string.IsNullOrEmpty(newRule) &&
                    Widgets.ButtonText(addRect.RightPart(0.08f), "BUM.Add".Translate()))
                {
                    if (Instance.settings.CustomFilters.Contains(newRule))
                    {
                        Messages.Message("BUM.RuleExists".Translate(), MessageTypeDefOf.RejectInput);
                    }
                    else
                    {
                        Instance.settings.CustomFilters.Add(newRule);
                        Instance.settings.ExposeData();
                        newRule = string.Empty;
                    }
                }

                customList.Label("BUM.CustomSettingsInfo".Translate());
                break;
            case Tab.Messages:
                customList.Label("BUM.MessageSettings".Translate());
                Text.Font = GameFont.Small;
                customList.CheckboxLabeled("BUM.DeterioratedTainted".Translate(), ref settings.taintedMessagePatch);
                break;
            case Tab.Alerts:
                customList.Label("BUM.AlertSettings".Translate());
                Text.Font = GameFont.Small;
                customList.CheckboxLabeled("BUM.IdleColonists".Translate(), ref settings.idleColonistsPatch);
                break;
            case Tab.Letters:
                customList.Label("BUM.LetterSettings".Translate());
                Text.Font = GameFont.Small;
                customList.Label("BUM.NoCustomLetters".Translate());
                break;
            case Tab.Misc:
                customList.Label("BUM.MiscellaneousSettings".Translate());
                Text.Font = GameFont.Small;
                customList.CheckboxLabeled("BUM.NoAddFood".Translate(), ref settings.drawAutoSelectCheckboxPatch);
                break;
        }

        customList.Gap();
        customList.GapLine();
        customList.End();

        if (tab is Tab.Misc)
        {
            base.DoSettingsWindowContents(inRect);
            return;
        }

        if (tab is Tab.Custom)
        {
            var leftViewRect = genericRect.LeftHalf().ContractedBy(1f);
            var leftListing = new Listing_Standard();
            leftListing.Begin(leftViewRect);
            Text.Font = GameFont.Medium;
            var titleRect = leftListing.Label("BUM.CurrentMatches".Translate());
            Text.Font = GameFont.Small;
            leftListing.End();

            var borderRect = leftViewRect;
            borderRect.y += titleRect.y + 40;
            borderRect.height -= titleRect.y + 40;

            var scrollContentRect = leftViewRect;
            scrollContentRect.height = Math.Max(Instance.settings.CustomFilters.Count * 50f, borderRect.height);
            scrollContentRect.width -= 20f;
            scrollContentRect.x = 0;
            scrollContentRect.y = 0;

            var scrollListing = new Listing_Standard();

            Widgets.BeginScrollView(borderRect, ref scrollPosition2, scrollContentRect);

            scrollListing.Begin(scrollContentRect);
            var alternate = false;
            for (var index = 0; index < Instance.settings.CustomFilters.Count; index++)
            {
                var filter = Instance.settings.CustomFilters[index];
                alternate = !alternate;
                var sliderRect = scrollListing.GetRect(50f);
                if (alternate)
                {
                    Widgets.DrawBoxSolid(sliderRect, alternateBackground);
                }

                if (filter.Length > 100)
                {
                    Text.Font = GameFont.Tiny;
                }

                Widgets.Label(sliderRect.ContractedBy(1).LeftPart(0.9f), filter);
                Text.Font = GameFont.Small;

                if (!Widgets.ButtonImageFitted(
                        sliderRect.ContractedBy(1).RightPartPixels(sliderRect.height)
                            .ContractedBy(sliderRect.height / 4), TexButton.CloseXSmall))
                {
                    continue;
                }

                Instance.settings.CustomFilters.Remove(filter);
                Instance.settings.ExposeData();
            }

            Widgets.EndScrollView();
            scrollListing.End();


            var rightViewRect = genericRect.RightHalf().ContractedBy(1f);
            var rightListing = new Listing_Standard();
            rightListing.Begin(rightViewRect);
            Text.Font = GameFont.Medium;
            var headerLabel =
                rightListing.Label("BUM.LatestSeen".Translate(), tooltip: "BUM.LatestSeenTooltip".Translate());
            Text.Font = GameFont.Small;
            rightListing.End();

            borderRect = rightViewRect;
            borderRect.y += headerLabel.y + 40;
            borderRect.height -= headerLabel.y + 40;

            scrollContentRect = rightViewRect;
            scrollContentRect.height = Math.Max(Instance.settings.SeenText.Count * 51f, borderRect.height);
            scrollContentRect.width -= 20f;
            scrollContentRect.x = 0;
            scrollContentRect.y = 0;

            scrollListing = new Listing_Standard();

            Widgets.BeginScrollView(borderRect, ref scrollPosition3, scrollContentRect);

            scrollListing.Begin(scrollContentRect);
            alternate = false;
            foreach (var seen in Instance.settings.SeenText)
            {
                alternate = !alternate;
                var sliderRect = scrollListing.GetRect(50f);
                if (alternate)
                {
                    Widgets.DrawBoxSolid(sliderRect, alternateBackground);
                }

                var originalColor = GUI.color;
                if (Instance.settings.Matches(newRule, seen))
                {
                    GUI.color = Color.green;
                }

                if (seen.Length > 100)
                {
                    Text.Font = GameFont.Tiny;
                }

                Widgets.Label(sliderRect.ContractedBy(1).LeftPart(0.9f), seen);
                Text.Font = GameFont.Small;
                GUI.color = originalColor;
                if (!Widgets.ButtonImageFitted(
                        sliderRect.ContractedBy(1).RightPartPixels(sliderRect.height)
                            .ContractedBy(sliderRect.height / 4), TexButton.Copy))
                {
                    continue;
                }

                newRule = seen;
            }

            Widgets.EndScrollView();
            scrollListing.End();

            base.DoSettingsWindowContents(inRect);
            return;
        }

        var genericTitle = new Listing_Standard();
        genericTitle.Begin(genericTitleRect);
        Text.Font = GameFont.Medium;
        switch (tab)
        {
            case Tab.Messages:
                genericTitle.Label("BUM.MessageBlockers".Translate());
                break;
            case Tab.Alerts:
                genericTitle.Label("BUM.AlertBlockers".Translate());
                break;
            case Tab.Letters:
                genericTitle.Label("BUM.LetterBlockers".Translate());
                break;
        }

        Text.Font = GameFont.Small;
        searchText = genericTitle.TextEntry(searchText);
        genericTitle.End();

        if (Widgets.ButtonText(genericBtnRect, "BUM.SelectAll".Translate()))
        {
            ChangeTabPatches(true);
        }

        genericBtnRect.x += 105f;
        if (Widgets.ButtonText(genericBtnRect, "BUM.SelectNone".Translate()))
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
        Misc,
        Custom
    }
}