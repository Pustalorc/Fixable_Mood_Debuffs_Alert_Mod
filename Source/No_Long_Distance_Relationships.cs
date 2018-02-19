using UnityEngine;
using Verse;

namespace No_Long_Distance_Relationships
{
    class No_Long_Distance_Relationships : Mod
    {
        public static No_Long_Distance_Relationship_Settings settings;

        public No_Long_Distance_Relationships(ModContentPack content) : base(content)
        {
            settings = GetSettings<No_Long_Distance_Relationship_Settings>();
        }

        public override string SettingsCategory() => "PriorityClean";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);
            listing_Standard.CheckboxLabeled("Alert when colonists with animal bonds aren't masters: ", ref settings.alertOnWrongMaster);
            listing_Standard.End();
            settings.Write();
        }
    }
}
