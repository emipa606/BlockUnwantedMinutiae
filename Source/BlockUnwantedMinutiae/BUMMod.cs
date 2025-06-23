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
    private const int LineMax = 100;
    private static readonly List<TabRecord> tabsList = [];
    private static string searchText = "";
    public static BUMMod Instance;
    private static readonly Color alternateBackground = new(0.1f, 0.1f, 0.1f, 0.5f);
    public readonly BUMSettings Settings;
    private string newRule;
    private Vector2 scrollPosition;
    private Vector2 scrollPosition2;
    private Vector2 scrollPosition3;
    private Tab tab = Tab.Custom;

    public BUMMod(ModContentPack content) : base(content)
    {
        Instance = this;
        Settings = GetSettings<BUMSettings>();
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
                    if (Instance.Settings.CustomFilters.Contains(newRule))
                    {
                        Messages.Message("BUM.RuleExists".Translate(), MessageTypeDefOf.RejectInput);
                    }
                    else
                    {
                        Instance.Settings.CustomFilters.Add(newRule);
                        Instance.Settings.ExposeData();
                        newRule = string.Empty;
                    }
                }

                customList.Label("BUM.CustomSettingsInfo".Translate());
                break;
            case Tab.Messages:
                customList.Label("BUM.MessageSettings".Translate());
                Text.Font = GameFont.Small;
                customList.CheckboxLabeled("BUM.DeterioratedTainted".Translate(), ref Settings.TaintedMessagePatch);
                break;
            case Tab.Alerts:
                customList.Label("BUM.AlertSettings".Translate());
                Text.Font = GameFont.Small;
                customList.CheckboxLabeled("BUM.IdleColonists".Translate(), ref Settings.IdleColonistsPatch);
                break;
            case Tab.Letters:
                customList.Label("BUM.LetterSettings".Translate());
                Text.Font = GameFont.Small;
                customList.Label("BUM.NoCustomLetters".Translate());
                break;
            case Tab.Misc:
                customList.Label("BUM.MiscellaneousSettings".Translate());
                Text.Font = GameFont.Small;
                customList.CheckboxLabeled("BUM.NoAddFood".Translate(), ref Settings.DrawAutoSelectCheckboxPatch);
                break;
        }

        customList.Gap();
        customList.GapLine();
        customList.End();

        switch (tab)
        {
            case Tab.Misc:
                base.DoSettingsWindowContents(inRect);
                return;
            case Tab.Custom:
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
                scrollContentRect.height = Math.Max(Instance.Settings.CustomFilters.Count * 50f, borderRect.height);
                scrollContentRect.width -= 20f;
                scrollContentRect.x = 0;
                scrollContentRect.y = 0;

                var scrollListing = new Listing_Standard();

                Widgets.BeginScrollView(borderRect, ref scrollPosition2, scrollContentRect);

                scrollListing.Begin(scrollContentRect);
                var alternate = false;
                for (var index = 0; index < Instance.Settings.CustomFilters.Count; index++)
                {
                    var filter = Instance.Settings.CustomFilters[index];
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

                    Instance.Settings.CustomFilters.Remove(filter);
                    Instance.Settings.ExposeData();
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
                scrollContentRect.height = Math.Max(Instance.Settings.SeenText.Count * 51f, borderRect.height);
                scrollContentRect.width -= 20f;
                scrollContentRect.x = 0;
                scrollContentRect.y = 0;

                scrollListing = new Listing_Standard();

                Widgets.BeginScrollView(borderRect, ref scrollPosition3, scrollContentRect);

                scrollListing.Begin(scrollContentRect);
                alternate = false;
                foreach (var seen in Instance.Settings.SeenText)
                {
                    alternate = !alternate;
                    var sliderRect = scrollListing.GetRect(50f);
                    if (alternate)
                    {
                        Widgets.DrawBoxSolid(sliderRect, alternateBackground);
                    }

                    var originalColor = GUI.color;
                    if (BUMSettings.Matches(newRule, seen))
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
            changeTabPatches(true);
        }

        genericBtnRect.x += 105f;
        if (Widgets.ButtonText(genericBtnRect, "BUM.SelectNone".Translate()))
        {
            changeTabPatches(false);
        }

        var scrollRect = genericViewRect;
        switch (tab)
        {
            case Tab.Messages:
                scrollRect.height = 26.1f * BUMSettings.GenericMessageLabels.Count;
                break;
            case Tab.Alerts:
                scrollRect.height = 26.1f * BUMSettings.GenericAlertLabels.Count;
                break;
            case Tab.Letters:
                scrollRect.height = 26.1f * BUMSettings.GenericLetterLabels.Count;
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
                for (var i = 0; i < BUMSettings.GenericMessageLabels.Count; i++)
                {
                    var label = BUMSettings.GenericMessageLabels[i];
                    string message = $"{label} - " + label.Translate();

                    if (searchOn && !message.ToLower().Contains(searchText.ToLower()))
                    {
                        continue;
                    }

                    if (message.Length > LineMax)
                    {
                        message = $"{message[..LineMax]}...";
                    }

                    listingStandard.CheckboxLabeled(message, ref Settings.GenericMessageValues[i]);
                }

                break;
            case Tab.Alerts:
                for (var i = 0; i < BUMSettings.GenericAlertLabels.Count; i++)
                {
                    var label = BUMSettings.GenericAlertLabels[i];
                    string message = $"{label} - " + label.Translate();

                    if (searchOn && !message.ToLower().Contains(searchText.ToLower()))
                    {
                        continue;
                    }

                    if (message.Length > LineMax)
                    {
                        message = $"{message[..LineMax]}...";
                    }

                    listingStandard.CheckboxLabeled(message, ref Settings.GenericAlertValues[i]);
                }

                break;
            case Tab.Letters:
                for (var i = 0; i < BUMSettings.GenericLetterLabels.Count; i++)
                {
                    var label = BUMSettings.GenericLetterLabels[i];
                    string message = $"{label} - " + label.Translate();

                    if (searchOn && !message.ToLower().Contains(searchText.ToLower()))
                    {
                        continue;
                    }

                    if (message.Length > LineMax)
                    {
                        message = $"{message[..LineMax]}...";
                    }

                    listingStandard.CheckboxLabeled(message, ref Settings.GenericLetterValues[i]);
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

    private void changeTabPatches(bool newState)
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
                for (var i = 0; i < Settings.GenericMessageValues.Length; i++)
                {
                    Settings.GenericMessageValues[i] = newState;
                }

                break;
            case Tab.Alerts:
                for (var i = 0; i < Settings.GenericAlertValues.Length; i++)
                {
                    Settings.GenericAlertValues[i] = newState;
                }

                break;
            case Tab.Letters:
                for (var i = 0; i < Settings.GenericLetterValues.Length; i++)
                {
                    Settings.GenericLetterValues[i] = newState;
                }

                break;
        }
    }

    public override void WriteSettings()
    {
        base.WriteSettings();
        Settings.ResetPatches();
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