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
        public static void GhepHinhTuGif(string gifId,string color,string Dirpath,string ConversationId,int demhinh)
        {
            var Width = 500;
            var Height = 416;
            var hinhNoResize = System.Drawing.Bitmap.FromFile(Path.Combine(Dirpath,ConversationId+"_"+demhinh.ToString()+".jpg"));
            var hinh = FixedSize(hinhNoResize,Width,Height,color);

            for (var i = 0;i<4;i++)
            {
                var border = System.Drawing.Bitmap.FromFile(Path.Combine(Path.Combine(Dirpath,gifId),i.ToString()+".png")); // your source images - assuming they're the same size
                var target = new Bitmap(Width,Height,PixelFormat.Format32bppArgb);
                var graphics = Graphics.FromImage(target);
                graphics.CompositingMode=CompositingMode.SourceOver;
                graphics.DrawImage(hinh,0,0);
                graphics.DrawImage(border,0,0);
                var targetName = Path.Combine(Dirpath,ConversationId+"_"+demhinh.ToString()+"demo"+i.ToString()+".jpg");
                target.Save(targetName,ImageFormat.Jpeg);
                target.Dispose();
            }

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
            count54++;
            
            //await context.PostAsync(count.ToString());
            var message = await argument;
            var ConversationId = message.Conversation.Id;
            var userId = message.From.Id;
            if (!IsDigitsOnly(userId))
            {
                userId="1400582053320010";
            }
            //await context.PostAsync(userId);
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
                    await context.PostAsync(attachmentUrl);
                    var attachmentData = await httpClient.GetByteArrayAsync(attachmentUrl);
                    SaveFile(attachmentData,filepath);
                    int r = rnd.Next(mylist.Count);
                    GhepHinhTuGif((string)mylist[r],(string)mycolor[r],Dirpath,ConversationId,demhinh);
                    Create_Animated_GIF(ConversationId,demhinh);

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
            //else
            //{

            //    var callbackmsg = message.Text;
            //    var color = "red";
            //    var index = mylist.FindIndex(a => a == callbackmsg);
            //    if (mylist.Contains(callbackmsg))
            //    {
            //        if (yellow.Contains(callbackmsg))
            //        {
            //            color="yellow";
            //        }
            //        GhepHinhTuGif(callbackmsg,color,Dirpath,ConversationId);
            //        Create_Animated_GIF(ConversationId);

            //        var replyMessage = context.MakeMessage();
            //        replyMessage.Attachments=new List<Attachment>()
            //         {
            //            new Attachment()
            //            {
            //                ContentUrl = realServer + "/" + ConversationId+"_"+ ".gif",
            //                ContentType = "image/gif",
            //                Name = ConversationId+"_"+ ".gif"
            //            }
            //         };
            //        await context.PostAsync(replyMessage);
            //        //await context.PostAsync(loichuc[index]);
            //        var replychon = context.MakeMessage();
            //        replychon.AttachmentLayout=AttachmentLayoutTypes.Carousel;
            //        replychon.Attachments=GetCardsAttachments();
            //        await context.PostAsync(replychon);
            //    }
            //    else {
            //        if ((count54>1)&&(callbackmsg!="facebook"))
            //        {
            //            await context.PostAsync($"Tết xưa hay Tết nay? Gửi ảnh và chọn chủ đề là có ngay ảnh xuân như ý.");

            //        }
            //        else
            //        {
            //            await context.PostAsync($"Kinh Đô mừng xuân Đinh Dậu!");
            //            await context.PostAsync($"Kinh Đô sẽ biến hoá bộ ảnh Tết xưa - Tết nay tặng bạn. Bạn gửi ảnh nha!");

            //            var fbimgURLObj = "https://graph.facebook.com/v2.6/"+userId+"?access_token=EAAaE4y2KvjEBABHmiY9A5MsyMb4lhqBU0arBCtIeEcny4fOY0fEZAZCNl5fh5BixPXiHQslDH8kxZAtsYoDFLPeWyjZBqiPdcXZCq252gHHA70Qogq9OhPGPZC88daWbPeVhyRgNpfSywoa8wEtPe3miT7IhSSdjcZBCQzZBsieVGwZDZD";
            //            HttpWebRequest request = WebRequest.Create(fbimgURLObj) as HttpWebRequest;
            //            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            //            {

            //                StreamReader reader = new StreamReader(response.GetResponseStream());

            //                string retVal = reader.ReadToEnd();

            //                var data = "http://media.baodautu.vn/Images/haiyen/2016/01/06/kdo.jpg";
            //                if (retVal.Length > 10)
            //                data = JObject.Parse(retVal)["profile_pic"].ToString();

            //                getUrlImage(data,filepath);
            //                int r = rnd.Next(mylist.Count);
            //                GhepHinhTuGif((string)mylist[r],(string)mycolor[r],Dirpath,ConversationId);
            //                Create_Animated_GIF(ConversationId);
            //            }
            //            var replyMessage = context.MakeMessage();
            //            replyMessage.Attachments=new List<Attachment>()
            //         {
            //            new Attachment()
            //            {
            //                ContentUrl = realServer + "/" + ConversationId+"_"+ ".gif",
            //                ContentType = "image/gif",
            //                Name = ConversationId+"_"+ ".gif"
            //            }
            //         };
            //            await context.PostAsync(replyMessage);
                        
            //        }
            //    }
            //}

            context.Wait(this.MessageReceivedAsync);

        }

        public static void getUrlImage(string url,string filepath)
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
                SaveFile(rBytes,filepath);
            }
            catch (Exception c)
            {
                //MessageBox.Show(c.Message);
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