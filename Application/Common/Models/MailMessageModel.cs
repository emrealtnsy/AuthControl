namespace AuthControl.Application.Common.Models;

public sealed record MailMessageModel(string To, string Subject, string Body, bool IsBodyHtml);
