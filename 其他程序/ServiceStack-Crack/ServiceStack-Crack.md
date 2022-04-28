

## 步骤一： 修改ServiceStack.Text

~~~shell
下载路径
https://github.com/ServiceStack/ServiceStack.Text
找到LicenseUtils类：

修改 ServiceStack.text源代码6000限制
方法：直接修改  LicenseUtils里面的ApprovedUsage，直接return
代码如下
public static void ApprovedUsage(LicenseFeature licenseFeature, LicenseFeature requestedFeature,
            int allowedUsage, int actualUsage, string message)
        {
            return;//新增return
            var hasFeature = (requestedFeature & licenseFeature) == requestedFeature;
            if (hasFeature)
                return;

            if (actualUsage > allowedUsage)
                throw new LicenseException(message.Fmt(allowedUsage)).Trace();
        }
 
 编译生成ServiceStack.Text.dll，在.netstandard2.0目录
~~~



## 步骤二：获取全套ServiceStack.Redis


~~~shell
下载地址  https://github.com/ServiceStack/ServiceStack.Redis #其实不用下载
1 建立控制台ServiceStack.RedisCrack
2 nuget ServiceStack.Redis
3 写如下测试代码
  Parallel.For(0, 10000, (i) =>
			{
				using (RedisClient client = new RedisClient("192.168.3.254"))
				{
					client.Set("zxname" + i, i);
					client.Incr("zxname" + i);
					Console.WriteLine(i);
				}

			});
4  运行会6000次错误
5  找到控制台生成的bin/.net5.0
   复制以下dll，单独保存：
   Microsoft.Bcl.AsyncInterfaces.dll
   ServiceStack.Common.dll
   ServiceStack.Interfaces.dll
   ServiceStack.Redis.dll
   ServiceStack.Text.dll
6  将步骤1生成的Dll替换进来，替换ServiceStack项目中的ServiceStack.Text.dll
7  将控制台nuget卸载，再浏览本地，添加5个dll即可
~~~

### 步骤三：应用

~~~shell
			
项目中使用需要卸载掉nuget对ServiceStack.Redis的引用，然后引用上述5个dll
~~~

