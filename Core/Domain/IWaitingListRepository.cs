using System;
using System.Collections.Generic;
using Core.Entity;

namespace Core.Domain
{
    public interface IWaitingListRepository: IRepository<WaitingListItem>
    {
        IEnumerable<WaitingListItem> GetAllIncludeAll();
    }
}
