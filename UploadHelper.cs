  public partial class UploadFile : Form
    {
        private static readonly string apiUrl = "http://localhost:13843/api/upload";
        //private static readonly string apiUrl = "http://127.0.0.1:8888";

        public UploadFile()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() != DialogResult.OK) return;
            var fileName = this.openFileDialog1.FileName;
            var bytes = File.ReadAllBytes(fileName);
            try
            {
                //var result = UploadHelper.MyUploader(apiUrl, fileName, bytes);
                var info = new UploadInfo
                {
                    url = new Uri(apiUrl),
                    ComparyId = "13",
                    UploadType = "1",
                    TokenKey = "f4bcbc76-b88d-4e6f-bf73-388df2a91001",
                    FileName = fileName,
                    Bytes = bytes
                };
                var result = UploadHelper.PostDataFile(info);
                this.richTextBox1.Text = result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public class UploadHelper
    {
        public static string MyUploader(string url, string fileName, byte[] bytes)
        {
            var strBoundary = "--f4bcbc76-b88d-4e6f-bf73-38ddf2391001";
            var strbtyes = Encoding.UTF8.GetBytes("\r\n" + strBoundary + "--\r\n");
            var sb = new StringBuilder();
            //第一个参数(companyID)
            sb.Append(strBoundary);
            sb.Append("\r\n");
            sb.Append("Content-Type: text/plain; charset=utf-8");
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=companyID");
            sb.Append("\r\n\r\n");
            sb.Append("10");
            sb.Append("\r\n");
            //第二个参数(uploadType)
            sb.Append(strBoundary);
            sb.Append("\r\n");
            sb.Append("Content-Type: text/plain; charset=utf-8");
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=uploadType");
            sb.Append("\r\n\r\n");
            sb.Append("3");
            sb.Append("\r\n");
            //第三个参数(tokenKey)
            sb.Append(strBoundary);
            sb.Append("\r\n");
            sb.Append("Content-Type: text/plain; charset=utf-8");
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=tokenKey");
            sb.Append("\r\n\r\n");
            sb.Append("f4bcbc76-b88d-4e6f-bf73-388df2a91001");
            sb.Append("\r\n");
            //第四个参数(文件信息)
            sb.Append(strBoundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=attachment; filename=\"" + fileName + "\"");
            sb.Append("\r\n\r\n");
            var headerBytes = Encoding.UTF8.GetBytes(sb.ToString());
            var request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.ContentType = "multipart/form-data; boundary=\"f4bcbc76-b88d-4e6f-bf73-38ddf2391001\"";
            request.Method = "POST";
            request.AllowWriteStreamBuffering = false;
            request.KeepAlive = true;
            request.ServicePoint.Expect100Continue = true;
            //request.Expect = "100-continue";
            //长度
            request.ContentLength = headerBytes.Length + bytes.Length + strbtyes.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(headerBytes, 0, headerBytes.Length);
                stream.Write(bytes, 0, bytes.Length);
                stream.Write(strbtyes, 0, strbtyes.Length);
                using (var oWResponse = request.GetResponse())
                {
                    using (var s = oWResponse.GetResponseStream())
                    {
                        var sr = new StreamReader(s);
                        return sr.ReadToEnd();
                    }
                }
            }
        }

        public static string PostDataFile(UploadInfo info)
        {
            #region 
            if (info == null) throw new ArgumentNullException(nameof(info));
            if (string.IsNullOrEmpty(info.ComparyId)) throw new ArgumentNullException(info.ComparyId);
            if (string.IsNullOrEmpty(info.UploadType)) throw new ArgumentNullException(info.UploadType);
            if (string.IsNullOrEmpty(info.TokenKey)) throw new ArgumentNullException(info.TokenKey);
            if (string.IsNullOrEmpty(info.FileName)) throw new ArgumentNullException(info.FileName);
            int tmpInt;
            if (!int.TryParse(info.ComparyId, out tmpInt)) throw new InvalidCastException(info.FileName);
            if (!int.TryParse(info.UploadType, out tmpInt)) throw new InvalidCastException(info.UploadType);
            if (info.Bytes == null || info.Bytes.Length < 1) throw new ArgumentException("info.Bytes");
            #endregion

            #region build Boundary
            var tmpGuid = Guid.NewGuid();
            var strBoundary = "--" + tmpGuid;
            var strbtyes = Encoding.UTF8.GetBytes("\r\n" + strBoundary + "--\r\n");
            var sb = new StringBuilder();
            //第一个参数(companyID)
            sb.Append(strBoundary);
            sb.Append("\r\n");
            sb.Append("Content-Type: text/plain; charset=utf-8");
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=companyID");
            sb.Append("\r\n\r\n");
            sb.Append(info.ComparyId);
            sb.Append("\r\n");
            //第二个参数(uploadType)
            sb.Append(strBoundary);
            sb.Append("\r\n");
            sb.Append("Content-Type: text/plain; charset=utf-8");
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=uploadType");
            sb.Append("\r\n\r\n");
            sb.Append(info.UploadType);
            sb.Append("\r\n");
            //第三个参数(tokenKey)
            sb.Append(strBoundary);
            sb.Append("\r\n");
            sb.Append("Content-Type: text/plain; charset=utf-8");
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=tokenKey");
            sb.Append("\r\n\r\n");
            sb.Append(info.TokenKey);
            sb.Append("\r\n");
            //第四个参数(文件信息)
            sb.Append(strBoundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=attachment; filename=\"" + info.FileName + "\"");
            sb.Append("\r\n\r\n");
            #endregion
            var headerBytes = Encoding.UTF8.GetBytes(sb.ToString());
            var request = (HttpWebRequest)WebRequest.Create(info.url);
            request.ContentType = "multipart/form-data; boundary=\"" + tmpGuid + "\"";
            request.Method = "POST";
            request.AllowWriteStreamBuffering = false;
            request.KeepAlive = true;
            request.Timeout = 1000 * 60 * 5;
            request.ServicePoint.Expect100Continue = true;
            request.ContentLength = headerBytes.Length + info.Bytes.Length + strbtyes.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(headerBytes, 0, headerBytes.Length);
                stream.Write(info.Bytes, 0, info.Bytes.Length);
                stream.Write(strbtyes, 0, strbtyes.Length);
                using (var oWResponse = request.GetResponse())
                {
                    using (var s = oWResponse.GetResponseStream())
                    {
                        if (s == null) throw new ArgumentNullException(info.ComparyId);
                        return (new StreamReader(s)).ReadToEnd();
                    }
                }
            }
        }
    }

    public sealed class UploadInfo
    {
        public Uri url { get; set; }
        public string FileName { get; set; }
        public string ComparyId { get; set; }
        public string UploadType { get; set; }
        public string TokenKey { get; set; }
        public byte[] Bytes { get; set; }
    }
