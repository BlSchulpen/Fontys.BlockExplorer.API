﻿namespace Fontys.BlockExplorer.Domain.CoinResponseModels.BtcCore
{
    public class BtcInputResponse
    {
        public string TxId { get; set; }
        public int Vout { get; set; }//todo readup on how vin transactions work
        public string? Coinbase { get; set; }

        //========================================
        //TODO should be removed from response --> never expect Value and addres has to be set manually, consider adding a new DTO
        public long? Value { get; set; }
        public BtcAddressResponse? Address { get; set; }

    }
}
