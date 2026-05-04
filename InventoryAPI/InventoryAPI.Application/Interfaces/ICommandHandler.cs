using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryAPI.Application.Interfaces
{
    public interface ICommandHandler<in TCommand, TResult>
    {
        Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
    }
}
