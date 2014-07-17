
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain
{
    public class BlackPhone : EntityBase
    {
        public Int32 Id { get; set; }
        public String Phone { get; set; }
    }
}
