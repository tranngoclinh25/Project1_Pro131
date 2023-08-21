using System.Linq;
using ASM.Models;

namespace ASM.Repostitory
{
    public class ProductImageRepository : GenericRepository<ProductImage>
    {
        DMMContext _context;
        public ProductImageRepository(DMMContext context)
        {
            _context = context; 
        }
        public ProductImage GetFistImageProduct(long productId)
        {
           return _context.ProductImages.Where(c=>c.ProductId == productId).FirstOrDefault();
        }
    }
}
