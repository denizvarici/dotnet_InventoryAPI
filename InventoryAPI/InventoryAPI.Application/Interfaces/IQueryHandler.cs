using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryAPI.Application.Interfaces
{
    public interface IQueryHandler<in TQuery,TResult>
    {
        Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
    }
}
