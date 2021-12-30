using System;
using System.Collections.Generic;

namespace SeeTrue.Infrastructure.Types
{
    public record Pagination<T>(int Page, int PerPage, int ItemCount, List<T> Items);
}
