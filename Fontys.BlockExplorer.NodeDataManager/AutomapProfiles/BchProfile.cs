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
            CreateMap<BchNodeBlock, Block>()
                .ForMember(dest => dest.Transactions, act => act.MapFrom(src => src.Tx))
                .ForMember(dest => dest.CoinType, act => act.MapFrom(src => CoinType.BTC))
                .ForMember(dest => dest.NetworkType, act => act.MapFrom(src => NetworkType.BtcMainet));
            CreateMap<BchNodeBlockTransaction, Transaction>()
                .ForMember(dest => dest.Hash, act => act.MapFrom(scr => scr.Hash));

            CreateMap<BchNodeRawTransaction, Transaction>()
                .ForMember(dest => dest.Hash, act => act.MapFrom(scr => scr.TxId))
                .ForMember(dest => dest.Outputs, act => act.MapFrom(scr => scr.Vout))
                .ForMember(dest => dest.Inputs, act => act.MapFrom(scr => scr.Vin));

            CreateMap<BchNodeVin, TxInput>()
                     .ForMember(dest => dest.IsNewlyGenerated, act => act.MapFrom(src => src.Vout == null));
                      // TODO only vouts have addresses so get original vout from N 
           
            CreateMap<BchNodeVout, TxOutput>()
                    .ForMember(dest => dest.Address, act => act.MapFrom(src => new Address() { Hash = src.Addresses[0].Address }));

            CreateMap<BchNodeAddress, Address>()
                .ForMember(dest => dest.Hash, act => act.MapFrom(src => src.Address));
        }
    }
}