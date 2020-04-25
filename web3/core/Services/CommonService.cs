using Microsoft.AspNetCore.Http;

namespace web3.core.Services
{
    public class CommonService
    {
        private IHttpContextAccessor _httpContextAccessor;

        public CommonService(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
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