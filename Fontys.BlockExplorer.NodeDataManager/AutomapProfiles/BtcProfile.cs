using AutoMapper;
using Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore;
using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.Domain.Models;
using Fontys.BlockExplorer.Domain.NodeModels.BtcCore;
 
namespace Fontys.BlockExplorer.NodeDataManager.AutomapProfiles
{
    public class BtcProfile : Profile
    {
        public BtcProfile()
        {
            CreateMap<BtcBlockResponse, Block>()
                .ForMember(dest => dest.Transactions, act => act.MapFrom(src => src.Tx))
                .ForMember(dest => dest.CoinType, act => act.MapFrom(src => CoinType.BTC))
                .ForMember(dest => dest.NetworkType, act => act.MapFrom(src => NetworkType.BtcMainet));
            CreateMap<BtcTransactionResponse, Transaction>()
                .ForMember(dest => dest.Transfers, act => act.MapFrom(src => src.Vin))
                .ForMember(dest => dest.Transfers, act => act.MapFrom(src => src.Vout)); //TODO Sepreate Vin and vout destination object: so that automapper works and to indicate if from coin base
            CreateMap<BtcInputResponse, Transfer>()
                     .ForMember(dest => dest.TransferType, act => act.MapFrom(src => TransferType.Input));
            CreateMap<BtcOutputResponse, Transfer>()
                     .ForMember(dest => dest.TransferType, act => act.MapFrom(src => TransferType.Output));
            CreateMap<BtcAddressResponse, Address>();
        }
    }
}
