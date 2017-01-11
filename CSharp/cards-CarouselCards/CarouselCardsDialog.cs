namespace CarouselCardsBot
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using System.Net.Http;
    using System.Linq;
    using System.IO;
    using System.Drawing;
    using System.Web;
    using System.Web.Hosting;
    using Gif.Components;
    using System.Drawing.Imaging;
    using System.Drawing.Drawing2D;
    using System.Net;
    using Newtonsoft.Json.Linq;
    [Serializable]
    public class CarouselCardsDialog:IDialog<object>
    {
        int count54 = 0;
        int demhinh = 0;
        byte[] attachmentData = new byte[] { 0x20,0x20,0x20,0x20,0x20,0x20,0x20 };
        static Random rnd = new Random();
        List<string> mylist = new List<string>(new string[] { "gif1a","gif1b","gif2a","gif2b","gif3a","gif3b" });
        List<string> mycolor = new List<string>(new string[] { "yellow","red","yellow","red","yellow","red" });
        List<string> yellow = new List<string>(new string[] { "gif1a","gif2a","gif3a" });
        List<string> red = new List<string>(new string[] { "gif1b","gif2b","gif3b" });
        List<string> loichuc = new List<string>(new string[] {
            "Chúc xuân đoàn viên ấm áp, gia đình sum họp giòn tan tiếng cười",
            "Chúc cho sức khoẻ dồi dào, quyến thân an lạc tuổi vàng bách niên",
            "Chúc bạn xuất hành như ý, đón vạn tin mừng đến nguyên năm",
            "Chúc bạn may mắn vạn lần, bước ra đến cửa có Tài thần theo",
            "Chúc bạn vinh hoa tràn đến cửa, phú quý theo lân đến tận nhà",
            "Chúc bạn phát lộc phát tài, như pháo hoa nở rộn ràng mùa xuân" });


        public static string GetFileFullPath(string path)
        {
            string relName = path.StartsWith("~") ? path : path.StartsWith("/") ? string.Concat("~",path) : path;

            string filePath = relName.StartsWith("~") ? HostingEnvironment.MapPath(relName) : relName;

            return filePath;
        }

        public static void SaveFile(byte[] content,string path)
        {
            string filePath = GetFileFullPath(path);
            // Check folder exist
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }
            // Check file exist
            if (System.IO.File.Exists(filePath))
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                File.Delete(filePath);
                //System.IO.File.Delete(filePath);
            }

            //Save file
            //System.IO.File.Create(filePath)
            using (FileStream str = System.IO.File.OpenWrite(filePath))
            {
                str.Write(content,0,content.Length);
            }
        }

        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        public static void GhepHinhTuGif(string gifId,string color,string Dirpath,string ConversationId,int demhinh,byte[] attachmentData)
        {
            var Width = 500;
            var Height = 416;
            //var hinhNoResize = System.Drawing.Bitmap.FromFile(Path.Combine(Dirpath,ConversationId+"_"+demhinh.ToString()+".jpg"));
            var hinhNoResize = byteArrayToImage(attachmentData);
            var hinh = FixedSize(hinhNoResize,Width,Height,color);

            //for (var i = 0;i<4;i++)
            //{
                var border1 = System.Drawing.Bitmap.FromFile(Path.Combine(Path.Combine(Dirpath,gifId),0.ToString()+".png")); // your source images - assuming they're the same size
                var target1 = new Bitmap(Width,Height,PixelFormat.Format32bppArgb);
                var graphics1 = Graphics.FromImage(target1);
                graphics1.CompositingMode=CompositingMode.SourceOver;
                graphics1.DrawImage(hinh,0,0);
                graphics1.DrawImage(border1,0,0);
                var targetName1 = Path.Combine(Dirpath,ConversationId+"_"+demhinh.ToString()+"demo"+1.ToString()+".jpg");

            var border2 = System.Drawing.Bitmap.FromFile(Path.Combine(Path.Combine(Dirpath,gifId),1.ToString()+".png")); // your source images - assuming they're the same size
            var target2 = new Bitmap(Width,Height,PixelFormat.Format32bppArgb);
            var graphics2 = Graphics.FromImage(target2);
            graphics2.CompositingMode=CompositingMode.SourceOver;
            graphics2.DrawImage(hinh,0,0);
            graphics2.DrawImage(border2,0,0);
            var targetName2 = Path.Combine(Dirpath,ConversationId+"_"+demhinh.ToString()+"demo"+2.ToString()+".jpg");

            var border3 = System.Drawing.Bitmap.FromFile(Path.Combine(Path.Combine(Dirpath,gifId),2.ToString()+".png")); // your source images - assuming they're the same size
            var target3 = new Bitmap(Width,Height,PixelFormat.Format32bppArgb);
            var graphics3 = Graphics.FromImage(target3);
            graphics3.CompositingMode=CompositingMode.SourceOver;
            graphics3.DrawImage(hinh,0,0);
            graphics3.DrawImage(border3,0,0);
            var targetName3 = Path.Combine(Dirpath,ConversationId+"_"+demhinh.ToString()+"demo"+3.ToString()+".jpg");

            var border4 = System.Drawing.Bitmap.FromFile(Path.Combine(Path.Combine(Dirpath,gifId),3.ToString()+".png")); // your source images - assuming they're the same size
            var target4 = new Bitmap(Width,Height,PixelFormat.Format32bppArgb);
            var graphics4 = Graphics.FromImage(target4);
            graphics4.CompositingMode=CompositingMode.SourceOver;
            graphics4.DrawImage(hinh,0,0);
            graphics4.DrawImage(border4,0,0);
            var targetName4 = Path.Combine(Dirpath,ConversationId+"_"+demhinh.ToString()+"demo"+4.ToString()+".jpg");
            // target.Save(targetName,ImageFormat.Jpeg);
            //target.Dispose();
            //}
            
            var Output_File_Path = Path.Combine(Dirpath,ConversationId+"_"+demhinh.ToString()+".gif");

            AnimatedGifEncoder GEncoder = new AnimatedGifEncoder();

            GEncoder.Start(Output_File_Path);
            GEncoder.SetDelay(300);
            GEncoder.SetRepeat(0);
           
            GEncoder.AddFrame(target1);
            GEncoder.AddFrame(target2);
            GEncoder.AddFrame(target3);
            GEncoder.AddFrame(target4);
            GEncoder.Finish();

        }

        public async Task ShowMenu(IDialogContext context)
        {
            var replychon = context.MakeMessage();
            replychon.AttachmentLayout=AttachmentLayoutTypes.Carousel;
            replychon.Attachments=GetCardsAttachments();
            await context.PostAsync(replychon);
        }
     

        public void Create_Animated_GIF(string ConversationId,int demhinh)
        {
            var Dirpath = Path.Combine(HttpRuntime.AppDomainAppPath,"images");
            var Output_File_Path = Path.Combine(Dirpath,ConversationId+"_"+demhinh.ToString()+".gif");

            AnimatedGifEncoder GEncoder = new AnimatedGifEncoder();

            GEncoder.Start(Output_File_Path);
            GEncoder.SetDelay(300);
            GEncoder.SetRepeat(0);

            for (int i = 0;i<4;i++)
            {
                var fileName = Path.Combine(Dirpath,ConversationId+"_"+demhinh.ToString()+"demo"+i.ToString()+".jpg");
                using (var image = System.Drawing.Image.FromFile(fileName))
                {
                    GEncoder.AddFrame(image);
                }
            }

            /*
            try
            {
                System.IO.Directory.Delete(Environment.CurrentDirectory + "\\Temp2", true);
            }
            catch
            {
            }
            */
            GEncoder.Finish();
        }

        static Image FixedSize(Image imgPhoto,int Width,int Height,string color)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;
            var colorbg = Color.Red;
            if (color =="yellow")
            {
                colorbg =Color.FromArgb(255,199,0);
            }
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW=((float)Width/(float)sourceWidth);
            nPercentH=((float)Height/(float)sourceHeight);
            if (nPercentH<nPercentW)
            {
                nPercent=nPercentH;
                destX=System.Convert.ToInt16((Width-
                              (sourceWidth*nPercent))/2);
            }
            else
            {
                nPercent=nPercentW;
                destY=System.Convert.ToInt16((Height-
                              (sourceHeight*nPercent))/2);
            }

            int destWidth = (int)(sourceWidth*nPercent);
            int destHeight = (int)(sourceHeight*nPercent);

            Bitmap bmPhoto = new Bitmap(Width,Height,
                              PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(colorbg);
            
            grPhoto.InterpolationMode=
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX,destY,destWidth,destHeight),
                new Rectangle(sourceX,sourceY,sourceWidth,sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public async Task StartAsync(IDialogContext context)
        {

            context.Wait(this.MessageReceivedAsync);
        }

        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c<'0'||c>'9')
                    return false;
            }

            return true;
        }


        public virtual async Task MessageReceivedAsync(IDialogContext context,IAwaitable<IMessageActivity> argument)
        {
            
            var message = await argument;
            var ConversationId = message.Conversation.Id;
            var userId = message.From.Id;
            if (!IsDigitsOnly(userId))
            {
                userId="1400582053320010";
            }
            
            var Dirpath = Path.Combine(HttpRuntime.AppDomainAppPath,"images");
           
            var Output_File_Path = Path.Combine(Dirpath,ConversationId+"_"+".gif");
            var realServer = "http://ttcl.eduu.vn/images";
            //var realServer = Dirpath;
            if (message.Attachments!=null&&message.Attachments.Any())
            {
                demhinh++;
                var filepath = Path.Combine(Dirpath,ConversationId+"_"+demhinh.ToString()+".jpg");
                var attachment = message.Attachments.First();

                using (HttpClient httpClient = new HttpClient())
                {
                    var attachmentUrl = message.Attachments[0].ContentUrl;
                   
                    attachmentData = await httpClient.GetByteArrayAsync(attachmentUrl);
                    //SaveFile(attachmentData,filepath);
                    int r = rnd.Next(mylist.Count);
                    GhepHinhTuGif((string)mylist[r],(string)mycolor[r],Dirpath,ConversationId,demhinh,attachmentData);
                    //Create_Animated_GIF(ConversationId,demhinh);

                    var replyMessage = context.MakeMessage();
                    replyMessage.Attachments=new List<Attachment>()
                     {
                        new Attachment()
                        {
                            ContentUrl = realServer + "/" + ConversationId+"_"+demhinh.ToString()+  ".gif",
                            ContentType = "image/gif",
                            Name = ConversationId+"_"+ ".gif"
                        }
                     };
                    await context.PostAsync(replyMessage);
                    var replychon = context.MakeMessage();
                    replychon.AttachmentLayout=AttachmentLayoutTypes.Carousel;
                    replychon.Attachments=GetCardsAttachments();
                    await context.PostAsync(replychon);

                }
            }
            else
            {
                var callbackmsg = message.Text;
                if (count54 == 0)
                {
                    await context.PostAsync($"Kinh Đô mừng xuân Đinh Dậu!");
                    await context.PostAsync($"Kinh Đô sẽ biến hoá bộ ảnh Tết xưa - Tết nay tặng bạn. Bạn gửi ảnh nha!");
                    var fbimgURLObj = "https://graph.facebook.com/v2.6/"+userId+"?access_token=EAAaE4y2KvjEBABHmiY9A5MsyMb4lhqBU0arBCtIeEcny4fOY0fEZAZCNl5fh5BixPXiHQslDH8kxZAtsYoDFLPeWyjZBqiPdcXZCq252gHHA70Qogq9OhPGPZC88daWbPeVhyRgNpfSywoa8wEtPe3miT7IhSSdjcZBCQzZBsieVGwZDZD";
                    HttpWebRequest request = WebRequest.Create(fbimgURLObj) as HttpWebRequest;
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {

                        StreamReader reader = new StreamReader(response.GetResponseStream());

                        string retVal = reader.ReadToEnd();

                        var data = "http://media.baodautu.vn/Images/haiyen/2016/01/06/kdo.jpg";
                        if (retVal.Length>10)
                            data=JObject.Parse(retVal)["profile_pic"].ToString();
                        var filepath = Path.Combine(Dirpath,ConversationId+"_"+demhinh.ToString()+".jpg");
                        var dataimg = getUrlImage(data,filepath);
                        int r = rnd.Next(mylist.Count);
                        GhepHinhTuGif((string)mylist[r],(string)mycolor[r],Dirpath,ConversationId,demhinh,dataimg);
                       
                        //Create_Animated_GIF(ConversationId);
                    }
                    var replyMessage = context.MakeMessage();
                    replyMessage.Attachments=new List<Attachment>()
                     {
                        new Attachment()
                        {
                           ContentUrl = realServer + "/" + ConversationId+"_"+demhinh.ToString()+  ".gif",
                            ContentType = "image/gif",
                            Name = ConversationId+"_"+ ".gif"
                        }
                     };
                    await context.PostAsync(replyMessage);
                    count54++;
                }
                else
                {
                    if (!string.IsNullOrEmpty(callbackmsg))
                    {
                        await context.PostAsync($"Tết xưa hay Tết nay? Gửi ảnh và chọn chủ đề là có ngay ảnh xuân như ý.");
                        
                    }
                    
                }
                
                
                if (mylist.Contains(callbackmsg))
                {
                    var color = "red";
                    if (yellow.Contains(callbackmsg))
                    {
                        color="yellow";
                    }
                    GhepHinhTuGif(callbackmsg,color,Dirpath,ConversationId,demhinh,attachmentData);
                    //Create_Animated_GIF(ConversationId);

                    var replyMessage = context.MakeMessage();
                    replyMessage.Attachments=new List<Attachment>()
                     {
                        new Attachment()
                        {
                            ContentUrl = realServer + "/" + ConversationId+"_"+demhinh.ToString()+  ".gif",
                            ContentType = "image/gif",
                            Name = ConversationId+"_"+ ".gif"
                        }
                     };
                    await context.PostAsync(replyMessage);
                    var index = mylist.FindIndex(a => a==callbackmsg);
                    await context.PostAsync(loichuc[index]);
                    var replychon = context.MakeMessage();
                    replychon.AttachmentLayout=AttachmentLayoutTypes.Carousel;
                    replychon.Attachments=GetCardsAttachments();
                    await context.PostAsync(replychon);
                }
                
            }
            context.Wait(this.MessageReceivedAsync);
        }

        public static byte[] getUrlImage(string url,string filepath)
        {
            WebResponse result = null;
            try
            {
                WebRequest request = WebRequest.Create(url);
                result=request.GetResponse();
                Stream stream = result.GetResponseStream();
                BinaryReader br = new BinaryReader(stream);
                byte[] rBytes = br.ReadBytes(1000000);
                br.Close();
                result.Close();
                return rBytes;
                //SaveFile(rBytes,filepath);
            }
            catch (Exception c)
            {
                //MessageBox.Show(c.Message);
                return null;
            }
            finally
            {
                if (result!=null) result.Close();
            }
        }

        private static IList<Attachment> GetCardsAttachments()
        {
            return new List<Attachment>()
            {
                GetHeroCard(
                    "Xưa, Tết có chú thợ ảnh",
                    new CardImage(url: "http://ttcl.eduu.vn/1a.jpg"),
                    new CardAction(ActionTypes.PostBack, "Chọn chủ đề này", value: "gif1a")),
                GetHeroCard(
                    "Nay, Tết có 'dế' xì-mát-phôn",
                    new CardImage(url: "http://ttcl.eduu.vn/1b.jpg"),
                    new CardAction(ActionTypes.PostBack, "Chọn chủ đề này", value: "gif1b")),
                GetHeroCard(
                    "Xưa, Tết du xuân trẩy hội",
                    new CardImage(url: "http://ttcl.eduu.vn/2a.jpg"),
                    new CardAction(ActionTypes.PostBack, "Chọn chủ đề này", value: "gif2a")),
                GetHeroCard(
                    "Nay, Tết rộn chốn thành đô",
                    new CardImage(url: "http://ttcl.eduu.vn/2b.jpg"),
                    new CardAction(ActionTypes.PostBack, "Chọn chủ đề này", value: "gif2b")),
                GetHeroCard(
                    "Xưa, pháo lân mừng Tết",
                    new CardImage(url: "http://ttcl.eduu.vn/3a.jpg"),
                    new CardAction(ActionTypes.PostBack, "Chọn chủ đề này", value: "gif3a")),
                GetHeroCard(
                    "Nay, pháo hoa rợp trời xuân",
                    new CardImage(url: "http://ttcl.eduu.vn/3b.jpg"),
                    new CardAction(ActionTypes.PostBack, "Chọn chủ đề này", value: "gif3b")),
            };
        }

        private static Attachment GetHeroCard(string title,CardImage cardImage,CardAction cardAction)
        {
            var heroCard = new HeroCard
            {
                Title=title,
                Buttons=new List<CardAction>() { cardAction },
                
            };

            return heroCard.ToAttachment();
        }

    }
}