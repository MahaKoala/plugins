using System;

namespace SpotifySampler.Models
{
    public sealed class ResponseModel : EventArgs
    {
        public string Data { get; set; }
    }
}