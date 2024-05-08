using _3._20_Hw.Data;

namespace _3._20_Hw.Web.Models
{
    public class ImageViewModel
    {
        public List<Images> Images { get; set; }
        public Images Image { get; set; }
        public bool CanLike { get; set; }
    }
}
