using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Web;

namespace phiNdus.fundus.Core.Web.Helpers
{
    public class BlueImpFileUploadStore
    {
        private readonly string _path;
        private readonly string _url;
        private readonly string _deleteActionUrl;
        public BlueImpFileUploadStore(string path, string url, string deleteActionUrl)
        {
            _path = path;
            _url = url;
            _deleteActionUrl = deleteActionUrl;
        }

        public BlueImpFileUploadJsonResult[] Post(HttpFileCollectionBase files)
        {
            var result = new List<BlueImpFileUploadJsonResult>();
            foreach (string file in files)
            {
                var postedFile = files[file];
                if (postedFile == null || postedFile.ContentLength == 0)
                    continue;

                var originalFileName = postedFile.FileName;
                ;
                if (!Directory.Exists(_path))
                    Directory.CreateDirectory(_path);

                var savedFileName = _path + Path.DirectorySeparatorChar + originalFileName;
                postedFile.SaveAs(savedFileName);


                result.Add(new BlueImpFileUploadJsonResult
                {
                    delete_type = "DELETE",
                    delete_url = _deleteActionUrl + '/' + originalFileName,
                    thumbnail_url =_url + originalFileName,
                    url = _url + originalFileName,
                    name = originalFileName,
                    size = postedFile.ContentLength,
                    type = postedFile.ContentType
                });
            }
            return result.ToArray();
        }

        public BlueImpFileUploadJsonResult[] Get()
        {
            var result = new List<BlueImpFileUploadJsonResult>();
            if (Directory.Exists(_path))
            {
                foreach (var each in Directory.GetFiles(_path))
                {
                    var fileInfo = new FileInfo(each);
                    
                    result.Add(new BlueImpFileUploadJsonResult
                    {
                        delete_type = "DELETE",
                        delete_url = _deleteActionUrl + '/' + fileInfo.Name,
                        thumbnail_url = _url + '/' + 'a' + fileInfo.Name,
                        url = _url + '/' + fileInfo.Name,
                        name = fileInfo.Name,
                        size = fileInfo.Length,
                        type = "image/jpeg"
                    });
                }
            }
            return result.ToArray();
        }


        public void Delete(string name)
        {
            var fileName = Path.Combine(_path, name);
            if (File.Exists(fileName))
                File.Delete(fileName);
        }
    }

    public class BlueImpFileUploadJsonResult
    {
        /// <summary>
        /// Dunno?
        /// </summary>
        public string _ { get; set; }

        /// <summary>
        /// URL zum richtigen Bild
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// URL zum Thumbnail
        /// </summary>
        public string thumbnail_url { get; set; }

        /// <summary>
        /// Name des Bildes
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Typ, z.B. "image/jpeg"
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Grösse in Bytes
        /// </summary>
        public long size { get; set; }

        /// <summary>
        /// REST-Delete-Url
        /// </summary>
        public string delete_url { get; set; }

        /// <summary>
        /// HttpVerb
        /// </summary>
        public string delete_type { get; set; }

    }
}