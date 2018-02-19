using Verse;

namespace Fixable_Mood_Debuffs_Alert
{
    class Fixable_Mood_Debuffs_Alert_Settings : ModSettings
    {
        public bool alertOnWrongMaster = true;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref alertOnWrongMaster, "alertOnWrongMaster", true);
        }
    }
}

