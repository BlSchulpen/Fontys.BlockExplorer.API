using AutoMapper;
using Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore;
using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.Domain.Models;
using Fontys.BlockExplorer.Domain.NodeModels.BtcCore;
using Fontys.BlockExplorer.NodeWarehouse.NodeServices.Btc;

namespace Fontys.BlockExplorer.NodeDataManager.AutomapProfiles
{
    public class BtcProfile : Profile
    {
        public BtcProfile()
        {
            CreateMap<Block,BtcBlockResponse>();
            CreateMap<Transaction, BtcTransactionResponse>();
            CreateMap<Transfer, BtcInputResponse>();
            CreateMap<Transfer, BtcOutputResponse>();
            CreateMap<Address, BtcAddressResponse>();
            CreateMap<SourceAddition, DestinationAddition>() 
                 .ForMember(x => x.Hash, map => map.MapFrom(x => x.Hash));
            CreateMap<Source, Destination>()
                .ForMember(x => x.PreviousHash, map => map.MapFrom(c => c.Previousblockhash))
                .ForMember(dest => dest.CoinType, act => act.MapFrom(src => CoinType.BTC))
                .ForMember(dest => dest.NetworkType, act => act.MapFrom(src => NetworkType.BtcMainet))
                .ForMember(x => x.Transactions, map => map.MapFrom(c => c.Tx));

        }
    }
}
