using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T>: IRepository<T> where T: BaseEntity
    {
        protected IEnumerable<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data;
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public Task DeleteByIdAsync(Guid id)
        {
            Data = Data.Where(d => d.Id != id);

            return Task.CompletedTask;
        }

        public Task InsertItemData(T item)
        {
            item.Id = Guid.NewGuid();
            Data = Data.Append(item);
            return Task.CompletedTask;
        }

        public Task UpdateItemData(T item)
        {
            var existingItem = Data.FirstOrDefault(x => x.Id == item.Id);
            if (existingItem == null)
            {
                return Task.FromException(new InvalidOperationException($"Object with Id {item.Id} not found"));
            }

            var newData = Data.Where(d => d.Id != item.Id);
            newData = newData.Append(item);
            Data = newData;
            return Task.CompletedTask;
        }
    }
}