using System.Collections.Generic;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace Fyarn.FixableMoodDebuffsAlert
{
    [UsedImplicitly]
    public class NoLongDistanceRelationshipsAlert : Alert
    {
        private List<Pawn> m_AllLDRPawns;

        [UsedImplicitly]
        private List<Pawn> AllLDRPawns
        {
            get => m_AllLDRPawns = AllPawnsInLongDistanceRelationships();
            set => m_AllLDRPawns = value;
        }

        private readonly List<PawnRelationDef> m_LoverDefs = new List<PawnRelationDef>
            {PawnRelationDefOf.Spouse, PawnRelationDefOf.Lover, PawnRelationDefOf.Fiance};

        public NoLongDistanceRelationshipsAlert()
        {
            defaultLabel = "Pawns want to sleep together";
            defaultPriority = AlertPriority.Medium;
        }

#if V10
        public override string GetExplanation()
#else
        public override TaggedString GetExplanation()
#endif
        {
            return $"{m_AllLDRPawns.Count} colonists on this map want to sleep together:\n\n{FormatString()}";
        }

        public override AlertReport GetReport()
        {
            return AllLDRPawns.NullOrEmpty() ? false : AlertReport.CulpritIs(m_AllLDRPawns[0]);
        }

        private string FormatString()
        {
            var ret = "";
            var listedPawns = new List<Pawn>();

            foreach (var p in m_AllLDRPawns)
            {
                if (listedPawns.Contains(p)) continue;

                var lover = p.relations.DirectRelations.Find(
                    relation => m_LoverDefs.Contains(relation.def)
                ).otherPawn;

                listedPawns.Add(p);
                listedPawns.Add(lover);

                ret += p.Name.ToStringShort + " - " + lover.Name.ToStringShort + "\n";
            }

            return ret;
        }

        private static List<Pawn> AllPawnsInLongDistanceRelationships()
        {
            return new List<Pawn>(Find.CurrentMap.mapPawns.FreeColonists).FindAll(p =>
            {
                var outThoughts = new List<Thought>();
                p.needs.mood.thoughts.GetAllMoodThoughts(outThoughts);

                return outThoughts.Any(thought =>
                    thought.def.workerClass == typeof(ThoughtWorker_WantToSleepWithSpouseOrLover));
            });
        }
    }
}