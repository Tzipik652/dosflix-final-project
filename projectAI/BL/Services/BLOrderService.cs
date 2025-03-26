﻿using Bl.Api;
using Dal.Api;
using Bl.Models;
using Dal.Models;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Colors;

namespace Bl.Services
{
    public class BLOrderService : IBLOrders
    {
        IDal dal;
        public BLOrderService(IDal d)
        {
            dal = d;
        }

        public async Task<List<BLOrder>> GetAll()
        {
            List<Order> list = await dal.Order.GetAll();
            return list.Select(o => new BLOrder()
            {
                IdOrder = o.IdOrder,
                IdCustomer = o.IdCustomer,
                DateOrder = o.DateOrder,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
            }).ToList();
        }

        public async Task<List<BLOrder>> GetByIdCustomer(int idC)
        {
            //בדיקה שקיים לקוח כזה 
            List<Order> list =await dal.Order.GetAll();
            var isExist = list.FirstOrDefault(o => o.IdCustomer == idC);
            if(isExist==null)
                throw new Exception("The Customer is not exists in the system");
            else
            {
                List<Order> list1 = await dal.Order.GetOrdersByIdCustomer(idC);
                return list1.Select(o => new BLOrder()
                {
                    IdOrder = o.IdOrder,
                    DateOrder = o.DateOrder,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                }).ToList();
            }
        }

        public async Task<List<BLOrder>> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            //בדיקה שהתאריכים שהתקבלו הגיוניים
            if (startDate>DateTime.Today||endDate>DateTime.Today)
                throw new Exception("Incorrect date range.");
            else
            {
                List<Order> list2 = await dal.Order.GetOrdersByDateRange(startDate, endDate);
                return list2.Select(o => new BLOrder()
                {
                    IdOrder = o.IdOrder,
                    IdCustomer = o.IdCustomer,
                    DateOrder = o.DateOrder,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                }).ToList();
            }

        }

        public async Task<List<BLOrder>> GetOrdersToday()
        {
            List<Order> list3 = await dal.Order.GetOrdersToday();
            return list3.Select(o => new BLOrder()
            {
                IdOrder = o.IdOrder,
                IdCustomer = o.IdCustomer,
                DateOrder = o.DateOrder,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
            }).ToList();
        }


        public async Task<List<BLOrder>> GetOrdersByStatusFalse()
        {
            List<Order> list4 = await dal.Order.GetOrdersByStatusFalse();
            return list4.Select(o => new BLOrder()
            {
                IdOrder = o.IdOrder,
                IdCustomer = o.IdCustomer,
                DateOrder = o.DateOrder,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
            }).ToList();
        }

        public static async Task GenerateOrdersPdf(IEnumerable<BLOrder> orders)
        {
            //יצירת שם קובץ עם תאריך
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string fileName = $"OrdersReport {date}.pdf";
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            string directoryPath = Path.Combine(projectDirectory, "PDFs Orders");

            //אם התיקיה לא קיימת, ניצור אותה
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            //יצירת נתיב מלא לקובץ PDF
            string filePath = Path.Combine(directoryPath, fileName);

            //יצירת קובץ PDF
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (PdfWriter writer = new PdfWriter(fs))
                {
                    using (PdfDocument pdf = new PdfDocument(writer))
                    {
                        Document document = new Document(pdf);

                        //יצירת גופנים
                        PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                        PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                        //הוספת תאריך בראש העמוד בצד שמאל
                        Paragraph dateParagraph = new Paragraph(DateTime.Now.ToString("dd/MM/yyyy"))
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetFont(regularFont)
                            .SetFontSize(10)
                            .SetMarginTop(20);
                        document.Add(dateParagraph);

                        //כותרת ראשית עם עיצוב
                        Paragraph title = new Paragraph("List of All Orders").SetTextAlignment(TextAlignment.CENTER)
                            .SetFont(boldFont)
                            .SetFontSize(24)
                            .SetMarginTop(20)
                            .SetMarginBottom(20);
                           // .SetBackgroundColor(ColorConstants.CYAN); // הוספת צבע רקע לכותרת

                        // הוספת קו תחתון לכותרת
                        //title.SetBorderBottom(new SolidBorder(ColorConstants.BLACK, 1));

                        document.Add(title);

                        //יצירת טבלה עם כותרות
                        Table table = new Table(5);
                        table.SetWidth(UnitValue.CreatePercentValue(100));
                        table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                        //כותרות בטבלה עם עיצוב
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Order ID").SetFont(boldFont).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(ColorConstants.LIGHT_GRAY)));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Order Date").SetFont(boldFont).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(ColorConstants.LIGHT_GRAY)));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Status Order").SetFont(boldFont).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(ColorConstants.LIGHT_GRAY)));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Total Amount").SetFont(boldFont).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(ColorConstants.LIGHT_GRAY)));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Is Paid?").SetFont(boldFont).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(ColorConstants.LIGHT_GRAY)));
                        //מילוי הנתונים בטבלה
                        foreach (var order in orders)
                        {
                            table.AddCell(new Cell().Add(new Paragraph(order.IdOrder.ToString()).SetTextAlignment(TextAlignment.CENTER).SetFont(regularFont)));
                            table.AddCell(new Cell().Add(new Paragraph(order.DateOrder.ToString("dd/MM/yyyy")).SetTextAlignment(TextAlignment.CENTER).SetFont(regularFont)));
                            table.AddCell(new Cell().Add(new Paragraph(order.Status ? "Completed" : "Not completed").SetTextAlignment(TextAlignment.CENTER).SetFont(regularFont)));
                            table.AddCell(new Cell().Add(new Paragraph(order.TotalAmount.ToString("N2")).SetTextAlignment(TextAlignment.CENTER).SetFont(regularFont)));
                           // table.AddCell(new Cell().Add(new Paragraph(order.IsPaid ? "Paid" : "Unpaid").SetTextAlignment(TextAlignment.CENTER).SetFont(regularFont)));
                        }
                        document.Add(table);
                        document.Close();
                    }
                }
            }
            Console.WriteLine($"PDF file successfully created at: {filePath}");
        }
    }
}

