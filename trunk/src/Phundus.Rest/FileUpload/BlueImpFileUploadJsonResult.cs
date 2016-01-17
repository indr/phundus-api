namespace Phundus.Rest.FileUpload
{
    public class BlueImpFileUploadJsonResult
    {
        /// <summary>
        /// URL zum richtigen Bild
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// URL zum Thumbnail
        /// </summary>
        public string thumbnailUrl { get; set; }

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
        public string deleteUrl { get; set; }

        /// <summary>
        /// HttpVerb
        /// </summary>
        public string deleteType { get; set; }

        public bool isPreview { get; set; }
    }
}