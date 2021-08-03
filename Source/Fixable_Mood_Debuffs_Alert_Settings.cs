using Verse;

namespace Fyarn.FixableMoodDebuffsAlert
{
    internal sealed class FixableMoodDebuffsAlertSettings : ModSettings
    {
        public bool AlertOnWrongMaster = true;
        public bool AlertOnNightOwlInDay = true;
        public bool AlertOnClothedNudist = true;
        public bool AlertOnProsthophile = true;
        public bool AlertOnBedroom = true;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref AlertOnWrongMaster, "alertOnWrongMaster", true);
            Scribe_Values.Look(ref AlertOnNightOwlInDay, "alertOnNightOwlInDay", true);
            Scribe_Values.Look(ref AlertOnClothedNudist, "alertOnClothedNudist", true);
            Scribe_Values.Look(ref AlertOnBedroom, "alertOnBedroom", true);
            Scribe_Values.Look(ref AlertOnProsthophile, "alertOnProsthophile", true);
        }
    }
}