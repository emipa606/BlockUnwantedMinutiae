using Verse;
using UnityEngine;

namespace BlockUnwantedMinutiae
{
    public class BUMSettings : ModSettings
    {
        public bool taintedMessagePatch = true;
        public bool idleColonistsPatch = true;
        public bool drawAutoSelectCheckboxPatch = true;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref taintedMessagePatch, "taintedMessagePatch");
            Scribe_Values.Look(ref idleColonistsPatch, "idleColonistsPatch");
            Scribe_Values.Look(ref drawAutoSelectCheckboxPatch, "drawAutoSelectCheckboxPatch");
            base.ExposeData();
        }
    }

    public class BUMMod : Mod
    {
        readonly BUMSettings settings;

        public BUMMod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<BUMSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled("Disable the message for when tainted clothing deteriorates in a stockpile zone", ref settings.taintedMessagePatch);
            listingStandard.CheckboxLabeled("Make the alert for idle pawns only appear for your own colonists, not guests", ref settings.idleColonistsPatch);
            listingStandard.CheckboxLabeled("Remove and disable the automatically add food to caravan check", ref settings.drawAutoSelectCheckboxPatch);
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "BUM: Block Unwanted Minutiae";
        }
    }
}