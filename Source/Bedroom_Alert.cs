using System.Collections.Generic;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace Fyarn.FixableMoodDebuffsAlert
{
    [UsedImplicitly]
    public class BedroomAlert : Alert
    {
        private List<Pawn> m_Pawns;

        [UsedImplicitly]
        private List<Pawn> Pawns
        {
            get => m_Pawns = AllUnhappyPawns();
            set => m_Pawns = value;
        }

        public BedroomAlert()
        {
            defaultLabel = "Unhappy with bedrooms";
            defaultPriority = AlertPriority.Medium;
        }

#if V10
        public override string GetExplanation()
#else
        public override TaggedString GetExplanation()
#endif
        {
            return $"{m_Pawns.Count} colonists on this map are unhappy with their bedrooms:\n\n{FormatString()}";
        }

        public override AlertReport GetReport()
        {
            return FixableMoodDebuffsAlert.Settings.AlertOnBedroom && !Pawns.NullOrEmpty()
                ? AlertReport.CulpritIs(m_Pawns[0])
                : false;
        }

        private string FormatString()
        {
            var ret = "";
            foreach (var p in m_Pawns)
            {
                var outThoughts = new List<Thought>();
                p.needs.mood.thoughts.GetAllMoodThoughts(outThoughts);
                outThoughts.FindAll(thought =>
                    new List<System.Type>
                    {
                        typeof(ThoughtWorker_BedroomJealous), typeof(ThoughtWorker_Ascetic),
                        typeof(ThoughtWorker_Greedy)
                    }.Contains(thought.def.workerClass) && thought.MoodOffset() < 0f).ForEach(thought =>
                    ret += $"{p.Name.ToStringShort} ({thought.LabelCap}): {thought.MoodOffset()}\n");
            }

            return ret;
        }

        private static List<Pawn> AllUnhappyPawns()
        {
            return new List<Pawn>(Find.CurrentMap.mapPawns.FreeColonists).FindAll(p =>
            {
                var outThoughts = new List<Thought>();
                p.needs.mood.thoughts.GetAllMoodThoughts(outThoughts);
                return outThoughts.Any(thought =>
                    new List<System.Type>
                    {
                        typeof(ThoughtWorker_BedroomJealous), typeof(ThoughtWorker_Ascetic),
                        typeof(ThoughtWorker_Greedy)
                    }.Contains(thought.def.workerClass) && thought.MoodOffset() < 0f);
            });
        }
    }
}