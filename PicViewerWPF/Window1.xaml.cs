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

        private ImageListGen imageListGen;
        List<string> filelist;
       
        public Window1()
        {
           
            InitializeComponent();
            ImageListlen= ImageListlenMax;
            imageListGen = new ImageListGen();
            filelist = imageListGen.generateImageList(imageDir, ImageListlenMax);
            ImageListlen = filelist.Count;

            startViewingList();
        }

        private void OnKeyDownHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                //Debug.WriteLine("Move left ");
                moveLeft();
            }

            if (e.Key == Key.Right)
            {
                //Debug.WriteLine("Move right ");
                moveRight();
            }

        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            moveLeft();
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            moveRight();
        }

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

        private void New_Click(object sender, RoutedEventArgs e)
        {
            startViewingList();
        }

        private void startViewingList()
        {
            filelist = imageListGen.generateImageList(imageDir, ImageListlenMax);
            currIndx = 0;
            ImageListlen = filelist.Count;

            if (filelist.Count>0)
                updateDispSource(0);
            //  ImageDisp.Source = new BitmapImage(new Uri(filelist[0]));//start with a picture
        }

        private void updateDispSource(int index)
        {
            PicLabel.Content = filelist[index] + " " + index.ToString();
            ImageDisp.Source = new BitmapImage(new Uri(filelist[index]));
        }

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

    }
}
