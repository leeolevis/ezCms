using ezModel.BaseModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace ezLay.Mvc.TagsHelpers
{
    [HtmlTargetElement("div", Attributes = "jstree-for")]
    public class JsTreeTagHelper : TagHelper
    {
        [HtmlAttributeName("jstree-for")]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var tree = For.Model as JsTree;

            output.Attributes.SetAttribute("class", (output.Attributes["class"]?.Value + " js-tree").Trim());
            output.Content.AppendHtml(HiddenIdsFor(tree));
            output.Content.AppendHtml(JsTreeFor(tree));
        }

        private void Add(TagBuilder root, List<JsTreeNode> nodes)
        {
            var branch = new TagBuilder("ul");
            foreach (var node in nodes)
            {
                var item = new TagBuilder("li");
                item.InnerHtml.Append(node.Title);
                var id = node.Id;
                item.Attributes["id"] = id.ToString();

                Add(item, node.Nodes);
                branch.InnerHtml.AppendHtml(item);
            }

            if (nodes.Count > 0)
                root.InnerHtml.AppendHtml(branch);
        }
        private TagBuilder HiddenIdsFor(JsTree model)
        {
            var name = For.Name + ".SelectedIds";
            var ids = new TagBuilder("div");
            ids.AddCssClass("js-tree-view-ids");

            foreach (var id in model.SelectedIds)
            {
                var input = new TagBuilder("input") { TagRenderMode = TagRenderMode.SelfClosing };
                input.Attributes["value"] = id.ToString();
                input.Attributes["type"] = "hidden";
                input.Attributes["name"] = name;

                ids.InnerHtml.AppendHtml(input);
            }

            return ids;
        }
        private TagBuilder JsTreeFor(JsTree model)
        {
            var name = For.Name + ".SelectedIds";
            var tree = new TagBuilder("div");
            tree.AddCssClass("js-tree-view");
            tree.Attributes["for"] = name;

            Add(tree, model.Nodes);

            return tree;
        }
    }
}
