using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace Office4U.Articles.ImportExport.Api.Controllers
{
    public class ControllerTestsBase
    {
        protected readonly ControllerContext TestControllerContext;

        public ControllerTestsBase()
        {
            var headerDictionary = new HeaderDictionary();
            var responseMock = new Mock<HttpResponse>();
            responseMock.SetupGet(r => r.Headers).Returns(headerDictionary);
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(a => a.Response).Returns(responseMock.Object);
            var actionContext = new ActionContext(httpContextMock.Object, new RouteData(), new ControllerActionDescriptor());
            TestControllerContext = new ControllerContext(actionContext) { HttpContext = httpContextMock.Object };
        }
    }
}
