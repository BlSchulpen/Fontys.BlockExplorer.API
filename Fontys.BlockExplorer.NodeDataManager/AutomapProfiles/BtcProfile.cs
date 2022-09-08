﻿using AutoMapper;
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
                .ForMember(dest => dest.Inputs, act => act.MapFrom(src => src.Vin))
                .ForMember(dest => dest.Outputs, act => act.MapFrom(src => src.Vout)); //TODO Sepreate Vin and vout destination object: so that automapper works and to indicate if from coin base
            CreateMap<BtcInputResponse, TxInput>()
                     .ForMember(dest => dest.IsNewlyGenerated, act => act.MapFrom(src => src.Coinbase != null));
            CreateMap<BtcOutputResponse, TxOutput>();
            CreateMap<BtcAddressResponse, Address>();
        }
    }
}
