using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSACommerce.Model;
using MSACommerce.Model;
using MSACommerce.Model.DTO;

namespace MSACommerce.Interface
{
	public interface IOrderService
	{
		public const string KEY_PAY_PREFIX = "order:pay:url:";

		/**
         * 创建订单
         * @param orderDto
         * @return
         */
		long CreateOrder(OrderDto orderDto, UserInfo user);

		/**
         * 生成支付链接
         * @param orderId
         * @return
         */
		string GenerateUrl(long orderId, UserInfo user);

		/**
         * 根据订单号，查询订单信息
         * @param orderId
         * @return
         */
		TbOrder QueryById(long orderId);

		/**
         * 根据订单编号查询订单状态码
         * @param orderId
         * @return
         */
		int QueryOrderStateByOrderId(long orderId);

		/**
         * 处理回到通知
         * @param msg
         */
		void HandleNotify(Dictionary<string, string> msg);

		/**
         * 分页查询订单信息
         * @param page
         * @param rows
         * @return
         */
		PageResult<TbOrder> QueryOrderByPage(int page, int rows);
	}
}
