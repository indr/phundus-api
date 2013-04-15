namespace phiNdus.fundus.Web.Helpers.FileUpload
{
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