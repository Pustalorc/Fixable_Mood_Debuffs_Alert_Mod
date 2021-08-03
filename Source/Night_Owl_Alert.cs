using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace Fyarn.FixableMoodDebuffsAlert
{
    [UsedImplicitly]
    public class NightOwlAlert : Alert
    {
        private List<Pawn> m_NightOwls;

        [UsedImplicitly]
        private List<Pawn> NightOwls
        {
            get => m_NightOwls = AllPawnsInLongDistanceRelationships();
            set => m_NightOwls = value;
        }

        public NightOwlAlert()
        {
            defaultLabel = "Unhappy night owls";
            defaultPriority = AlertPriority.Medium;
        }

#if V10
        public override string GetExplanation()
#else
        public override TaggedString GetExplanation()
#endif
        {
            return $"{m_NightOwls.Count} colonists on this map want a better schedule:\n\n{FormatString()}";
        }

        public override AlertReport GetReport()
        {
            return FixableMoodDebuffsAlert.Settings.AlertOnNightOwlInDay && !NightOwls.NullOrEmpty()
                ? AlertReport.CulpritIs(m_NightOwls[0])
                : false;
        }

        private string FormatString()
        {
            return m_NightOwls.Aggregate("", (current, p) => current + p.Name.ToStringShort + "\n");
        }

        private static List<Pawn> AllPawnsInLongDistanceRelationships()
        {
            return new List<Pawn>(Find.CurrentMap.mapPawns.FreeColonists).FindAll(p =>
            {
                var outThoughts = new List<Thought>();
                p.needs.mood.thoughts.GetAllMoodThoughts(outThoughts);
                return outThoughts.Any(thought =>
                    thought.def.workerClass == typeof(ThoughtWorker_IsDayForNightOwl));
            });
        }
    }
}