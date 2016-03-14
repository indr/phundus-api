﻿namespace Phundus.Shop.Model.Pdf
{
    using System;
    using System.IO;
    using System.Linq;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using Model;
    using Orders.Model;

    public class OrderPdf
    {
        private Document doc;
        private PdfReader reader;
        private PdfWriter writer;
        private Order order;
        private GrayColor backGroundColor;
        private Font defaultFont;
        private Font defaultFontGray;
        private Font defaultFontBold;

        public OrderPdf(Order aOrder)
        {
            order = aOrder;


            var fontsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);

            FontFactory.RegisterDirectory(fontsFolder, false);
            backGroundColor = new GrayColor(210);
            defaultFont = FontFactory.GetFont("calibri", 11);
            defaultFontGray = FontFactory.GetFont("calibri", 11, BaseColor.GRAY);
            defaultFontBold = FontFactory.GetFont("calibri-bold", 11);

        }

        public Stream GeneratePdf()
        {
            var result = new MemoryStream();
            doc = new Document(PageSize.A4, 0, 0, 36.0f, 36.0f);
            writer = PdfWriter.GetInstance(doc, result);
            writer.CloseStream = false;

            doc.Open();

            reader = GetPdfReader(order);
            if (reader != null)
            {
                var importedPage = writer.GetImportedPage(reader, 1);

                writer.DirectContentUnder.AddTemplate(importedPage, 0, 0);
            }

            


            
            AddTitle();


            

            
            AddHeading();


            AddOrderLines();
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

        private void AddOrderLines()
        {
            var table = new PdfPTable(new float[] {5, 5, 36, 10, 12, 12, 10, 10});
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
            foreach (var item in order.Lines)
            {
                pos++;
                table.AddCell(new Phrase(pos.ToString(), defaultFont));
                table.AddCell(new Phrase(item.Quantity.ToString(), defaultFont));
                table.AddCell(new Phrase(item.Text, defaultFont));
                table.AddCell(new Phrase(item.ArticleShortId.Id.ToString(), defaultFont));
                table.AddCell(new Phrase(item.Period.FromUtc.ToLocalTime().ToString("d"), defaultFont));
                table.AddCell(new Phrase(item.Period.ToUtc.ToLocalTime().ToString("d"), defaultFont));
                table.AddCell(new Phrase(item.UnitPricePerWeek.ToString("N"), defaultFont));
                table.AddCell(new Phrase(item.LineTotal.ToString("N"), defaultFont));
            }

            table.AddCell(new Phrase("", defaultFont));
            table.AddCell(new Phrase("", defaultFont));
            table.AddCell(new Phrase("", defaultFont));
            table.AddCell(new Phrase("", defaultFont));
            table.AddCell(new Phrase("", defaultFont));
            table.AddCell(new Phrase("", defaultFont));
            table.AddCell(new Phrase("", defaultFont));
            table.AddCell(new Phrase(order.OrderTotal.ToString("N"), defaultFontBold));
            doc.Add(table);
        }

        private void AddHeading()
        {
            var table = new PdfPTable(3);

            table.SpacingBefore = 20;
            table.SpacingAfter = 10;

            table.WidthPercentage = 100;
            table.DefaultCell.BorderWidth = 0;
            table.DefaultCell.BackgroundColor = backGroundColor;
            table.DefaultCell.Padding = 3;

            table.TotalWidth = 100;

            var orderNumberCell = new PdfPCell(new Phrase(order.OrderShortId.Id.ToString(),
                FontFactory.GetFont("calibri-bold", 36, BaseColor.WHITE)));
            orderNumberCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            orderNumberCell.Rowspan = 5;
            orderNumberCell.PaddingTop = 0;
            orderNumberCell.PaddingRight = 18.0f;
            orderNumberCell.PaddingBottom += 20;
            orderNumberCell.BorderWidth = 0;
            orderNumberCell.BackgroundColor = backGroundColor;


            table.SetWidths(new[] {2, 4, 2});


            PdfPCell cell = null;

            cell = new PdfPCell(new Phrase("Mietende Person:", defaultFontGray));
            cell.BorderWidth = 0;
            cell.BackgroundColor = backGroundColor;
            cell.Padding = 3;
            cell.PaddingLeft = 36.0f;
            table.AddCell(cell);
            table.AddCell(new Phrase(order.Lessee.FullName, defaultFont));
            table.AddCell(orderNumberCell);

            cell = new PdfPCell(new Phrase("J+S-Nummer:", defaultFontGray));
            cell.BorderWidth = 0;
            cell.BackgroundColor = backGroundColor;
            cell.Padding = 3;
            cell.PaddingLeft = 36.0f;
            table.AddCell(cell);
            table.AddCell(new Phrase(order.Lessee.MemberNumber, defaultFontBold));

            cell = new PdfPCell(new Phrase("Telefon / E-Mail:", defaultFontGray));
            cell.BorderWidth = 0;
            cell.BackgroundColor = backGroundColor;
            cell.Padding = 3;
            cell.PaddingLeft = 36.0f;
            table.AddCell(cell);
            table.AddCell(new Phrase(order.Lessee.PhoneNumber + " / " + order.Lessee.EmailAddress, defaultFont));

            var firstFrom = order.Lines.Count == 0 ? (DateTime?) null : order.Lines.Min(s => s.Period.FromUtc);
            if (firstFrom.HasValue)
            {
                cell = new PdfPCell(new Phrase("Abholen:", defaultFontGray));
                cell.BorderWidth = 0;
                cell.BackgroundColor = backGroundColor;
                cell.Padding = 3;
                cell.PaddingLeft = 36.0f;
                table.AddCell(cell);
                table.AddCell(new Phrase(firstFrom.Value.ToLocalTime().ToString("d"), defaultFont));
            }

            var lastTo = order.Lines.Count == 0 ? (DateTime?) null : order.Lines.Max(s => s.Period.ToUtc);
            if (lastTo.HasValue)
            {
                cell = new PdfPCell(new Phrase("Rückgabe:", defaultFontGray));
                cell.BorderWidth = 0;
                cell.BackgroundColor = backGroundColor;
                cell.Padding = 3;
                cell.PaddingLeft = 36.0f;
                table.AddCell(cell);
                table.AddCell(new Phrase(lastTo.Value.ToLocalTime().ToString("d"), defaultFont));
            }

            if (table.Rows.Count > 0)
                foreach (var each in table.Rows[0].GetCells())
                {
                    each.PaddingTop += 5;
                }

            if (table.Rows.Count > 4)
                foreach (var each in table.Rows[4].GetCells())
                {
                    if (each != null)
                        each.PaddingBottom += 10;
                }
            doc.Add(table);
        }

        private void AddTitle()
        {
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
        }

        private PdfReader GetPdfReader(Order order)
        {
            return null;
            //var organization = OrganizationRepository.FindById(order.Lessor.LessorId.Id);
            //if (organization == null)
            //    return null;

            //PdfReader reader = null;
            //if (!String.IsNullOrEmpty(organization.DocTemplateFileName))
            //{
            //    var fileName = HostingEnvironment.MapPath(
            //        String.Format(@"~\Content\Uploads\{0}\{1}", organization.Id.Id.ToString("N"),
            //            organization.DocTemplateFileName));
            //    if (File.Exists(fileName))
            //    {
            //        try
            //        {
            //            reader = new PdfReader(fileName);
            //        }
            //        catch (InvalidPdfException)
            //        {
            //            reader = null;
            //        }
            //    }
            //}
            //return reader;
        }
    }
}