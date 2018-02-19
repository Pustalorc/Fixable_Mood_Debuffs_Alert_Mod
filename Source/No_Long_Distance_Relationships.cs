using RimWorld;
using System.Collections.Generic;
using Verse;

namespace No_Long_Distance_Relationships
{
    public class No_Long_Distance_Relationships : Alert
    {
        private List<Pawn> allLDRPawns;

        private List<Pawn> AllLDRPawns {
            get => allLDRPawns = AllPawnsInLongDistanceRelationships();
            set => allLDRPawns = value;
        }

        private List<PawnRelationDef> loverDefs = new List<PawnRelationDef>()
            { PawnRelationDefOf.Spouse, PawnRelationDefOf.Lover, PawnRelationDefOf.Fiance };

        public No_Long_Distance_Relationships()
        {
            defaultLabel = "Pawns in long distance relationship";
            defaultPriority = AlertPriority.Medium;
        }

        public override string GetExplanation()
        {
            return string.Format("{0} colonists on this map are in long-distance relationships:\n{1}",
                allLDRPawns.Count, FormatString());
        }

        public override AlertReport GetReport()
        {
            return AllLDRPawns.NullOrEmpty() ? false : AlertReport.CulpritIs(allLDRPawns[0]);
        }

        private string FormatString()
        {
            string ret = "";
            List<Pawn> listedPawns = new List<Pawn>();

            foreach (Pawn p in allLDRPawns) {
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

        private List<Pawn> AllPawnsInLongDistanceRelationships()
        {
            return (new List<Pawn>(Find.VisibleMap.mapPawns.FreeColonists)).FindAll(p => {
                List<Thought> outThoughts = new List<Thought>();
                p.needs.mood.thoughts.GetAllMoodThoughts(outThoughts);
                return outThoughts.Any(thought =>
                    thought.def.thoughtClass == typeof(Thought_WantToSleepWithSpouseOrLover));
            });
        }
    }
}