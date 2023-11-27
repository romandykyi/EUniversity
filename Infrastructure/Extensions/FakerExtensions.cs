using Bogus;
using EUniversity.Core.Models;

namespace EUniversity.Infrastructure.Extensions;

public static class FakerExtensions
{
    public static Faker<T> RuleForDates<T>(this Faker<T> faker)
        where T : class, IHasCreationDate, IHasUpdateDate
    {
        return faker
            .RuleFor(x => x.CreationDate, f => f.Date.RecentOffset(180))
            .RuleFor(x => x.UpdateDate,
            (f, x) => f.Date.BetweenOffset(x.CreationDate, x.CreationDate + TimeSpan.FromDays(90)));
    }
}
