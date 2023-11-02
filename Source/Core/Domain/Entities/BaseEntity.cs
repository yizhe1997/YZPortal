﻿namespace Domain.Entities
{
    public abstract class BaseEntity<TId> : IEntity<TId>
    {
        public TId Id { get; set; }
    }
}
