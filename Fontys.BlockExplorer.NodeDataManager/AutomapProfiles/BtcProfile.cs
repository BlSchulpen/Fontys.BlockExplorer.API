using AutoMapper;
using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.Domain.Models;
using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BtcCore.Block;
using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BtcCore.RawTransaction;

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
            CreateMap<BtcBlockTxResponse, Transaction>()
                .ForMember(dest => dest.Hash, act => act.MapFrom(scr => scr.Hash));
            CreateMap<BtcInputResponse, TxInput>()
                     .ForMember(dest => dest.IsNewlyGenerated, act => act.MapFrom(src => src.Coinbase != null))
                     .ForMember(dest => dest.Address, act => act.MapFrom(src => new Address() { Hash = src.Addresses.FirstOrDefault() }));
            CreateMap<BtcOutputResponse, TxOutput>()
                    .ForMember(dest => dest.Address, act => act.MapFrom(src => new Address() { Hash = src.ScriptPubKey.Addresses[0] }));
            CreateMap<BtcAddressResponse, Address>();
        }
    }
}
