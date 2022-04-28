using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSACommerce.Model;
using MSACommerce.Model;
using MSACommerce.Model.Search;

namespace MSACommerce.Interface
{
	public interface ISearchService
	{
		SearchResult<Goods> search(SearchRequest searchRequest);
		public void ImpDataBySpu();
		public SearchResult<Goods> GetData(SearchRequest  searchRequest);
	}
}
