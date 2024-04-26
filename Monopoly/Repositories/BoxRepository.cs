using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Monopoly.Models;

namespace Monopoly.Repositories;

public class BoxRepository(WarehouseContext context) : IBaseRepository<Box>, IDisposable
{
    public async Task<IEnumerable<Box>> GetAll()
    {
        return await context.Boxes.ToListAsync();
    }

    public async Task<IEnumerable<Box>> GetList(Expression<Func<Box, bool>> predicate)
    {
        return await context.Boxes.Where(predicate).ToListAsync();
    }

    public async Task<Box> GetById(Guid id)
    {
        return await context.Boxes.FirstOrDefaultAsync(a => a.Id == id) ?? throw new InvalidOperationException();
    }

    public async Task Add(Box box)
    {
        if (box == null) throw new ArgumentNullException(nameof(box));
        await context.Boxes.AddAsync(box);
        await Save();
    }

    public async Task Update(Box box)
    {
        if (box == null) throw new ArgumentNullException(nameof(box));
        context.Entry(box).State = EntityState.Modified;
        await Save();
    }

    public async Task<bool> Remove(Guid id)
    {
        try
        {
            var box = GetById(id);
            context.Entry(box).State = EntityState.Modified;
            await Save();
            return true;
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e); // должен быть logger
            return false;
        }
    }

    public async Task Save()
    {
        await context.SaveChangesAsync();
    }
    
    private bool _disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
        _disposed = true;
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}