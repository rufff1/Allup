//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace Allup.Extension
//{
//    public static class FileManeger
//    {



//        public static string CreateImage(this IFormFile file, IWebHostEnvironment env,params string[] folders )
//        {
//            string fileName = $"{Guid.NewGuid()}_{DateTime.Now.ToString("yyyyyMMddHHmmss")}_{file.FileName}";

//            string path = env.WebRootPath;

//            foreach (string folder in folders)
//            {
//                path = Path.Combine(path, folder);
//            }


//            path = Path.Combine(path, fileName);

//            using (FileStream fileStream = new FileStream(path , FileMode.Create))
//            {
//                file.CopyTo(fileStream);
//            }
//            return fileName;
//        }


//        public static bool CheckFileSize(this IFormFile file, double size)
//        {
//            return Math.Round((double)file.Length / 1024) < size ;
//        }

//        public static bool CheckFileType(this IFormFile file, string contentType)
//        {
//            return file.ContentType == contentType;
//        }


//    }
//}
