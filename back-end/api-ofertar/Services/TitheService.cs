using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_ofertar.DTOs;
using api_ofertar.Data;
using api_ofertar.Entities;
using Microsoft.EntityFrameworkCore;

namespace api_ofertar.Services
{
    public class TitheService
    {
        private readonly DataBaseConfig _dbContext;

        public TitheService(DataBaseConfig dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateTitheAsync(TitheCreateDTO titheDto)
        {
            var tithe = new Tithe
            {
                Amount = titheDto.Amount,
                OfferingDate = titheDto.OfferingDate,
                TitherId = titheDto.TitherId
            };

            _dbContext.Tithes.Add(tithe);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Tithe> GetTitheByIdAsync(int id, int churchId)
        {
            var tithe = await _dbContext.Tithes
                .Include(t => t.Tither)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id && t.Tither.ChurchId == churchId);

            if (tithe == null)
                throw new KeyNotFoundException($"Tithe with id {id} not found.");

            return tithe;
        }

        public async Task UpdateTitheAsync(int id, TitheUpdateDTO titheDto, int churchId)
        {
            var tithe = await GetTitheByIdAsync(id, churchId);  

            tithe.Amount = titheDto.Amount;
            tithe.OfferingDate = titheDto.OfferingDate;
            tithe.TitherId = titheDto.TitherId;

            _dbContext.Tithes.Update(tithe);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteTitheAsync(int id, int churchId)
        {
            var tithe = await GetTitheByIdAsync(id, churchId);

            _dbContext.Tithes.Remove(tithe);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Tithe>> GetAllTithesAsync(int churchId)
        {
            return await _dbContext.Tithes
                .Include(t => t.Tither)
                .Include(t => t.User)
                .Where(t => t.Tither.ChurchId == churchId)
                .ToListAsync();
        }
    }
}