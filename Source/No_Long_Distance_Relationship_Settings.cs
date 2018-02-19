using Verse;

namespace No_Long_Distance_Relationships
{
    class No_Long_Distance_Relationship_Settings : ModSettings
    {
        public bool alertOnWrongMaster = true;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref alertOnWrongMaster, "alertOnWrongMaster", true);
        }
    }
}

