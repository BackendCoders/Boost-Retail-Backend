using Boost.Admin.Data;
using Boost.Admin.Data.Models;
using Boost.Admin.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boost.Admin.Logic.Interface
{
    public interface ISupplierFeedLogic
    {
        Task<SupplierFeed> AddAsync(SupplierFeedDto item);

        Task<int> UpdateAsync(int id, SupplierFeedDto updatedItem);

        Task<int> DeleteAsync(int id);

        Task<List<SupplierFeedDto>> GetAllSupplierFeed(PaginationDto req);

    }
}
    