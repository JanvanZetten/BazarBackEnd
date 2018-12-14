using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace infrastructure
{
    public class ResetRepository : IResetRepository
    {
        private readonly BazarContext _ctx;

        public ResetRepository(BazarContext ctx)
        {
            _ctx = ctx;
        }

        public string Reset()
        {
            var booths = _ctx.Booth.ToList();
            booths.ForEach(b => b.Booker = null);
            _ctx.SaveChanges();
            return $"Alle {booths.Count} stande er blevet nulstillet.";
        }
        

    }
}
