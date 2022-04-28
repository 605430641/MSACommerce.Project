 8070：前端网站
 "Url": "server=172.56.33.135;port=3306;database=liuzexi;user id=sa;password=idea@1234"

 "RedisConn": {
    "Host": "192.168.3.254",
    "Prot": 6379,
    "DB": 0
  },

内部服务之间不走网关

启动网关
dotnet run --urls="http://*:6299" --ip="127.0.0.1" --port=6299

启动鉴权中心
dotnet run --urls="http://*:7200" --ip="127.0.0.1" --port=7200

启动用户服务
dotnet run --urls="http://*:5726" --ip="127.0.0.1" --port=5726





