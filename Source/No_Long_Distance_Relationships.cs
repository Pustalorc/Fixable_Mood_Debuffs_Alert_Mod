using RimWorld;
using System.Collections.Generic;
using Verse;

namespace No_Long_Distance_Relationships
{
    public class No_Long_Distance_Relationships : Alert
    {
        private List<PawnRelationDef> loverDefs = new List<PawnRelationDef>()
            { PawnRelationDefOf.Spouse, PawnRelationDefOf.Lover, PawnRelationDefOf.Fiance };

        public No_Long_Distance_Relationships()
        {
            defaultLabel = "Pawns in long distance relationship";
            defaultPriority = AlertPriority.Medium;
        }

        public override string GetExplanation()
        {
            List<Pawn> pawns = AllPawnsInLongDistanceRelationships();
            return string.Format("{0} colonists on this map are in long-distance relationships:\n{1}",
                pawns.Count, FormatString(pawns));
        }

        public override AlertReport GetReport()
        {
            return AnyPawnsInLongDistanceRelationships();
        }

        private string FormatString(List<Pawn> pawns)
        {
            string ret = "";
            List<Pawn> listedPawns = new List<Pawn>();

            foreach (Pawn p in pawns) {
                if (listedPawns.Contains(p)) {
                    continue;
                }

                Pawn lover = p.relations.DirectRelations.Find(
                    relation => loverDefs.Contains(relation.def)
                ).otherPawn;

                listedPawns.Add(p);
                listedPawns.Add(lover);

                ret += p.NameStringShort + " - " + lover.NameStringShort + "\n";
            }

            return ret;
        }

        private bool AnyPawnsInLongDistanceRelationships()
        {
            return ((List<Pawn>) Find.VisibleMap.mapPawns.FreeColonists).Any(p =>
                 p.needs.mood.thoughts.memories.Memories.Any(memory =>
                    memory.GetType() == typeof(Thought_WantToSleepWithSpouseOrLover)));
        }

        private List<Pawn> AllPawnsInLongDistanceRelationships()
        {
            return ((List<Pawn>) Find.VisibleMap.mapPawns.FreeColonists).FindAll(p =>
                p.needs.mood.thoughts.memories.Memories.Any(memory =>
                    memory.GetType() == typeof(Thought_WantToSleepWithSpouseOrLover)));
        }
    }
}