namespace Phundus.Shop.Infrastructure
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Application;
    using Common.Infrastructure;
    using Inventory.Application;
    using iTextSharp.text;
    using iTextSharp.text.exceptions;
    using iTextSharp.text.pdf;
    using Model;
    using Orders.Model;

    public class OrderPdfGenerator
    {
        private readonly Lessor _lessor;
        private readonly StoreDetailsData _store;
        private readonly StoredFileInfo _templateFileInfo;
        private readonly Order _order;
        private GrayColor backGroundColor;
        private Font defaultFont;
        private Font defaultFontBold;
        private Font defaultFontGray;
        private Document doc;
        private PdfReader reader;
        private MemoryStream stream;
        private PdfWriter writer;

        public OrderPdfGenerator(Order order, StoreDetailsData store, StoredFileInfo templateFileInfo)
        {
            if (order == null) throw new ArgumentNullException("order");
            _order = order;
            _lessor = _order.Lessor;            
            _store = store;
            _templateFileInfo = templateFileInfo;


            var fontsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);

            FontFactory.RegisterDirectory(fontsFolder, false);
            backGroundColor = new GrayColor(210);
            defaultFont = FontFactory.GetFont("calibri", 11);
            defaultFontGray = FontFactory.GetFont("calibri", 11, BaseColor.GRAY);
            defaultFontBold = FontFactory.GetFont("calibri-bold", 11);
        }

        public Stream GeneratePdf()
        {
            CreateDocument();

            AddTitle();
            AddHeading();
            AddOrderLines();

            AddFooter();

            return CloseDocument();
        }

        private void AddFooter()
        {
            //            var path = HttpContext.Current.Server.MapPath(@"~\Content\Images\PdfFooter.png");
            //            var img = iTextSharp.text.Image.GetInstance(path);
            //            img.ScaleToFit(doc.PageSize.Width, doc.PageSize.Height);
            //            img.SetAbsolutePosition(0, 40);
            //            img.BorderColor = BaseColor.LIGHT_GRAY;
            //            img.BorderWidthTop = 1.0f;
            //            img.BorderWidthBottom = 1.0f;
            //            doc.Add(img);


            var contactDetails = _store != null ? GetStoreContactDetails() : GetLessorContactDetails();

            var table = new PdfPTable(1);
            var cell = new PdfPCell
            {
                CellEvent = new RoundRectangle(),
                Border = Rectangle.NO_BORDER,
                Padding = 10,
                PaddingTop = 0,
                HorizontalAlignment = Element.ALIGN_RIGHT
            };
            var font = FontFactory.GetFont("calibri", 9);


            cell.AddElement(new Paragraph(contactDetails, font) {Alignment = Element.ALIGN_RIGHT});

            table.AddCell(cell);
            table.TotalWidth = doc.PageSize.Width/2.8f;
            table.WriteSelectedRows(0, -1, -10, 150, writer.DirectContent);
        }

        private string GetLessorContactDetails()
        {
            if (_lessor == null)
                return null;

            var sb = new StringBuilder();
            sb.AppendLine(_lessor.PostalAddress);

            if (!String.IsNullOrWhiteSpace(_lessor.PhoneNumber))
                sb.AppendLine("Tel. " + _lessor.PhoneNumber);
            if (!String.IsNullOrWhiteSpace(_lessor.EmailAddress))
                sb.AppendLine(_lessor.EmailAddress);
            if (!String.IsNullOrWhiteSpace(_lessor.Website))
                sb.AppendLine(_lessor.Website);

            return sb.ToString().Trim();
        }

        private string GetStoreContactDetails()
        {
            if (_store == null)
                return null;

            var sb = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(_store.PostalAddress))
                sb.AppendLine(_store.PostalAddress);
            else
                sb.AppendLine(_lessor.PostalAddress);

            if (!String.IsNullOrWhiteSpace(_store.PhoneNumber))
                sb.AppendLine("Tel. " + _store.PhoneNumber);
            else if (!String.IsNullOrWhiteSpace(_lessor.PhoneNumber))
                sb.AppendLine("Tel. " + _lessor.PhoneNumber);

            if (!String.IsNullOrWhiteSpace(_store.EmailAddress))
                sb.AppendLine(_store.EmailAddress);
            else if (!String.IsNullOrWhiteSpace(_lessor.EmailAddress))
                sb.AppendLine(_lessor.EmailAddress);

            if (!String.IsNullOrWhiteSpace(_lessor.Website))
                sb.AppendLine(_lessor.Website);

            return sb.ToString().Trim();
        }

        private Stream CloseDocument()
        {
            doc.Close();
            if (reader != null)
                reader.Close();
            stream.Position = 0;
            return stream;
        }

        private void CreateDocument()
        {
            stream = new MemoryStream();
            doc = new Document(PageSize.A4, 0, 0, 36.0f, 36.0f);
            writer = PdfWriter.GetInstance(doc, stream);
            writer.CloseStream = false;

            doc.Open();

            reader = GetPdfReader(_templateFileInfo);
            if (reader != null)
            {
                var importedPage = writer.GetImportedPage(reader, 1);

                writer.DirectContentUnder.AddTemplate(importedPage, 0, 0);
            }
        }

        private void AddOrderLines()
        {
            var table = new PdfPTable(new float[] {4, 5, 36, 10, 12, 12, 10});
            table.WidthPercentage = 90;
            table.DefaultCell.Padding = 3;
            table.DefaultCell.BorderWidth = 0.5f;
            table.DefaultCell.BorderWidthLeft = 0;
            table.DefaultCell.BorderWidthRight = 0;
            table.DefaultCell.BorderWidthTop = 0;
            table.DefaultCell.BorderColor = BaseColor.LIGHT_GRAY;
            table.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

            table.AddCell(new Phrase("#", defaultFontBold));
            table.AddCell(new Phrase("Stk.", defaultFontBold));
            table.AddCell(new Phrase("Artikel", defaultFontBold));
            table.AddCell(new Phrase("Art.-Nr.", defaultFontBold));
            table.AddCell(new Phrase("Von", defaultFontBold));
            table.AddCell(new Phrase("Bis", defaultFontBold));
            //table.AddCell(new Phrase("Stk. Preis", defaultFontBold));
            table.AddCell(new Phrase("Total", defaultFontBold));

            table.DefaultCell.BorderWidthTop = 0.5f;
            int pos = 0;
            foreach (var item in _order.Lines)
            {
                pos++;
                table.AddCell(new Phrase(pos.ToString(), defaultFont));
                //table.AddCell(new Phrase(item.Quantity.ToString(), defaultFont));
                table.AddCell(PhraseCell(table.DefaultCell, new Phrase(item.Quantity.ToString(), defaultFont),
                    Element.ALIGN_LEFT));

                table.AddCell(new Phrase(item.Text, defaultFont));
                table.AddCell(new Phrase(item.ArticleShortId.Id.ToString(), defaultFont));
                table.AddCell(new Phrase(item.Period.FromUtc.ToLocalTime().ToString("d"), defaultFont));
                table.AddCell(new Phrase(item.Period.ToUtc.ToLocalTime().ToString("d"), defaultFont));
                //table.AddCell(PhraseCell(table.DefaultCell, new Phrase(item.UnitPricePerWeek.ToString("N"), defaultFont), Element.ALIGN_RIGHT));
                table.AddCell(PhraseCell(table.DefaultCell, new Phrase(item.LineTotal.ToString("N"), defaultFont),
                    Element.ALIGN_RIGHT));
            }

            table.AddCell(new Phrase("", defaultFont));
            table.AddCell(new Phrase("", defaultFont));
            table.AddCell(new Phrase("", defaultFont));
            table.AddCell(new Phrase("", defaultFont));
            table.AddCell(new Phrase("", defaultFont));
            table.AddCell(new Phrase("", defaultFont));
            table.AddCell(PhraseCell(table.DefaultCell, new Phrase(_order.OrderTotal.ToString("N"), defaultFontBold),
                Element.ALIGN_RIGHT));
            doc.Add(table);
        }


        private static PdfPCell PhraseCell(PdfPCell defaultCell, Phrase phrase, int align)
        {
            var cell = new PdfPCell(phrase);


            cell.Padding = 3;
            cell.BorderWidth = defaultCell.BorderWidth;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0;
            cell.BorderColor = defaultCell.BorderColor;
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = defaultCell.VerticalAlignment;


            return cell;
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

            var orderNumberCell = new PdfPCell(new Phrase(_order.OrderShortId.Id.ToString(),
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

            cell = new PdfPCell(new Phrase("Mieter:", defaultFontGray));
            cell.BorderWidth = 0;
            cell.BackgroundColor = backGroundColor;
            cell.Padding = 3;
            cell.PaddingLeft = 36.0f;
            table.AddCell(cell);
            table.AddCell(new Phrase(_order.Lessee.FullName, defaultFont));
            table.AddCell(orderNumberCell);

            var lessee = _order.Lessee;
            var postalAddress = lessee.Street + "\n" + lessee.Postcode + " " + lessee.City;
            cell = new PdfPCell(new Phrase("Addresse:", defaultFontGray));
            cell.BorderWidth = 0;
            cell.BackgroundColor = backGroundColor;
            cell.Padding = 3;
            cell.PaddingLeft = 36.0f;
            table.AddCell(cell);
            table.AddCell(new Phrase(postalAddress, defaultFont));

            cell = new PdfPCell(new Phrase("Telefon / E-Mail:", defaultFontGray));
            cell.BorderWidth = 0;
            cell.BackgroundColor = backGroundColor;
            cell.Padding = 3;
            cell.PaddingLeft = 36.0f;
            table.AddCell(cell);
            table.AddCell(new Phrase(_order.Lessee.PhoneNumber + " / " + _order.Lessee.EmailAddress, defaultFont));

            var firstFrom = _order.Lines.Count == 0 ? (DateTime?) null : _order.Lines.Min(s => s.Period.FromUtc);
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

            var lastTo = _order.Lines.Count == 0 ? (DateTime?) null : _order.Lines.Max(s => s.Period.ToUtc);
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
            switch (_order.Status)
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

        private PdfReader GetPdfReader(StoredFileInfo templateFileInfo)
        {
            if (templateFileInfo == null)
                return null;

            try
            {
                using (var s = templateFileInfo.GetStream())
                {
                    return new PdfReader(s);
                }
            }
            catch (InvalidPdfException)
            {
                return null;
            }
        }
    }

    public class RoundRectangle : IPdfPCellEvent
    {
        #region IPdfPCellEvent Members

        public void CellLayout(
            PdfPCell cell, Rectangle rect, PdfContentByte[] canvas
            )
        {
            PdfContentByte cb;

            cb = canvas[PdfPTable.BACKGROUNDCANVAS];
            cb.RoundRectangle(
                rect.Left,
                rect.Bottom,
                rect.Width,
                rect.Height,
                8 // change to adjust how "round" corner is displayed
                );
            cb.SetColorFill(BaseColor.WHITE);
            cb.Fill();

            cb = canvas[PdfPTable.LINECANVAS];
            cb.RoundRectangle(
                rect.Left,
                rect.Bottom,
                rect.Width,
                rect.Height,
                8 // change to adjust how "round" corner is displayed
                );
            cb.SetLineWidth(0.5f);
            cb.SetCMYKColorStrokeF(0f, 0f, 0f, 1f);
            cb.Stroke();
        }

        #endregion
    }
}