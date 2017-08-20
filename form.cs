private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.openFileDialog1.ShowDialog() != DialogResult.OK) return;
                PostDa(this.openFileDialog1.FileName, File.ReadAllBytes(this.openFileDialog1.FileName));
            }
            catch (Exception ex)
            {
                this.richTextBox1.Text = ex.Message;
            }
        }

        private async void PostDa(string filename, byte[] bytes)
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    var values = new[] { new KeyValuePair<string, string>("companyID", "10"), new KeyValuePair<string, string>("uploadType", "3"), new KeyValuePair<string, string>("tokenKey", "f4bcbc76-b88d-4e6f-bf73-388df2a91001") };
                    foreach (var keyValuePair in values)
                        content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                    var fileContent = new ByteArrayContent(bytes);
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "attachment", FileName = filename };
                    content.Add(fileContent);
                    var response = await client.PostAsync("http://127.0.0.1:8888", content);
                    response.EnsureSuccessStatusCode();
                    var responseBody = await response.Content.ReadAsStringAsync();
                    this.richTextBox1.Text = responseBody;
                }
            }
        }
