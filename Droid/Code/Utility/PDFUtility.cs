using System;
using System.Collections.Generic;
using System.IO;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Pdf;
using Android.Views;

namespace actchargers.Droid
{
    public class PDFUtility
    {
        public static string GeneratePDF(Context context, int width, int height, List<Bitmap> bmps,string exportedFileName)
        {
            try
            {
                View documentView = new CustomView(context, CreateBitmap(width, height, bmps));
                //Pdf code check
                string filePath = GetFilePath(exportedFileName);
                PdfDocument pdfDoc = new PdfDocument();
                PdfDocument.PageInfo info = new PdfDocument.PageInfo.Builder(width,height, 1).Create();
                PdfDocument.Page page = pdfDoc.StartPage(info);

                View content = documentView;
                content.Draw(page.Canvas);

                pdfDoc.FinishPage(page);
                var stream = new FileStream(filePath, FileMode.Create);
                pdfDoc.WriteTo(stream);
                //close the document
                pdfDoc.Close();
                stream.Close();
                //End of PDF code check 
                return filePath;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }

       private static string GetFilePath(string exportedFileName)
        {
            var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            var filePath = System.IO.Path.Combine(sdCardPath, exportedFileName);
            bool isExist = System.IO.File.Exists(filePath);
            if (isExist)
            {
                File.Delete(filePath);
            }
            return filePath;
        }

        private static bool DeleteFile(string filePath)
        {
            try
            {
                bool isExist = System.IO.File.Exists(filePath);
                if (isExist)
                {
                    File.Delete(filePath);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return false;
        }

        private static Bitmap CreateBitmap(int width,int height,List<Bitmap> bmps)
        {
            Bitmap bigbitmap = Bitmap.CreateBitmap(width, height,
                                                  Bitmap.Config.Argb8888);
            Canvas bigcanvas = new Canvas(bigbitmap);
            bigcanvas.DrawColor(Color.White);
            Paint paint = new Paint();
            int iHeight = 0;
            for (int i = 0; i < bmps.Count; i++)
            {
                Bitmap bmp = bmps[i];//.Copy(Bitmap.Config.Rgb565, false);
                bigcanvas.DrawBitmap(bmp, 0, iHeight, paint);
                iHeight += bmp.Height;
                bmp.Recycle();
                bmp = null;
                System.GC.Collect();
            }

            return bigbitmap;
        }

    }
}
