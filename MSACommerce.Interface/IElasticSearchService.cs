using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSACommerce.Interface
{
    public interface IElasticSearchService
    {

        public ElasticClient GetElasticClient();
        public void Send<T>(List<T> model) where T : class;
    }
}
