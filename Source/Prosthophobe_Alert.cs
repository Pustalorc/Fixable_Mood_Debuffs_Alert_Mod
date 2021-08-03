using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace Fyarn.FixableMoodDebuffsAlert
{
    [UsedImplicitly]
    public class ProsthophobeAlert : Alert
    {
        private List<Pawn> m_Pawns;

        [UsedImplicitly]
        private List<Pawn> Pawns
        {
            get => m_Pawns = UnhappyProsthophobes();
            set => m_Pawns = value;
        }

        public ProsthophobeAlert()
        {
            defaultLabel = "Prosthophobes have bionics";
            defaultPriority = AlertPriority.Medium;
        }

#if V10
        public override string GetExplanation()
#else
        public override TaggedString GetExplanation()
#endif
        {
            return $"{m_Pawns.Count} colonists on this map want less advanced prosthetics:\n\n{FormatString()}";
        }

        public override AlertReport GetReport()
        {
            return FixableMoodDebuffsAlert.Settings.AlertOnProsthophile && !Pawns.NullOrEmpty()
                ? AlertReport.CulpritIs(m_Pawns[0])
                : false;
        }

        private string FormatString()
        {
            return m_Pawns.Aggregate("", (current, p) => current + p.Name.ToStringShort + "\n");
        }

        private static List<Pawn> UnhappyProsthophobes()
        {
            return new List<Pawn>(Find.CurrentMap.mapPawns.FreeColonists).FindAll(p =>
            {
                var outThoughts = new List<Thought>();
                p.needs.mood.thoughts.GetAllMoodThoughts(outThoughts);
                return outThoughts.Any(thought => thought.def == ThoughtDef.Named("ProsthophobeUnhappy"));
            });
        }
    }
}