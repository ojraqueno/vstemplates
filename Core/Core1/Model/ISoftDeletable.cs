using System;

namespace Core1.Model
{
    public interface ISoftDeletable
    {
        DateTime? DeletedOn { get; set; }
    }
}