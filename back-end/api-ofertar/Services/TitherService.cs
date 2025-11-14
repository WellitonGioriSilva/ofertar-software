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
    public class TitherService
    {
        private readonly DataBaseConfig _dbContext;

        public TitherService(DataBaseConfig dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Tither>> GetAllTithersAsync(int churchId)
        {
            return await _dbContext.Tithers
                .Include(t => t.Profession)
                .Include(t => t.Address)
                .Include(t => t.Church)
                .Where(t => t.ChurchId == churchId)
                .ToListAsync();
        }

        public async Task<Tither> GetTitherByIdAsync(int id)
        {
            var tither = await _dbContext.Tithers
                .Include(t => t.Profession)
                .Include(t => t.Address)
                .Include(t => t.Church)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tither == null)
                throw new KeyNotFoundException($"Tither with id {id} not found.");

            return tither;
        }

        public async Task<Tither> CreateTitherAsync(TitherCreateDTO titherDto, int churchId)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // Create address for main tither if provided (shared with spouse)
                Address? address = null;
                if (titherDto.Address != null)
                {
                    address = new Address
                    {
                        Street = titherDto.Address.Street,
                        Number = titherDto.Address.Number,
                        ZipCode = titherDto.Address.ZipCode,
                        Complement = titherDto.Address.Complement,
                        Neighborhood = titherDto.Address.Neighborhood
                    };
                    _dbContext.Addresses.Add(address);
                    await _dbContext.SaveChangesAsync();
                }

                // Create spouse (if provided) with same address as main tither
                Tither? spouse = null;
                if (titherDto.Spouse != null)
                {
                    spouse = new Tither
                    {
                        Name = titherDto.Spouse.Name,
                        Phone = titherDto.Spouse.Phone,
                        Email = titherDto.Spouse.Email,
                        BirthDate = titherDto.Spouse.BirthDate ?? DateTime.Now,
                        MaritalStatus = titherDto.Spouse.MaritalStatus,
                        IsActive = true,
                        Company = titherDto.Spouse.Company,
                        ProfessionId = titherDto.Spouse.ProfessionId,
                        AddressId = address?.Id,  // Same address as main tither
                        ChurchId = churchId
                    };

                    _dbContext.Tithers.Add(spouse);
                    await _dbContext.SaveChangesAsync();
                }

                // Create main tither and link spouse/address
                var tither = new Tither
                {
                    Name = titherDto.Name,
                    Phone = titherDto.Phone,
                    Email = titherDto.Email,
                    BirthDate = titherDto.BirthDate ?? DateTime.Now,
                    MaritalStatus = titherDto.MaritalStatus,
                    IsActive = true,
                    Company = titherDto.Company,
                    ProfessionId = titherDto.ProfessionId,
                    AddressId = address?.Id,
                    SpouseId = spouse?.Id,
                    ChurchId = churchId,
                };

                _dbContext.Tithers.Add(tither);
                await _dbContext.SaveChangesAsync();

                // If spouse exists, set the spouse's SpouseId back to this tither (two-way link)
                if (spouse != null)
                {
                    spouse.SpouseId = tither.Id;
                    _dbContext.Tithers.Update(spouse);
                    await _dbContext.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return tither;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Tither> UpdateTitherAsync(int id, TitherUpdateDTO titherDto, int churchId)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var existing = await GetTitherByIdAsync(id);

                existing.Name = titherDto.Name ?? existing.Name;
                existing.Phone = titherDto.Phone ?? existing.Phone;
                existing.Email = titherDto.Email ?? existing.Email;
                if (titherDto.BirthDate.HasValue)
                    existing.BirthDate = titherDto.BirthDate.Value;
                existing.MaritalStatus = titherDto.MaritalStatus;
                existing.Company = titherDto.Company ?? existing.Company;
                if (titherDto.ProfessionId.HasValue)
                    existing.ProfessionId = titherDto.ProfessionId.Value;
                existing.ChurchId = churchId;

                // Update or create address
                if (titherDto.Address != null)
                {
                    if (existing.AddressId.HasValue)
                    {
                        var addr = await _dbContext.Addresses.FindAsync(existing.AddressId.Value);
                        if (addr != null)
                        {
                            addr.Street = titherDto.Address.Street;
                            addr.Number = titherDto.Address.Number;
                            addr.ZipCode = titherDto.Address.ZipCode;
                            addr.Complement = titherDto.Address.Complement;
                            addr.Neighborhood = titherDto.Address.Neighborhood;
                            _dbContext.Addresses.Update(addr);
                        }
                    }
                    else
                    {
                        var newAddr = new Address
                        {
                            Street = titherDto.Address.Street,
                            Number = titherDto.Address.Number,
                            ZipCode = titherDto.Address.ZipCode,
                            Complement = titherDto.Address.Complement,
                            Neighborhood = titherDto.Address.Neighborhood
                        };
                        _dbContext.Addresses.Add(newAddr);
                        await _dbContext.SaveChangesAsync();
                        existing.AddressId = newAddr.Id;
                    }
                }

                // Update or create spouse
                if (titherDto.Spouse != null)
                {
                    if (existing.SpouseId.HasValue)
                    {
                        var spouse = await _dbContext.Tithers.FindAsync(existing.SpouseId.Value);
                        if (spouse != null)
                        {
                            spouse.Name = titherDto.Spouse.Name ?? spouse.Name;
                            spouse.Phone = titherDto.Spouse.Phone ?? spouse.Phone;
                            spouse.Email = titherDto.Spouse.Email ?? spouse.Email;
                            if (titherDto.Spouse.BirthDate.HasValue)
                                spouse.BirthDate = titherDto.Spouse.BirthDate.Value;
                            spouse.MaritalStatus = titherDto.Spouse.MaritalStatus;
                            spouse.Company = titherDto.Spouse.Company ?? spouse.Company;
                            spouse.ProfessionId = titherDto.Spouse.ProfessionId ?? spouse.ProfessionId;

                            // spouse address
                            if (titherDto.Spouse.Address != null)
                            {
                                if (spouse.AddressId.HasValue)
                                {
                                    var sAddr = await _dbContext.Addresses.FindAsync(spouse.AddressId.Value);
                                    if (sAddr != null)
                                    {
                                        sAddr.Street = titherDto.Spouse.Address.Street;
                                        sAddr.Number = titherDto.Spouse.Address.Number;
                                        sAddr.ZipCode = titherDto.Spouse.Address.ZipCode;
                                        sAddr.Complement = titherDto.Spouse.Address.Complement;
                                        sAddr.Neighborhood = titherDto.Spouse.Address.Neighborhood;
                                        _dbContext.Addresses.Update(sAddr);
                                    }
                                }
                                else
                                {
                                    spouse.AddressId = existing.AddressId;
                                }
                            }

                            _dbContext.Tithers.Update(spouse);
                        }
                    }
                    else
                    {
                        // create spouse
                        var newSpouse = new Tither
                        {
                            Name = titherDto.Spouse.Name,
                            Phone = titherDto.Spouse.Phone,
                            Email = titherDto.Spouse.Email,
                            BirthDate = titherDto.Spouse.BirthDate ?? DateTime.Now,
                            MaritalStatus = titherDto.Spouse.MaritalStatus,
                            IsActive = true,
                            Company = titherDto.Spouse.Company,
                            ProfessionId = titherDto.Spouse.ProfessionId,
                            AddressId = existing.AddressId,
                            ChurchId = churchId
                        };
                        _dbContext.Tithers.Add(newSpouse);
                        await _dbContext.SaveChangesAsync();

                        existing.SpouseId = newSpouse.Id;
                        newSpouse.SpouseId = existing.Id;
                        _dbContext.Tithers.Update(newSpouse);
                    }
                }

                _dbContext.Tithers.Update(existing);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return existing;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteTitherAsync(int id)
        {
            var tither = await GetTitherByIdAsync(id);

            _dbContext.Tithers.Remove(tither);
            await _dbContext.SaveChangesAsync();
        }
    }
}
