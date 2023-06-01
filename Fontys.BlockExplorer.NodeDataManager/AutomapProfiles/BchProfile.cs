using AutoMapper;
using Fontys.BlockExplorer.Domain.Enums;
using Fontys.BlockExplorer.Domain.Models;
using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BchNode.Block;
using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.BchNode.RawTransaction;

namespace Fontys.BlockExplorer.NodeDataManager.AutomapProfiles
{
    public class BchProfile : Profile
    {
        public BchProfile()
        {
            CreateMap<BchBlockResponse, Block>()
                .ForMember(dest => dest.Transactions, act => act.MapFrom(src => src.Tx))
                .ForMember(dest => dest.CoinType, act => act.MapFrom(src => CoinType.BTC))
                .ForMember(dest => dest.NetworkType, act => act.MapFrom(src => NetworkType.BtcMainet));
            CreateMap<BchBlockTxResponse, Transaction>()
                .ForMember(dest => dest.Hash, act => act.MapFrom(scr => scr.Hash));

            CreateMap<BchRawTransactionResponse, Transaction>()
                .ForMember(dest => dest.Hash, act => act.MapFrom(scr => scr.TxId))
                .ForMember(dest => dest.Outputs, act => act.MapFrom(scr => scr.Vout))
                .ForMember(dest => dest.Inputs, act => act.MapFrom(scr => scr.Vin));

            CreateMap<BchVinResponse, TxInput>()
                     .ForMember(dest => dest.IsNewlyGenerated, act => act.MapFrom(src => src.Vout == null));

            CreateMap<BchVoutResponse, TxOutput>()
                    .ForMember(dest => dest.Address, act => act.MapFrom(src => new Address() { Hash = src.Addresses[0].Address }));

            CreateMap<BchAddressResponse, Address>()
                .ForMember(dest => dest.Hash, act => act.MapFrom(src => src.Address));
        }
    }
}