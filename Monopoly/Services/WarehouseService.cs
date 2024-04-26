using Monopoly.Enum;
using Monopoly.Models;
using Monopoly.Repositories;

namespace Monopoly.Services;

public class WarehouseService(IBaseRepository<Box> boxRepository, IBaseRepository<Pallet> palletRepository)
    : IWarehouseService
{

    public async Task<Pallet> AddPallet(Pallet pallet)
    {
        var newPallet = new Pallet()
        {
            Id = Guid.NewGuid(),
            Width = pallet.Width,
            Height = pallet.Height,
            Depth = pallet.Depth,
            Weight = pallet.Weight,
            ExpirationDate = pallet.ExpirationDate,
            Volume = pallet.SelfVolume(),
        };
        await palletRepository.Add(newPallet);
        return await palletRepository.GetById(newPallet.Id);
    }

    public async Task<Box> AddBox(Box box)
    {
        var pallet = await palletRepository.GetById(box.PalletId);
        if (pallet.Width < box.Width || pallet.Depth < box.Depth)
            return new Box();
        
        if (box.ExpirationDate == null && box.ProductionDate == null)
            return new Box();

        if (box.ExpirationDate == null)
        {
            box.ExpirationDate = ((DateOnly)box.ProductionDate!).AddDays(100);
        }

        await boxRepository.Add(box);
        if (box != await boxRepository.GetById(box.Id))
            return new Box();
        
        var newPallet = new Pallet()
        {
            Id = pallet.Id,
            Width = pallet.Width,
            Height = pallet.Height,
            Depth = pallet.Depth,
            Weight = pallet.Weight + box.Weight,
            ExpirationDate = box.ExpirationDate < pallet.ExpirationDate || pallet.ExpirationDate == null ? box.ExpirationDate : pallet.ExpirationDate,
            Volume = pallet.Volume + box.SelfVolume(),
        };
        await palletRepository.Update(newPallet);
        return box;
    }

    public async Task<IEnumerable<Pallet>> GetPallets(List<SortByColumnPallet>? sortBy = null, int top = 0)
    {
        var listPallet = await palletRepository.GetAll();
        if (sortBy == null) return listPallet;
        foreach (var column in sortBy)
        {
            listPallet = column switch
            {
                SortByColumnPallet.Weight => listPallet.OrderBy(p => p.Weight),
                SortByColumnPallet.Volume => listPallet.OrderBy(p => p.Volume),
                SortByColumnPallet.ExpirationDate => listPallet.OrderBy(p => p.ExpirationDate),
                SortByColumnPallet.WeightDesc => listPallet.OrderByDescending(p => p.Weight),
                SortByColumnPallet.VolumeDesc => listPallet.OrderByDescending(p => p.Volume),
                SortByColumnPallet.ExpirationDateDesc => listPallet.OrderByDescending(p => p.ExpirationDate),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        if (top > 0)
        {
            return listPallet.Take(top);
        }

        return listPallet;
    }
}