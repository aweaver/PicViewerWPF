#region File Description
//-----------------------------------------------------------------------------
// Window1.xaml.cs
//
// handles the main window and messages
// 
// 
// 
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using System.Windows.Forms;
#endregion

namespace PicViewerWPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private int ImageListlen=100;//this is what is possible
        private int ImageListlenMax = 100;//this is what I want
        private int currIndx=0;
        private string[] imagelist;
        private string imageDir = @"f:\\artref\\pony\\";//f:/artref/pony/ #
       // d:/desktop/downloads/recro/anthro/strangem/set1/
       // d:/desktop/downloads/recro/anthro/futam/futa series/
        //d:\desktop\downloads\recro\anthro\strangem\set1\
        private double dScale=1.0;

        private ImageListGen imageListGen;// makes the list of images
        List<string> filelist;// list of files to use
        int pixelwidth, pixelheight;

        /// <summary>
        /// 
        /// </summary>
        public Window1()
        {
           
            InitializeComponent();
            ImageListlen= ImageListlenMax;
            imageListGen = new ImageListGen();
            filelist = imageListGen.generateImageList(imageDir, ImageListlenMax);
            ImageListlen = filelist.Count;

            startViewingList();
        }

        /// <summary>
        /// OnKeyDownHandler -Handles arrow keys
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDownHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //go back through picture list
            if (e.Key == Key.Left)
            {
                //Debug.WriteLine("Move left ");
                moveLeft();
            }

            //go forwards through image list
            if (e.Key == Key.Right)
            {
                //Debug.WriteLine("Move right ");
                moveRight();
            }

        }

        /// <summary>
        /// BackBtn_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            moveLeft();
        }

        /// <summary>
        /// Forward_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            moveRight();
        }

        /// <summary>
        ///  moveRight - moves forward through the list of pictures.
        /// </summary>
        private void moveRight()
        {
            if (filelist.Count == 0)
                return;

            currIndx++;
            if (currIndx >= ImageListlen)
                currIndx = 0;

            //ImageDisp.Source = new BitmapImage(new Uri(filelist[currIndx]));
            updateDispSource(currIndx);
        }

        /// <summary>
        /// moveLeft - moves backwards through the list
        /// </summary>
        private void moveLeft()
        {
            if (filelist.Count == 0)
                return;

            currIndx--;

            if (currIndx < 0)
                currIndx = ImageListlen - 1;

           // ImageDisp.Source = new BitmapImage(new Uri(filelist[currIndx]));
            updateDispSource(currIndx);
        }

        /// <summary>
        /// New_Click-  makes a new list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void New_Click(object sender, RoutedEventArgs e)
        {
            startViewingList();
        }

        /// <summary>
        /// startViewingList- makes a new viewing list
        /// </summary>
        private void startViewingList()
        {
            filelist = imageListGen.generateImageList(imageDir, ImageListlenMax);
            currIndx = 0;
            ImageListlen = filelist.Count;

            if (filelist.Count>0)
                updateDispSource(0);
            //  ImageDisp.Source = new BitmapImage(new Uri(filelist[0]));//start with a picture
        }

        /// <summary>
        /// updateDispSource- load the bitmap and send it to the imagedisp control to display it.
        /// </summary>
        /// <remarks > Later on we may have to save the bitmap</remarks>
        /// <param name="index"></param>
        private void updateDispSource(int index)
        {
            BitmapImage bmpImage;

            PicLabel.Content = filelist[index] + " " + index.ToString();
            bmpImage = new BitmapImage(new Uri(filelist[index]));
            ImageDisp.Source = bmpImage;

            pixelwidth = bmpImage.PixelWidth;
            pixelheight = bmpImage.PixelHeight;
            
            Debug.WriteLine("W "+bmpImage.PixelWidth+" H " + bmpImage.PixelHeight);
            dScale = 1;
            ImageDisp.RenderTransform = new ScaleTransform(dScale, dScale, (double)(pixelwidth / 2.0f), (double)(pixelheight / 2.0f));
        }

        /// <summary>
        /// SetDir_Click- Gets the new dir and gets the new viewing set from it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetDir_Click(object sender, RoutedEventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a Cursor.
            FolderBrowserDialog pickFoldersDialog = new FolderBrowserDialog();

            pickFoldersDialog.ShowNewFolderButton = false;
            //pickFoldersDialog.SelectedPath = imageDir;
            pickFoldersDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;//System.Environment.SpecialFolder.MyComputer

            DialogResult result = pickFoldersDialog.ShowDialog();
            // Show the Dialog.
            // If the user clicked OK in the dialog and
            // a .CUR file was selected, open it.
            if (result.ToString() == "OK")
            {

                //Debug.WriteLine("Got dir " + pickFoldersDialog.SelectedPath);
                imageDir = pickFoldersDialog.SelectedPath;
                startViewingList();
            }
     }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs args)
        {
            base.OnPreviewMouseWheel(args);
            Transform temptransform;
   
           // Debug.WriteLine("Mouse wheel " + args.Delta);

           // ImageDisp.LayoutTransform=new ScaleTransform(2.0,2.0) ;

           
            if (args.Delta > 0)
               dScale+=0.10;

            if (args.Delta < 0)
            {
                dScale -= 0.10;
                if (dScale < 1.0)
                    dScale = 1.0;
            }
                
             //Debug.WriteLine(" dScale " +dScale);
             //Debug.WriteLine(" ImageDisp.LayoutTransform " + ImageDisp.LayoutTransform);
             ImageDisp.RenderTransform = new ScaleTransform(dScale, dScale,(double)(pixelwidth / 2.0f), (double)(pixelheight / 2.0f));
            
           
           // ImageDisp.InvalidateMeasure();
            //ImageDisp.InvalidateVisual();
            //ImageDisp.
        }


    }
}
