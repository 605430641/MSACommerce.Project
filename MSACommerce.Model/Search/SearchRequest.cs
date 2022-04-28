using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSACommerce.Model.Search
{
	public class SearchRequest
	{
		public static readonly int DEFAULT_PAGE = 1;
		public static readonly int DEFAULT_SIZE = 20;
		public string key;
		public int page;
		//排序字段
		public string sortBy;
		//是否降序
		public bool descending;
		//过滤字段
		public Dictionary<string, string> filter=new Dictionary<string, string> ();

		public int getPage()
		{
			if (page == null)
			{
				return DEFAULT_PAGE;
			}
			// 获取页码时做一些校验，不能小于1
			return Math.Max(DEFAULT_PAGE, page);
		}

		public int getSize()
		{
			return DEFAULT_SIZE;
		}


	}
}
