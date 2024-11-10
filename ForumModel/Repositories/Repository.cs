using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ForumModel.Context;
using ForumModel.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ForumModel.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected ApplicationDbContext _context;
        public Repository(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<IEnumerable<T>> FindAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression).ToList();
        }
        public async Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }
        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public async Task SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return;
            }catch
            {
                throw new Exception("Error al guardar los cambios.");
            }
        }
        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
