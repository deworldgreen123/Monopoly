using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Monopoly.Enum;
using Monopoly.Models;
using Monopoly.Repositories;
using Monopoly.Services;

await using var context = new WarehouseContext();
var service = new WarehouseService(new BoxRepository(context), new PalletRepository(context));

if (args.Length != 0 && args[0] == "Gen")
{
    
    for (var i = 1; i < 6; i++)
    {
        var pallet = new Pallet()
        {
            Id = Guid.NewGuid(),
            Width = i,
            Height = i,
            Depth = i,
            Weight = i,
            ExpirationDate = null,
            Volume = i * i * i,
        };
        pallet = await service.AddPallet(pallet);
        for (var j = 0; j < i; j++)
        {
            var box = new Box()
            {
                Id = Guid.NewGuid(),
                Width = i,
                Height = i,
                Depth = i,
                Weight = i,
                ProductionDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-i)),
                ExpirationDate = null,
                PalletId = pallet.Id
            };
            await service.AddBox(box);
        }

        if (i == 3)
        {
            var pallet3 = new Pallet()
            {
                Id = Guid.NewGuid(),
                Width = i,
                Height = i,
                Depth = i,
                Weight = i,
                ExpirationDate = null,
                Volume = i * i * i,
            };
            pallet3 = await service.AddPallet(pallet3);
            for (var j = 0; j < i; j++)
            {
                var box = new Box()
                {
                    Id = Guid.NewGuid(),
                    Width = i,
                    Height = i,
                    Depth = i,
                    Weight = i,
                    ProductionDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-i+1)),
                    ExpirationDate = null,
                    PalletId = pallet3.Id
                };
                await service.AddBox(box);
            }
        }
    }
}
else
{
    var res1 = await service.GetPallets(new List<SortByColumnPallet>() { SortByColumnPallet.Weight , SortByColumnPallet.ExpirationDate});
    foreach (var pallet in res1)
    {
        Console.WriteLine(pallet.Id + "\t" + pallet.Volume + "\t" + pallet.Weight + "\t" + pallet.ExpirationDate);
    }
    Console.WriteLine();
    
    
    var res3 = await service.GetPallets(new List<SortByColumnPallet>() { SortByColumnPallet.Volume , SortByColumnPallet.ExpirationDateDesc}, 3);
    foreach (var pallet in res3)
    {
        Console.WriteLine(pallet.Id + "\t" + pallet.Volume + "\t" + pallet.Weight + "\t" + pallet.ExpirationDate);
    }
    Console.WriteLine();
}