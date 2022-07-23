namespace Fontys.BlockExplorer.API.Profiles
{
    using AutoMapper;
    using Fontys.BlockExplorer.API.Dto.Response;
    using Fontys.BlockExplorer.Domain.Models;

    public class ExplorerProfile : Profile
    {
        public ExplorerProfile()
        {
            CreateMap<Block, BlockResponse>();
            CreateMap<Transaction, TransactionResponse>();
            CreateMap<Transfer, TransferResponse>();
        }
    }
}
