using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgileFramework.Common.IOCOptions;
using MSACommerce.Interface;

namespace MSACommerce.Service
{
	public class ElasticSearchService: IElasticSearchService
	{
		private readonly IOptionsMonitor<ElasticSearchOptions> _elasticSearchOptions;

		public ElasticSearchService(IOptionsMonitor<ElasticSearchOptions> optionsMonitor)
		{
			_elasticSearchOptions = optionsMonitor;
			var settings = new ConnectionSettings(new Uri(_elasticSearchOptions.CurrentValue.Url))
	.DefaultIndex("spu");
			Client = new ElasticClient(settings);
		}
		private ElasticClient Client;
		public ElasticClient GetElasticClient()
		{
			return Client;
		}
		public void Send<T>(List<T> model) where T : class
		{
			Client.IndexMany(model);
		}
	}
}
