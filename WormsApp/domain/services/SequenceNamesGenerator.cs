using WormsApp.Domain.Services;

namespace WormsApp.domain.services
{
    public class SequenceNamesGenerator : INamesGenerator
    {
        private int _namesSequence;

        public string NextName()
        {
            return "Worm#" + _namesSequence++;
        }
    }
}