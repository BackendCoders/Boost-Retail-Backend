using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boost.Admin.DTOs
{
    public enum TreeNodeType
    {
        Category,
        Brand
    }
    public class TreeNode
    {
        public string Title { get; set; }
        public TreeNodeType Type { get; set; } = TreeNodeType.Category;
        public List<TreeNode> Children { get; set; } = new();
        public int Count { get; set; } = 0;
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
    }

    public class CategoryNode
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; } = 0;
        public int? ParentId { get; set; }
        public List<CategoryNode> Children { get; set; } = new();
    }
}
