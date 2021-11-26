
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WYNK.Data.Model;


namespace WYNK.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelExampleController : ControllerBase
    {
        private CMPSContext _Cmpscontext;
        private readonly IHttpContextAccessor _httpContext;
        public ExcelExampleController(CMPSContext context, IHttpContextAccessor httpContext)
        {
            _Cmpscontext = context;
            _httpContext = httpContext;
        }
     

        [Route("UploadExcel")]
        [HttpPost]
     
        public string ExcelUpload()
    {
        string message = "";
        HttpResponseMessage result = null;
           
                Stream stream = null;
            
        return message;
    }

    [Route("UserDetails")]
    [HttpGet]
    public List<Users> BindUser()
    {
        List<Users> lstUser = new List<Users>();
             lstUser = _Cmpscontext.Users.ToList();
      
            return lstUser;
    }


        public void MyMethod(Microsoft.AspNetCore.Http.HttpContext context)
        {
            var host = $"{context.Request.Scheme}://{context.Request.Host}";

           
        }
    }  
}  