using System;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Database;
using Java.IO;
using Android.Webkit;
using System.Threading.Tasks;

namespace Plugin.FilePicker
{
    public class IOUtil
    {

        public static string getPath (Context context, Android.Net.Uri uri)
        {
            bool isKitKat = Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat;

            // DocumentProvider
            if (isKitKat && DocumentsContract.IsDocumentUri (context, uri)) {
                // ExternalStorageProvider
                if (isExternalStorageDocument (uri)) {
                    var docId = DocumentsContract.GetDocumentId (uri);
                    string [] split = docId.Split (':');
                    var type = split [0];

                    if ("primary".Equals (type, StringComparison.OrdinalIgnoreCase)) {
                        return Android.OS.Environment.ExternalStorageDirectory + "/" + split [1];
                    }

                    // TODO handle non-primary volumes
                }
                // DownloadsProvider
                else if (isDownloadsDocument (uri)) {

                    string id = DocumentsContract.GetDocumentId (uri);
                    Android.Net.Uri contentUri = ContentUris.WithAppendedId (
                            Android.Net.Uri.Parse ("content://downloads/public_downloads"), long.Parse (id));

                    return getDataColumn (context, contentUri, null, null);
                }
                // MediaProvider
                else if (isMediaDocument (uri)) {
                    var docId = DocumentsContract.GetDocumentId (uri);
                    string [] split = docId.Split (':');
                    var type = split [0];

                    Android.Net.Uri contentUri = null;
                    if ("image".Equals (type)) {
                        contentUri = MediaStore.Images.Media.ExternalContentUri;
                    } else if ("video".Equals (type)) {
                        contentUri = MediaStore.Video.Media.ExternalContentUri;
                    } else if ("audio".Equals (type)) {
                        contentUri = MediaStore.Audio.Media.ExternalContentUri;
                    }

                    var selection = "_id=?";
                    var selectionArgs = new string [] {
                        split[1]
                    };

                    return getDataColumn (context, contentUri, selection, selectionArgs);
                }
            }
            // MediaStore (and general)
            else if ("content".Equals (uri.Scheme, StringComparison.OrdinalIgnoreCase)) {
                return getDataColumn (context, uri, null, null);
            }
            // File
            else if ("file".Equals (uri.Scheme, StringComparison.OrdinalIgnoreCase)) {
                return uri.Path;
            }

            return null;
        }

        public static string getDataColumn (Context context, Android.Net.Uri uri, string selection,
        string [] selectionArgs)
        {

            ICursor cursor = null;
            var column = "_data";
            string [] projection = {
                column
            };

            try {
                cursor = context.ContentResolver.Query (uri, projection, selection, selectionArgs,
                        null);
                if (cursor != null && cursor.MoveToFirst ()) {
                    int column_index = cursor.GetColumnIndexOrThrow (column);
                    return cursor.GetString (column_index);
                }
            } finally {
                if (cursor != null)
                    cursor.Close ();
            }
            return null;
        }

        /**
         * @param uri The Uri to check.
         * @return Whether the Uri authority is ExternalStorageProvider.
         */
        public static bool isExternalStorageDocument (Android.Net.Uri uri)
        {
            return "com.android.externalstorage.documents".Equals (uri.Authority);
        }

        /**
         * @param uri The Uri to check.
         * @return Whether the Uri authority is DownloadsProvider.
         */
        public static bool isDownloadsDocument (Android.Net.Uri uri)
        {
            return "com.android.providers.downloads.documents".Equals (uri.Authority);
        }

        /**
         * @param uri The Uri to check.
         * @return Whether the Uri authority is MediaProvider.
         */
        public static bool isMediaDocument (Android.Net.Uri uri)
        {
            return "com.android.providers.media.documents".Equals (uri.Authority);
        }

        public static byte [] readFile (string file)
        {
            try {
                return readFile (new File (file));
            } catch (Exception ex) {
                System.Diagnostics.Debug.Write (ex);
                return new byte [0];
            }
        }

        public static byte [] readFile (File file)
        {
            // Open file
            var f = new RandomAccessFile (file, "r");

            try {
                // Get and check length
                long longlength = f.Length ();
                var length = (int)longlength;

                if (length != longlength)
                    throw new IOException ("Filesize exceeds allowed size");
                // Read file and return data
                byte [] data = new byte [length];
                f.ReadFully (data);
                //String content = System.Text.Encoding.UTF8.GetString(data);
                return data;
            } catch (Exception ex) {
                System.Diagnostics.Debug.Write (ex);
                return new byte [0];
            } finally {
                f.Close ();
            }
        }

        public static string GetMimeType (string url)
        {
            string type = null;
            var extension = MimeTypeMap.GetFileExtensionFromUrl (url);

            if (extension != null) {
                type = MimeTypeMap.Singleton.GetMimeTypeFromExtension (extension);
            }

            return type;
        }
    }
}