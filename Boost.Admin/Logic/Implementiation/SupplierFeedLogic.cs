using AutoMapper;
using Azure.Core;
using Boost.Admin.Data;
using Boost.Admin.Data.Models;
using Boost.Admin.DTOs;
using Boost.Admin.Logic.Interface;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boost.Admin.Logic.Implementiation
{
    public class SupplierFeedLogic : ISupplierFeedLogic
    {
        private readonly ILogger _logger;
        private readonly SimDbContext _db;
        private readonly IMapper _mapper;

        public SupplierFeedLogic(SimDbContext db, IMapper mapper) 
        {
            _db = db;
            _logger = Log.ForContext<CategoryLogic>();
            _mapper = mapper;
        }

        public async Task<SupplierFeed> AddAsync(SupplierFeedDto item)
        {
            SupplierFeed newFeed = new SupplierFeed()
            {
                SupplierId = item.SupplierId,   
                FeedNameId = item.FeedNameId,
                FeedAddress = item.FeedAddress,
                APIKey = item.APIKey,
                UserName = item.UserName,
                Password = item.Password,
                IsActive = item.IsActive
            };

            _db.SupplierFeeds.Add(newFeed);
            await _db.SaveChangesAsync();

            return newFeed;
        }


        public async Task<int> UpdateAsync(int id, SupplierFeedDto updatedItem)
        {
            var existingItem = await _db.SupplierFeeds.FindAsync(id);

            if (existingItem == null) 
                throw new KeyNotFoundException();

            existingItem.SupplierId = updatedItem.SupplierId;
            existingItem.FeedNameId = updatedItem.FeedNameId;
            existingItem.FeedAddress = updatedItem.FeedAddress;
            existingItem.APIKey = updatedItem.APIKey;
            existingItem.UserName = updatedItem.UserName;
            existingItem.Password = updatedItem.Password;
            existingItem.IsActive = updatedItem.IsActive;

            var res = await _db.SaveChangesAsync();

            return res;
        }
              

        public async Task<int> DeleteAsync(int id)
        {
            var item = await _db.SupplierFeeds.FindAsync(id);

            if (item == null) 
                throw new KeyNotFoundException();

            _db.SupplierFeeds.Remove(item);
            var res = await _db.SaveChangesAsync();

            return res;
        }



        public async Task<List<SupplierFeedDto>> GetAllSupplierFeed(PaginationDto req)
        {
            var items = _db.SupplierFeeds.AsNoTracking();

            items =  items.Skip((req.PageNumber - 1) * req.PageSize)
                .Take(req.PageSize);

            var result = await items.Select(s => new SupplierFeedDto
            {
                Id = s.Id,     
                SupplierId = s.SupplierId,
                FeedNameId = s.FeedNameId,
                FeedAddress = s.FeedAddress,
                APIKey = s.APIKey,
                UserName = s.UserName,
                Password = s.Password,
                IsActive = s.IsActive
            }).ToListAsync();

            return result;
        }


    }
}
