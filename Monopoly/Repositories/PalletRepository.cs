using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Monopoly.Models;

namespace Monopoly.Repositories;

public class PalletRepository(WarehouseContext context) : IBaseRepository<Pallet>, IDisposable
{
    public async Task<IEnumerable<Pallet>> GetAll()
    {
        return await context.Pallets.ToListAsync();
    }

    public async Task<IEnumerable<Pallet>> GetList(Expression<Func<Pallet, bool>> predicate)
    {
        return await context.Pallets.Where(predicate).ToListAsync();
    }

    public async Task<Pallet> GetById(Guid id)
    {
        return await context.Pallets.FirstOrDefaultAsync(a => a.Id == id) ?? throw new InvalidOperationException();
    }

    public async Task Add(Pallet pallet)
    {
        if (pallet == null) throw new ArgumentNullException(nameof(pallet));
        await context.Pallets.AddAsync(pallet);
        await Save();
    }

    public async Task Update(Pallet pallet)
    {
        if (pallet == null) throw new ArgumentNullException(nameof(pallet));
        var oldPallet = await GetById(pallet.Id);
        oldPallet.ExpirationDate = pallet.ExpirationDate;
        oldPallet.Width = pallet.Width;
        oldPallet.Height = pallet.Height;
        oldPallet.Depth = pallet.Depth;
        oldPallet.Volume = pallet.Volume;
        oldPallet.Weight = pallet.Weight;
        context.Pallets.Update(oldPallet);
        await Save();
    }

    public async Task<bool> Remove(Guid id)
    {
        try
        {
            var pallet = await GetById(id);
            context.Pallets.Remove(pallet);
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