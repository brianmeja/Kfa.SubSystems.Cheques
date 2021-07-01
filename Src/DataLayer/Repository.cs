using Kfa.SubSystems.Cheques.Datalayer;
using Kfa.SubSystems.Cheques.Datalayer.Src.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kfa.SubSystems.Cheques.DataLayer
{
    public class Repository<T> : IRepository<T> where T:KfaData,new()
    {
        public async Task AddAsync(params T[] objects)
        {
            using var db = new ChequesContext();
            await db.Set<T>().AddRangeAsync(objects);
            await db.SaveChangesAsync();
          
        }

        public async Task DeleteAsync(params T[] objects)
        {
            using var db = new ChequesContext();
             db.Set<T>().RemoveRange(objects);
            await db.SaveChangesAsync();

        }

        public async Task<IEnumerable<T>> GetAsync(params long[] ids)
        {
            using var db = new ChequesContext();
            return await db.Set<T>().Where(x => ids.Contains(x.Id)).ToListAsync();
         
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            using var db = new ChequesContext();
            return await db.Set<T>().ToListAsync();

        }

        public async Task UpdateAsync(params T[] objects)
        {
            using var db = new ChequesContext();
            await db.SaveChangesAsync();
        }
    }
}
