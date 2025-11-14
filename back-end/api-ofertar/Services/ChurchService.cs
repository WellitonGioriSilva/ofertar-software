using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_ofertar.Data;
using api_ofertar.DTOs;
using api_ofertar.Entities;
using Microsoft.EntityFrameworkCore;

namespace api_ofertar.Services
{
    public class ChurchService
    {
        private readonly DataBaseConfig _dbContext;

        public ChurchService(DataBaseConfig dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Church>> GetAllChurchesAsync()
        {
            var churches = await _dbContext.Churches.ToListAsync();
            return churches;
        }

        public async Task<Church> GetChurchByIdAsync(int id)
        {
            var church = await _dbContext.Churches.FirstOrDefaultAsync(c => c.Id == id);
            if (church == null)
                throw new KeyNotFoundException($"Church with id {id} not found.");

            return church;
        }

        public async Task<Church> CreateChurchAsync(ChurchCreateDTO churchDto)
        {
            var church = new Church
            {
                Name = churchDto.Name,
                IsActive = true
            };
            
            _dbContext.Churches.Add(church);
            await _dbContext.SaveChangesAsync();
            
            return church;
        }

        public async Task<Church> UpdateChurchAsync(int id, ChurchUpdateDTO churchDto)
        {
            var church = await GetChurchByIdAsync(id);
            
            church.Name = churchDto.Name;
            _dbContext.Churches.Update(church);
            await _dbContext.SaveChangesAsync();
            
            return church;
        }

        public async Task DeleteChurchAsync(int id)
        {
            var church = await GetChurchByIdAsync(id);
            _dbContext.Churches.Remove(church);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Church> UpdateChurchIsActiveAsync(int id, bool isActive)
        {
            var church = await GetChurchByIdAsync(id);
            
            church.IsActive = isActive;
            _dbContext.Churches.Update(church);
            await _dbContext.SaveChangesAsync();
            
            return church;
        }

        public async Task<bool> IsChurchActiveAsync(int id)
        {
            var church = await GetChurchByIdAsync(id);
            return church.IsActive;
        }
    }
}
