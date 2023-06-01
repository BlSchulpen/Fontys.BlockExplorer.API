using AutoMapper;
using Fontys.BlockExplorer.Domain.Models;
using Fontys.BlockExplorer.NodeWarehouse.CoinResponseModels.EthGeth;

namespace Fontys.BlockExplorer.NodeDataManager.AutomapProfiles
{
    public class EthProfile : Profile
    {
        public EthProfile()
        {
            CreateMap<EthBlockResponse, Block>()
                .ForMember(dest => dest.Hash, act => act.MapFrom(src => src.Hash))
                .ForMember(dest => dest.PreviousBlockHash, act => act.MapFrom(src => src.ParentHash))
                .ForMember(dest => dest.Height, act => act.MapFrom(src => Convert.ToInt32(src.Number, 16)));

            CreateMap<EthTransactionResponse, Transaction>()
                .ForMember(dest => dest.Hash, act => act.MapFrom(src => src.Hash))
                .ForMember(dest => dest.Inputs, act => act.MapFrom(src => new TxInput()
                {
                    Address = new Address() { Hash = src.From },
                    Value = Convert.ToInt32(src.Value)
                }))
                .ForMember(dest => dest.Outputs, act => act.MapFrom(src => new TxOutput()
                {
                    Address = new Address() { Hash = src.To },
                    Value = Convert.ToInt32(src.Value)
                }));
        }
    }
}
