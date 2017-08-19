using System;
using System.Web;
using System.Drawing;

namespace HelpClassLib.Web
{
    /// <summary>
    /// ��ɫ�����֤��������
    /// </summary>
    public class VerifyCode
    {
        // Properties

        #region ��֤�볤��(Ĭ��4����֤��ĳ���)
        int length = 4;
        public int Length
        {
            get { return length; }
            set { length = value; }
        }
        #endregion

        #region ��֤�������С(Ĭ��12����)
        int fontSize = 12;
        public int FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }
        #endregion

        #region �߿�(Ĭ��2����)
        int padding = 2;
        public int Padding
        {
            get { return padding; }
            set { padding = value; }
        }
        #endregion

        #region �Ƿ�������(Ĭ�ϲ����)
        bool chaos = false;
        public bool Chaos
        {
            get { return chaos; }
            set { chaos = value; }
        }
        #endregion

        #region ���������ɫ(Ĭ�ϻ�ɫ)
        Color chaosColor = Color.LightGray;
        public Color ChaosColor
        {
            get { return chaosColor; }
            set { chaosColor = value; }
        }
        #endregion

        #region �Զ��屳��ɫ(Ĭ�ϰ�ɫ)
        Color backgroundColor = Color.White;
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }
        #endregion

        #region �Զ��������ɫ����
        Color[] colors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
        public Color[] Colors
        {
            get { return colors; }
            set { colors = value; }
        }
        #endregion

        #region �Զ�����������
        string[] fonts = { "Arial", "Georgia" };
        public string[] Fonts
        {
            get { return fonts; }
            set { fonts = value; }
        }
        #endregion

        #region �Զ���������ַ�������(ʹ�ö��ŷָ�)
        string codeSerial = "2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";
        public string CodeSerial
        {
            get { return codeSerial; }
            set { codeSerial = value; }
        }
        #endregion

        // Methods

        #region ����У����ͼƬ
        /// <summary>
        /// ����У����ͼƬ
        /// </summary>
        /// <param name="code">�Ѿ����ɵ������</param>
        /// <returns>λͼ</returns>
        public Bitmap CreateImageCode(string code)
        {
            int fSize = FontSize;
            int fWidth = fSize + Padding;

            int imageWidth = (int)(code.Length * fWidth) + 4 + Padding * 2;
            int imageHeight = fSize * 2 + Padding;

            System.Drawing.Bitmap image = new System.Drawing.Bitmap(imageWidth, imageHeight);

            Graphics g = Graphics.FromImage(image);

            g.Clear(BackgroundColor);

            Random rand = new Random();

            //���������������ɵ����
            if (this.Chaos)
            {

                Pen pen = new Pen(ChaosColor, 0);
                int c = Length * 10;

                for (int i = 0; i < c; i++)
                {
                    int x = rand.Next(image.Width);
                    int y = rand.Next(image.Height);

                    g.DrawRectangle(pen, x, y, 1, 1);
                }
            }

            int left = 0, top = 0, top1 = 1, top2 = 1;

            int n1 = (imageHeight - FontSize - Padding * 2);
            int n2 = n1 / 4;
            top1 = n2;
            top2 = n2 * 2;

            Font f;
            Brush b;

            int cindex, findex;

            //����������ɫ����֤���ַ�
            for (int i = 0; i < code.Length; i++)
            {
                cindex = rand.Next(Colors.Length - 1);
                findex = rand.Next(Fonts.Length - 1);

                f = new System.Drawing.Font(Fonts[findex], fSize, System.Drawing.FontStyle.Bold);
                b = new System.Drawing.SolidBrush(Colors[cindex]);

                if (i % 2 == 1)
                {
                    top = top2;
                }
                else
                {
                    top = top1;
                }

                left = i * fWidth;

                g.DrawString(code.Substring(i, 1), f, b, left, top);
            }

            //��һ���߿�
            g.DrawRectangle(new Pen(Color.Black, 0), 0, 0, image.Width - 1, image.Height - 1);

            g.Dispose();

            return image;
        }
        #endregion

        #region �������õ�ͼƬ�����ҳ��
        /// <summary>
        /// �������õ�ͼƬ�����ҳ��
        /// </summary>
        /// <param name="code">�Ѿ����ɵ������</param>
        /// <param name="context">Web������</param>
        public void CreateImageOnPage(string code, HttpContext context)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            Bitmap image = this.CreateImageCode(code);

            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            context.Response.ClearContent();
            context.Response.ContentType = "image/Jpeg";
            context.Response.BinaryWrite(ms.GetBuffer());

            ms.Close();
            ms = null;
            image.Dispose();
            image = null;
        }
        #endregion

        #region ��������ַ���
        /// <summary>
        /// ��������ַ���
        /// </summary>
        /// <param name="codeLen">����볤��</param>
        /// <returns>�����������</returns>
        public string CreateVerifyCode(int codeLen)
        {
            if (codeLen == 0)
            {
                codeLen = Length;
            }

            string[] arr = CodeSerial.Split(',');

            string code = "";

            int randValue = -1;

            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < codeLen; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);

                code += arr[randValue];
            }

            return code;
        }
        public string CreateVerifyCode()
        {
            return CreateVerifyCode(0);
        }
        #endregion

    }
}
