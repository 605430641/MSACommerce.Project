using System;
using System.Collections.Generic;
using System.Text;
using MSACommerce.Model;
using MSACommerce.Model;
using MSACommerce.Model.DTO;

namespace MSACommerce.Interface
{
	public interface ISpecService
	{

		public List<TbSpecGroup> QuerySpecGroupByCid(long cid);

		public List<TbSpecParam> QuerySpecParams(long? gid, long? cid, bool? searching, bool? generic);

		public List<TbSpecGroup> QuerySpecsByCid(long cid);

		void SaveSpecGroup(TbSpecGroup specGroup);

		void DeleteSpecGroup(long id);

		void UpdateSpecGroup(TbSpecGroup specGroup);

		void SaveSpecParam(TbSpecParam specParam);

		void UpdateSpecParam(TbSpecParam specParam);

		void DeleteSpecParam(long id);


	}
}
