namespace Fontys.BlockExplorer.API.Profiles
{
    using AutoMapper;
    using Domain.Models;
    using Dto.Response;

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
