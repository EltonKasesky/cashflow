using CashFlow.Application.Fonts;
using CashFlow.Communication.Report.Requests;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System.Reflection;

namespace CashFlow.Application.UseCases.Expenses.Report.Pdf;

internal class GenerateExpensesPdfReportUseCase : IGenerateExpensesPdfReportUseCase
{
    private const int ROW_HEIGHT = 25;
    private readonly IExpensesReadRepository _repository;

    public GenerateExpensesPdfReportUseCase(IExpensesReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<byte[]> Execute(RequestInformationReport request)
    {
        DateTime startDate = ReportHelper.GetStartDate(request.Month);
        DateTime endDate = ReportHelper.GetEndDate(request.Month);

        List<Expense> expenses = await _repository.FilterByMonth(startDate, endDate);

        if (!expenses.Any())
            return [];

        Document document = CreateDocument(request.Month);
        Section page = CreatePage(document);

        InsertHeader(page);
        InsertTotalSpent(page, expenses, request);
        InsertExpenses(expenses, page);

        return RenderDocument(document);
    }

    private Document CreateDocument(DateOnly date)
    {
        Document document = new Document();
        document.Info.Title = $"{ResourceReportGenerationMessages.EXPENSES_FOR} {date.ToString("Y")}";
        document.Info.Author = "Elton Kasesky";

        Style? style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.DEFAULT_FONT;

        return document;
    }

    private Section CreatePage(Document document)
    {
        Section section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();

        section.PageSetup.PageFormat = PageFormat.A4;

        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;

        return section;
    }

    private void InsertHeader(Section page)
    {
        Table table = page.AddTable();

        table.AddColumn();
        table.AddColumn("300");

        Row row = table.AddRow();

        row.Cells[0].AddImage(GetLogoImage());

        row.Cells[1].AddParagraph($"{ResourceReportGenerationMessages.HEY}, Elton Kasesky");
        row.Cells[1].Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 16 };
        row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
    }

    private string GetLogoImage()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        string? path = Path.GetDirectoryName(assembly.Location);

        return Path.Combine(path!, "Logo", "logo.png");
    }

    private void InsertTotalSpent(Section page, List<Expense> expenses, RequestInformationReport request)
    {
        Paragraph paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = "40";
        paragraph.Format.SpaceAfter = "40";

        string title = string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, request.Month.ToString("Y"));
        paragraph.AddFormattedText(text: title, font: new Font { Name = FontHelper.RALEWAY_REGULAR, Size = 15 });

        paragraph.AddLineBreak();

        decimal total = expenses.Sum(expense => expense.Amount);
        paragraph.AddFormattedText(text: $"{ReportHelper.CURRENCY_SIMBOL} {total}", font: new Font { Name = FontHelper.WORKSANS_BLACK, Size = 50 });
    }

    private void InsertExpenses(List<Expense> expenses, Section page)
    {
        expenses.ForEach(expense =>
        {
            Table table = CreateExpenseTable(page);
            Row row = table.AddRow();
            row.Height = ROW_HEIGHT;

            AddExpenseHeaderTitle(row.Cells[0], expense.Title);
            AddExpenseHeaderAmount(row.Cells[3]);
            AddExpenseValues(table, row, expense);
            AddWhiteSpace(table);
        });
    }

    private Table CreateExpenseTable(Section page)
    {
        Table table = page.AddTable();

        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

        return table;
    }

    private void AddExpenseHeaderTitle(Cell cell, string title)
    {
        cell.AddParagraph(title);
        cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Color = ColorHelper.BLACK, Size = 14 };
        cell.Shading.Color = ColorHelper.RED_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
    }

    private void AddExpenseHeaderAmount(Cell cell)
    {
        cell.AddParagraph(ResourceReportGenerationMessages.AMOUNT);
        cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Color = ColorHelper.WHITE, Size = 14 };
        cell.Shading.Color = ColorHelper.RED_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddExpenseValues(Table table, Row row, Expense expense)
    {
        row = table.AddRow();
        row.Height = ROW_HEIGHT;

        row.Cells[0].AddParagraph(expense.Date.ToString("D"));
        SetStyleBaseForExpenseInformation(row.Cells[0]);

        row.Cells[1].AddParagraph(expense.Date.ToString("t"));
        SetStyleBaseForExpenseInformation(row.Cells[1]);

        row.Cells[2].AddParagraph(expense.PaymentType.PaymentTypeToString());
        SetStyleBaseForExpenseInformation(row.Cells[2]);

        AddAmountForExpense(row.Cells[3], expense.Amount);

        if (!string.IsNullOrWhiteSpace(expense.Description))
        {
            row.Cells[3].MergeDown = 1;
            AddExpenseDescription(table.AddRow(), expense.Description);
        }
    }

    private void SetStyleBaseForExpenseInformation(Cell cell)
    {
        cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Color = ColorHelper.BLACK, Size = 12 };
        cell.Shading.Color = ColorHelper.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddAmountForExpense(Cell cell, decimal amount)
    {
        cell.AddParagraph($"-{ReportHelper.CURRENCY_SIMBOL} {amount}");
        cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Color = ColorHelper.BLACK, Size = 14 };
        cell.Shading.Color = ColorHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddExpenseDescription(Row row, string description)
    {
        row.Height = ROW_HEIGHT;
        row.Cells[0].AddParagraph(description);
        row.Cells[0].Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Color = ColorHelper.BLACK, Size = 10 };
        row.Cells[0].Shading.Color = ColorHelper.GREEN_LIGHT;
        row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
        row.Cells[0].MergeRight = 2;
    }

    private void AddWhiteSpace(Table table)
    {
        Row row = table.AddRow();
        row.Height = 30;
        row.Borders.Visible = false;
    }

    private byte[] RenderDocument(Document document)
    {
        PdfDocumentRenderer renderer = new PdfDocumentRenderer
        {
            Document = document
        };

        renderer.RenderDocument();

        using MemoryStream file = new MemoryStream();
        renderer.PdfDocument.Save(file);

        return file.ToArray();
    }
}
