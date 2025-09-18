using AutoMapper;
using Boost.Admin.Data;
using Boost.Admin.Logic.Interface;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boost.Admin.Logic.Implementiation
{
    public class SupplierLogic : ISupplierLogic
    {
        private readonly ILogger _logger;
        private readonly SimDbContext _db;
        private readonly IMapper _mapper;
        public SupplierLogic(SimDbContext db, IMapper mapper)
        {
            _db = db;
            _logger = Log.ForContext<SupplierLogic>();
            _mapper = mapper;
        }
    }
}
