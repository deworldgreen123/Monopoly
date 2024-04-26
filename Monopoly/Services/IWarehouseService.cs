using Monopoly.Enum;
using Monopoly.Models;

namespace Monopoly.Services;

public interface IWarehouseService
{
    Task<Pallet> AddPallet(Pallet pallet);
    Task<Box> AddBox(Box box);
    // Task<double> GetVolumePalletById(int id);
    // Task<double> GetWeightBoxById(int id);
    // Task<double> GetVolumeBoxById(int id);
    Task<IEnumerable<Pallet>> GetPallets(List<SortByColumnPallet>? sortBy, int top = 0);
}