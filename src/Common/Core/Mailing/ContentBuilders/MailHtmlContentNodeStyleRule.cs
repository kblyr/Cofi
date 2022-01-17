namespace Cofi.Mailing.ContentBuilders;

public class MailHtmlContentNodeStyleRule
{
    public string RuleName { get; }
    public string Value { get; }

    public MailHtmlContentNodeStyleRule(string ruleName, string value)
    {
        RuleName = ruleName;
        Value = value;
    }
}