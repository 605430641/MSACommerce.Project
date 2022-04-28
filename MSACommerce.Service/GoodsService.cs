using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSACommerce.Model;
using MSACommerce.Interface;
using MSACommerce.Model;
using MSACommerce.Model.DTO;

namespace MSACommerce.Service
{
	public class GoodsService : IGoodsService
	{
		private OrangeContext _orangeContext;
		public GoodsService(OrangeContext orangeContext)
		{
			_orangeContext = orangeContext;
		}

		public void AddGoods(TbSpu spu)
		{
			throw new NotImplementedException();
		}
		public void AddSeckillGoods(SeckillParameter seckillParameter)
		{
			throw new NotImplementedException();
		}

		//todo
		public void DecreaseStock(List<CartDto> cartDtos)
		{
			foreach (CartDto cartDto in cartDtos)
			{
				//
				int count = _orangeContext.Database.ExecuteSqlRaw($"update tb_stock set stock = stock - {cartDto.num} where sku_id = {cartDto.skuId} and stock >= {cartDto.num}");
				//.decreaseStock(cartDto.getSkuId(), cartDto.getNum());
				if (count != 1)
				{
					throw new Exception("扣减库存失败");
				}
			}
		}

		public void DeleteGoodsBySpuId(long spuId)
		{
			throw new NotImplementedException();
		}

		public void HandleSaleable(TbSpu spu)
		{
			throw new NotImplementedException();
		}

		public List<SeckillGoods> QuerySeckillGoods()
		{
			var list = _orangeContext.TbSeckillSku.AsParallel();
			// 可以秒杀 
			list = list.Where(m => m.Enable == true);
			List<TbSeckillSku> tbSeckillSkus = list.ToList();
			List<SeckillGoods> seckillGoods = new List<SeckillGoods>();
			foreach (var item in tbSeckillSkus)
			{
				var stock = _orangeContext.TbStock.Where(m => m.SkuId == item.SkuId).FirstOrDefault();
				SeckillGoods goods = new SeckillGoods();
				goods.CurrentTime = item.EndTime;
				goods.Enable = item.Enable;
				goods.EndTime = item.EndTime;
				goods.Id = item.Id;
				goods.Image = item.Image;
				goods.SeckillPrice = item.SeckillPrice;
				goods.SkuId = item.SkuId;
				goods.StartTime = item.StartTime;
				goods.Stock = stock.SeckillStock;
				goods.Title = item.Title;
				goods.SeckillTotal = stock.SeckillTotal;

			}
			return seckillGoods;
		}

		public List<TbSku> QuerySkuBySpuId(long spuId)
		{
			TbSku sku = new TbSku();
			List<TbSku> skuList = _orangeContext.TbSku.Where(m => m.SpuId == spuId).ToList();
			if (skuList.Count <= 0)
			{
				throw new Exception("查询的商品的SKU失败");
			}
			//查询库存
			foreach (TbSku sku1 in skuList)
			{
				sku1.Stock = _orangeContext.TbStock.Where(m => m.SkuId == sku1.Id).FirstOrDefault().Stock;
			}
			return skuList;
		}

		public List<TbSku> QuerySkusByIds(List<long> ids)
		{
			List<TbSku> skus = _orangeContext.TbSku.Where(m => ids.Contains(m.Id)).ToList();
			if (skus.Count <= 0)
			{
				throw new Exception("查询");
			}
			//填充库存
			FillStock(ids, skus);
			return skus;
		}

		private void FillStock(List<long> ids, List<TbSku> skus)
		{
			//批量查询库存
			List<TbStock> stocks = _orangeContext.TbStock.Where(m => ids.Contains(m.SkuId)).ToList();
			if (stocks.Count <= 0)
			{
				throw new Exception("保存库存失败");
			}
			Dictionary<long, int> map = stocks.ToDictionary(s => s.SkuId, s => s.Stock);
			//首先将库存转换为map，key为sku的ID
			//遍历skus，并填充库存
			foreach (var sku in skus)
			{
				sku.Stock = map[sku.Id];
			}
		}


		public PageResult<TbSpu> QuerySpuByPage(int page, int rows, string key, bool? saleable)
		{
			var list = _orangeContext.TbSpu.AsQueryable();
			if (!string.IsNullOrEmpty(key))
			{
				list = list.Where(m => m.Title.Contains(key));
			}
			if (saleable != null)
			{
				list = list.Where(m => m.Saleable == saleable);
			}
			//默认以上一次更新时间排序
			list = list.OrderByDescending(m => m.LastUpdateTime);

			//只查询未删除的商品 
			list = list.Where(m => m.Valid == true);

			//查询
			List<TbSpu> spuList = list.ToList();

			if (spuList.Count <= 0)
			{
				throw new Exception("查询的商品不存在");
			}
			//对查询结果中的分类名和品牌名进行处理
			HandleCategoryAndBrand(spuList);
			return new PageResult<TbSpu>(spuList.Count, spuList);
		}

		/**
		 * 处理商品分类名和品牌名
		 *
		 * @param spuList
		 */
		private void HandleCategoryAndBrand(List<TbSpu> spuList)
		{
			foreach (TbSpu spu in spuList)
			{
				//根据spu中的分类ids查询分类名
				var ids = new List<string>() { spu.Cid1.ToString(), spu.Cid2.ToString(), spu.Cid3.ToString() };
				List<string> nameList = _orangeContext.TbCategory.Where(m => m.Id.ToString().Contains(m.Id.ToString())).Select(m => m.Name).ToList();
				//对分类名进行处理
				spu.Cname = string.Join('/', nameList);
				//查询品牌
				spu.Bname = _orangeContext.TbBrand.Where(m => m.Id == spu.BrandId).FirstOrDefault()?.Name;
			}
		}
		public TbSpu QuerySpuBySpuId(long spuId)
		{
			//根据spuId查询spu
			TbSpu spu = _orangeContext.TbSpu.Where(m => m.Id == spuId).FirstOrDefault();
			//查询spuDetail
			TbSpuDetail detail = QuerySpuDetailBySpuId(spuId);
			//查询skus
			List<TbSku> skus = QuerySkuBySpuId(spuId);
			spu.SpuDetail = detail;
			spu.Skus = skus;

			return spu;
		}

		public TbSpuDetail QuerySpuDetailBySpuId(long spuId)
		{
			TbSpuDetail spuDetail = _orangeContext.TbSpuDetail.Where(m => m.SpuId == spuId).FirstOrDefault();
			if (spuDetail == null)
			{
				throw new Exception("查询的商品不存在");
			}
			return spuDetail;
		}

		public void UpdateGoods(TbSpu spu)
		{
			throw new NotImplementedException();
		}
	}
}
