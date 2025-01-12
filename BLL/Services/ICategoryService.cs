using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.DBModels;

namespace BLL.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDBModel>> GetAllCategoriesAsync();
    }
}
