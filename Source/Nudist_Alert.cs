using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace Fyarn.FixableMoodDebuffsAlert
{
    [UsedImplicitly]
    public class NudistAlert : Alert
    {
        private List<Pawn> m_Nudists;

        [UsedImplicitly]
        private List<Pawn> Nudists
        {
            get => m_Nudists = AllPawnsInLongDistanceRelationships();
            set => m_Nudists = value;
        }

        public NudistAlert()
        {
            defaultLabel = "Nudists are too clothed";
            defaultPriority = AlertPriority.Medium;
        }

#if V10
        public override string GetExplanation()
#else
        public override TaggedString GetExplanation()
#endif
        {
            return $"{m_Nudists.Count} colonists on this map want to wear less clothes:\n\n{FormatString()}";
        }

        public override AlertReport GetReport()
        {
            return FixableMoodDebuffsAlert.Settings.AlertOnClothedNudist && !Nudists.NullOrEmpty()
                ? AlertReport.CulpritIs(m_Nudists[0])
                : false;
        }

        private string FormatString()
        {
            return m_Nudists.Aggregate("", (current, p) => current + p.Name.ToStringShort + "\n");
        }

        private static List<Pawn> AllPawnsInLongDistanceRelationships()
        {
            return new List<Pawn>(Find.CurrentMap.mapPawns.FreeColonists).FindAll(p =>
            {
                var outThoughts = new List<Thought>();
                p.needs.mood.thoughts.GetAllMoodThoughts(outThoughts);
                return outThoughts.Any(thought => thought.def == ThoughtDef.Named("ClothedNudist"));
            });
        }
    }
}