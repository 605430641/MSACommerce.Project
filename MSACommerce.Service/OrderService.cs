using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSACommerce.Model;
using MSACommerce.Interface;
using MSACommerce.Model;
using MSACommerce.Model.DTO;
using MSACommerce.Core;

namespace MSACommerce.Service
{
	public class OrderService : IOrderService
	{
		private IGoodsService _goodsService;
		private OrangeContext _orangeContext;
		public OrderService(IGoodsService goodsService, OrangeContext orangeContext)
		{
			_goodsService = goodsService;
			_orangeContext = orangeContext;

		}
		public long CreateOrder(OrderDto orderDto, UserInfo user)
		{
			//生成订单ID，采用自己的算法生成订单ID
			long orderId = SnowflakeHelper.Next();
			//填充order
			TbOrder order = new TbOrder();
			order.CreateTime = DateTime.Now;
			order.OrderId = orderId;
			order.PaymentType = orderDto.paymentType;
			order.PostFee = 0L; // TODO调用物流信息，根据地址计算邮费
								//设置用户信息
			order.UserId = user.id.ToString();
			order.BuyerNick = user.username;
			order.BuyerRate = false; //TODO 卖家为留言
									 //收货人地址信息，应该从数据库中物流信息中获取，这里使用的是假的数据
			AddressDTO addressDTO = AddressClient.FindById(orderDto.addressId);
			if (addressDTO == null)
			{
				// 商品不存在，抛出异常
				throw new Exception("收货地址不存在");
			}
			order.Receiver = addressDTO.name;
			order.ReceiverAddress = addressDTO.address;
			order.ReceiverCity = addressDTO.city;
			order.ReceiverDistrict = addressDTO.district;
			order.ReceiverMobile = addressDTO.phone;
			order.ReceiverZip = addressDTO.zipCode;
			order.ReceiverState = addressDTO.state;
			//付款金额相关，首先把orderDto转化成map，其中key为skuId,值为购物车中该sku的购买数量
			Dictionary<long, int> skuNumMap = orderDto.carts.ToDictionary(m => m.skuId, m => m.num);
			//查询商品信息，根据skuIds批量查询sku详情
			List<TbSku> skus = _goodsService.QuerySkusByIds(skuNumMap.Keys.ToList());
			if (skus.Count <= 0)
			{
				throw new Exception("查询的商品信息不存在");
			}
			Double totalPay = 0.0;
			//填充orderDetail
			List<TbOrderDetail> orderDetails = new List<TbOrderDetail>();
			//遍历skus，填充orderDetail
			foreach (TbSku sku in skus)
			{
				// 获取购买商品数量
				int num = skuNumMap[sku.Id];
				// 计算金额
				totalPay += num * sku.Price;
				TbOrderDetail orderDetail = new TbOrderDetail();
				orderDetail.OrderId = orderId;
				orderDetail.OwnSpec = sku.OwnSpec;
				orderDetail.SkuId = sku.Id;
				orderDetail.Title = sku.Title;
				orderDetail.Num = num;
				orderDetail.Price = sku.Price;
				// 获取商品展示第一张图片
				orderDetail.Image = sku.Images.Split(',')[0];
				orderDetails.Add(orderDetail);
			}
			order.ActualPay = (long)(totalPay + order.PostFee);  //todo 还要减去优惠金额
			order.TotalPay = (long)totalPay;
			//保存order
			_orangeContext.TbOrder.Add(order);

			//保存detail
			_orangeContext.TbOrderDetail.AddRange(orderDetails);
			//填充orderStatus
			TbOrderStatus orderStatus = new TbOrderStatus();
			orderStatus.OrderId = orderId;
			orderStatus.Status = (int)OrderStatusEnum.INIT;
			orderStatus.CreateTime = DateTime.Now;

			//保存orderStatus
			_orangeContext.TbOrderStatus.AddRange(orderStatus);

			//减库存（1、下订单减库存，2、支付完成后减库存）
			// TODO 需要处理强一致分布式事务
			//goodsClient.decreaseStock(orderDto.getCarts());
			_goodsService.DecreaseStock(orderDto.carts);
			/// 模拟操作失败
			// throw new RuntimeException("模拟操作失败");
			//删除购物车中已经下单的商品数据, 采用异步mq的方式通知购物车系统删除已购买的商品，传送商品ID和用户ID
			Dictionary<string, object> map = new Dictionary<string, object>();
			try
			{
				map.Add("skuIds", skuNumMap.Keys);
				map.Add("userId", user.id);
				//amqpTemplate.convertAndSend("yt.cart.exchange", "cart.delete", JsonUtils.toString(map));
			}
			catch (Exception e)
			{
				//log.error("删除购物车消息发送异常，商品ID：{}", skuNumMap.keySet(), e);
				throw new Exception($"删除购物车消息发送异常，商品ID：{string.Join(',', skuNumMap.Keys.ToArray())}");
			}

			//	log.info("生成订单，订单编号：{}，用户id：{}", orderId, user.getId());
			var isok = _orangeContext.SaveChanges() > 0;
			return orderId;
		}

		public string GenerateUrl(long orderId, UserInfo user)
		{
			throw new NotImplementedException();
		}

		public void HandleNotify(Dictionary<string, string> msg)
		{
			throw new NotImplementedException();
		}

		public TbOrder QueryById(long orderId)
		{
			TbOrder order = _orangeContext.TbOrder.Where(m => m.OrderId == orderId).FirstOrDefault();
			if (order == null)
			{
				throw new Exception("订单不存在");
			}
			List<TbOrderDetail> orderDetails = _orangeContext.TbOrderDetail.Where(m => m.OrderId == orderId).ToList();
			order.orderDetails = orderDetails;
			TbOrderStatus orderStatus = _orangeContext.TbOrderStatus.Where(m => m.OrderId == orderId).FirstOrDefault();
			order.orderStatus = orderStatus;
			return order;
		}

		public PageResult<TbOrder> QueryOrderByPage(int page, int rows)
		{
			//查询订单
			List<TbOrder> orders = _orangeContext.TbOrder.ToList(); ;
			foreach (var item in orders)
			{
				List<TbOrderDetail> orderDetails = _orangeContext.TbOrderDetail.Where(m => m.OrderId == item.OrderId).ToList();
				item.orderDetails = orderDetails;
				TbOrderStatus orderStatus = _orangeContext.TbOrderStatus.Where(m => m.OrderId == item.OrderId).FirstOrDefault();
				item.orderStatus = orderStatus;
			}
			return new PageResult<TbOrder>(orders.Count, orders);
		}

		public int QueryOrderStateByOrderId(long orderId)
		{
			throw new NotImplementedException();
		}
	}
}
