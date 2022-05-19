using System;

namespace Hermes.Models
{
    public class Message : BaseModel
    {
        public string User { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
    }
}