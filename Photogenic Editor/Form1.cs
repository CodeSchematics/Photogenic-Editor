using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

using MaterialSkin;
using MaterialSkin.Controls;

namespace Photogenic_Editor
{
    public partial class Form1 : MaterialForm
    {
        Image mImage;
        Boolean mIsOpen = false; // is image currently open?

        public Form1()
        {
            InitializeComponent();

            pictureBox1.Image = Photogenic_Editor.Properties.Resources.stock;

            pictureBox1.MouseDown += new MouseEventHandler(pictureBox1_MouseDown);
            pictureBox1.MouseUp += new MouseEventHandler(pictureBox1_MouseUp);
            pictureBox1.MouseMove += new MouseEventHandler(pictureBox1_MouseMove);
            pictureBox1.Paint += new PaintEventHandler(pictureBox1_Paint);


            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;


            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Blue400, Primary.Blue500,
                Primary.Blue500, Accent.LightBlue200,
                TextShade.WHITE
            );
        }

        private void OpenImage()
        {
            DialogResult dialogResult = openFileDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                mImage = Image.FromFile(openFileDialog1.FileName);
                pictureBox1.Image = mImage;
                mIsOpen = true;
            }
        }

        private void Reload()
        {
            if (!mIsOpen)
            {
                // MessageBox.Show("Open an Image and then apply changes
            }
            else
            {
                if (mIsOpen)
                {
                    mImage = Image.FromFile(openFileDialog1.FileName);
                    pictureBox1.Image = mImage;
                    mIsOpen = true;
                }
            }
        }

        private void SaveImage()
        {
            if (mIsOpen)
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Images|*.png;*.bmp;*.jpg";
                ImageFormat format = ImageFormat.Png; // default
                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string extention = Path.GetExtension(saveDialog.FileName);
                    switch (extention)
                    {
                        case ".jpg":
                            format = ImageFormat.Jpeg;
                            break;
                        case ".bmp":
                            format = ImageFormat.Bmp;
                            break;
                    }
                    pictureBox1.Image.Save(saveDialog.FileName, format);
                }
                else
                {
                    MessageBox.Show("You must open an image");
                }
            }
            else
            {
                MessageBox.Show("No image open");
            }
        }

        // click listeners

        // Picture Box
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        
        private void pictureBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            
        }

        // Draw Rectangle
        //
        private void pictureBox1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            
        }
        
        private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        // button 1 - Restore
        private void button1_Click(object sender, EventArgs e)
        {
            Reload();
        }

        // button 2 (filter2) - grayscale
        private void button2_Click(object sender, EventArgs e)
        {
            Reload(); // reload the image (set to original) so that effect doesn't multiply
            grayscale();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Reload();
            filter1();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Reload();
            filter2();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Reload();
            filter3();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Reload();
            filter4();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Reload();
            filter5();

        }

        // flip
        private void button11_Click(object sender, EventArgs e)
        {
            if (!mIsOpen)
            {
                MessageBox.Show("Open an Image then apply changes");
            }
            else
            {
                Image flipedImage = pictureBox1.Image;
                flipedImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                pictureBox1.Image = flipedImage;
            }
        }

        // rotate
        private void button12_Click(object sender, EventArgs e)
        {
            if (!mIsOpen)
            {
                MessageBox.Show("Open an Image then apply changes");
            }
            else
            {
                Image rotatedImage = pictureBox1.Image;
                rotatedImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                pictureBox1.Image = rotatedImage;
            }
        }

        // Open Image Button
        private void button8_Click_1(object sender, EventArgs e)
        {
            OpenImage();
        }

        // Save Image Button
        private void button9_Click(object sender, EventArgs e)
        {
            SaveImage();
        }

        // filters
        // TODO: add more filters

        void grayscale()
        {
            if (!mIsOpen)
            {
                MessageBox.Show("Open an Image then apply changes");
            }
            else
            {
                Image image = pictureBox1.Image;
                Bitmap bmpInverted = new Bitmap(image.Width, image.Height);

                ImageAttributes attributes = new ImageAttributes();

                //Dim cm As ColorMatrix = New ColorMatrix(New Single()() _
                //           {New Single() {-1, 0, 0, 0, 0}, _
                //            New Single() {0, -1, 0, 0, 0}, _
                //            New Single() {0, 0, -1, 0, 0}, _
                //            New Single() {0, 0, 0, 1, 0}, _
                //            New Single() {0, 0, 0, 0, 1}})

                ColorMatrix cmPicture = new ColorMatrix(new float[][]
                {
                    new float[]{0.299f, 0.299f, 0.299f, 0, 0},
                    new float[]{0.587f, 0.587f, 0.587f, 0, 0},
                    new float[]{0.114f, 0.114f, 0.114f, 0, 0},
                    new float[]{0, 0, 0, 1, 0},
                    new float[]{0, 0, 0, 0, 1}
                });
                attributes.SetColorMatrix(cmPicture);
                Graphics graphics = Graphics.FromImage(bmpInverted);

                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                
                //Release all resources used by this Graphics.
                graphics.Dispose();
                pictureBox1.Image = bmpInverted;
            }
        }

        //Peach
        void filter1()
        {
            if (!mIsOpen)
            {
                MessageBox.Show("Open an Image then apply changes");
            }
            else
            {
                Image image = pictureBox1.Image;
                Bitmap bmpInverted = new Bitmap(image.Width, image.Height);

                ImageAttributes attributes = new ImageAttributes();

                ColorMatrix cmPicture = new ColorMatrix(new float[][]
                {
                    new float[]{1, 0, 0, 0, 0},
                    new float[]{0, 1, 0, 0, 0},
                    new float[]{0, 0, 1, 0, 0},
                    new float[]{0, 0, 0, 1, 0},
                    new float[]{0.40f, 0, 0, 0, 1}
                });
                attributes.SetColorMatrix(cmPicture);
                Graphics graphics = Graphics.FromImage(bmpInverted);

                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);

                //Release all resources used by this Graphics.
                graphics.Dispose();
                pictureBox1.Image = bmpInverted;
            }
        }

        // Radioactive
        void filter2()
        {
            if (!mIsOpen)
            {
                MessageBox.Show("Open an Image then apply changes");
            }
            else
            {

                Image image = pictureBox1.Image;
                Bitmap bmpInverted = new Bitmap(image.Width, image.Height);
                
                //ImageAttributes change the attribute of images
                ImageAttributes attributes = new ImageAttributes();                
                // the color matrix object will change the colors or apply image filter on image
                ColorMatrix cmPicture = new ColorMatrix(new float[][]
                {
                    //red
                    //green
                    //blue
                    //hue
                    //saturation
                    new float[]{.393f, .349f+0.5f, .272f, 0, 0},
                    new float[]{.769f+0.3f, .686f, .534f, 0, 0},
                    new float[]{.189f, .168f, .131f+0.5f, 0, 0},
                    new float[]{0, 0, 0, 1, 0},
                    new float[]{ 0, 0, 0, 0, 1}
                });
                attributes.SetColorMatrix(cmPicture);
                Graphics graphics = Graphics.FromImage(bmpInverted);
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                
                //Release all resources used by this Graphics.
                graphics.Dispose();
                pictureBox1.Image = bmpInverted;

            }
        }

        // Aqua
        void filter3()
        {
            if (!mIsOpen)
            {
                MessageBox.Show("Open an Image then apply changes");
            }
            else
            {

                Image image = pictureBox1.Image;
                Bitmap bmpInverted = new Bitmap(image.Width, image.Height);

                //ImageAttributes change the attribute of images
                ImageAttributes attributes = new ImageAttributes();
                // the color matrix object will change the colors or apply image filter on image
                ColorMatrix cmPicture = new ColorMatrix(new float[][]
                {
                    //red
                    //green
                    //blue
                    //hue
                    //saturation
                    new float[]{0.50f, 0, 0, 0, 0},
                    new float[]{0, 1, 0, 0, 0},
                    new float[]{0, 0, 1, 0, 0},
                    new float[]{0, 0, 0, 1, 0},
                    new float[]{0, 0, 0, 0, 1}
                });
                attributes.SetColorMatrix(cmPicture);
                Graphics graphics = Graphics.FromImage(bmpInverted);
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);

                //Release all resources used by this Graphics.
                graphics.Dispose();
                pictureBox1.Image = bmpInverted;

            }
        }

        //sepia
        void filter4()
        {
            if (!mIsOpen)
            {
                MessageBox.Show("Open an Image then apply changes");
            }
            else
            {

                Image image = pictureBox1.Image;
                Bitmap bmpInverted = new Bitmap(image.Width, image.Height);

                //ImageAttributes change the attribute of images
                ImageAttributes attributes = new ImageAttributes();
                // the color matrix object will change the colors or apply image filter on image
                ColorMatrix cmPicture = new ColorMatrix(new float[][]
                {
                    //red
                    //green
                    //blue
                    //hue
                    //saturation
                    new float[]{0, 0, 0, 1, 1},
                    new float[]{1, 1, 0, 0, 0},
                    new float[]{0, 0, 1, 1, 0},
                    new float[]{0, 0, 0, 1, 0},
                    new float[]{0, 0, 0, 0, 1}
                });
                attributes.SetColorMatrix(cmPicture);
                Graphics graphics = Graphics.FromImage(bmpInverted);
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);

                //Release all resources used by this Graphics.
                graphics.Dispose();
                pictureBox1.Image = bmpInverted;

            }
        }

        // Tangerine
        void filter5()
        {
            if (!mIsOpen)
            {
                MessageBox.Show("Open an Image then apply changes");
            }
            else
            {

                Image image = pictureBox1.Image;
                Bitmap bmpInverted = new Bitmap(image.Width, image.Height);

                //ImageAttributes change the attribute of images
                ImageAttributes attributes = new ImageAttributes();
                // the color matrix object will change the colors or apply image filter on image
                ColorMatrix cmPicture = new ColorMatrix(new float[][]
                {
                    //red
                    //green
                    //blue
                    //hue
                    //saturation
                    new float[]{2, 0, 0, 0, 0},
                    new float[]{0, 1, 0, 0, 0},
                    new float[]{0, 0, 1, 0, 0},
                    new float[]{0, 0, 0, 1, 0},
                    new float[]{0.2f, 0.2f, 0.2f, 0, 1}
                });
                attributes.SetColorMatrix(cmPicture);
                Graphics graphics = Graphics.FromImage(bmpInverted);
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);

                //Release all resources used by this Graphics.
                graphics.Dispose();
                pictureBox1.Image = bmpInverted;

            }
        }

        // track bars
        void hue()
        {
            Reload();
            if (!mIsOpen)
            {
                MessageBox.Show("Open an Image then apply changes");
            }
            else
            {

                Image image = pictureBox1.Image;
                Bitmap bmpInverted = new Bitmap(image.Width, image.Height);                                                     

                //ImageAttributes change the attribute of images
                ImageAttributes attributes = new ImageAttributes();
                // the color matrix object will change the colors or apply image filter on image
                ColorMatrix cmPicture = new ColorMatrix(new float[][]
                {
                    //red
                    //green
                    //blue
                    //hue
                    //saturation
                    new float[]{1+(trackBar1.Value*0.01f), 0, 0, 0, 0},
                    new float[]{0, 1+(trackBar2.Value * 0.01f), 0, 0, 0},
                    new float[]{0, 0, 1+(trackBar3.Value * 0.01f), 0, 0},
                    new float[]{0, 0, 0, 1, 0},
                    new float[]{0, 0, 0, 0, 1}
                });
                attributes.SetColorMatrix(cmPicture);
                Graphics graphics = Graphics.FromImage(bmpInverted);

                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                
                //Release all resources used by this Graphics.
                graphics.Dispose();
                pictureBox1.Image = bmpInverted;

            }
        }


        // Trackbar value change listener
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            hue();
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            hue();
        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            hue();
        }

        // crop ---------------------------------------------------------------------
        

    }
}
