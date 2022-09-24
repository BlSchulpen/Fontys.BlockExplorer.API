using AutoMapper;
using Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore.Block;
using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.Domain.Models;

namespace Fontys.BlockExplorer.NodeDataManager.AutomapProfiles
{
    public class BtcProfile : Profile
    {
        public BtcProfile()
        {
            CreateMap<BtcBlockResponse, Block>()
                .ForMember(dest => dest.Transactions, act => act.MapFrom(src => src.Transactions))
                .ForMember(dest => dest.CoinType, act => act.MapFrom(src => CoinType.BTC))
                .ForMember(dest => dest.NetworkType, act => act.MapFrom(src => NetworkType.BtcMainet));
            CreateMap<BtcTransactionResponse, Transaction>()
                .ForMember(dest => dest.Inputs, act => act.MapFrom(src => src.Vin))
                .ForMember(dest => dest.Outputs, act => act.MapFrom(src => src.Vout)); //TODO Sepreate Vin and vout destination object: so that automapper works and to indicate if from coin base
            CreateMap<BtcInputResponse, TxInput>()
                     .ForMember(dest => dest.IsNewlyGenerated, act => act.MapFrom(src => src.Coinbase != null))
                     .ForMember(dest => dest.Address, act => act.MapFrom(src => new Address() { Hash = src.Addresses.FirstOrDefault() })); //todo maybe change
            CreateMap<BtcOutputResponse, TxOutput>()
                    .ForMember(dest => dest.Address, act => act.MapFrom(src => new Address() { Hash = src.ScriptPubKey.Addresses[0] })); //todo maybe add a null check? or do mappers ignore null anyway
            CreateMap<BtcAddressResponse, Address>();
        }
    }
}
