using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimMVC.Interfaces
{
    public interface IUnitOfWork
    {
        void UploadImage(IFormFile file, string path);
    }


}
