using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using System.Collections.Generic;
using System.IO;

namespace PDFMerge
{
    class Program
    {
        static void Main(string[] args)
        {
            //reading byte of pdf file from location, in web app also after taking input we can use file byte 
            var pdf1 = File.ReadAllBytes(@"D:\pdf\ChessEndgame.pdf");
            var pdf2 = File.ReadAllBytes(@"D:\pdf\Queen'sGambit.pdf");
            var pdfList = new List<byte[]> { pdf1, pdf2 };
            var mergePdf=Combine(pdfList);//accepting merged file byte
            //file  path where i want to save file with name and extension
            string filePath = @"D:\pdf\newpdf.pdf";
            //writing byte to file at provided path
            File.WriteAllBytes(filePath, mergePdf);
        }
         public static byte[] Combine(IEnumerable<byte[]> pdfs)
         {
            using (var writerMemoryStream = new MemoryStream())
            {
                using (var writer = new PdfWriter(writerMemoryStream))
                {
                    using (var mergedDocument = new iText.Kernel.Pdf.PdfDocument(writer))
                    {
                        var merger = new PdfMerger(mergedDocument);

                        foreach (var pdfBytes in pdfs)
                        {
                            using (var copyFromMemoryStream = new MemoryStream(pdfBytes))
                            {
                                using (var reader = new iText.Kernel.Pdf.PdfReader(copyFromMemoryStream))
                                {
                                    //have to set unethical reading to true else will get password error 
                                    reader.SetUnethicalReading(true);
                                    using (var copyFromDocument = new iText.Kernel.Pdf.PdfDocument(reader))
                                    {
                                        //second parameter 1 is page number from where to start merge
                                        merger.Merge(copyFromDocument, 1, copyFromDocument.GetNumberOfPages());
                                    }
                                }
                            }
                        }
                    }
                }
                return writerMemoryStream.ToArray();
            }
         }
        //private static string[] GetFiles()
        //{
        //    DirectoryInfo di = new DirectoryInfo(@"D:\pdf");
        //    FileInfo[] files = di.GetFiles("*.pdf");
        //    int i = 0;
        //    string[] names = new string[files.Length];
        //    foreach (var r in files)
        //    {
        //        names[i] = r.FullName;
        //        i = i + 1;
        //    }
        //    return names;
        //    //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        //    //string[] files = GetFiles();
        //    //PdfDocument outputDocument = new PdfDocument();
        //    //foreach (string file in files)
        //    //{
        //    //    PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);
        //    //    int count = inputDocument.PageCount;
        //    //    for (int id = 0; id < count; id++)
        //    //    {
        //    //        PdfPage page = inputDocument.Pages[id];
        //    //        outputDocument.AddPage(page);
        //    //    }
        //    //}
        //    //const string filename = @"D:\pdf\newpdf.pdf";
        //    //outputDocument.Save(string.Format(filename));
        //}
    }
}
