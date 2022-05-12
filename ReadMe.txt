数据库链接

 "Url": "server=172.56.33.135;port=3306;database=liuzexi;user id=sa;password=idea@1234"
redis链接

 "RedisConn": {
    "Host": "192.168.3.254",
    "Prot": 6379,
    "DB": 0
  },

内部服务之间不走网关

 8070：前端网站  是nginx 转发的
启动网关
dotnet run --urls="http://*:6299" --ip="127.0.0.1" --port=6299

启动鉴权中心
dotnet run --urls="http://*:7200" --ip="127.0.0.1" --port=7200

启动用户服务
dotnet run --urls="http://*:5726" --ip="127.0.0.1" --port=5726

启动静态页面
dotnet run --urls="http://*:5728" --ip="127.0.0.1" --port=5728


git提交规范
# 主要type
feat:     增加新功能
fix:      修复bug

# 特殊type
docs:     只改动了文档相关的内容
style:    不影响代码含义的改动，例如去掉空格、改变缩进、增删分号
build:    构造工具的或者外部依赖的改动，例如webpack，npm
refactor: 代码重构时使用
revert:   执行git revert打印的message

# 暂不使用type
test:     添加测试或者修改现有测试
perf:     提高性能的改动
ci:       与CI（持续集成服务）有关的改动
chore:    不修改src或者test的其余修改，例如构建过程或辅助工具的变动

解决浏览器 Axios跨域请求是 浏览器会先发起Options与请求处理




