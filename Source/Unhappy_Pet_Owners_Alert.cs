using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace Fyarn.FixableMoodDebuffsAlert
{
    [UsedImplicitly]
    public class UnhappyPetOwnersAlert : Alert
    {
        private List<Pawn> m_AllUnhappyPawns;

        [UsedImplicitly]
        private List<Pawn> AllUnhappyPawns
        {
            get => m_AllUnhappyPawns = AllPawnsWhoWantToMaster();
            set => m_AllUnhappyPawns = value;
        }

        public UnhappyPetOwnersAlert()
        {
            defaultLabel = "Pawns want to master their bonded pets";
            defaultPriority = AlertPriority.Medium;
        }

#if V10
        public override string GetExplanation()
#else
        public override TaggedString GetExplanation()
#endif
        {
            return $"{m_AllUnhappyPawns.Count} colonists on this map want to master their pets:\n\n{FormatString()}";
        }

        public override AlertReport GetReport()
        {
            return FixableMoodDebuffsAlert.Settings.AlertOnWrongMaster && !AllUnhappyPawns.NullOrEmpty()
                ? AlertReport.CulpritIs(m_AllUnhappyPawns[0])
                : false;
        }

        private string FormatString()
        {
            return m_AllUnhappyPawns.Aggregate("", (current, p) => current + p.Name.ToStringShort + "\n");
        }

        private static List<Pawn> AllPawnsWhoWantToMaster()
        {
            return new List<Pawn>(Find.CurrentMap.mapPawns.FreeColonists).FindAll(p =>
            {
                var outThoughts = new List<Thought>();
                p.needs.mood.thoughts.GetAllMoodThoughts(outThoughts);
                return outThoughts.Any(thought =>
                    thought.def.workerClass == typeof(ThoughtWorker_NotBondedAnimalMaster));
            });
        }
    }
}