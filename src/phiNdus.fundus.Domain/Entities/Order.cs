using System;
using System.IO;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using log4net;
using phiNdus.fundus.Domain.Inventory;
using phiNdus.fundus.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Domain.Entities
{
    public class Order : Entity
    {
        private DateTime _createDate;
        private ISet<OrderItem> _items = new HashedSet<OrderItem>();

        public Order() : this(0, 0)
        {
        }

        public Order(int id, int version) : base(id, version)
        {
            _createDate = DateTime.Now;
        }

        public virtual DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        public virtual ISet<OrderItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public virtual User Reserver { get; set; }

        public virtual OrderStatus Status { get; protected set; }

        public virtual User Modifier { get; protected set; }

        public virtual DateTime? ModifyDate { get; protected set; }

        public virtual double TotalPrice { get { return Items.Sum(x => x.LineTotal); } }

        public virtual bool AddItem(OrderItem item)
        {
            var checker = new AvailabilityChecker(item.Article);
            if (!checker.Check(item.From, item.To, item.Amount))
                throw new ArticleNotAvailableException(item);

            var result = Items.Add(item);
            item.Order = this;
            return result;
        }

        public virtual bool AddItem(int articleId, int amount, DateTime begin, DateTime end)
        {
            var item = new OrderItem();
            item.Article = IoC.Resolve<IArticleRepository>().Get(articleId);
            item.Amount = amount;
            item.From = begin;
            item.To = end;
            return AddItem(item);
        }

        public virtual bool RemoveItem(OrderItem item)
        {
            var result = Items.Remove(item);
            item.Order = null;
            return result;
        }

        public virtual void Checkout()
        {
            foreach (var each in Items)
            {
                var checker = new AvailabilityChecker(each.Article);
                if (!checker.Check(each.From, each.To, each.Amount))
                    throw new ArticleNotAvailableException(each);
            }
                
            Status = OrderStatus.Pending;
        }

        public virtual void Approve(User approver)
        {
            Guard.Against<ArgumentNullException>(approver == null, "approver");

            Guard.Against<InvalidOperationException>(Status == OrderStatus.Approved,
                                                     "Die Bestellung wurde bereits bewilligt.");
            Guard.Against<InvalidOperationException>(Status == OrderStatus.Rejected,
                                                     "Die Bestellung wurde bereits abgelehnt.");

            Status = OrderStatus.Approved;
            Modifier = approver;
            ModifyDate = DateTime.Now;
        }

        public virtual void Reject(User rejecter)
        {
            Guard.Against<ArgumentNullException>(rejecter == null, "rejecter");

            Guard.Against<InvalidOperationException>(Status == OrderStatus.Approved,
                                                     "Die Bestellung wurde bereits bewilligt.");
            Guard.Against<InvalidOperationException>(Status == OrderStatus.Rejected,
                                                     "Die Bestellung wurde bereits abgelehnt.");

            Status = OrderStatus.Rejected;
            Modifier = rejecter;
            ModifyDate = DateTime.Now;
        }

        public virtual Stream GeneratePdf()
        {
            var result = new MemoryStream();
            var doc = new Document(PageSize.A4, 0, 0, 36.0f, 36.0f);
            var writer = PdfWriter.GetInstance(doc, result);
            writer.CloseStream = false;
            doc.Open();

            var fontsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts);

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

            var title = new Paragraph("Bestätigung - fundus", FontFactory.GetFont("calibri-bold", 22));
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
            
            var orderNumberCell = new PdfPCell(new Phrase(Id.ToString(),
                FontFactory.GetFont("calibri-bold", 36, BaseColor.WHITE)));
            orderNumberCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            orderNumberCell.Rowspan = 5;
            orderNumberCell.PaddingTop = 0;
            orderNumberCell.PaddingRight = 18.0f;
            orderNumberCell.PaddingBottom += 20; 
            orderNumberCell.BorderWidth = 0;
            orderNumberCell.BackgroundColor = backGroundColor;

            
            table.SetWidths(new int[] { 2, 4, 2 });

            

            PdfPCell cell = null;

            cell = new PdfPCell(new Phrase("Mietende Person:", defaultFontGray));
            cell.BorderWidth = 0;
            cell.BackgroundColor = backGroundColor;
            cell.Padding = 3;
            cell.PaddingLeft = 36.0f;
            table.AddCell(cell);
            table.AddCell(new Phrase("Hans Muster", defaultFont));
            table.AddCell(orderNumberCell);

            cell = new PdfPCell(new Phrase("Abholdatum / -zeit:", defaultFontGray));
            cell.BorderWidth = 0;
            cell.BackgroundColor = backGroundColor;
            cell.Padding = 3;
            cell.PaddingLeft = 36.0f;
            table.AddCell(cell);
            table.AddCell(new Phrase("Donnerstag 12.November 2011 – 17:00", defaultFontBold));
            
            cell = new PdfPCell(new Phrase("Abholort:", defaultFontGray));
            cell.BorderWidth = 0;
            cell.BackgroundColor = backGroundColor;
            cell.Padding = 3;
            cell.PaddingLeft = 36.0f;
            table.AddCell(cell);
            table.AddCell(new Phrase("Pfadisekretariat - Rodteggstrasse 31 - 6005 Luzern", defaultFont));

            cell = new PdfPCell(new Phrase("Rückgabedatum / -zeit:", defaultFontGray));
            cell.BorderWidth = 0;
            cell.BackgroundColor = backGroundColor;
            cell.Padding = 3;
            cell.PaddingLeft = 36.0f;
            table.AddCell(cell);
            table.AddCell(new Phrase("Donnerstag 12.November 2011 – 17:00", defaultFont));

            cell = new PdfPCell(new Phrase("Rückgabeort:", defaultFontGray));
            cell.BorderWidth = 0;
            cell.BackgroundColor = backGroundColor;
            cell.Padding = 3;
            cell.PaddingLeft = 36.0f;
            table.AddCell(cell);
            table.AddCell(new Phrase("Pfadisekretariat - Rodteggstrasse 31 - 6005 Luzern", defaultFont));


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
            
            doc.Close();



            result.Position = 0;
            return result;
        }
    }

    public class ArticleNotAvailableException : Exception
    {
        public ArticleNotAvailableException(OrderItem orderItem)
            : base("Die gewünschte Menge ist im gewünschten Zeitraum nicht vorhanden.")
        {
            OrderItemId = orderItem.Id;
        }

        public int OrderItemId { get; set; }
    }
}