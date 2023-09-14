using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentPlanner.Services.Exceptions
{
    public class MatchNotFoundIdRequestExeption : Exception
    {
        public MatchNotFoundIdRequestExeption(string message) : base(message)
        {
            
        }
    }
}
