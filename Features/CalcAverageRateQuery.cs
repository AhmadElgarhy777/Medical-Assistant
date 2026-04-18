using DataAccess.EntittySpecifcation;
using DataAccess.Repositry.IRepositry;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features
{
    public record CalcAverageRateQuery(string TargetId):IRequest<double>;
    public class CalcAverageRateQueryHandler : IRequestHandler<CalcAverageRateQuery, double>
    {
        private readonly IRatingRepositry ratingRepositry;

        public CalcAverageRateQueryHandler(IRatingRepositry ratingRepositry)
        {
            this.ratingRepositry = ratingRepositry;
        }
        public async Task<double> Handle(CalcAverageRateQuery request, CancellationToken cancellationToken)
        {
            var TargetId = request.TargetId;
            var spec = new RatingSpecifcation(r => r.TargetId == TargetId);
            var ratings = await ratingRepositry.GetAll(spec).ToListAsync();
            if (ratings.Count == 0)
                return 0;

            var average = ratings.Average(r => ((double)r.Stars));
            return average;
        }
    }
}
