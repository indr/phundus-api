using System;

namespace phiNdus.fundus.Business.Dto
{
    public class ImageDto
    {
        public int Id { get; set; }
        public int Version { get; set; }

        public bool IsPreview { get; set; }
        public long Length { get; set; }
        public string Type { get; set; }
        public string FileName { get; set; }

        public string DisplayLength
        {
            get
            {
                string[] sizes = { "B", "KB", "MB", "GB" };
                double len = Length;
                int order = 0;
                while (len >= 1024 && order + 1 < sizes.Length)
                {
                    order++;
                    len = len / 1024;
                }

                // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
                // show a single decimal place, and no space.
                return String.Format("{0:0.##} {1}", len, sizes[order]);
            }
        }
    }
}