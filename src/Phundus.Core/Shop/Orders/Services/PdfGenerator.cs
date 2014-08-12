namespace Phundus.Core.Shop.Orders.Services
{
    using System;
    using System.IO;
    using System.Web.Hosting;
    using IdentityAndAccess.Organizations.Model;
    using iTextSharp.text;
    using iTextSharp.text.exceptions;
    using iTextSharp.text.pdf;
    using Model;

    public class PdfGenerator
    {
        public static Stream GeneratePdf(Order order, Organization organization)
        {
            PdfReader reader = null;
            if (!String.IsNullOrEmpty(organization.DocTemplateFileName))
            {
                var fileName = HostingEnvironment.MapPath(
                    String.Format(@"~\Content\Uploads\Organizations\{0}\{1}", organization.Id,
                        organization.DocTemplateFileName));
                if (File.Exists(fileName))
                {
                    try
                    {
                        reader = new PdfReader(fileName);
                    }
                    catch (InvalidPdfException)
                    {
                        reader = null;
                    }
                }
            }


            var result = new MemoryStream();
            var doc = new Document(PageSize.A4, 0, 0, 36.0f, 36.0f);
            var writer = PdfWriter.GetInstance(doc, result);
            writer.CloseStream = false;

            doc.Open();

            if (reader != null)
            {
                var importedPage = writer.GetImportedPage(reader, 1);

                writer.DirectContentUnder.AddTemplate(importedPage, 0, 0);
            }

            var fontsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);

            FontFactory.RegisterDirectory(fontsFolder, false);

            //Environment
            //FontFactory.Register(@"C:\\Windows"););
            //foreach (var each in FontFactory.RegisteredFonts)
            // {
            //     doc.Add(new Paragraph(each));
            //}

            var defaultFont = FontFactory.GetFont("calibri", 11);
            var defaultFontGray = FontFactory.GetFont("calibri", 11, BaseColor.GRAY);
            var defaultFontBold = FontFactory.GetFont("calibri-bold", 11);

            var titleCaption = "Bestellung - Provisorisch";
            switch (order.Status)
            {
                case OrderStatus.Pending:
                    break;
                case OrderStatus.Approved:
                    titleCaption = "Bestellung - Bestätigung";
                    break;
                case OrderStatus.Rejected:
                    titleCaption = "Bestellung - Abgelehnt";
                    break;
                case OrderStatus.Closed:
                    titleCaption = "Bestellung - Abgeschlossen";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var title = new Paragraph(titleCaption, FontFactory.GetFont("calibri-bold", 22));
            title.IndentationLeft = 36.0f;
            doc.Add(title);


            //doc.
            var backGroundColor = new GrayColor(210);
            var table = new PdfPTable(3);

            table.SpacingBefore = 20;
            table.SpacingAfter = 10;

            table.WidthPercentage = 100;
            table.DefaultCell.BorderWidth = 0;
            table.DefaultCell.BackgroundColor = backGroundColor;
            table.DefaultCell.Padding = 3;

            table.TotalWidth = 100;

            var orderNumberCell = new PdfPCell(new Phrase(order.Id.ToString(),
                FontFactory.GetFont("calibri-bold", 36, BaseColor.WHITE)));
            orderNumberCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            orderNumberCell.Rowspan = 5;
            orderNumberCell.PaddingTop = 0;
            orderNumberCell.PaddingRight = 18.0f;
            orderNumberCell.PaddingBottom += 20;
            orderNumberCell.BorderWidth = 0;
            orderNumberCell.BackgroundColor = backGroundColor;


            table.SetWidths(new int[] {2, 4, 2});


            PdfPCell cell = null;

            cell = new PdfPCell(new Phrase("Mietende Person:", defaultFontGray));
            cell.BorderWidth = 0;
            cell.BackgroundColor = backGroundColor;
            cell.Padding = 3;
            cell.PaddingLeft = 36.0f;
            table.AddCell(cell);
            table.AddCell(new Phrase(order.Borrower.DisplayName, defaultFont));
            table.AddCell(orderNumberCell);

            cell = new PdfPCell(new Phrase("J+S-Nummer:", defaultFontGray));
            cell.BorderWidth = 0;
            cell.BackgroundColor = backGroundColor;
            cell.Padding = 3;
            cell.PaddingLeft = 36.0f;
            table.AddCell(cell);
            table.AddCell(new Phrase(order.Borrower.MemberNumber, defaultFontBold));

            cell = new PdfPCell(new Phrase("Telefon / E-Mail:", defaultFontGray));
            cell.BorderWidth = 0;
            cell.BackgroundColor = backGroundColor;
            cell.Padding = 3;
            cell.PaddingLeft = 36.0f;
            table.AddCell(cell);
            table.AddCell(new Phrase(order.Borrower.MobilePhoneNumber + " / " + order.Borrower.EmailAddress, defaultFont));


            cell = new PdfPCell(new Phrase("Abholen:", defaultFontGray));
            cell.BorderWidth = 0;
            cell.BackgroundColor = backGroundColor;
            cell.Padding = 3;
            cell.PaddingLeft = 36.0f;
            table.AddCell(cell);
            table.AddCell(new Phrase(order.FirstFrom.ToString("d"), defaultFont));

            cell = new PdfPCell(new Phrase("Rückgabe:", defaultFontGray));
            cell.BorderWidth = 0;
            cell.BackgroundColor = backGroundColor;
            cell.Padding = 3;
            cell.PaddingLeft = 36.0f;
            table.AddCell(cell);
            table.AddCell(new Phrase(order.LastTo.ToString("d"), defaultFont));


            foreach (var each in table.Rows[0].GetCells())
            {
                each.PaddingTop += 5;
            }

            foreach (var each in table.Rows[4].GetCells())
            {
                if (each != null)
                    each.PaddingBottom += 10;
            }
            doc.Add(table);


            table = new PdfPTable(new float[] {5, 5, 36, 10, 12, 12, 10, 10});
            table.WidthPercentage = 90;
            table.DefaultCell.Padding = 3;
            table.DefaultCell.BorderWidth = 0.5f;
            table.DefaultCell.BorderColor = BaseColor.LIGHT_GRAY;
            table.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(new Phrase("Pos.", defaultFontBold));
            table.AddCell(new Phrase("Stk.", defaultFontBold));
            table.AddCell(new Phrase("Artikel", defaultFontBold));
            table.AddCell(new Phrase("Art.-Nr.", defaultFontBold));
            table.AddCell(new Phrase("Von", defaultFontBold));
            table.AddCell(new Phrase("Bis", defaultFontBold));
            table.AddCell(new Phrase("Stk. Preis", defaultFontBold));
            table.AddCell(new Phrase("Total", defaultFontBold));


            int pos = 0;
            foreach (var item in order.Items)
            {
                pos++;
                table.AddCell(new Phrase(pos.ToString(), defaultFont));
                table.AddCell(new Phrase(item.Amount.ToString(), defaultFont));
                table.AddCell(new Phrase(item.Article.Caption, defaultFont));
                table.AddCell(new Phrase(item.Article.Id.ToString(), defaultFont));
                table.AddCell(new Phrase(item.From.ToString("d"), defaultFont));
                table.AddCell(new Phrase(item.To.ToString("d"), defaultFont));
                table.AddCell(new Phrase(item.UnitPrice.ToString("N"), defaultFont));
                table.AddCell(new Phrase(item.LineTotal.ToString("N"), defaultFont));
            }

            table.AddCell(new Phrase("", defaultFont));
            table.AddCell(new Phrase("", defaultFont));
            table.AddCell(new Phrase("", defaultFont));
            table.AddCell(new Phrase("", defaultFont));
            table.AddCell(new Phrase("", defaultFont));
            table.AddCell(new Phrase("", defaultFont));
            table.AddCell(new Phrase("", defaultFont));
            table.AddCell(new Phrase(order.TotalPrice.ToString("N"), defaultFontBold));
            doc.Add(table);
//            var path = HttpContext.Current.Server.MapPath(@"~\Content\Images\PdfFooter.png");
//            var img = iTextSharp.text.Image.GetInstance(path);
//            img.ScaleToFit(doc.PageSize.Width, doc.PageSize.Height);
//            img.SetAbsolutePosition(0, 40);
//            img.BorderColor = BaseColor.LIGHT_GRAY;
//            img.BorderWidthTop = 1.0f;
//            img.BorderWidthBottom = 1.0f;
//            doc.Add(img);


//            table = new PdfPTable(1);
//            cell = new PdfPCell()
//            {
//                CellEvent = new RoundRectangle(),
//                Border = PdfPCell.NO_BORDER,
//                Padding = 10,
//                PaddingTop = 0,
//                HorizontalAlignment = Element.ALIGN_RIGHT
//            };
//            defaultFont = FontFactory.GetFont("calibri", 10);
//            cell.AddElement(new Paragraph(@"Sekretariat PFADI Luzern
//c/o Stiftung Rodtegg, bürowärkstatt
//Rodteggstrasse 3a, 6005 Luzern 
//Tel. 041 368 40 35   Fax 041 368 42 94
//Web www.pfadiluzern.ch
//E-Mail sekretariat@pfadiluzern.ch", defaultFont) { Alignment = Element.ALIGN_RIGHT });

//            table.AddCell(cell);
//            table.TotalWidth = doc.PageSize.Width / 2.8f;
//            table.WriteSelectedRows(0, -1, -10, 150, writer.DirectContent);

            doc.Close();
            if (reader != null)
                reader.Close();
            result.Position = 0;
            return result;
        }
    }
}