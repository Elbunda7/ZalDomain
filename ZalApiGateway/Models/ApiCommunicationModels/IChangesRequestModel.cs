using System;

namespace ZalApiGateway.Models.ApiCommunicationModels
{
    public interface IChangesRequestModel
    {
        int Count { get; set; }
        DateTime LastCheck { get; set; }
    }
}
