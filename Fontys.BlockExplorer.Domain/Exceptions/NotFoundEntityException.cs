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
