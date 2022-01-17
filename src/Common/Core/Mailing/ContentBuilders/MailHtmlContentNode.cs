using System.Text;
namespace Cofi.Mailing.ContentBuilders;

public class MailHtmlContentNode
{
    public string Tag { get; }
    public ICollection<MailHtmlContentNodeStyleRule> StyleRules { get; } = new List<MailHtmlContentNodeStyleRule>();
    public ICollection<MailHtmlContentNode> Children { get; } = new List<MailHtmlContentNode>();

    public MailHtmlContentNode(string tag)
    {
        Tag = tag;
    }

    public void Render(StringBuilder builder)
    {
        
    }
}
