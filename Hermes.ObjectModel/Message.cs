using System;

namespace Hermes.ObjectModel
{
    public class Message : BaseEntity
    {
        public string User { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
    }
}