using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PickUp.Common.Domain.BaseModels;

namespace PickUp.Common.Infrastructure.Persistance
{
    public class ChangeTrackingEntityInterceptor : ISaveChangesInterceptor
    {
        public virtual InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if (eventData.Context == null)
                return result;

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntityWithChangeTracking addedEntity && entry.State == EntityState.Added)
                {
                    addedEntity.CreatedOn = DateTime.UtcNow;
                    addedEntity.UpdatedOn = DateTime.UtcNow;
                }

                if (entry.Entity is BaseEntityWithChangeTracking modifiedEntity && entry.State == EntityState.Modified)
                {
                    modifiedEntity.UpdatedOn = DateTime.UtcNow;
                }
            }

            return result;
        }
    }
}
