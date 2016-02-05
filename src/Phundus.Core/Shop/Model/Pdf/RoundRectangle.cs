namespace Phundus.Shop.Orders.Services
{
    using iTextSharp.text;
    using iTextSharp.text.pdf;

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