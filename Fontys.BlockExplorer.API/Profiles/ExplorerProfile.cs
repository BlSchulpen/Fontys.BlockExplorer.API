using AutoMapper;
using Fontys.BlockExplorer.API.Dto.Response;
using Fontys.BlockExplorer.Domain.Models;

namespace Fontys.BlockExplorer.API.Profiles
{
    public class ExplorerProfile : Profile
    {
        public ExplorerProfile()
        {
            CreateMap<Block, BlockSummaryResponse>()
                 .ForMember(dest => dest.NumberOfTransactions, act => act.MapFrom(scr => scr.Transactions.Count));
            CreateMap<Block, BlockResponse>();
            CreateMap<Transaction, TransactionResponse>();
            CreateMap<Transfer, TransferResponse>();
        }
    }
}
