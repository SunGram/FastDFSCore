using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace FastDFSCore.Client
{
    /// <summary>请求执行器
    /// </summary>
    public class DefaultExecuter : IExecuter
    {
        private readonly IConnectionManager _connectionManager;

        /// <summary>Ctor
        /// </summary>
        /// <param name="connectionManager">连接管理器</param>
        public DefaultExecuter(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        /// <summary>请求执行器
        /// </summary>
        /// <typeparam name="T">请求的类型<see cref="FastDFSCore.Client.FDFSRequest"/></typeparam>
        /// <param name="request">请求</param>
        /// <param name="endPoint">返回</param>
        /// <returns></returns>
        public async Task<T> Execute<T>(FDFSRequest<T> request, IPEndPoint endPoint = null) where T : FDFSResponse, new()
        {
            var connection = endPoint == null ? await _connectionManager.GetTrackerConnection() : await _connectionManager.GetStorageConnection(endPoint);
            await connection.OpenAsync();

            var task = connection.SendRequestAsync<T>(request);

            using (var timeoutCancellationTokenSource = new CancellationTokenSource())
            {
                var delayTask = Task.Delay(request.Timeout, timeoutCancellationTokenSource.Token);
                if (await Task.WhenAny(task, delayTask) == task)
                {
                    timeoutCancellationTokenSource.Cancel();
                    var response = await task.ConfigureAwait(false);
                    await connection.CloseAsync();

                    if (response.Header.Status != 0)
                    {
                        throw new Exception($"返回Status不正确:{response.Header.Status}");
                    }
                    return response as T;
                }
                throw new TimeoutException("The operation has timed out.");
            }
        }
    }
}
