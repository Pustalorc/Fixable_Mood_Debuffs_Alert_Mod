using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace Fyarn.FixableMoodDebuffsAlert
{
    [UsedImplicitly]
    internal class FixableMoodDebuffsAlert : Mod
    {
        public static FixableMoodDebuffsAlertSettings Settings;

        public FixableMoodDebuffsAlert(ModContentPack content) : base(content)
        {
            Settings = GetSettings<FixableMoodDebuffsAlertSettings>();
        }

        public override string SettingsCategory()
        {
            return "Fixable Mood Debuffs Alert";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            var listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled("Alert when colonists with animal bonds aren't masters: ",
                ref Settings.AlertOnWrongMaster);
            listingStandard.CheckboxLabeled("Alert when night-owl colonists are working in the daytime: ",
                ref Settings.AlertOnNightOwlInDay);
            listingStandard.CheckboxLabeled("Alert when nudists are wearing clothes: ",
                ref Settings.AlertOnClothedNudist);
            listingStandard.CheckboxLabeled("Alert when colonists want a different bedroom: ",
                ref Settings.AlertOnBedroom);
            listingStandard.End();
            Settings.Write();
        }
    }
}