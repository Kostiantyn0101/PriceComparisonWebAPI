using DLL.Context;
using Domain.Models.DBModels;
using Domain.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository
{
    //public class CategoryRepository : BaseRepository<CategoryDBModel>, IRepositoryUpdate<CategoryDBModel>
    //{
    //    public CategoryRepository(AppDbContext context) : base(context) { }

    //    public async Task<OperationDetailsResponseModel> UpdateAsync(int id, CategoryDBModel entityNew)
    //    {
    //        try
    //        {
    //            var entityOld = await context.Categories.FindAsync(id);
    //            if (entityOld == null)
    //                return new OperationDetailsResponseModel { IsError = true, Message = "Entity not found" };

    //            entityOld.Title = entityNew.Title;
    //            entityOld.ImageUrl = entityNew.ImageUrl;
    //            entityOld.ParentCategoryId = entityNew.ParentCategoryId;

    //            context.Entry(entityOld).State = EntityState.Modified;
    //            await context.SaveChangesAsync();
    //            return new OperationDetailsResponseModel { IsError = false, Message = "Entity Updated" };
    //        }
    //        catch (Exception ex)
    //        {
    //            return new OperationDetailsResponseModel { IsError = true, Message = "Update Error", Exception = ex };
    //        }
    //    }
    //}
}
