using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MimMVC.Interfaces;

namespace MimMVC.Utility
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        
        public UnitOfWork(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public void UploadImage(IFormFile file, string path)
        {
            throw new NotImplementedException();

            //// https://www.youtube.com/watch?v=QpJvqiHl1Fo
            //// Save image to wwwroot/img/post
            //string wwwRootPath = _hostEnvironment.WebRootPath;
            //string fileName = Path.GetFileNameWithoutExtension(postNewContent.ImageFile.FileName);
            //string extension = Path.GetExtension(postNewContent.ImageFile.FileName);
            //postNewContent.ImageName = fileName = fileName + extension;
            //string path = Path.Combine(wwwRootPath + path, fileName);
            //using (var fileStream = new FileStream(path, FileMode.Create))
            //{
            //    await postNewContent.ImageFile.CopyToAsync(fileStream);
            //}

            ////Insert record
        }     
    }
}
