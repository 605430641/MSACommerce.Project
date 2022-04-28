using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSACommerce.Model
{
	public class SeckillParameter
	{ /**
     * 要秒杀的sku id
     */
		private long id;

		/**
         * 秒杀开始时间
         */
		private string startTime;

		/**
         * 秒杀结束时间
         */
		private string endTime;

		/**
         * 参与秒杀的商品数量
         */
		private int count;

		/**
         * 折扣
         */
		private double discount;
	}
}
