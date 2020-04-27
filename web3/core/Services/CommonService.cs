using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace web3.core.Services
{
    public class CommonService
    {
        private IHttpContextAccessor _httpContextAccessor;
        private IConfiguration _configuration;

        public CommonService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._configuration = configuration;
        }

        public IHttpContextAccessor GetHttpContext()
        {
            return this._httpContextAccessor;
        }

        public string getRemoteAddress()
        {
            return this._httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}