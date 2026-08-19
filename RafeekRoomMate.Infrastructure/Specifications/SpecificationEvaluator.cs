using Microsoft.EntityFrameworkCore;
using RafeekRoomMate.Domain.Specifications;

namespace RafeekRoomMate.Infrastructure.Specifications
{
    public static class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> specification)
        {
            if (specification.Criteria != null)
                query = query.Where(specification.Criteria);

            query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

            if (specification.OrderBy != null)
                query = query.OrderBy(specification.OrderBy);

            if (specification.OrderByDescending != null)
                query = query.OrderByDescending(specification.OrderByDescending);

            if (specification.IsPagingEnabled)

                query = query
                    .Skip(specification.Skip)
                    .Take(specification.Take);

            return query;
        }
    }
}
