using System.Collections.Generic;
using MSACommerce.Model;
using MSACommerce.Model.DTO;

namespace MSACommerce.Interface
{
    public interface IBrandService
    {
        PageResult<TbBrand> QueryBrandByPageAndSort(int page, int rows, string sortBy, bool desc, string key);

        void SaveBrand(TbBrand brand, List<long> cids);

        List<TbCategory> QueryCategoryByBid(long bid);

        void UpdateBrand(BrandBo brandbo);

        void DeleteBrand(long bid);

        List<TbBrand> QueryBrandByCid(long cid);

        TbBrand QueryBrandByBid(long id);

        List<TbBrand> QueryBrandByIds(List<long> ids);

    }
}
