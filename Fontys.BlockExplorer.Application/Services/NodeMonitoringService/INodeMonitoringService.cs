namespace Fontys.BlockExplorer.Application.Services.NodeMonitoringService
{
    using Fontys.BlockExplorer.Domain.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    
    public interface INodeMonitoringService
    {
        Task<ICollection<Block>> UpdateStoredAsync();
    }
}
