using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fontys.BlockExplorer.Domain.Exceptions
{
    [Serializable]
    public class NotFoundEntityException : Exception
    {
        public Guid[] EntityIds { get; }

        public NotFoundEntityException(Guid entityId)
        {
            EntityIds = new Guid[] { entityId };
        }

        public NotFoundEntityException(Guid[] entityIds)
        {
            EntityIds = entityIds;
        }
    }
}
