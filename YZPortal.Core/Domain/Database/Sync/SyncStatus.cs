using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.EntityTypes;
using YZPortal.Core.Extensions;

namespace YZPortal.Core.Domain.Database.Sync
{
    public class SyncStatus : AuditableEntity
    {
        public int Name { get; set; }
        public bool Status { get; set; }
        public string? Notes { get; set; }
        public int Type { get; set; }
        public bool IsSyncDisabled { get; set; }
        public DateTime ExecutionDateTime { get; set; }
    }

    #region Sync status abstract and extensions

    public abstract class SyncStatusTypeAbstract
    {
        protected PortalContext Database { get; }
        public SyncStatusTypeAbstract(PortalContext dbContext)
        {
            Database = dbContext;
        }

        public virtual void CreateSyncStatuses<T>(int syncStatusType)
        {
            List<int> syncStatusTypeNames = TypeExtension.GetEnumDataTypeValues<T>();

            var synStatusesBasedOnType = Database.SyncStatuses.Where(x => x.Type == syncStatusType).Select(x => x.Name).ToList();

            foreach (var constant in syncStatusTypeNames)
            {
                if (synStatusesBasedOnType.Contains(constant) == false)
                {
                    Database.Add(new SyncStatus() { Name = constant, Type = syncStatusType });
                }
            }
            Database.SaveChanges();
        }
    }

    public class SyncStatusTypeUser : SyncStatusTypeAbstract
    {
        public SyncStatusTypeUser(PortalContext dbContext) : base(dbContext)
        {
        }
        public void CreateSyncStatuses()
        {
            base.CreateSyncStatuses<SyncStatusTypeUserNames>((int)SyncStatusTypes.User);
        }
    }

    public class SyncStatusTypeDevice : SyncStatusTypeAbstract
    {
        public SyncStatusTypeDevice(PortalContext dbContext) : base(dbContext)
        {
        }
        public void CreateSyncStatuses()
        {
            base.CreateSyncStatuses<SyncStatusTypeDeviceNames>((int)SyncStatusTypes.Device);
        }
    }

    #endregion

    #region SyncStatusEnums

    [Flags]
    public enum SyncStatusTypes
    {
        None = 0,
        User = 1,
        Device = 2
    }

    [Flags]
    public enum SyncStatusTypeUserNames
    {
        None = 0,
        Users = 1,
        Dealers = 2,
        Memberships = 3
    }

    [Flags]
    public enum SyncStatusTypeDeviceNames
    {
        None = 0,
        DeviceOrders = 1,
        DeviceOrderLines = 2,
        DeliveryReports = 3
    }

    [Flags]
    public enum SyncStatusTypeObsoleteNames
    {
        None = 0,
        DeviceLifeCycleMajorStatuses = 1,
        DevicePackingSlips = 2,
        DeviceBackOrders = 3
    }

    #endregion
}
